using application.ports;
using core.errors;
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
        var res = context.Files.Find(file.Id);
        if (res == null) return false;
        res.Title = file.Title;
        res.FileContents = file.FileContents;
        context.SaveChanges();
        return true;
    }

    public void AddUserFileAccess(UserFileAccess userFileAccess)
    {
        var alreadyExists =
            context.UserFileAccesses.Any(x => x.FileId == userFileAccess.FileId && x.UserId == userFileAccess.UserId);
        if (alreadyExists) throw new EntityAlreadyExistsException("User already has access to this file!");
        context.UserFileAccesses.Add(userFileAccess);
        context.SaveChanges();
    }

    public UserFileAccess? GetUserFileAccess(string userId, string fileId)
    {
        return context.UserFileAccesses.FirstOrDefault(f => f.FileId == fileId && f.UserId == userId);
    }

    public Group? GetFileGroup(string fileId)
    {
        return context.Files.Include(e => e.Group).FirstOrDefault(e => e.Id == fileId)?.Group;
    }

    public List<UserFileAccess> GetAllUserFileAccess(string fileId)
    {
        return context.UserFileAccesses.Where(x => x.FileId == fileId).ToList();
    }
}