using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vesuvius.Models;
using System.Threading;

namespace Vesuvius.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //channel that the user is viewing
        public Channel selectedChannel;

        private ManualResetEvent manualResetEvent;

        const int PollingInterval = 2000;

        /// <summary>
        /// Starting up the main client window
        /// </summary>
        /// <param name="SessionContext"></param>
        public MainWindow()
        {
            InitializeComponent();
            manualResetEvent = new ManualResetEvent(false);

            AppContext.Current.ChannelButtons = new Dictionary<int, CustomControls.ChannelButton>();
            txtBlockUserName.Text = AppContext.Current.SessionContext.User.Alias;
            iconPanelRequests.UserNotificationVisibility = Visibility.Hidden;
            iconPanelRequests.ChannelNotificationVisibility = Visibility.Hidden;

            if (AppContext.Current.SessionContext.Channels != null)
            {
                foreach (var channel in AppContext.Current.SessionContext.Channels)
                {
                    var cb = new Vesuvius.UI.CustomControls.ChannelButton
                    {
                        NotificationsVisible = Visibility.Hidden,
                        ChannelName = channel.Name,
                        Margin = new Thickness(25,10,25,10)
                    };
                    cb.Tag = channel;
                    cb.CustomClick += SwapChannelView;
                    stackPanelChannels.Children.Add(cb);
                    AppContext.Current.ChannelButtons.Add(channel.Id, cb);
                }
                selectedChannel = AppContext.Current.SessionContext.Channels.FirstOrDefault();
                txtBlockChannelTitle.Text = selectedChannel.Name;
            }

            Task.Run(new Action(ProcessNotifications));
        }

        public delegate void NotifyUserDelegate(List<Message> messages);

        /// <summary>
        /// Background thread Checking Every 2 Seconds for any new notifications
        /// </summary>
        private void ProcessNotifications()
        {
            while (!manualResetEvent.WaitOne(PollingInterval, true))
            {
                var newMessages = AppContext.Current.SessionContext.GetNotifications();
                if(newMessages != null && newMessages.Any())
                {
                    this.Dispatcher.Invoke(new NotifyUserDelegate(NotifyUser),newMessages);
                }
            }
        }

        /// <summary>
        /// Creates a Noitifcation icon popup if there is a notification
        /// for a specific channel
        /// </summary>
        /// <param name="newMessages"></param>
        public void NotifyUser(List<Message> newMessages)
        {
            var channelButtons = stackPanelChannels.Children.OfType<CustomControls.ChannelButton>();
            foreach (var msg in newMessages)
            {
                msg.User = AppContext.Current.SessionContext.Users.Where(p => p.Id == msg.UserID).Single();
                AppContext.Current.SessionContext.Channels.Where(p => p.Id == msg.ChannelID).Single().Messages.Add(msg);
                foreach (var btn in channelButtons)
                {
                    if (((Channel)btn.Tag).Id == msg.ChannelID)
                    {
                        btn.NotificationCount++;
                        //AppContext.Current.ChannelButtons.Where(p => p.Key == msg.ChannelID).Single().Value.NotificationCount++;
                        btn.NotificationsVisible = Visibility.Visible;
                    }
                    btn.Refresh();
                }

            }
        }

        private void SwapChannelView(object sender, RoutedEventArgs e)
        {
            stackPanelMessages.Children.RemoveRange(1, stackPanelMessages.Children.Count);
            stackPanelUsers.Children.RemoveRange(1, stackPanelUsers.Children.Count);

            var channelButton = ((CustomControls.ChannelButton)sender);
            var channelSelected = channelButton.Tag as Channel;
            channelButton.NotificationCount = 0;
            channelButton.NotificationsVisible = Visibility.Hidden;

            selectedChannel = AppContext.Current.SessionContext.Channels.Where(p => p.Id == channelSelected.Id).SingleOrDefault();

            //Populating the Content of the Channel Title lbl
            txtBlockChannelTitle.Text  = selectedChannel.Name;
            scrollViewMessages.ScrollToBottom();

            foreach(var user in selectedChannel.Users)
            {
                stackPanelUsers.Children.Add(new CustomControls.UserButton
                {
                   UserName = user.Alias
                });            
            }

            //If The Active user had sent the message prior or not determines label color

            var previousMessage = selectedChannel.Messages.First();
            var lastSeenDate = previousMessage.TimeStamp;
            var initialBanner = new CustomControls.MessageBanner(lastSeenDate);
            stackPanelMessages.Children.Add(initialBanner);

            foreach(var msg in selectedChannel.Messages)
            {
                AddMessageToStackPanel(msg, ref lastSeenDate, previousMessage.UserID);
            }
        }

        /// <summary>
        /// Adding messages to the Stackpanel.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lastSeenDate"></param>
        /// <param name="previousMessageUserId"></param>
        public void AddMessageToStackPanel(Message msg,ref DateTime lastSeenDate,int previousMessageUserId)
        {
            // If the comment previous has an earlier date of origin
            if (msg.TimeStamp.Date > lastSeenDate.Date)
            {
                var banner = new CustomControls.MessageBanner(msg.TimeStamp);
                lastSeenDate = msg.TimeStamp.Date;
                stackPanelMessages.Children.Add(banner);
            }
            MessageContainer lastMessage = new MessageContainer();
            try
            {
                lastMessage = (MessageContainer)stackPanelMessages.Children[stackPanelMessages.Children.Count - 1];
            }
            catch { }
            // If UserId of last Message in stackpanel is == to this message UserID and The last Child was not a Banner
            if (previousMessageUserId == msg.UserID && stackPanelMessages.Children[stackPanelMessages.Children.Count -1].GetType() != typeof(CustomControls.MessageBanner))
            {
                // Adding a Message to the stackpanel without a label
                lastMessage.AddTextToMessage(msg.Content);
            }
            else
            {
                AddContainerToStackPanel(msg);
            }
        }

        /// <summary>
        /// Adds the Message Container to the stackpanel depending on who the sender is
        /// </summary>
        /// <param name="msg"></param>
        public void AddContainerToStackPanel(Message msg)
        {
            // If ActiveUser is not the person who sent this message
            if (msg.User.Id != AppContext.Current.SessionContext.User.Id)
            {
                // Adding a Message to the stackpanel with the sender coloring on label
                stackPanelMessages.Children.Add(CreateMessageContainer(false, true, msg));
            }
            else
            {
                // Adding a Message to the stackpanel with the not sender coloring on label
                stackPanelMessages.Children.Add(CreateMessageContainer(true, true, msg));
            }
        }

        /// <summary>
        /// Deciding he design of MessageContainer being added to the Stackpanel
        /// </summary>
        /// <param name="isActiveUser"></param>
        /// <param name="userNameVisible"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private MessageContainer CreateMessageContainer(bool isActiveUser, bool userNameVisible, Message msg)
        {
            var message = new MessageContainer(isActiveUser, userNameVisible)
            {
                Sender = msg.User.Alias,
                Message = msg.Content,
                Margin = new Thickness(0,0,0,15)
            };
            return message;
        }

        /// <summary>
        /// User presses button to create a new server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewServer_Click(object sender, RoutedEventArgs e)
        {
            var newServer = new ChannelCreation();
            newServer.Show();
            
        }

        /// <summary>
        /// When the User presses enter within the Message Textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            //When Enter is Pressed
            if (e.Key == Key.Return)
            {
                var message = new Message()
                {
                    ChannelID = selectedChannel.Id,
                    Content = txtBoxMessage.Text,
                    UserID = AppContext.Current.SessionContext.User.Id,
                    TypeId = 0,
                    User = AppContext.Current.SessionContext.User
                };

                AppContext.Current.SessionContext.AddMessage(message);

                AddContainerToStackPanel(message);
            }  
        }
    }
}
