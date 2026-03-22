using Azure.Messaging;
using ChatStudents_Kazakov.Classes;
using ChatStudents_Kazakov.Classes.Common;
using ChatStudents_Kazakov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ChatStudents_Kazakov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public DispatcherTimer _timer;
        public Main()
        {
            InitializeComponent();
            LoadUsers();
            InitTimer();
        }

        public void InitTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += Timer_TIck;
            _timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (SelectedUser != null)
                SelectUser(SelectedUser);
        }

        public void LoadUsers()
        {
            foreach (Users user in UsersContext.Users) 
            {
                if (user.Id != MainWindow.Instance.LoginUser.Id)
                    ParentUsers.Children.Add(new Pages.Items.Users(user, this));)
            }
        }

        public void SelectUser(Users User)
        {
            SelectedUser = User;
            Chat.Visibility = Visibility.Visible;
            imgUser.Source = BitmapFromArrayByte.LoadImage(User.Photo);
            FIO.Content = User.ToFIO();
            ParentMessages.Children.Clear();
            foreach (Messages Message in MessageContent.Messages.Where(x =>
                (x.UserFrom == User.Id && x.UserTo == MainWindow.Instance.LoginUser.Id) ||
                (x.UserFrom == MainWindow.Instance.LoginUser.Id && x.UserTo == User.Id)))
            {
                ParentMessages.Children.Add(new Pages.Items.Message(Message, UsersContext.Users.Where(x => x.Id == Message.UserFrom).First()));
            }
        }

        private void Send(object sender, EventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Messages message = new Messages(
                    SelectedUser.Id,
                    Message.Text);
                MessagesContext.Messages.Add(message);
                MessagesContext.SaveChanges();
                ParentMessages.Children.Add(new Pages.Items.Message(message, MainWindow.Instance.LoginUser));
                Message.Text = "";
            }
        }
    }
}
