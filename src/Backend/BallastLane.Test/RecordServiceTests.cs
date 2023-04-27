using BallastLane.Domain.Validation;
using FluentValidation;

namespace BallastLane.Test;

public class RecordServiceTests
{
    private readonly IRecordService _recordService;
    private readonly Mock<IRecordRepository> _recordRepositoryMock;
    private readonly IValidator<Domain.Entities.Record> _recordValidator;

    public RecordServiceTests()
    {
        _recordRepositoryMock = new Mock<IRecordRepository>();
        _recordValidator = new RecordValidator();
        _recordService = new RecordService(_recordRepositoryMock.Object, _recordValidator);
    }

    [Fact]
    public async Task RecordService_AddAsync_ShouldAddRecord()
    {
        // Arrange
        var recordDto = new RecordCreationModel { Title = "Test Title", Description = "Test Description" };
        var record = new Domain.Entities.Record { Id = 1, Title = "Test Title", Description = "Test Description", UserId = 1 };
        var userId = 1;

        _recordRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.Record>()))
            .ReturnsAsync(new Domain.Entities.Record { Id = 1, UserId = userId, Title = record.Title, Description = record.Description, CreatedAt = DateTime.UtcNow });


        // Act
        var result = await _recordService.CreateAsync(recordDto, userId);

        // Assert
        _recordRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Domain.Entities.Record>()), Times.Once());
        Assert.Equal(record.Id, result.Id);
        Assert.Equal(record.Title, result.Title);
        Assert.Equal(record.Description, result.Description);
        Assert.Equal(record.UserId, result.UserId);
    }

    [Fact]
    public async Task RecordService_AddAsync_ShouldThrowValidationException_WhenTitleIsEmpty()
    {
        // Arrange
        var recordDto = new RecordCreationModel { Title = "", Description = "Test Description" };
        var userId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _recordService.CreateAsync(recordDto, userId));
    }

    [Fact]
    public async Task RecordService_AddAsync_ShouldThrowValidationException_WhenDescriptionIsEmpty()
    {
        // Arrange
        var recordDto = new RecordCreationModel { Title = "Test Title", Description = "" };
        var userId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _recordService.CreateAsync(recordDto, userId));
    }

    [Fact]
    public async Task RecordService_AddAsync_ShouldThrowValidationException_WhenTitleExceedsMaximumLength()
    {
        // Arrange
        var recordDto = new RecordCreationModel { Title = new string('x', 501), Description = "Test Description" };
        var userId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _recordService.CreateAsync(recordDto, userId));
    }

    [Fact]
    public async Task RecordService_AddAsync_ShouldThrowValidationException_WhenDescriptionExceedsMaximumLength()
    {
        // Arrange
        var recordDto = new RecordCreationModel { Title = "Test Title", Description = new string('x', 501) };
        var userId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _recordService.CreateAsync(recordDto, userId));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRecord_WhenRecordExistsAndIsOwnedByUser()
    {
        // Arrange
        int userId = 1;
        int recordId = 1;
        _recordRepositoryMock
            .Setup(x => x.GetByIdAsync(recordId, userId))
            .ReturnsAsync(new Domain.Entities.Record { Id = recordId, UserId = userId, Title = "Test title", Description = "Test description" });

        // Act
        var result = await _recordService.GetByIdAsync(recordId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(recordId, result.Id);
        Assert.Equal(userId, result.UserId);
        _recordRepositoryMock.Verify(x => x.GetByIdAsync(recordId, userId), Times.Once);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_ShouldReturnRecords_WhenRecordsAreOwnedByUser()
    {
        // Arrange
        int userId = 1;
        var records = new List<Domain.Entities.Record>
{
    new Domain.Entities.Record { Id = 1, UserId = userId, Title = "Test title 1", Description = "Test description 1" },
    new Domain.Entities.Record { Id = 2, UserId = userId, Title = "Test title 2", Description = "Test description 2" }
};
        _recordRepositoryMock
            .Setup(x => x.GetAllByUserIdAsync(userId))
            .ReturnsAsync(records);

        // Act
        var result = await _recordService.GetAllByUserIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(records.Count, result.Count());
        _recordRepositoryMock.Verify(x => x.GetAllByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRecord_WhenRecordExistsAndIsOwnedByUser()
    {
        // Arrange
        int userId = 1;
        var record = new Domain.Entities.Record { Id = 1, UserId = userId, Title = "Test title", Description = "Test description" };
        _recordRepositoryMock
            .Setup(x => x.GetByIdAsync(record.Id, userId))
            .ReturnsAsync(record);

        // Act
        var updatedRecord = new Domain.Entities.Record { Id = 1, UserId = userId, Title = "Updated title", Description = "Updated description" };
        await _recordService.UpdateAsync(updatedRecord, userId);

        // Assert
        _recordRepositoryMock.Verify(x => x.GetByIdAsync(record.Id, userId), Times.Once);
        _recordRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Domain.Entities.Record>(r => r.Title == updatedRecord.Title && r.Description == updatedRecord.Description)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteRecord_WhenRecordExistsAndIsOwnedByUser()
    {
        // Arrange
        int userId = 1;
        int recordId = 1;
        var record = new Domain.Entities.Record { Id = recordId, UserId = userId, Title = "Test title", Description = "Test description" };
        _recordRepositoryMock
            .Setup(x => x.GetByIdAsync(recordId, userId))
            .ReturnsAsync(record);

        // Act
        await _recordService.DeleteAsync(recordId, userId);

        // Assert
        _recordRepositoryMock.Verify(x => x.GetByIdAsync(recordId, userId), Times.Once);
        _recordRepositoryMock.Verify(x => x.DeleteAsync(record), Times.Once);
    }
}
