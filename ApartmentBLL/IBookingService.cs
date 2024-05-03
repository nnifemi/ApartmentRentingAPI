using Entities.Model;
using Entities.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentDAL
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDTO>> GetAllAsync();
        Task<BookingDTO> GetByIdAsync(int id);
        Task AddAsync(BookingDTO dto);
        Task UpdateAsync(int id, BookingDTO dto);
        Task DeleteAsync(int id);
    }
}