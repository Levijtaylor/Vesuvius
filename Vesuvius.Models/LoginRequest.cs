using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesuvius.Models
{
    /// <summary>
    /// This class will be used as an object to represent a request to the API for Login.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username that was input to the client
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password that was input to the client
        /// </summary>
        public string Password { get; set; }
    }
}
