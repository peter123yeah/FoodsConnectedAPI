using FoodsConnectedAPI.Data.Models;
using FoodsConnectedAPI.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodsConnectedAPI.Data.Repository
{
    /// <summary>
    /// User Repository with functionality to interact with User table in DB
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Stored DB context
        /// </summary>
        private readonly AppDBContext appDbContext;

        /// <summary>
        /// Constructor with supplied DB context
        /// </summary>
        /// <param name="appDbContext">Reference to DB context</param>
        public UserRepository(AppDBContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Get all users currently in DB
        /// </summary>
        /// <returns>User list ordered by username</returns>
        public async Task<List<User>> GetUsers()
        {
            return await appDbContext.Users.OrderBy(s => s.Username).ToListAsync();
        }

        /// <summary>
        /// Get user from DB based on supplied user ID
        /// </summary>
        /// <param name="userId">ID of user to get from DB</param>
        /// <returns>User with ID supplied</returns>
        public async Task<User?> GetUserByID(int userId)
        {
            return await appDbContext.Users
                .FirstOrDefaultAsync(e => e.Id == userId);
        }

        /// <summary>
        /// Add new user to DB
        /// </summary>
        /// <param name="user">User to add to DB</param>
        /// <returns>User added to DB</returns>
        public async Task<User> AddUser(User User)
        {
            var result = await appDbContext.Users.AddAsync(User);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        /// <summary>
        /// Update existing record in DB
        /// </summary>
        /// <param name="user">User to edit to DB</param>
        /// <returns>Edited User</returns>
        public async Task<User?> UpdateUser(User User)
        {
            var result = await appDbContext.Users
                .FirstOrDefaultAsync(e => e.Id == User.Id);

            if (result != null)
            {
                appDbContext.Entry(result).CurrentValues.SetValues(User);
                await appDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }

        /// <summary>
        /// Delete existing user from DB 
        /// </summary>
        /// <param name="userId">ID of user to delete from DB</param>
        /// <returns>User deleted from DB</returns>
        public async Task<User?> DeleteUser(int userId)
        {
            var result = await appDbContext.Users
                .FirstOrDefaultAsync(e => e.Id == userId);
            if (result != null)
            {
                appDbContext.Users.Remove(result);
                await appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

        /// <summary>
        /// Get user from DB based on supplied username
        /// </summary>
        /// <param name="username">Username of user to get from DB</param>
        /// <returns>User with username supplied</returns>
        public async Task<User?> GetUserByUsername(string username)
        {
            return await appDbContext.Users
                .FirstOrDefaultAsync(e => e.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));

        }
    }
}
