using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FoodsConnectedAPI.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public required string Username { get; set; }
    }
}
