using Entites.Model;

namespace HRMSAPI.Repository.Interfaces
{
    public interface IHolidayRepository
    {
        Task<List<Holiday>> GetAll();

        Task<Holiday?> GetById(int id);

        Task<Holiday> Create(Holiday holiday);

        Task<Holiday> Update(Holiday holiday);

        Task<bool> Delete(int id);

        Task<bool> Exists(DateTime date);
    }
}