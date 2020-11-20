using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private decimal _balance;

        public User(int id, string firstName, string lastName, string userName, string email, decimal balance)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UserName = userName;
            this.Email = email;
            this._balance = balance;
        }

        /// <returns>Balance</returns>
        public decimal GetBalance()
        {
            return _balance;
        }

        /// <summary>
        /// Add to balance, can either be + or -
        /// </summary>
        /// <param name="amount">Can be + or -</param>
        public void AddBalance(decimal amount)
        {
            _balance += amount;
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
    }
}
