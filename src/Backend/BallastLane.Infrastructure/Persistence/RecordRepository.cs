using BallastLane.Application.Interfaces;
using BallastLane.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BallastLane.Infrastructure.Persistence;

public class RecordRepository : IRecordRepository
{
    private readonly AppDBContext _dbContext;

    public RecordRepository(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Record> GetByIdAsync(int id, int userId)
    {
        return await _dbContext.Records.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
    }

    public async Task<IEnumerable<Record>> GetAllByUserIdAsync(int userId)
    {
        return await _dbContext.Records.Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<Record> CreateAsync(Record record)
    {
        var result = await _dbContext.Records.AddAsync(record);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task UpdateAsync(Record record)
    {
        _dbContext.Entry(record).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Record record)
    {
        _dbContext.Records.Remove(record);
        await _dbContext.SaveChangesAsync();
    }
}
