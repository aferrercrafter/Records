using BallastLane.Domain.Entities;

namespace BallastLane.Application.Interfaces;

public interface IRecordRepository
{
    Task<Record> GetByIdAsync(int id, int userId);
    Task<IEnumerable<Record>> GetAllByUserIdAsync(int userId);
    Task<Record> CreateAsync(Record record);
    Task UpdateAsync(Record record);
    Task DeleteAsync(Record record);
}
