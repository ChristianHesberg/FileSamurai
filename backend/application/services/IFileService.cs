using application.dtos;
using core.models;
using File = core.models.File;

namespace application.services;

public interface IFileService
{
    public PostFileResultDto AddFile(AddFileDto file);
    public GetFileDto? GetFile(GetFileOrAccessInputDto dto);
    public bool UpdateFile(FileDto orGetFile);
    public void AddUserFileAccess(AddOrGetUserFileAccessDto orGetUserFileAccess);
    public AddOrGetUserFileAccessDto? GetUserFileAccess(GetFileOrAccessInputDto dto);
    public GroupDto? GetFileGroup(string fileId);
}