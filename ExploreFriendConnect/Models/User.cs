using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ExploreFriendConnect.Models
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string FCId { get; set; }


        /*
        /// <summary>
        /// Public constructor.
        /// </summary>
        public User() {
          this.id = 0;
          this.name = "";
          this.password = "";
          this.image = "";
          this.fcid = "";
        }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int Id {
          get { return id; }
          set { id = value; }
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Name {
          get { return name; }
          set { name = value; }
        }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string Password {
          get { return password; }
          set { password = value; }
        }

        /// <summary>
        /// Gets or sets the user's thumbnail image.
        /// </summary>
        public string Image {
          get { return image; }
          set { image = value; }
        }

        /// <summary>
        /// Gets or sets the user's FriendConnect Id.
        /// </summary>
        public string FCId {
          get { return fcid; }
          set { fcid = value; }
        }

        /// <summary>
        /// User id.
        /// </summary>
        private int id;

        /// <summary>
        /// User name.
        /// </summary>
        private string name;

        /// <summary>
        /// User password.
        /// </summary>
        private string password;

        /// <summary>
        /// User's thumbnail image.
        /// </summary>
        private string image;

        /// <summary>
        /// User's FriendConnect Id, if available.
        /// </summary>
        private string fcid;
        */
    }
}