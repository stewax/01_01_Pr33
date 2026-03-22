using ChatStudents_Kazakov.Classes;
using ChatStudents_Kazakov.Classes.Common;
using ChatStudents_Kazakov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChatStudents_Kazakov.Pages
{
    public partial class Main : Page
    {
        public DispatcherTimer _timer;

        public Users SelectedUser;

        private UsersContext _db = new UsersContext();

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
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (SelectedUser != null)
                SelectUser(SelectedUser);
        }

        public void LoadUsers()
        {
            ParentUsers.Children.Clear();

            foreach (Users user in _db.Users.ToList())
            {
                if (user.Id != MainWindow.Instance.LoginUser.Id)
                {
                    ParentUsers.Children.Add(new Pages.Items.User(user, this));
                }
            }
        }

        public void SelectUser(Users User)
        {
            SelectedUser = User;
            Chat.Visibility = Visibility.Visible;
            imgUser.Source = BitmapFromArrayByte.LoadImage(User.Photo);
            FIO.Content = User.ToFIO();

            ParentMessages.Children.Clear();

            var messages = _db.Messages.Where(x =>
                (x.UserFrom == User.Id && x.UserTo == MainWindow.Instance.LoginUser.Id) ||
                (x.UserFrom == MainWindow.Instance.LoginUser.Id && x.UserTo == User.Id)).ToList();

            foreach (Messages message in messages)
            {
                var senderUser = _db.Users.FirstOrDefault(x => x.Id == message.UserFrom);
                ParentMessages.Children.Add(new Pages.Items.Message(message, senderUser));
            }
        }

        private void Send(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SelectedUser != null && !string.IsNullOrWhiteSpace(Message.Text))
            {
                Messages newMessage = new Messages(
                    MainWindow.Instance.LoginUser.Id,
                    SelectedUser.Id,
                    Message.Text);

                _db.Messages.Add(newMessage);
                _db.SaveChanges();

                ParentMessages.Children.Add(new Pages.Items.Message(newMessage, MainWindow.Instance.LoginUser));
                Message.Text = "";
            }
        }
    }
}