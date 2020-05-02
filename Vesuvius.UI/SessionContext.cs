using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesuvius.Models;

namespace Vesuvius.UI
{
    public class SessionContext
    {

        /// <summary>
        /// The User Object the contains the User information
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// List of Channels tha User has Access too.
        /// Also Contains the Messages per channel that has been loaded to local memory
        /// </summary>
        public List<Channel> Channels { get; set; }

        /// <summary>
        /// Initial Declaration of WebClient
        /// </summary>
        private WebClient _webClient;

        /// <summary>
        /// A list of Users that have been interacted with during runtime.
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// Constructor to instantiate Webclient
        /// </summary>
        public SessionContext()
        {
            _webClient = new WebClient();
            
        }

        /// <summary>
        /// Adds a new channel to list of _channels
        /// </summary>
        /// <param name="channel"></param>
        public void AddChannel(Channel channel)
        {
           var newChannel = _webClient.CreateChannel(channel);
            Channels.Add(newChannel);
        }

        /// <summary>
        /// Adds a single message to a specific channel by Id
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channelId"></param>
        public Message AddMessage(Message message)
        {
            try
            {
                var newMessage = _webClient.CreateMessage(message);
                Channels.Where(p => p.Id == newMessage.ChannelID).Single().Messages.Add(message);
                return message;
            }
            catch(NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Adds a new user to the database and local state
        /// returns bool to let new User they can sign in now
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            var newUser =_webClient.CreateUser(user);
            if (newUser != null)
                return true;
            else
                return false;
            
        }

        /// <summary>
        /// Logs the User in if appropriate Credentials are present
        /// </summary>
        /// <param name="request"></param>
        public bool Login(LoginRequest request)
        {
            var response = _webClient.LoginRequest(request);

            //If Login Failed
            if (response == null)
                return false;

            //Setting local state to logged in User Information
            User = response.User;
            Channels = response.Channels;
            Users = response.Users;
            

            return true;
        }

        /// <summary>
        /// Grabs all new notifications
        /// </summary>
        /// <returns></returns>
        public List<Message> GetNotifications()
        {
          var messages = _webClient.GetMessageNotifications(User.Id);
          return messages;
        }

    }
}
