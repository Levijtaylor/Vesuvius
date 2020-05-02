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

            DateTime lastDate = selectedChannel.Messages.First().TimeStamp;
            stackPanelMessages.Children.Add(new CustomControls.MessageBanner(lastDate));

            foreach(var msg in selectedChannel.Messages)
            {
                if(msg.TimeStamp.Date > lastDate.Date)
                {
                    var banner = new CustomControls.MessageBanner(msg.TimeStamp);
                    lastDate = msg.TimeStamp.Date;
                    stackPanelMessages.Children.Add(banner);
                }
                
                if (msg.User.Id != AppContext.Current.SessionContext.User.Id)
                {
                    stackPanelMessages.Children.Add(new MessageContainer(false)
                    {
                        Sender = msg.User.Alias,
                        Message = msg.Content
                    });
                }
                else
                {
                    stackPanelMessages.Children.Add(new MessageContainer(true)
                    {
                        Sender = msg.User.Alias,
                        Message = msg.Content
                    });
                }
                
            }

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
                AppContext.Current.SessionContext.AddMessage(new Message
                {
                    ChannelID = selectedChannel.Id,
                    Content = txtBoxMessage.Text,
                    UserID = AppContext.Current.SessionContext.User.Id,
                    TypeId = 0,
                    User = AppContext.Current.SessionContext.User
                });
                
                stackPanelMessages.Children.Add(new MessageContainer(true)
                {
                    Sender = AppContext.Current.SessionContext.User.Alias,
                    Message = txtBoxMessage.Text
                });
            }  
        }
    }
}
