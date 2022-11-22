using SharedDomain.DTOs;
using SharedDomain.Models;

namespace HttpClients.ClientInterfaces;

public interface IPostService
{
    Task<IEnumerable<Post>> GetAsync(PostFilterDTO? filters = null);
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<IEnumerable<Post>> GetPostsByFiltering(string? username, string? titleContains);
    Task<Post> GetByIdAsync(int id);
    Task<Post> CreateAsync(PostCreationDTO dto);
    Task<int> GetNumberOfUpVotes(int id);
    Task<int> GetNumberOfDownVotes(int id);
    Task<IEnumerable<Comment>?> GetAllComments(int id);
    Task<Comment> CommentPost(CommentToSendDTO dto);
    Task UpVote(int id, VoteDTO dto);
    Task DownVote(int id, VoteDTO dto);
}