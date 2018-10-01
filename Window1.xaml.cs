using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace BList
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        List<Person> people;
        private static string dir = @"c:\blacklist\";
        private string serializationFile = System.IO.Path.Combine(dir, "blacklist.bin");

        public Window1()
        {
            InitializeComponent();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            window.Show();
            this.Close();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1();
            win2.Show();
            this.Close();
        }

        private void Blacklist_Search(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            bool fNameSet = !(string.IsNullOrEmpty(txtFirstName.Text));
            bool lNameSet = !(string.IsNullOrEmpty(txtLastName.Text));
            bool pNoSet = !(string.IsNullOrEmpty(txtPhoneNumber.Text));

            people = people.OrderBy(person => person.LastName).ThenBy(person => person.FirstName).ToList();

            if ((fNameSet) && (!lNameSet) && (!pNoSet))
            {
                //ONLY FIRST NAME SET
                List<Person> result = people.FindAll(person => person.FirstName.Equals(txtFirstName.Text));
                listbox1.ItemsSource = result;
            }
            else if ((!fNameSet) && (lNameSet) && (!pNoSet))
            {
                //ONLY LAST NAME SET
                List<Person> result = people.FindAll(person => person.LastName.Equals(txtLastName.Text));
                listbox1.ItemsSource = result;
            }
            else if ((!fNameSet) && (!lNameSet) && (pNoSet))
            {
                //ONLY PHONE NUMBER SET
                List<Person> result = people.FindAll(person => person.PhoneNumber.Equals(txtPhoneNumber.Text));
                listbox1.ItemsSource = result;
            }
            else if ((fNameSet) && (lNameSet) && (!pNoSet))
            {
                //ONLY FIRST NAME AND LAST NAME SET
                List<Person> result = people.FindAll(person => person.FirstName.Equals(txtFirstName.Text) && person.LastName.Equals(txtLastName.Text));
                listbox1.ItemsSource = result;
            }
            else if ((fNameSet) && (!lNameSet) && (pNoSet))
            {
                //ONLY FIRST NAME AND PHONE NUMBER SET
                List<Person> result = people.FindAll(person => person.FirstName.Equals(txtFirstName.Text) && person.PhoneNumber.Equals(txtPhoneNumber.Text));
                listbox1.ItemsSource = result;
            }
            else if ((!fNameSet) && (lNameSet) && (pNoSet))
            {
                //ONLY LAST NAME AND PHONE NUMBER SET
                List<Person> result = people.FindAll(person => person.LastName.Equals(txtLastName.Text) && person.PhoneNumber.Equals(txtPhoneNumber.Text));
                listbox1.ItemsSource = result;
            }
            else if ((fNameSet) && (lNameSet) && (pNoSet))
            {
                //Everything Set
                List<Person> result = people.FindAll(person => person.FirstName.Equals(txtFirstName.Text) && person.LastName.Equals(txtLastName.Text) && person.PhoneNumber.Equals(txtPhoneNumber.Text));
                listbox1.ItemsSource = result;
            }
            else
            {
                //Nothing is Set
                listbox1.ItemsSource = people;
            }
        }

        internal void populateList(List<Person> people2)
        {
            people = people2;
        }

        private  void Blacklist_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to delete this person from being blacklisted?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                people.Remove((Person)listbox1.SelectedItem);
                listbox1.ItemsSource = null;
                listbox1.ItemsSource = people;
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