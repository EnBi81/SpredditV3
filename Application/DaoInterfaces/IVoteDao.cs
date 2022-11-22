using SharedDomain.Models;

namespace Application.DaoInterfaces;

public interface IVoteDao
{
    public Task VoteAsync(int postId, Vote vote);
    
    public Task<int> GetNumberOfUpVote(int id);
    public Task<int> GetNumberOrDownVote(int id);
}