using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Vesuvius.Models;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Vesuvius.CoreData
{
    /// <summary>
    /// This Class Contains all Methods dealing with the Database.
    /// For use by the WebAPI only. [Using Dapper for this Application]
    /// </summary>
    public class DbRepository: IDbRepository
    {
        /// <summary>
        /// My Connection to the Vesuvius Database
        /// </summary>
        private readonly IDbConnection _connection;

        public DbRepository()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
        }

        /// <summary>
        /// Adds a new Channel to the Db
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public int CreateChannel(Channel channel)
        {
            var id = _connection.ExecuteScalar<int>("Execute sp_CreateChannel @Name,@UserID",
                new
                {
                    Name = channel.Name,
                    UserID = channel.Users.Single().Id
                });

            return id;

        }

        /// <summary>
        /// Creates a new Connection between a user and Channel
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channel"></param>
        public void ConnectToChannel(int UserID,int ChannelID)
        {
            _connection.Execute("Insert into ChannelMapping(ChannelID,UserID,[Enabled]) values(@ChannelID,@UserID,1)",
                new
                {
                    UserID =  UserID,
                    ChannelID = ChannelID
                });

        }

        /// <summary>
        /// Creates and adds a new Message to the database
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public int CreateMessage(Message message)
        {
            var data = _connection.ExecuteScalar<int>("Insert into Message(ChannelID,TypeID,UserID,Content,[Enabled],[TimeStamp]) values(@ChannelID,@TypeID,@UserID,@Content,@Enabled,SYSDATETIME()) select SCOPE_IDENTITY()",
                new
                {
                    ChannelID = message.ChannelID,
                    Content = message.Content,
                    TypeID = message.TypeId,
                    Enabled = message.Enabled,
                    UserID = message.UserID,
                    TimeStamp = message.TimeStamp
                });
            return data ;
        }

        /// <summary>
        /// Adds a User to the Database. Grabs the Created ID from the Db and sets it to the user.
        /// Then returns user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int CreateUser(User user,string passHash)
        {

            int id = _connection.ExecuteScalar<int>("execute sp_CreateUser @PasswordHash,@Email,@Alias,@Name",
                new
                {
                    Name = user.Name,
                    Alias = user.Alias,
                    PasswordHash = passHash,
                    Email = user.Email
                });

           return id;
        }

        /// <summary>
        ///  Setting the Enabled Flag to 0 to let the system know that when Routine cleanup occurs,
        ///  that this is garbage
        /// </summary>
        /// <param name="id"></param>
        public void RemoveChannel(int id)
        {
            _connection.Execute("update Message set [Enabled] = 0 where ID = @ID",
                new
                {
                    ID = id
                });
        }

        /// <summary>
        ///  Setting the Enabled Flag to 0 to let the system know that when Routine cleanup occurs,
        ///  that this is garbage
        /// </summary>
        /// <param name="id"></param>
        public void RemoveMessage(int id)
        {
            _connection.Execute("update Message set [Enabled] = 0 where ID = @ID",
                new
                {
                    ID = id
                });
        }

        /// <summary>
        /// Setting the Enabled Flag to 0 to let the system know that when Routine cleanup occurs,
        /// That this is garbage
        /// </summary>
        /// <param name="id"></param>
        public void RemoveUser(int id)
        {
            //Look into updating with a flag to let the system deletes on routine as to prevent Contention
            _connection.Execute($"update [User] set [Enabled] = 0 where ID = @ID",
                new
                {
                    ID = id
                });
        }

        /// <summary>
        /// Retrieves a Select Channel from the Db by ID
        /// Returns a Channel object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Channel GetChannel(int id)
        {
            var data = _connection.Query("select * from Channel where ID = @ID",
                new
                {
                    ID = id
                });

            return MapChannel(data.SingleOrDefault());
        }

        /// <summary>
        /// Used to Create a new Channel from Db dynamic data
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Channel MapChannel(dynamic row)
        {
            return new Channel
            {
                Id = row.ID,
                DateCreated = row.Created,
                Name = row.Name,
            };
        }

        /// <summary>
        /// Gathers a list of Channels that the user is enabled and connected with.
        /// Returns a list of Channel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Channel> GetChannels(int id)
        {
            dynamic data = _connection.Query(
                 "select C.* from Channel as C inner join ChannelMapping as M on C.ID = M.ChannelID where UserID = @UserID and M.[Enabled] = 1",
              new
              {
                  UserID = id
              });

            var channels = new List<Channel>();
            foreach(var row in data)
            {
                channels.Add(MapChannel(row));
            }
            return channels;
        }

        public Message GetMessage(int id)
        {
            return null;
        }

        /// <summary>
        /// Retrieves messages from a specific Channel.
        /// Returns a list of Messages
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public List<Message> GetMessages(int channelID)
        {   
                dynamic data = _connection.Query("select M.*,U.Alias from [Message] as M Inner join [User] as U on U.ID = M.UserID where M.ChannelID = @ChannelID",
                    new
                    {
                        ChannelID = channelID
                    });

            var messages = new List<Message>();
            foreach(var row in data)
            {
                messages.Add(new Message
                {
                    Content = row.Content,
                    Id = row.ID,
                    TimeStamp = row.TimeStamp,
                    TypeId = row.TypeID,
                    ChannelID = row.ChannelID,
                    User = new User
                    {
                        Alias = row.Alias,
                        Id = row.UserID
                    }
                });
            }
            return messages;
        }

        public List<Message> GetNotifciationsForUser(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a User By ID from the Db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUser(int id)
        {
            var data = _connection.Query("select * from [User] as U inner join SystemRole as S on U.SystemRoleID = S.ID Where [User].ID = @UserID",
                new
                {
                    UserID = id
                });

            return MapUser(data.SingleOrDefault());
        }

        /// <summary>
        /// Gathers Users from the database that are within specific Channel
        /// returns a list of Users
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public List<User> GetUsers(int channelID)
        {      
          dynamic data = _connection.Query("select U.* from [User] as U inner join SystemRole as S on U.SystemRoleID = S.ID inner join ChannelMapping as M on U.ID = M.UserID where M.ChannelID = @ChannelID and M.[Enabled] = 1",
                new
                {
                    ChannelID = channelID
                });

            var users = new List<User>();
            foreach(var row in data)
            {
                users.Add(MapUser(row));            
            }
            return users;
        }

        /// <summary>
        /// Maps dynamic data from the Db to a User
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public User MapUser(dynamic row)
        {
            return new User
            {
                Id = row.ID,
                Alias = row.Alias,
                Name = row.Name,
                Role = ((SystemRole)row.SystemRoleID)
            };
        }

        /// <summary>
        /// Grabs all Users and joined system roles from the database.
        /// Returns list of Users
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers()
        {
            dynamic data = _connection.Query("select * from [User] as U inner join SystemRole as S on S.ID = U.SystemRoleID");

            var users = new List<User>();
            foreach(var row in data)
            {
                users.Add(MapUser(row));
                
            }
            return users;
        }

        /// <summary>
        /// Updating Information within the database of a single channel
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public Channel UpdateChannel(Channel channel)
        {
            var data = _connection.Query("Update Channel set [Enabled] = @Enabled, [Name] = @Name where ID = @ID",
                new
                {
                    Enabled = channel.Enabled,
                    Name = channel.Name,
                    ID = channel.Id
                });

            return MapChannel(data.SingleOrDefault());
        }

        public Message UpdateMessage(Message message)
        {
            //var data = _connection.Query("Update [Message] set [Content] = @Content",
            //    new
            //    {
            //        message.
            //    });
            return null;
        }

        /// <summary>
        /// Updating information within the database of a single User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User UpdateUser(User user)
        {
           var data = _connection.Query("Update [User] set Alias = @Alias, [Name] = @Name, [Enabled] = @Enabled where ID = @ID",
                new
                {
                    Alias = user.Alias,
                    Name = user.Name,
                    Enabled = user.IsEnabled,
                    ID = user.Id
                });

            return (MapUser(data.SingleOrDefault()));
        }

        /// <summary>
        /// Executes the Stored Procedure that checks login credentials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string userName,string passwordHash)
        {
            var data = _connection.Query("execute sp_LoginForUser @userName,@password",
                new
                {
                    @userName = userName,
                    @password = passwordHash
                });

            return MapUser(data.SingleOrDefault());
        }

        public List<Message> GetNotifications(int userId)
        {
            var data = _connection.Query("execute sp_CheckNotifications @ID",
                new
                {
                    ID = userId
                });

            var notifications = new List<Message>();
            foreach(var row in data)
            {
                notifications.Add(new Message
                {
                    Id = row.ID,
                    ChannelID = row.ChannelID,
                    Content = row.Content,
                    TypeId = row.TypeID,
                    UserID = row.UserID
                });
            }
            return notifications;
        }
    }
}
