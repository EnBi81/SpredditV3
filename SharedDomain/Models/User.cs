using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace SharedDomain.Models;

public class User
{
    [MinLength(5)]
    [MaxLength(20)]
    public string Username { get; set; }
    public string Password { get; set; }
    //public ICollection<Post> Posts { get; set; }
    public DateTime DateOfBirth { get; set; }

    public User()
    {
    }
}