using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesuvius.Models;

namespace Vesuvius.UI
{
    public class AppContext
    {
        private static AppContext _instance;

        public SessionContext SessionContext { get; set; }

        public Dictionary<int, CustomControls.ChannelButton> ChannelButtons { get; set; }

        // public WebContext WebContext { get; set; }

        /// <summary>
        /// SingleTon for ThreadSafe Access to Appcontext
        /// </summary>
        public static AppContext Current
        {
            get
            {
                if (_instance == null)
                    _instance = new AppContext();

                return _instance;
            }
        }

        //public delegate void NewNotification(Message message);
        public event EventHandler NewNotification;

        public void FireNewNotification(NewNotificationEventArgs e)
        {
            EventHandler handler = NewNotification;
            handler.Invoke(this, e);
        }
    }

    /// <summary>
    /// Event data class for gathering Notification data
    /// </summary>
    public class NewNotificationEventArgs : EventArgs
    {
        public List<Message> Messages { get; set; }
    }
}
