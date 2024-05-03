using Entities.Model;
using System.Threading.Tasks;

namespace ApartmentDAL
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}

