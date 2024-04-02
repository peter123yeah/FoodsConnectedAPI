using FoodsConnectedAPI.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace FoodsConnectedAPITestProject
{
    /// <summary>
    /// Test User RestAPI and DB connections
    /// </summary>
    [TestClass]
    public class TestUserRestAPI
    {
        static Random rd = new Random();

        /// <summary>
        /// Test add user API and DB connections
        /// </summary>
        [TestMethod]
        public void Test1_AddUsersAPIConnection_ShouldPass()
        {
            // arrange
            User user = new User() {Id = 0, Username = CreateString(8) };
            RestClient client = new RestClient("https://localhost:7140/");
            RestRequest request = new RestRequest("User", Method.Post)
            {
                RequestFormat = DataFormat.Json
            }; 
            string jsonToSend = JsonConvert.SerializeObject(user);

            request.AddBody(jsonToSend);

            // act
            RestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, response.Content);
        }

        /// <summary>
        /// Test edit user API and DB connections
        /// </summary>
        [TestMethod]
        public void Test2_UpdateUsersAPIConnection_ShouldPass()
        {
            // arrange
            User user = new User() { Id = 1, Username = CreateString(7) };
            RestClient client = new RestClient("https://localhost:7140/");
            RestRequest request = new RestRequest($"User/1", Method.Put)
            {
                RequestFormat = DataFormat.Json
            };
            string jsonToSend = JsonConvert.SerializeObject(user);

            request.AddBody(jsonToSend);

            // act
            RestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, response.Content);
        }

        /// <summary>
        /// Test get user API and DB connections
        /// </summary>
        [TestMethod]
        public void Test3_GetAllUsersAPIConnection_ShouldPass()
        {
            // arrange
            RestClient client = new RestClient("https://localhost:7140/");
            RestRequest request = new RestRequest("User", Method.Get);

            // act
            RestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        /// <summary>
        /// Test delete user API and DB connections
        /// </summary>
        [TestMethod]
        public void Test4_DeleteUsersAPIConnection_ShouldPass()
        {
            // arrange
            RestClient client = new RestClient("https://localhost:7140/");
            RestRequest request = new RestRequest($"User/1", Method.Delete);
            
            // act
            RestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, response.Content);
        }

        internal static string CreateString(int stringLength)
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}