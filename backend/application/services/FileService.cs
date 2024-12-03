using application.dtos;
using application.ports;
using core.models;
using File = core.models.File;

namespace application.services;

public class FileService(IFilePort filePort): IFileService
{
    public void AddFile(AddFileDto file)
    {
        throw new NotImplementedException();
    }

    public UpdateOrGetFileDto? GetFile(string fileId)
    {
        throw new NotImplementedException();
    }

    public bool UpdateFile(UpdateOrGetFileDto orGetFile)
    {
        throw new NotImplementedException();
    }

    public void AddUserFileAccess(AddOrGetUserFileAccessDto orGetUserFileAccess)
    {
        throw new NotImplementedException();
    }

    public AddOrGetUserFileAccessDto? GetUserFileAccess(string userId, string fileId)
    {
        throw new NotImplementedException();
    }
}