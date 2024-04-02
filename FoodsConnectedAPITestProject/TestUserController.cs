using FoodsConnectedAPI.Controllers;
using FoodsConnectedAPI.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodsConnectedAPITestProject
{
    /// <summary>
    /// Test functionality of user controller during 
    /// expected & unexpected scenarios 
    /// </summary>
    [TestClass]
    public class TestUserController
    {
        /// <summary>
        /// Add new user to DB with valid data 
        /// </summary>
        [TestMethod]
        public async Task AddUser_ShouldPass()
        {
            // arrange
            User testUser = new User() { Id = 0, Username = "Test1"};
            UserController userController = new UserController(new TestUserRepository());
            // act
            ActionResult<User> result = await userController.Post(testUser);
            // assert
            Assert.IsInstanceOfType<OkObjectResult>(result.Result);
        }

        /// <summary>
        /// Add new user to DB with an username that exists in DB 
        /// </summary>
        [TestMethod]
        public async Task AddUserWithExistingUsername_ShouldFail()
        {
            // arrange
            List<User> users = GetTestUsers();
            User testUser = new User() { Id = 0, Username = "Peter" };
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User> result = await userController.Post(testUser);
            // assert
            Assert.IsInstanceOfType<ConflictObjectResult>(result.Result);
        }

        /// <summary>
        /// Add null record into DB
        /// </summary>
        [TestMethod]
        public async Task AddUserNullObject_ShouldFail()
        {
            // arrange
            UserController userController = new UserController(new TestUserRepository());
            // act
            ActionResult<User> result = await userController.Post(null);
            BadRequestResult? badRequestResult = result.Result as BadRequestResult;
            // assert
            Assert.IsNotNull(badRequestResult);
        }

        /// <summary>
        /// Get all users in DB ordered by username
        /// Check all users are returned & they are ordered by username
        /// </summary>
        [TestMethod]
        public async Task GetUser_ShouldPass()
        {
            // arrange
            List<User> users = GetTestUsers();
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User> result = await userController.GetUsers();
            OkObjectResult? okObjectResult = result.Result as OkObjectResult;
            List<User> resultUsers =  okObjectResult?.Value as List<User> ?? new List<User>();
            // assert
            CollectionAssert.AreNotEqual(resultUsers, users);
            CollectionAssert.AreEqual(resultUsers, users.OrderBy(s =>s.Username).ToList());
        }

        /// <summary>
        /// Edit username of existing user record in DB
        /// </summary>
        [TestMethod]
        public async Task EditUser_ShouldPass()
        {
            // arrange
            List<User> users = GetTestUsers();
            User testEditUser = new User() { Id = 1, Username = "PeterChange" };
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User> result = await userController.Put(1, testEditUser);
            ActionResult<User> getUsersResult = await userController.GetUsers();
            OkObjectResult? okObjectResult = getUsersResult.Result as OkObjectResult;

            IEnumerable<User>? resultUsers = okObjectResult?.Value as IEnumerable<User>;
            User? updatedUser = resultUsers?.FirstOrDefault(s => s.Id == 1 && s.Username == "PeterChange");

            // assert
            Assert.IsNotNull(updatedUser);
        }

        /// <summary>
        /// Check ID of username being edited & username supplied in object to edit
        /// </summary>
        [TestMethod]
        public async Task EditUser_IDMismatch_ShouldFail()
        {
            // arrange
            List<User> users = GetTestUsers();
            User testEditUser = new User() { Id = 2, Username = "PeterChange" };
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User> result = await userController.Put(1, testEditUser);
            // assert
            Assert.IsInstanceOfType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Edit user that doesn't exist in DB
        /// </summary>
        [TestMethod]
        public async Task EditUser_UserNotExist_ShouldFail()
        {
            // arrange
            List<User> users = GetTestUsers();
            User testEditUser = new User() { Id = 1000, Username = "PeterChange" };
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User> result = await userController.Put(1000, testEditUser);
            // assert
            Assert.IsInstanceOfType<NotFoundObjectResult>(result.Result);
        }

        /// <summary>
        /// Edit user username to one that already exists in DB
        /// </summary>
        [TestMethod]
        public async Task EditUser_UsernameExist_ShouldFail()
        {
            // arrange
            List<User> users = GetTestUsers();
            User testEditUser = new User() { Id = 1, Username = "Adam" };
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User> result = await userController.Put(1, testEditUser);
            // assert
            Assert.IsInstanceOfType<ConflictObjectResult>(result.Result);
        }

        /// <summary>
        /// Delete user in DB with valid ID
        /// </summary>
        [TestMethod]
        public async Task DeleteUser_ShouldPass()
        {
            // arrange
            List<User> users = GetTestUsers();
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User?> result = await userController.Delete(1);
            // assert
            Assert.IsInstanceOfType<OkObjectResult>(result.Result);
        }

        /// <summary>
        /// Delete user that doesn't exist in DB
        /// </summary>
        [TestMethod]
        public async Task DeleteUser_NotExist_ShouldFail()
        {
            // arrange
            List<User> users = GetTestUsers();
            UserController userController = new UserController(new TestUserRepository(users));
            // act
            ActionResult<User?> result = await userController.Delete(1000);

            // assert
            Assert.IsInstanceOfType<NotFoundObjectResult>(result.Result);
        }

        /// <summary>
        /// Create & return list of users with valid data for testing purposes
        /// </summary>
        /// <returns>List of users</returns>
        private List<User> GetTestUsers()
        {
            List<User> users = new List<User>();
            users.Add(new User() { Id = 0, Username = "Peter" });
            users.Add(new User() { Id = 0, Username = "Adam" });
            users.Add(new User() { Id = 0, Username = "James" });

            return users;
        }
    }
}
