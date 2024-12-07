using application.ports;
using core.models;
using File = core.models.File;

namespace infrastructure.adapters;

public class FileAdapter(Context context) : IFilePort
{
    public void AddFile(File file)
    {
        context.Files.Add(file);
        context.SaveChanges();
    }

    public File? GetFile(string fileId)
    {
        return context.Files.FirstOrDefault(file => file.Id == fileId);
    }

    public bool UpdateFile(File file)
    {
        var res= context.Files.Find(file.Id);
        if (res == null) return false;
        res.Title = file.Title;
        res.FileContents = file.FileContents;
        context.SaveChanges();
        return true;
    }

    public void AddUserFileAccess(UserFileAccess userFileAccess)
    {
        context.UserFileAccesses.Add(userFileAccess);
        context.SaveChanges();
    }

    public UserFileAccess? GetUserFileAccess(string userId, string fileId)
    {
        return context.UserFileAccesses.FirstOrDefault(f => f.FileId == fileId && f.UserId == userId);
    }
}