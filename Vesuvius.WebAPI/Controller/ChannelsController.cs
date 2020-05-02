using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vesuvius.Models;

namespace Vesuvius.WebAPI.Controller
{
    public class ChannelsController : ApiController
    {
        //DbRepository
        public static Vesuvius.CoreData.DbRepository _repo;

        public ChannelsController()
        {
            _repo = new Vesuvius.CoreData.DbRepository();
        }

        /// <summary>
        /// Retrieves all Channels that a Single User has Access too
        /// Returns a list Channel.
        /// [Note:] Also returns every user within each channel in list
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("Users/{UserID}/Channels")]
        [HttpGet]
        public List<Channel> GetChannelsForUser(int UserID)
        {
            return _repo.GetChannels(UserID);
        }

        /// <summary>
        /// Returns a channel from the Db by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel Get(int id)
        {
            return _repo.GetChannel(id);
        }

        /// <summary>
        /// Adds a new Channel to the Db and Creates a connection between User and Channel
        /// Returns a Response Message header.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public HttpResponseMessage Post([FromBody]Channel channel)
        {
            var id = _repo.CreateChannel(channel);

            var response = Request.CreateResponse(id);
            response.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri + "/" + id);
            
            return response;
        }

        /// <summary>
        /// Removing a Channel from the database when routine cleanup occurs
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _repo.RemoveChannel(id);
        }

        /// <summary>
        /// Grabbing all Users and first 25 Messages of select channel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Channels/{id}/Users/Messages")]
        [HttpGet]
        public Channel GetFullChannelInfo(int id)
        {
            var users = _repo.GetUsers(id);
            var messages = _repo.GetMessages(id);

            return new Channel
            {
                Messages = messages,
                Users = users
            };
        }
    }
}
