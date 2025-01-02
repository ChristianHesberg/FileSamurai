using application.dtos;
using application.ports;
using application.services;
using FluentValidation;
using infrastructure.adapters;
using Moq;

namespace unit_test;

public class FileServiceTests
{
    private IFileService fileService;
    private Mock<IFilePort> fileRepo;
    private Mock<IValidator<AddFileDto>> addFileValidator;
    private Mock<IValidator<GetFileOrAccessInputDto>> getFileOrAccessInputDtoValidator;
    private Mock<IValidator<FileDto>> fileDtoValidator;
    private Mock<IValidator<AddOrGetUserFileAccessDto>> addOrGetUserFileAccessDtoValidator;
    
    public FileServiceTests(){
        fileRepo = new Mock<IFilePort>();
        addFileValidator = new Mock<IValidator<AddFileDto>>();
        getFileOrAccessInputDtoValidator = new Mock<IValidator<GetFileOrAccessInputDto>>();
        fileDtoValidator = new Mock<IValidator<FileDto>>();
        addOrGetUserFileAccessDtoValidator = new Mock<IValidator<AddOrGetUserFileAccessDto>>();
        fileService = new FileService(
            fileRepo.Object,
            addFileValidator.Object,
            getFileOrAccessInputDtoValidator.Object,
            fileDtoValidator.Object,
            addOrGetUserFileAccessDtoValidator.Object
        );
    }
    
    [Theory]
    [MemberData(nameof(AddFile_ThrowsValidationException_Data))]
    public void AddFile_ThrowsValidationException(DateTime start, DateTime end)
    {
        // Act
        Action act = () => bookingManager.FindAvailableRoom(start, end);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    public static IEnumerable<object[]> AddFile_ThrowsValidationException_Data()
    {
        var data = new List<object[]>
        {
            new object[] { DateTime.Today, DateTime.Today },
            new object[] { DateTime.Today.AddDays(-1), DateTime.Today },
            new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(-1) },
        };
        return data;
    }
    
    public AddFileDtoValidator()
    {
        RuleFor(x => x.FileContents).NotEmpty();
        RuleFor(x => x.Nonce).NotEmpty();
        RuleFor(x => x.Nonce).MaximumLength(30);
        RuleFor(x => x.Tag).NotEmpty();
        RuleFor(x => x.Tag).MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(50);
        RuleFor(x => x.GroupId).NotEmpty();
        RuleFor(x => x.GroupId).MaximumLength(36);
    }
}