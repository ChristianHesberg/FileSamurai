using core.models;
using File = core.models.File;
using Group = core.models.Group;

namespace application.ports;

public interface IFilePort
{
    public File AddFile(File file);
    public File GetFile(string fileId);
    public bool UpdateFile(File file);
    public void AddUserFileAccess(UserFileAccess userFileAccess);
    void AddUserFileAccesses(List<UserFileAccess> accesses);
    public UserFileAccess GetUserFileAccess(string userId, string fileId);
    public Group GetFileGroup(string fileId);
    public List<UserFileAccess> GetAllUserFileAccess(string fileId);
    public void DeleteUserFileAccess(string userId, string fileId);
    public List<File> GetAllFilesUserHasAccessTo(string fileId);
}