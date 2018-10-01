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

namespace BList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Person> people = new List<Person>();
        private static string dir = @"c:\blacklist\";
        private string serializationFile = System.IO.Path.Combine(dir, "blacklist.bin");

        public MainWindow()
        {
            if (File.Exists(serializationFile))
            {
                OpenFile();
            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(dir);
            }
            InitializeComponent();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            window.Show();
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1();
            OpenFile();
            win2.populateList(people);
            win2.Show();

        }

        private void Blacklist_Click(object sender, RoutedEventArgs e)
        {
            if ((string.IsNullOrEmpty(textFirstName.Text)) || (string.IsNullOrEmpty(textLastName.Text)) || (string.IsNullOrEmpty(textPhoneNumber.Text)) || (string.IsNullOrEmpty(textBlacklist.Text)) || (!Regex.IsMatch(textPhoneNumber.Text, @"^[0-9]{10}$")))
            {
                MessageBox.Show("Please check to see if the inputs are empty. Ensure that the phone number follows a format of: 5550001234");
            }
            else
            {
                Person result = people.Find(person => person.FirstName.Equals(textFirstName.Text) && person.LastName.Equals(textLastName.Text) && person.PhoneNumber.Equals(textPhoneNumber.Text));
                if (result == null)
                {

                    //Not Duplicate
                    Person person = new Person
                    {
                        FirstName = textFirstName.Text,
                        LastName = textLastName.Text,
                        PhoneNumber = textPhoneNumber.Text,
                        Reason = textBlacklist.Text
                    };
                    people.Add(person);
                    MessageBox.Show(textFirstName.Text + " " + textLastName.Text + " was successfully blacklisted.");
                    textFirstName.Text = String.Empty;
                    textLastName.Text = String.Empty;
                    textPhoneNumber.Text = String.Empty;
                    textBlacklist.Text = String.Empty;
                    WriteFile();
                }
                else
                {
                    //Dupe
                    MessageBox.Show("That person is already blacklisted!");
                }
            }

        }

        private void WriteFile()
        {
            //serialize
            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, people);
                stream.Close();
            }
        }

        private void OpenFile()
        {
            //deserialize
            using (Stream stream = File.Open(serializationFile, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                people = (List<Person>)bformatter.Deserialize(stream);
                stream.Close();
            }
        }
    }
}
