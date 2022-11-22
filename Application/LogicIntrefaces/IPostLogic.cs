using SharedDomain.DTOs;
using SharedDomain.Models;

namespace Application.LogicIntrefaces;

public interface IPostLogic
{
    public Task<Post> CreateAsync(PostCreationDTO dto);
    Task<IEnumerable<Post>> GetByParameterAsync(PostFilterDTO dto);
    public Task <IEnumerable<Post>> GetByUserAsync(string username);
    public Task<Post?> GetByIdAsync(int id);
}