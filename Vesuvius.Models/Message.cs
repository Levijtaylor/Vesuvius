using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesuvius.Models
{
    //public class Message<T>  where T : class
    public class Message
    {
        /// <summary>
        /// The Identification integer of each Message
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Determining which Type of Data is being sent
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// Text based Content of the Message
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The Id of the channel that message is in
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// Id of the User who sent the message
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Date and Time Message was created
        /// </summary>
        public DateTime TimeStamp {get; set;}

        /// <summary>
        /// Determining which user the Message belongs too
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Channel in which Message is Linked with
        /// </summary>
        public Channel Channel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AttachmentData { get; set; }

        /// <summary>
        /// Flag for to see if the User is selected for garbage cleanup after a certain time
        /// </summary>
        public bool Enabled { get; set; }

       
    }
}
