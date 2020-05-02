using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vesuvius.Models;

namespace Vesuvius.WebAPI.Controller
{
    public class NotificationsController : ApiController
    {
        private Vesuvius.CoreData.DbRepository _repo;

        public NotificationsController()
        {
            _repo = new CoreData.DbRepository();
        }

        /// <summary>
        /// Grabs all new messages from the database by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("Notifications/{userId}")]
        public List<Message> Get(int userId)
        {
            var notifications = _repo.GetNotifications(userId);
            return notifications;
        }
    }
}
