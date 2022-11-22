using Application.DaoInterfaces;
using SharedDomain.Models;

namespace FileData.DAOs;

public class VoteDao : IVoteDao
{
    private readonly FileContext fileContext;

    public VoteDao(FileContext fileContext)
    {
        this.fileContext = fileContext;
    }
    
    public Task VoteAsync(int postId, Vote vote)
    {
        Post post = fileContext.Posts.First(post => post.Id == postId);

        Vote? v = post.Votes.FirstOrDefault(v => v.Username == vote.Username);

        if (v is null)
            post.Votes.Add(vote);
        else
            v.Type = vote.Type;
        
        fileContext.SaveChange();
        
        return Task.CompletedTask;
    }

    public Task<int> GetNumberOfUpVote(int id)
    {
        int number = 0;
        
        Post? post = fileContext.Posts.FirstOrDefault(p => p.Id == id);
        List<Vote>? votes = new List<Vote>(post.Votes);

        for (int i = 0; i < votes.Count; i++)
        {
            if (votes[i].Type == VoteType.UpVote)
            {
                number++;
            }
        }

        return Task.FromResult(number);
    }

    public Task<int> GetNumberOrDownVote(int id)
    {
        int number = 0;
        
        Post? post = fileContext.Posts.FirstOrDefault(p => p.Id == id);
        List<Vote>? votes = new List<Vote>(post.Votes);

        for (int i = 0; i < votes.Count; i++)
        {
            if (votes[i].Type == VoteType.DownVote)
            {
                number++;
            }
        }

        return Task.FromResult(number);
    }
}