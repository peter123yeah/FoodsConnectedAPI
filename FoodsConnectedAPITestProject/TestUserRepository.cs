using FoodsConnectedAPI.Data;
using FoodsConnectedAPI.Data.Models;
using FoodsConnectedAPI.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodsConnectedAPITestProject
{
    /// <summary>
    /// Test User Repository used to mimic live repository
    /// Used for testing user controller and DB functionality
    /// </summary>
    public class TestUserRepository : IUserRepository
    {
        /// <summary>
        /// Store next Id for user record added to list of users
        /// Used to simulate auto increment in actual DB
        /// </summary>
        private int _nextId = 1;

        /// <summary>
        /// Store list of users 
        /// Used to simulate users table in actual DB
        /// </summary>
        private List<User> _users = new List<User>();

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public TestUserRepository() { 
        }

        /// <summary>
        /// Constructor with sample test data supplied
        /// </summary>
        /// <param name="testUsers">List of users supplied for testing</param>
        public TestUserRepository(List<User> testUsers)
        {
            foreach (User user in testUsers)
            {
                user.Id = _nextId++;
                _users.Add(user);
            }
        }

        /// <summary>
        /// Add new user to DB
        /// </summary>
        /// <param name="user">User to add to DB</param>
        /// <returns>User added to DB</returns>
        public Task<User> AddUser(User user)
        {
            user.Id = _nextId++;
            _users.Add(user);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Delete existing user from DB 
        /// </summary>
        /// <param name="userId">ID of user to delete from DB</param>
        /// <returns>User deleted from DB</returns>
        public Task<User?> DeleteUser(int userId)
        {
            User? user = _users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                _users.Remove(user);
            }
            return Task.FromResult(user);
        }

        /// <summary>
        /// Get user from DB based on supplied user ID
        /// </summary>
        /// <param name="userId">ID of user to get from DB</param>
        /// <returns>User with ID supplied</returns>
        public Task<User?> GetUserByID(int userId)
        {
            return Task.FromResult(_users.FirstOrDefault(x => x.Id == userId));
        }

        /// <summary>
        /// Get user from DB based on supplied username
        /// </summary>
        /// <param name="username">Username of user to get from DB</param>
        /// <returns>User with username supplied</returns>
        public Task<User?> GetUserByUsername(string username)
        {
            User? user = _users.FirstOrDefault(e => e.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));
            return Task.FromResult(user);
        }

        /// <summary>
        /// Get all users currently in DB
        /// </summary>
        /// <returns>User list ordered by username</returns>
        public Task<List<User>> GetUsers()
        {
            return Task.FromResult(_users.OrderBy(s => s.Username).ToList());
        }

        /// <summary>
        /// Update existing record in DB
        /// </summary>
        /// <param name="user">User to edit to DB</param>
        /// <returns>Edited User</returns>
        public Task<User?> UpdateUser(User user)
        {
            int toReplaceIndex = _users.FindIndex(r => r.Id == user.Id);
            _users[toReplaceIndex] = user;
            return Task.FromResult(user ?? null);
        }
    }
}
