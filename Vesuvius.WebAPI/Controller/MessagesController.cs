using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vesuvius.Models;
using Vesuvius.CoreData;

namespace Vesuvius.WebAPI.Controller
{
    public class MessagesController : ApiController
    {
        public static DbRepository _repo { get; set; }

        public MessagesController()
        {
            _repo = new DbRepository();
        }

        public Message Get(int id)
        {
            return null;
        }

        /// <summary>
        /// Returns a list of Messages within a single channel
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        [Route("Channels/{channelID}/Messages")]
        [HttpGet]
        public List<Message> GetMessagesFromChannel(int channelID)
        {
           return _repo.GetMessages(channelID);
        }

        /// <summary>
        /// Adds a new Message to the DB
        /// Returns an HTPP Response Message header
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]Message message)
        {
            var id = _repo.CreateMessage(message);
            message.Id = id;
            var response = Request.CreateResponse(message);
            response.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri + "/" + id);
            return response;
        }

        /// <summary>
        /// Removing this Message from the database when routine cleanup occurs
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _repo.RemoveMessage(id);
        }
    }
}
