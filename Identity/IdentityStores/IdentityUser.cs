using System;

namespace FanBlog.IdentityStores
{
    /// <summary>
    /// Represents a user in the identity system
    /// </summary>
    /// <typeparam name="TKey">The type used for the primary key for the user.</typeparam>
    public class IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
        /// </summary>
        public IdentityUser() { }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser{TKey}"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public virtual string Password { get; set; }

        public virtual string Name { get; set; }
        
        /// <summary>
        /// Returns the username for this user.
        /// </summary>
        public override string ToString()
        {
            return UserName;
        }/*ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQCtDGNQkfm2EPrUkrMdmW+3xfbsqxXeqglVlnY/HpabsVhmw7j4iyfxZt4YWDEyEVo3/3zzLPpE3HrGxXXp9E3oPPu6J5LNDQNZaaum4NOd4E5FPJTAs+Ab+xpth8Gsd2BTfBJq7+gDjdQ4RFZeJMpq4fM4WdTCnl2KvyvtQTIVu49NHYMPZ1fh2kWjwkgUv/lMtEZBpeB43/i2UtFFaPp7Yf9qx8TcFO7yXhKUU4fH9GlVm71N4ujyHjcuqUukiIILpDJ5qQP7wcUKUfGOmCirnr4ocmmx2S6jcfFlVfPHCjKV/7XrlRE7dNd+wPgiUj5UszNJb4haPp+kfP4jZwCpcZaTqtgFN6lroJw7+HXnzcJnK+pGGiDLU5eUf8xko1nBtMk/paWnuuoQ38KWF3sUYjm2w8mCmRNYpupTl/ZrJ9vxQD9ymMamI6GERCMLRn9YkAt9qlmCjXB6E7JebFORLsrFwaYm0FbeQEPntxWgxqo5Ql7NyE63FGt0ImEco+k= slfan@DESKTOP-PBOKRBF*/
    }
}
