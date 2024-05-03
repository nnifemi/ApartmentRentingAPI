using Entities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentDAL
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(int id);
        Task UpdateAsync(int id, UserDTO dto);
        Task DeleteAsync(int id);
    }
}
