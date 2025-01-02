using application.ports;
using core.models;
using Microsoft.EntityFrameworkCore;
using File = core.models.File;
using Group = core.models.Group;

namespace infrastructure.adapters;

public class FileAdapter(Context context) : IFilePort
{
    public File AddFile(File file)
    {
        var entity = context.Files.Add(file);
        context.SaveChanges();
        return entity.Entity;
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

    public Group? GetFileGroup(string fileId)
    {
        return context.Files.Include(e=> e.Group).FirstOrDefault(e => e.Id == fileId)?.Group;
    }
}