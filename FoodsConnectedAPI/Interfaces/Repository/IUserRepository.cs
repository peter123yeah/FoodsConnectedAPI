using FoodsConnectedAPI.Data.Models;

namespace FoodsConnectedAPI.Interfaces.Repository
{
    /// <summary>
    /// Interface for User Repository
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get all users currently in DB
        /// </summary>
        /// <returns>User list ordered by username</returns>
        Task<List<User>> GetUsers();

        /// <summary>
        /// Get user from DB based on supplied user ID
        /// </summary>
        /// <param name="userId">ID of user to get from DB</param>
        /// <returns>User with ID supplied</returns>
        Task<User?> GetUserByID(int userId);

        /// <summary>
        /// Get user from DB based on supplied username
        /// </summary>
        /// <param name="username">Username of user to get from DB</param>
        /// <returns>User with username supplied</returns>
        Task<User?> GetUserByUsername(string username);

        /// <summary>
        /// Add new user to DB
        /// </summary>
        /// <param name="user">User to add to DB</param>
        /// <returns>User added to DB</returns>
        Task<User> AddUser(User user);

        /// <summary>
        /// Update existing record in DB
        /// </summary>
        /// <param name="user">User to edit to DB</param>
        /// <returns>Edited User</returns>
        Task<User?> UpdateUser(User user);

        /// <summary>
        /// Delete existing user from DB 
        /// </summary>
        /// <param name="userId">ID of user to delete from DB</param>
        /// <returns>User deleted from DB</returns>
        Task<User?> DeleteUser(int userId);
    }
}
