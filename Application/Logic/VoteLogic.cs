using Application.DaoInterfaces;
using Application.LogicIntrefaces;
using SharedDomain.DTOs;
using SharedDomain.Models;

namespace Application.Logic;

/// <summary>
/// Vote logic implementation
/// </summary>
public class VoteLogic : IVoteLogic
{
    private readonly IUserDao userDao;
    private readonly IPostDao postDao;
    private readonly IVoteDao voteDao;

    public VoteLogic(IUserDao userDao, IPostDao postDao, IVoteDao voteDao)
    {
        this.userDao = userDao;
        this.postDao = postDao;
        this.voteDao = voteDao;
    }
    
    
    /// <inheritdoc/>
    public async Task UpVote(int postId, VoteDTO dto) => await VoteAsync(postId, dto, VoteType.UpVote);

    /// <inheritdoc/>
    public async Task DownVote(int postId, VoteDTO dto) => await VoteAsync(postId, dto, VoteType.DownVote);

    public async Task<int> GetNumberOfUpVote(int id)
    {
        return await voteDao.GetNumberOfUpVote(id);
    }

    public  async Task<int> GetNumberOgDownVote(int id)
    {
        return await voteDao.GetNumberOrDownVote(id);
    }

    /// <summary>
    /// Places a vote on a post
    /// </summary>
    /// <param name="postId">post to vote</param>
    /// <param name="dto">vote creation dto</param>
    /// <param name="type">type of the vote</param>
    private async Task VoteAsync(int postId, VoteDTO dto, VoteType type)
    {
        string? username = dto.Username;

        await CheckPostAndUser(postId, username);

        var vote = new Vote
        {
            Username = username!,
            Type = type,
            PostId = postId
        };

        await voteDao.VoteAsync(postId, vote);
    }


    /// <summary>
    /// Checks post and user if they are existing.
    /// </summary>
    /// <param name="postId">id of the post to check.</param>
    /// <param name="username">username of the user to check.</param>
    /// <exception cref="Exception">either if the user or the post does not exist.</exception>
    private async Task CheckPostAndUser(int postId, string? username)
    {
        // check if user exists
        if (string.IsNullOrEmpty(username) || await userDao.GetByUsernameAsync(username) == null)
            throw new Exception($"{username} does not exists.");

        Post? post = await postDao.GetByIdAsync(postId);

        // check if post exists
        if (post == null)
            throw new Exception($"Post with id {postId} does not exist.");
    }
}