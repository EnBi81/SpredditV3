using SharedDomain.DTOs;
using SharedDomain.Models;

namespace Application.DaoInterfaces
{
    public interface IPostDao
    {
        public Task<Post> CreateAsync(Post post);
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetByParameterAsync(PostFilterDTO dto);
        public Task <IEnumerable<Post>> GetByUserAsync(string username);
    }
}