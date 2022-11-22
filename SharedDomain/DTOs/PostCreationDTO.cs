using SharedDomain.Models;

namespace SharedDomain.DTOs;

public class PostCreationDTO
{
    public string Title { get; }
    public string Body { get; }
    public String Username { get;  }

    public PostCreationDTO(string title, string body, String username)
    {
        Title = title;
        Body = body;
        Username = username;
    }
    
}