using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesuvius.Models
{
    public class User
    {
        /// <summary>
        /// The User's Identification Number
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the User being Created. Not for use in Public View
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A Digital UserName of the User
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Email of User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Determines whether the User is able to use the Application at all.
        /// Only to be activated if User is to be banned but records still kept.
        /// </summary>     
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// The Role the User has in the system
        /// </summary>
        public SystemRole Role { get; set; }

        /// <summary>
        /// Used for Account creation only. Passes set password to API
        /// </summary>
        public string Password { get; set; }


    }
}
