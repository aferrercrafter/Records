using BallastLane.Application.DTO;
using BallastLane.Application.Interfaces;
using BallastLane.Domain.Entities;
using FluentValidation;

namespace BallastLane.Application.Services.Database;

public class RecordService : IRecordService
{
    private readonly IRecordRepository _recordRepository;
    private readonly IValidator<Record> _validator;

    public RecordService(IRecordRepository recordRepository, IValidator<Record> validator)
    {
        _recordRepository = recordRepository;
        _validator = validator;
    }

    public async Task<Record> GetByIdAsync(int id, int userId)
    {
        return await _recordRepository.GetByIdAsync(id, userId);
    }

    public async Task<IEnumerable<Record>> GetAllByUserIdAsync(int userId)
    {
        return await _recordRepository.GetAllByUserIdAsync(userId);
    }

    public async Task<Record> CreateAsync(RecordCreationModel recordDto, int userId)
    {
        var record = new Record
        {
            Title = recordDto.Title,
            Description = recordDto.Description,
            UserId = userId,
        };

        var validationResult = _validator.Validate(record);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        record.CreatedAt = DateTime.UtcNow;
        return await _recordRepository.CreateAsync(record);
    }

    public async Task UpdateAsync(Record record, int userId)
    {
        var existingRecord = await _recordRepository.GetByIdAsync(record.Id, userId);
        if (existingRecord == null)
        {
            throw new ArgumentException("Record not found or not owned by the user.");
        }

        existingRecord.Title = record.Title;
        existingRecord.Description = record.Description;
        await _recordRepository.UpdateAsync(existingRecord);
    }

    public async Task DeleteAsync(int id, int userId)
    {
        var record = await _recordRepository.GetByIdAsync(id, userId);
        if (record == null)
        {
            throw new ArgumentException("Record not found or not owned by the user.");
        }

        await _recordRepository.DeleteAsync(record);
    }
}
