using core.models;
using File = core.models.File;

namespace application.ports;

public interface IFilePort
{
    public void AddFile(File file);
    public File? GetFile(string fileId);
    public bool UpdateFile(File file);
    public void AddUserFileAccess(UserFileAccess userFileAccess);
    public UserFileAccess? GetUserFileAccess(string userId, string fileId);
}