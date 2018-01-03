using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;
using System;
using System.Linq;
using System.Reflection;

namespace FanBlog.Framework.SqlSugarExtensions
{
    /// <summary>
    /// SqlSugar 注入Service的扩展方法 <see cref="IServiceCollection" />.
    /// SqlSugar 和Dbcontext 不同，没有DbContext池 所有的 AddSQLSugarClientPool 已全部删除
    /// SqlSugarClient 没有无参的构造函数 configAction 可为空的方法已注释
    /// </summary>
    public static class SqlSugarServiceCollectionExtensions
    {
        /// <summary>
        ///     将给定的上下文作为服务注册在<see cref ="IServiceCollection"/>中
        ///     在应用程序中使用依赖注入时，如ASP.NET使用此方法。
        ///     有关设置依赖注入的更多信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=526890。
        /// </summary>
        /// <example>
        ///     <code>
        ///          services.AddSQLSugarClient&lt;SqlSugarClient&gt;(config =>
        ///          {
        ///              config.ConnectionString = connectionString;
        ///              config.DbType = DbType.MySql;
        ///              config.IsAutoCloseConnection = true;
        ///              config.InitKeyType = InitKeyType.Attribute;
        ///          });
        ///      </code>
        /// </example>
        /// <typeparam name="TSugarClient"> 要注册的上下文的类型。 </typeparam>
        /// <param name="serviceCollection"> <see cref="IServiceCollection" /> 添加服务。 </param>
        /// <param name="configAction">
        ///     <para>
        ///         为上下文配置<see cref ="ConnectionConfig"/> 的操作。
        ///     </para>
        ///     <para>
        ///         为了将选项传递到上下文中，需要在您的上下文中公开构造函数
        ///         <see cref="ConnectionConfig" /> and passes it to the base constructor of <see cref="TSugarClient" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> 用于在容器中注册TSugarClient服务的生命周期。 </param>
        /// <param name="configLifetime"> 用于在容器中注册ConnectionConfig服务的生命周期。 </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static IServiceCollection AddSQLSugarClient<TSugarClient>(
            [NotNull] this IServiceCollection serviceCollection,
            [NotNull] Action<ConnectionConfig> configAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime configLifetime = ServiceLifetime.Scoped)
            where TSugarClient : SqlSugarClient
            => AddSQLSugarClient<TSugarClient>(
                serviceCollection,
                (p, b) => configAction.Invoke(b), contextLifetime, configLifetime);

        /// <summary>
        ///     <para>
        ///         将给定的上下文作为服务注册在<see cref ="IServiceCollection"/>中。
        ///         在应用程序中使用依赖注入时，如ASP.NET使用此方法。
        ///         有关设置依赖注入的更多信息，请参阅http://go.microsoft.com/fwlink/?LinkId=526890。
        ///     </para>
        /// </summary>
        /// <typeparam name="TSugarClient"> 要注册的上下文的类型。 </typeparam>
        /// <param name="serviceCollection"> <see cref="IServiceCollection" /> 添加服务。 </param>
        /// <param name="configAction">
        ///     <para>
        ///         为上下文配置<see cref ="ConnectionConfig"/> 的操作。
        ///     </para>
        ///     <para>
        ///         为了将选项传递到上下文中，需要在您的上下文中公开构造函数
        ///         <see cref="ConnectionConfig" /> and passes it to the base constructor of <see cref="TSugarClient" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> 用于在容器中注册TSugarClient服务的生命周期。 </param>
        /// <param name="configLifetime"> 用于在容器中注册ConnectionConfig服务的生命周期。 </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static IServiceCollection AddSQLSugarClient<TSugarClient>(
            [NotNull] this IServiceCollection serviceCollection,
            [NotNull] Action<IServiceProvider, ConnectionConfig> configAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime configLifetime = ServiceLifetime.Scoped)
            where TSugarClient : SqlSugarClient
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            if (contextLifetime == ServiceLifetime.Singleton)
            {
                configLifetime = ServiceLifetime.Singleton;
            }
            //检查构造函数 应该不需要本步骤
            CheckContextConstructors<TSugarClient>();

            AddCoreServices<TSugarClient>(serviceCollection, configAction, configLifetime);

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TSugarClient), typeof(TSugarClient), contextLifetime));

            return serviceCollection;
        }

        private static void AddCoreServices<TSugarClient>(
            IServiceCollection serviceCollection,
            Action<IServiceProvider, ConnectionConfig> configAction,
            ServiceLifetime configLifetime)
            where TSugarClient : SqlSugarClient
        {
            serviceCollection
                .AddMemoryCache()
                .AddLogging();

            serviceCollection.TryAdd(
                new ServiceDescriptor(
                    typeof(ConnectionConfig),
                    p => ConnectionConfigFactory(p,configAction),
                    configLifetime));

            //为了不报错 不使用GetRequiredService方法
            serviceCollection.Add(
                new ServiceDescriptor(
                    typeof(ConnectionConfig),
                    //p => p.GetRequiredService<ConnectionConfig>(),
                    p => ConnectionConfigFactory(p, configAction),
                    configLifetime));
        }

        private static ConnectionConfig ConnectionConfigFactory(
            [NotNull] IServiceProvider applicationServiceProvider,
            [NotNull] Action<IServiceProvider, ConnectionConfig> configAction)
        {
            var config=new ConnectionConfig();

            configAction.Invoke(applicationServiceProvider,config);

            return config;
        }

        //检查 TSugarClient 构造函数是否有带参数的构造函数
        private static void CheckContextConstructors<TSugarClient>()
            where TSugarClient : SqlSugarClient
        {
            var declaredConstructors = IntrospectionExtensions.GetTypeInfo(typeof(TSugarClient)).DeclaredConstructors.ToList();
            if (declaredConstructors.Count == 1
                && declaredConstructors[0].GetParameters().Length == 0)
            {
                throw new ArgumentException(CoreStrings.SqlSugarClientMissingConstructor(typeof(TSugarClient).Name,typeof(ConnectionConfig).Name));
            }
        }

        #region 注释
        /// <summary>
        ///     Registers the given context as a service in the <see cref="IServiceCollection" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(IServiceCollection services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddSQLSugarClient&lt;MyContext&gt;(ServiceLifetime.Scoped);
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TSugarClient"> The type of context to be registered. </typeparam>
        /// <param name="serviceCollection"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="configLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        //public static IServiceCollection AddSQLSugarClient<TSugarClient>(
        //    [NotNull] this IServiceCollection serviceCollection,
        //    ServiceLifetime contextLifetime,
        //    ServiceLifetime configLifetime = ServiceLifetime.Scoped)
        //    where TSugarClient : SqlSugarClient
        //    => AddSQLSugarClient<TSugarClient>(serviceCollection, (Action<IServiceProvider, ConnectionConfig>)null, contextLifetime, configLifetime); 
        #endregion
    }
}
