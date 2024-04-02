using FoodsConnectedAPI.Data.Models;
using FoodsConnectedAPI.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FoodsConnectedAPI.Controllers
{
    /// <summary>
    /// Web API for User functions  
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Interface of user repository to interact with user DB
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor with supplied user repository
        /// </summary>
        /// <param name="userRepository">Concrete implementation of user repository</param>
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get all users in DB 
        /// </summary>
        /// <returns>ActionResult containing user list</returns>
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                return Ok(await _userRepository.GetUsers());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Delete user based on ID
        /// </summary>
        /// <param name="id">ID of user to delete</param>
        /// <returns>ActionResult containing deleted user</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<User?>> Delete(int id)
        {
            try
            {
                User? userToDelete = await _userRepository.GetUserByID(id);

                if (userToDelete == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }

                return Ok(await _userRepository.DeleteUser(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        /// <summary>
        /// Add User to DB
        /// </summary>
        /// <param name="user">User to add to DB</param>
        /// <returns>ActionResult containing added user</returns>
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var existingUser = await _userRepository.GetUserByUsername(user.Username);

                if (existingUser != null)
                {
                    return Conflict("Username already in use");
                }

                return Ok(await _userRepository.AddUser(user));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        /// <summary>
        /// Edit existing user in DB
        /// </summary>
        /// <param name="id">Id of user to update</param>
        /// <param name="user">User to update</param>
        /// <returns> ActionResult containing edited User</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<User>> Put(int id, User user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest("User ID mismatch");
                }

                User? userToUpdate = await _userRepository.GetUserByID(id);

                if (userToUpdate == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }

                User? existingUserWithUsername = await _userRepository.GetUserByUsername(user.Username);

                if (existingUserWithUsername != null && existingUserWithUsername != userToUpdate)
                {
                    return Conflict("Username already in use");
                }

                return Ok(await _userRepository.UpdateUser(user));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }
    }
}
