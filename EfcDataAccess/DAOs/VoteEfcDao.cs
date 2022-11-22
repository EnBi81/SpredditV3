using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SharedDomain.Models;

namespace EfcDataAccess.DAOs;

public class VoteEfcDao : IVoteDao
{
    private readonly SpredditContext context;

    public VoteEfcDao(SpredditContext context)
    {
        this.context = context;
    }
    public async Task VoteAsync(int postId, Vote vote)
    {
        var existingVote = await context.Votes.FirstOrDefaultAsync(v => v.PostId == vote.PostId && v.Username == vote.Username);

        if (existingVote is not null)
            existingVote.Type = vote.Type;
        else
            await context.Votes.AddAsync(vote);
        
        await context.SaveChangesAsync();
    }

    public async Task<int> GetNumberOfUpVote(int id)
    {
        var result = context.Votes
            .Where(v => v.PostId == id)
            .Where(v=>v.Type == VoteType.UpVote)
            .Select(v => (int)v.Type)
            .Sum();

        return result;
    }

    public async Task<int> GetNumberOrDownVote(int id)
    {
        var result = context.Votes
            .Where(v => v.PostId == id)
            .Where(v => v.Type == VoteType.DownVote)
            .Select(v => (int)v.Type)
            .Sum();
        return result;
    }
}