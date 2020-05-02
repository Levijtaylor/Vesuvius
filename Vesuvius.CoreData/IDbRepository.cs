using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesuvius.Models;

namespace Vesuvius.CoreData
{
    public interface IDbRepository
    {
        /// <summary>
        /// Returns all Users within the Database
        /// </summary>
        /// <returns></returns>
        List<User> GetUsers();

        /// <summary>
        /// Returns a User By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetUser(int id);

        /// <summary>
        /// Returns a User after database information has been updated
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User UpdateUser(User user);

        /// <summary>
        /// Removes a User from the database by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void RemoveUser(int id);

        /// <summary>
        /// Adds a new User to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int CreateUser(User user,string passwordHash);

        /// <summary>
        /// Used to Update a Users Role within the System
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        //SystemRole SetRole(User user, SystemRole role);

        /// <summary>
        /// Adds a new Message to the Datbase
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        int CreateMessage(Message message);

        /// <summary>
        /// Returns a Message by ID from the DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Message GetMessage(int id);

        /// <summary>
        /// Returns all Unread Messages within channels
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<Message> GetNotifciationsForUser(User user);

        /// <summary>
        /// Used to Gather All Messages within a Single Channel by id
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        List<Message> GetMessages(int channelID);

        /// <summary>
        /// Used to Remove a Message from the Channel
        /// Returns Message ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void RemoveMessage(int id);

        /// <summary>
        /// Edit an Existing Message's Contents
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Message UpdateMessage(Message message);

        /// <summary>
        /// Adds a new Channel to the DB
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        int CreateChannel(Channel channel);

        /// <summary>
        /// Returns all Channels that a user is in from the Db
        /// </summary>
        /// <returns></returns>
        List<Channel> GetChannels(int id);

        /// <summary>
        /// Returns a Single Channel from the DB by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Channel GetChannel(int id);

        /// <summary>
        /// Removes a Channel from the Database by ID
        /// Returns the Channel ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void RemoveChannel(int id);

        /// <summary>
        /// Edits an Existing Channel
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        Channel UpdateChannel(Channel channel);


    }
}
