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
        }
    }
}
