using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesuvius.Models
{
    public class LoginResponse
    {

        /// <summary>
        /// The user that logged in
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The list of Channels a User is allowed to access
        /// </summary>
        public List<Channel> Channels { get; set; }

        /// <summary>
        /// list of Users within channels for quick socialization logic
        /// </summary>
        public List<User> Users { get; set; }
    }
}
