using BallastLane.Application.DTO;
using BallastLane.Domain.Entities;

namespace BallastLane.Application.Interfaces;

public interface IRecordService
{
    Task<Record> GetByIdAsync(int id, int userId);
    Task<IEnumerable<Record>> GetAllByUserIdAsync(int userId);
    Task<Record> CreateAsync(RecordCreationModel record, int userId);
    Task UpdateAsync(Record record, int userId);
    Task DeleteAsync(int id, int userId);
}
