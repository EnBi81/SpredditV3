using SharedDomain.DTOs;

namespace Application.LogicIntrefaces;

/// <summary>
/// Logic interface for the voting system
/// </summary>
public interface IVoteLogic
{
    /// <summary>
    /// Upvotes a post.
    /// </summary>
    /// <param name="id">id of the post to upvote.</param>
    /// <param name="dto"></param>
    /// <returns>A task that represents the upvote.</returns>
    public Task UpVote(int postId, VoteDTO dto);
    
    /// <summary>
    /// Downvotes a post.
    /// </summary>
    /// <param name="postId">id of the post to downvote.</param>
    /// <param name="dto"></param>
    /// <returns>A task that represents the downvote.</returns>
    public Task DownVote(int postId, VoteDTO dto);

    public Task<int> GetNumberOfUpVote(int id);
    public Task<int> GetNumberOgDownVote(int id);
}