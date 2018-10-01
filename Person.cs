using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BList
{
    [Serializable]
    class Person
    {
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string reason;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                Regex regex = new Regex(@"^\d{10}$");
                if (regex.IsMatch(value))
                {
                    phoneNumber = value;
                }
                else
                {
                    phoneNumber = "0000000000";
                }
            }
        }

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        public override string ToString()
        {
            return "" + firstName + " " + lastName + " was blacklisted because " + reason +" Phone number: "+phoneNumber;
        }
    }
}
