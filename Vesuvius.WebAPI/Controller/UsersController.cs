using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vesuvius.Models;
using Vesuvius.CoreData;
using System.Text;

namespace Vesuvius.WebAPI.Controller
{
    public class UsersController : ApiController
    {
        public static DbRepository _repo;

        public UsersController()
        {
            _repo = new DbRepository();
        }

        /// <summary>
        /// Returns all users within the DB
        /// </summary>
        /// <returns></returns>
        public List<User> Get()
        {
           return _repo.GetUsers();
        }

        /// <summary>
        /// Returns a list of Users within a Specific Channel by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Channels/{id}/Users")]
        [HttpGet]
        public List<User> GetUserForChannel(int id)
        {
           return _repo.GetUsers(id);
        }

        /// <summary>
        /// Gets a single User from the Db by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public User GetUser(int id)
        {
            return _repo.GetUser(id);
        }

        /// <summary>
        /// Adds a new User to the Database.
        /// Returns a response header.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("Users/Create")]
        public HttpResponseMessage Post([FromBody]User user)
        {
            var hashPass = HashPassword(user.Password);

            var id = _repo.CreateUser(user,hashPass);

            var response = Request.CreateResponse();
            response.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri + "/" + id);
            
            return response;
        }

        /// <summary>
        /// Updating information of selected user within the Db
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User Put([FromBody]User user)
        {
           return _repo.UpdateUser(user);
        }

        /// <summary>
        /// Removing user from the database when routine cleanup occurs
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _repo.RemoveUser(id);
        }

        /// <summary>
        /// Converts inserted password to hash for Credential Check
        /// Checks the credentials of the password and userName for login.
        /// Returns a UserContext 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        [Route("Users/Login")]
        [HttpPost]
        public HttpResponseMessage UserLogin([FromBody]LoginRequest sRequest)
        {
            var hashPass = HashPassword(sRequest.Password);

            var user = _repo.Login(sRequest.UserName,hashPass);

            var channels = _repo.GetChannels(user.Id);

            var possibleInteractions = new List<User>();

            foreach(var channel in channels)
            {
                channel.Messages = _repo.GetMessages(channel.Id);
                channel.Users = _repo.GetUsers(channel.Id);
                foreach(var possUser in channel.Users)
                {
                    if(!possibleInteractions.Contains(possUser))
                    possibleInteractions.Add(possUser);
                }
                
            }

            var response = Request.CreateResponse<LoginResponse>(new LoginResponse
            {
                Channels = channels,               
                User = user,
                Users = possibleInteractions
            });

            if(user == null)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Login Credentials");

            return response;
        }

        /// <summary>
        /// Hashes passwords given within the securityrequest
        /// </summary>
        /// <param name="sRequest"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            var salt = Encoding.ASCII.GetBytes("");

            var hashProvider = new System.Security.Cryptography.PasswordDeriveBytes(Encoding.ASCII.GetBytes(password), salt);

            return Convert.ToBase64String(hashProvider.GetBytes(32));
        }

    }
}
