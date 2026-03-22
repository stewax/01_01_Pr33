using ChatStudents_Kazakov.Classes;
using ChatStudents_Kazakov.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ChatStudents_Kazakov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public string srcUserImage = "";
        UsersContext usersContext = new UsersContext();
        public Login()
        {
            InitializeComponent();
        }

        private void SelectPhoto(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите фотографию:";
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                imgUser.Source = new BitmapImage(new Uri(ofd.FileName));
                srcUserImage = ofd.FileName;
            }
        }

        private bool CheckEmpty(string Pattern, string Input)
        {
            Match m = Regex.Match(Input, Pattern);
            return m.Success;
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            if (!CheckEmpty("^[А-ЯЁ][а-яА-ЯЁё]*$", Lastname.Text))
            {
                MessageBox.Show("Укажите фамилию.");
                return;
            }

            if (!CheckEmpty("^[А-ЯЁ][а-яА-ЯЁё]*$", Firstname.Text))
            {
                MessageBox.Show("Укажите имя.");
                return;
            }

            if (!CheckEmpty("^[А-ЯЁ][а-яА-ЯЁё]*$", Surname.Text))
            {
                MessageBox.Show("Укажите отчество.");
                return;
            }

            if (String.IsNullOrEmpty(srcUserImage))
            {
                MessageBox.Show("Выберите изображение.");
                return;
            }

            if (usersContext.Users.Where(x => x.Firstname == Firstname.Text &&
                                             x.Lastname == Lastname.Text &&
                                             x.Surname == Surname.Text).Count() > 0)
            {
                MainWindow.Instance.LoginUser = usersContext.Users.Where(x => x.Firstname == Firstname.Text &&
                                                                              x.Lastname == Lastname.Text &&
                                                                              x.Surname == Surname.Text).First();

                MainWindow.Instance.LoginUser.Photo = File.ReadAllBytes(srcUserImage);
                usersContext.SaveChanges();
            }
            else
            {
                usersContext.Users.Add(new Users(Lastname.Text, Firstname.Text, Surname.Text, File.ReadAllBytes(srcUserImage)));
                usersContext.SaveChanges();
                MainWindow.Instance.LoginUser = usersContext.Users.Where(x => x.Firstname == Firstname.Text &&
                                                                              x.Lastname == Lastname.Text &&
                                                                              x.Surname == Surname.Text).First();
            }

            MainWindow.Instance.OpenPages(new Pages.Main());
        }
    }
}
