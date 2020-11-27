using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stregsystem.ProgramFiles 
{
    class User : IComparable
    {
        public int ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string UserName { get; }
        public string Email { get; }
        public decimal Balance { get; private set; }

        private static int IDCounter = 0;

        public User(string firstName, string lastName, string userName, string email, decimal balance)
        {
            this.ID = IDCounter++;
            if (firstName != null)
                this.FirstName = firstName;
            if (lastName != null) 
                this.LastName = lastName;
            if (ValidateUsername(userName))
                this.UserName = userName;
            if (ValidateEmail(email))
                this.Email = email;
            this.Balance = balance / 100;

            if (firstName == null || lastName == null || userName == null || email == null)
                throw new BadInfomationException();
        }

        /// <summary>
        /// Add to balance, can either be + or -
        /// </summary>
        /// <param name="amount">Can be + or -</param>
        public void AddBalance(decimal amount)
        {
            Balance += amount;
        }

        /// <returns>Returns user data to string "FirstName LastName (Email)"</returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Email})";
        }

        /// <summary>
        /// Compare ID to another user's ID
        /// </summary>
        /// <param name="obj">Other user</param>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is User otherUser)
                return this.ID.CompareTo(otherUser.ID);
            else
                throw new ArgumentException("Object is not a User");
        }

        /// <returns>Returns user ID</returns>
        public override int GetHashCode()
        {
            return this.ID;
        }

        private bool ValidateUsername(string username)
        {
            if (Regex.IsMatch(username, "[^a-zA-Z0-9_]+"))
                return false;
            else
                return true;
        }

        private bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
