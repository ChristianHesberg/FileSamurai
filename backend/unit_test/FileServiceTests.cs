using application.dtos;
using application.errors;
using application.ports;
using application.services;
using core.models;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using File = core.models.File;

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
        var validationResult = new ValidationResult();  
        fileRepo = new Mock<IFilePort>();
        addFileValidator = new Mock<IValidator<AddFileDto>>();
        addFileValidator.Setup(v => v.Validate(It.IsAny<AddFileDto>())).Returns(validationResult);  
        getFileOrAccessInputDtoValidator = new Mock<IValidator<GetFileOrAccessInputDto>>();
        getFileOrAccessInputDtoValidator.Setup(v => v.Validate(It.IsAny<GetFileOrAccessInputDto>())).Returns(validationResult);  
        fileDtoValidator = new Mock<IValidator<FileDto>>();
        fileDtoValidator.Setup(v => v.Validate(It.IsAny<FileDto>())).Returns(validationResult);  
        addOrGetUserFileAccessDtoValidator = new Mock<IValidator<AddOrGetUserFileAccessDto>>();
        addOrGetUserFileAccessDtoValidator.Setup(v => v.Validate(It.IsAny<AddOrGetUserFileAccessDto>())).Returns(validationResult); 
        fileService = new FileService(
            fileRepo.Object,
            addFileValidator.Object,
            getFileOrAccessInputDtoValidator.Object,
            fileDtoValidator.Object,
            addOrGetUserFileAccessDtoValidator.Object
        );
    }

    #region AddFile
    [Fact]
    public void AddFile_ThrowsValidationException()
    {
        //Arrange
        var dto = new AddFileDto()
        {
            Title = "title"
        };
        var validationFailures = new List<ValidationFailure>  
        {  
            new ValidationFailure(dto.Title, "Title is required.")  
        };  
        var validationResult = new ValidationResult(validationFailures);  
  
        addFileValidator.Setup(v => v.Validate(It.IsAny<AddFileDto>())).Returns(validationResult);  
        
        // Act
        Action act = () => fileService.AddFile(dto);

        // Assert
        Assert.Throws<CustomValidationException>(act);
        addFileValidator.Verify(v => v.Validate(dto), Times.Once);
    }
    
    [Fact]
    public void AddFile_ReturnsPostFileResultDto()
    {
        //Arrange
        var inputDto = new AddFileDto()
        {
            FileContents = "FileContents",
            GroupId = new Guid().ToString(),
            Nonce = "Nonce",
            Title = "Title"
        };
        var fileReturnedFromMock = new File()
        {
            Id = "Id",
            FileContents = "FileContents",
            GroupId = new Guid().ToString(),
            Nonce = "Nonce",
            Title = "Title"
        };
        var expectedResult = new PostFileResultDto()
        {
            Id = fileReturnedFromMock.Id,
            Title = fileReturnedFromMock.Title
        };

        fileRepo.Setup(repo => repo.AddFile(It.IsAny<File>())).Returns(fileReturnedFromMock);
        
        //Act
        var res = fileService.AddFile(inputDto);
        
        //Assert
        Assert.Equivalent(res, expectedResult);
    }
    
    [Fact]
    public void AddFile_CallsRepoWithCorrectParameters()
    {
        //Arrange
        var inputDto = new AddFileDto()
        {
            FileContents = "FileContents",
            GroupId = new Guid().ToString(),
            Nonce = "Nonce",
            Title = "Title"
        };
        var convertedFile = new File()
        {
            FileContents = "FileContents",
            GroupId = new Guid().ToString(),
            Nonce = "Nonce",
            Title = "Title"
        };
        var fileReturnedFromMock = new File()
        {
            FileContents = "FileContents",
            GroupId = new Guid().ToString(),
            Nonce = "Nonce",
            Title = "Title"
        };

        fileRepo.Setup(repo => repo.AddFile(It.IsAny<File>())).Returns(fileReturnedFromMock);
        
        //Act
        fileService.AddFile(inputDto);
        
        //Assert
        fileRepo.Verify(repo => repo.AddFile(It.Is<File>(
            f => f.FileContents == convertedFile.FileContents 
                 && f.Title == convertedFile.Title
                 && f.GroupId == convertedFile.GroupId
                 && f.Nonce == convertedFile.Nonce
            )), Times.Once);  
    }
    #endregion

    #region GetFile
    [Fact]
    public void GetFile_ThrowsValidationException()
    {
        //Arrange
        var dto = new GetFileOrAccessInputDto()
        {
            UserId = "userId"
        };
        var validationFailures = new List<ValidationFailure>  
        {  
            new ValidationFailure(dto.UserId, "UserId is required.")  
        };  
        var validationResult = new ValidationResult(validationFailures);  
  
        getFileOrAccessInputDtoValidator.Setup(v => v.Validate(It.IsAny<GetFileOrAccessInputDto>())).Returns(validationResult);  
        
        // Act
        Action act = () => fileService.GetFile(dto);

        // Assert
        Assert.Throws<CustomValidationException>(act);
        getFileOrAccessInputDtoValidator.Verify(v => v.Validate(dto), Times.Once);
    }
    
    [Fact]
    public void GetFile_ReturnsGetFileDto()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId"
        };
        var fileReturnedFromMock = new File()
        {
            Id = "Id",
            FileContents = "FileContents",
            Nonce = "Nonce",
            Title = "Title"
        };
        var fileAccessFromMock = new UserFileAccess()
        {
            FileId = "fileId",
            UserId = "userId",
            EncryptedFileKey = "FAK",
            Role = "Editor"
        };
        var expectedResult = new GetFileDto()
        {
            File = new FileDto()
            {
                FileContents = fileReturnedFromMock.FileContents,
                Id = fileReturnedFromMock.Id,
                Nonce = fileReturnedFromMock.Nonce,
                Title = fileReturnedFromMock.Title
            },
            UserFileAccess = new AddOrGetUserFileAccessDto()
            {
                EncryptedFileKey = fileAccessFromMock.EncryptedFileKey,
                FileId = fileAccessFromMock.FileId,
                UserId = fileAccessFromMock.UserId,
                Role = fileAccessFromMock.Role
            }
        };

        fileRepo.Setup(repo => repo.GetFile(It.IsAny<string>())).Returns(fileReturnedFromMock);
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>())).Returns(fileAccessFromMock);
        
        //Act
        var res = fileService.GetFile(inputDto);
        
        //Assert
        Assert.Equivalent(res, expectedResult);
    }
    [Fact]
    public void GetFile_ReturnsNullIfFilePortGetFileIsNull()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId"
        };
        var fileAccessFromMock = new UserFileAccess()
        {
            FileId = "fileId",
            UserId = "userId",
            EncryptedFileKey = "FAK",
            Role = "Editor"
        };

        fileRepo.Setup(repo => repo.GetFile(It.IsAny<string>())).Returns((File?)null);
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>())).Returns(fileAccessFromMock);
        
        //Act
        var res = fileService.GetFile(inputDto);
        
        //Assert
        Assert.Equivalent(res, null);
    }
    [Fact]
    public void GetFile_ReturnsNullIfFilePortGetUserFileAccessIsNull()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId"
        };
        var fileReturnedFromMock = new File()
        {
            Id = "Id",
            FileContents = "FileContents",
            Nonce = "Nonce",
            Title = "Title",
        };

        fileRepo.Setup(repo => repo.GetFile(It.IsAny<string>())).Returns(fileReturnedFromMock);
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>())).Returns((UserFileAccess?)null);
        
        //Act
        var res = fileService.GetFile(inputDto);
        
        //Assert
        Assert.Equivalent(res, null);
    }
    
    [Fact]
    public void GetFile_CallsFileRepoGetFileWithCorrectParameters()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId"
        };
        var fileReturnedFromMock = new File()
        {
            Id = "Id",
            FileContents = "FileContents",
            Nonce = "Nonce",
            Title = "Title",
        };
        var fileAccessFromMock = new UserFileAccess()
        {
            FileId = "fileId",
            UserId = "userId",
            EncryptedFileKey = "FAK",
            Role = "Editor"
        };

        fileRepo.Setup(repo => repo.GetFile(It.IsAny<string>())).Returns(fileReturnedFromMock);
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>())).Returns(fileAccessFromMock);

        //Act
        fileService.GetFile(inputDto);
        
        //Assert
        fileRepo.Verify(repo => repo.GetFile(It.Is<string>(
            fileId => fileId == inputDto.FileId
            )), Times.Once);  
    }
    
    [Fact]
    public void GetFile_CallsFileRepoGetUserFileAccessWithCorrectParameters()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId"
        };
        var fileReturnedFromMock = new File()
        {
            Id = "Id",
            FileContents = "FileContents",
            Nonce = "Nonce",
            Title = "Title",
        };
        var fileAccessFromMock = new UserFileAccess()
        {
            FileId = "fileId",
            UserId = "userId",
            EncryptedFileKey = "FAK",
            Role = "Editor"
        };

        fileRepo.Setup(repo => repo.GetFile(It.IsAny<string>())).Returns(fileReturnedFromMock);
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>())).Returns(fileAccessFromMock);

        //Act
        fileService.GetFile(inputDto);
        
        //Assert
        fileRepo.Verify(repo => repo.GetUserFileAccess(It.Is<string>(
            userId => userId == inputDto.UserId
        ),
        It.Is<string>(
            fileId => fileId == inputDto.FileId
        )), Times.Once);  
    }
    #endregion

    #region UpdateFile

    [Fact]
     public void UpdateFile_ThrowsValidationException()
    {
        //Arrange
        var dto = new FileDto()
        {
            Title = "title"
        };
        var validationFailures = new List<ValidationFailure>  
        {  
            new ValidationFailure(dto.Title, "Title is required.")  
        };  
        var validationResult = new ValidationResult(validationFailures);  
  
        fileDtoValidator.Setup(v => v.Validate(It.IsAny<FileDto>())).Returns(validationResult);  
        
        // Act
        Action act = () => fileService.UpdateFile(dto);

        // Assert
        Assert.Throws<CustomValidationException>(act);
        fileDtoValidator.Verify(v => v.Validate(dto), Times.Once);
    }
    
    [Fact]
    public void UpdateFile_ReturnsTrue()
    {
        //Arrange
        var inputDto = new FileDto()
        {
            FileContents = "FileContents",
            Nonce = "Nonce",
            Title = "Title",
        };
        
        fileRepo.Setup(repo => repo.UpdateFile(It.IsAny<File>())).Returns(true);
        
        //Act
        var res = fileService.UpdateFile(inputDto);
        
        //Assert
        Assert.Equivalent(res, true);
    }
    
    [Fact]
    public void UpdateFile_CallsRepoWithCorrectParameters()
    {
        //Arrange
        var inputDto = new FileDto()
        {
            Id = "Id",
            FileContents = "FileContents",
            Nonce = "Nonce",
            Title = "Title",
        };
        var convertedFile = new File()
        {
            Id = "Id",
            FileContents = "FileContents",
            Nonce = "Nonce",
            Title = "Title",
        };

        fileRepo.Setup(repo => repo.UpdateFile(It.IsAny<File>())).Returns(true);
        
        //Act
        fileService.UpdateFile(inputDto);
        
        //Assert
        fileRepo.Verify(repo => repo.UpdateFile(It.Is<File>(
            f => f.FileContents == convertedFile.FileContents 
                 && f.Title == convertedFile.Title
                 && f.Id == convertedFile.Id
                 && f.Nonce == convertedFile.Nonce
            )), Times.Once);  
    }

    #endregion

    #region AddUserFileAccess
    
    [Fact]
     public void AddUserFileAccess_ThrowsValidationException()
    {
        //Arrange
        var dto = new AddOrGetUserFileAccessDto()
        {
            UserId = "userId"
        };
        var validationFailures = new List<ValidationFailure>  
        {  
            new ValidationFailure(dto.UserId, "UserId is required.")  
        };  
        var validationResult = new ValidationResult(validationFailures);  
  
        addOrGetUserFileAccessDtoValidator.Setup(v => v.Validate(It.IsAny<AddOrGetUserFileAccessDto>())).Returns(validationResult);  
        
        // Act
        Action act = () => fileService.AddUserFileAccess(dto);

        // Assert
        Assert.Throws<CustomValidationException>(act);
        addOrGetUserFileAccessDtoValidator.Verify(v => v.Validate(dto), Times.Once);
    }
    
    [Fact]
    public void AddUserFileAccess_CallsRepoWithCorrectParameters()
    {
        //Arrange
        var inputDto = new AddOrGetUserFileAccessDto()
        {
            UserId = "userId",
            FileId = "fileId",
            EncryptedFileKey = "FAK",
            Role = "Editor"
        };
        
        fileRepo.Setup(repo => repo.AddUserFileAccess(It.IsAny<UserFileAccess>()));
        
        //Act
        fileService.AddUserFileAccess(inputDto);
        
        //Assert
        fileRepo.Verify(repo => repo.AddUserFileAccess(It.Is<UserFileAccess>(
            f => f.UserId == inputDto.UserId 
                 && f.FileId == inputDto.FileId
                 && f.EncryptedFileKey == inputDto.EncryptedFileKey
                 && f.Role == inputDto.Role
        )), Times.Once);  
    }

    #endregion
    
    #region GetUserFileAccess
    
    [Fact]
    public void GetUserFileAccess_ThrowsValidationException()
    {
        //Arrange
        var dto = new GetFileOrAccessInputDto()
        {
            UserId = "userId"
        };
        var validationFailures = new List<ValidationFailure>  
        {  
            new ValidationFailure(dto.UserId, "UserId is required.")  
        };  
        var validationResult = new ValidationResult(validationFailures);  
  
        getFileOrAccessInputDtoValidator.Setup(v => v.Validate(It.IsAny<GetFileOrAccessInputDto>())).Returns(validationResult);  
        
        // Act
        Action act = () => fileService.GetUserFileAccess(dto);

        // Assert
        Assert.Throws<CustomValidationException>(act);
        getFileOrAccessInputDtoValidator.Verify(v => v.Validate(dto), Times.Once);
    }
    
    [Fact]
    public void GetUserFileAccess_CallsRepoWithCorrectParameters()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId",
        };
        
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>()));
        
        //Act
        fileService.GetUserFileAccess(inputDto);
        
        //Assert
        fileRepo.Verify(repo => repo.GetUserFileAccess(
            It.Is<string>(
            f => f == inputDto.UserId 
        ),
        It.Is<string>(
            f => f == inputDto.FileId 
        )
            ), Times.Once);  
    }
    
    [Fact]
    public void GetUserFileAccess_ReturnsAddOrGetUserFileAccessDto_IfFoundInDb()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId",
        };
        var repoOutput = new UserFileAccess()
        {
            UserId = "userId",
            FileId = "fileId",
            EncryptedFileKey = "FAK",
            Role = "Editor"
        };
        
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>())).Returns(repoOutput);
        
        //Act
        var res = fileService.GetUserFileAccess(inputDto);
        
        //Assert
        Assert.Equivalent(res, new AddOrGetUserFileAccessDto()
        {
            UserId = repoOutput.UserId,
            FileId = repoOutput.FileId,
            EncryptedFileKey = repoOutput.EncryptedFileKey,
            Role = repoOutput.Role
        });
    }
    
    [Fact]
    public void GetUserFileAccess_ReturnsNull_IfNotFoundInDb()
    {
        //Arrange
        var inputDto = new GetFileOrAccessInputDto()
        {
            UserId = "userId",
            FileId = "fileId",
        };
        
        fileRepo.Setup(repo => repo.GetUserFileAccess(It.IsAny<string>(), It.IsAny<string>())).Returns((UserFileAccess?)null);
        
        //Act
        var res = fileService.GetUserFileAccess(inputDto);
        
        //Assert
        Assert.Equivalent(res, null);
    }

    #endregion
}