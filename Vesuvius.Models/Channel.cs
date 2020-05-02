using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesuvius.Models
{
    public class Channel
    {
        /// <summary>
        /// The Identification Number for a Channel
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Date that the Channel was Created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The Name of the channel for users to See
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of Users that have access to this channel
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// Flag for Determining if Channel is being deleted and ready for garbage cleanup
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// The list of Messages within the Channel that have been loaded by the database
        /// </summary>
        public List<Message> Messages { get; set; }


    }
}
