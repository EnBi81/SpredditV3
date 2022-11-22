
namespace SharedDomain.DTOs;

public class CommentToSendDTO
{
    public string Comment { get;}
    public string Username { get;}
    public int PostId { get; }

    public CommentToSendDTO(string comment,string username, int postId)
    {
        Comment = comment;
        Username = username;
        PostId = postId;
    }
    
}