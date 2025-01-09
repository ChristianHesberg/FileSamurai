using application.dtos;
using core.models;
using File = core.models.File;

namespace application.services;

public interface IFileService
{
    public PostFileResultDto AddFile(AddFileDto file);
    public GetFileDto GetFile(GetFileOrAccessInputDto dto);
    public bool UpdateFile(FileDto orGetFile);
    public void AddUserFileAccess(AddOrGetUserFileAccessDto orGetUserFileAccess);
    public void AddUserFileAccesses(List<AddOrGetUserFileAccessDto> userFileAccessDtos);
    public AddOrGetUserFileAccessDto GetUserFileAccess(GetFileOrAccessInputDto dto);
    void DeleteUserFileAccess(GetFileOrAccessInputDto dto);
    public Group GetFileGroup(string dtoFileId);
    public List<UserFileAccess> GetAllUserFileAccess(string dtoFileId);

    public List<FileOptionDto> GetFileOptionDtos(string userId);

}