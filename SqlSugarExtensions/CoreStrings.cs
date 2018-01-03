using JetBrains.Annotations;

namespace FanBlog.Framework.SqlSugarExtensions
{
    /// <summary>
    ///		此API支持实体框架核心基础结构，不打算使用
    ///     直接从你的代码。 未来的版本中，此API可能会更改或删除。
    /// </summary>
    public class CoreStrings
    {
        /// <summary>
        ///     字符串参数"{argumentName}"不能为空。
        /// </summary>
        public static string ArgumentIsEmpty([CanBeNull] object argumentName)
            => $"字符串参数 '{nameof(argumentName)}' 不能为空。";

        /// <summary>
        ///     指定的键属性{key}没有在实体类型"{entityType}"上声明。 确保在目标实体类型上声明关键属性。
        /// </summary>
        public static string KeyPropertiesWrongEntity([CanBeNull] object key, [CanBeNull] object entityType)
            => $"T指定的键属性 {nameof(key)} 没有在实体类型 '{nameof(entityType)}'上声明。 确保在目标实体类型上声明关键属性。";

        /// <summary>
        ///     集合参数"{argumentName}"必须至少包含一个元素。
        /// </summary>
        public static string CollectionArgumentIsEmpty([CanBeNull] object argumentName)
            => $"集合参数 '{nameof(argumentName)}' 必须至少包含一个元素。";

        /// <summary>
        ///     为参数"{argumentName}"提供的实体类型"{type}"必须是引用类型。
        /// </summary>
        public static string InvalidEntityType([CanBeNull] object type, [CanBeNull] object argumentName)
            => $"为参数 '{nameof(argumentName)}' 提供的实体类型 '{nameof(type)}' 必须是引用类型。";

        /// <summary>
        ///     AddSQLSugarClient是通过配置调用的，但上下文类型"{sqlSugarClientType}"只声明一个无参数的构造函数。 这意味着传递给AddSQLSugarClient的配置永远不会被使用。 如果配置被传递给AddSQLSugarClient，那么'{sqlSugarClientType}'应该声明一个构造函数，它接受一个 {argumentName} ; 并且必须将它传递给'{sqlSugarClientType}'的基础构造函数。
        /// </summary>
        public static string SqlSugarClientMissingConstructor([CanBeNull] object sqlSugarClientType, [CanBeNull] object argumentName)
            => $"AddSQLSugarClient是通过配置调用的，但上下文类型 '{nameof(sqlSugarClientType)}' 只声明一个无参数的构造函数。 这意味着传递给AddSQLSugarClient的配置永远不会被使用。 如果配置被传递给AddSQLSugarClient，那么 '{nameof(sqlSugarClientType)}' 应该声明一个构造函数，它接受一个 {nameof(argumentName)} ; 并且必须将它传递给 '{nameof(sqlSugarClientType)}' 的基础构造函数。";

        /// <summary>
        ///     指定的poolSize必须大于0。
        /// </summary>
        public static string InvalidPoolSize
            => "指定的poolSize必须大于0。";
    }
}
