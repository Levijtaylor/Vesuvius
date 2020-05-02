using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesuvius.Models;
using RestSharp;

namespace Vesuvius.UI
{
    public class WebClient
    {

        public readonly string _basePath;

        public RestClient _client;

        public WebClient()
        {
            _basePath = System.Configuration.ConfigurationManager.AppSettings["ServerUrl"];

            _client = new RestClient(_basePath);
        }

        /// <summary>
        /// Executes a Login Authorization request from the API
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public LoginResponse LoginRequest(LoginRequest sRequest)
        {
            var request = new RestRequest("Users/Login", Method.POST);
            request.AddJsonBody(sRequest);

            IRestResponse<LoginResponse> response = _client.Execute<LoginResponse>(request);
            return response.Data;
        }

        /// <summary>
        /// Creates a request to the Web API for creating a message.
        /// </summary>
        /// <param name="message"></param>
        public Message CreateMessage(Message message)
        {
            var request = new RestRequest("Messages", Method.POST);
            request.AddJsonBody(message);

            IRestResponse<Message> response = _client.Execute<Message>(request);
            return response.Data;
        }

        /// <summary>
        /// Creates a request to the web API to add a new channel to the database
        /// </summary>
        /// <param name="channel"></param>
        public Channel CreateChannel(Channel channel)
        {
            var request = new RestRequest("Channels", Method.POST);
            request.AddJsonBody(channel);

             IRestResponse<int> response = _client.Execute<int>(request);
            channel.Id = response.Data;
            return channel;
        }

        /// <summary>
        /// Retrieves all Users and the last 25 Messages of a specific channel
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public Channel RetrieveChannelInformation(int channelID)
        {
            var request = new RestRequest($"Channels/{channelID}/Users/Messages", Method.GET);

            IRestResponse<Channel> response = _client.Execute<Channel>(request);
            return response.Data;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="sRequest"></param>
        /// <returns></returns>
        public LoginResponse CreateUser(User user)
        {
            var request = new RestRequest("Users/Create", Method.POST);
            request.AddJsonBody(user);

            IRestResponse<LoginResponse> response = _client.Execute<LoginResponse>(request);
            return response.Data;
        }

        public List<Message> GetMessageNotifications(int userId)
        {
            var request = new RestRequest($"Notifications/{userId}", Method.GET);

            IRestResponse<List<Message>> response = _client.Execute<List<Message>>(request);
            return response.Data;
        }
    }
}
