using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedDomain.Models;

public class Post
{
    [Required]
    public string Title { get; set;  }
    [Required]
    public string Body { get; set;  }

    
    [ForeignKey(nameof(User))]
    public string CreatedBy { get; set; }
    public User User { get; set; }
    
    public int Id { get; set; }
    public ICollection<Comment> Comments { get; set; }
    
    public ICollection<Vote> Votes { get; set; }

    public int VoteScore
    {
        get
        {
            if (Votes == null)
                return 0;
            
            return Votes.Any() ? Votes.Select(vote => (int)vote.Type).Sum() : 0;
        }
    }


    public Post()
    {
        Votes = new List<Vote>();
        Comments = new List<Comment>();
    }

    public Post(string title, string body, User user)
    {
        Title = title;
        Body = body;
        User = user;
        CreatedBy = user.Username;
        Votes = new List<Vote>();
        Comments = new List<Comment>();
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

}