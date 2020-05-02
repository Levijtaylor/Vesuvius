using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesuvius.Models
{
    public enum MessageType
    {
        /// <summary>
        /// For Text based messages
        /// </summary>
        Text,
        /// <summary>
        /// For Web based Links
        /// </summary>
        Link,
        /// <summary>
        /// For Images 
        /// </summary>
        Image,
        /// <summary>
        /// For File Sharing
        /// </summary>
        File
    }
}
