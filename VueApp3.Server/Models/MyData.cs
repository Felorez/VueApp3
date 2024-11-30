
using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Models
{
    public class MyData
    {
        [Key]
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserDescription { get; set; }
    }
}