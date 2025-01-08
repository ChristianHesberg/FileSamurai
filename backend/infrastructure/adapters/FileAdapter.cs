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
        try
        {
            var entity = context.Files.Add(file);
            context.SaveChanges();
            return entity.Entity;
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
    }

    public File GetFile(string fileId)
    {
        var res = context.Files.FirstOrDefault(file => file.Id == fileId);
        if (res == null) throw new KeyNotFoundException($"Could not find file with id: {fileId}");
        return res;
    }

    public bool UpdateFile(File file)
    {
        var res = context.Files.Find(file.Id);
        if (res == null) throw new KeyNotFoundException();
        try
        {
            res.Title = file.Title;
            res.FileContents = file.FileContents;
            context.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
    }

    public void AddUserFileAccess(UserFileAccess userFileAccess)
    {
        var alreadyExists =
            context.UserFileAccesses.Any(x => x.FileId == userFileAccess.FileId && x.UserId == userFileAccess.UserId);
        if (alreadyExists) throw new EntityAlreadyExistsException("User already has access to this file!");
        try
        {
            context.UserFileAccesses.Add(userFileAccess);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
    }

    public UserFileAccess GetUserFileAccess(string userId, string fileId)
    {
        var res = context.UserFileAccesses.FirstOrDefault(f => f.FileId == fileId && f.UserId == userId);
        if (res == null) throw new KeyNotFoundException("Could not find UserFileAccess with given ids.");
        return res;
    }

    public Group GetFileGroup(string fileId)
    {
        var res = context.Files.Include(e => e.Group).FirstOrDefault(e => e.Id == fileId);
        if (res == null) throw new KeyNotFoundException($"Could not find file with id: {fileId}");
        return res.Group;
    }

    public List<UserFileAccess> GetAllUserFileAccess(string fileId)
    {
        return context.UserFileAccesses.Where(x => x.FileId == fileId).ToList();
    }

    public void DeleteUserFileAccess(string userId, string fileId)
    {
        var access = context.UserFileAccesses.FirstOrDefault(f => f.FileId == fileId && f.UserId == userId);
        if (access == null) throw new KeyNotFoundException("User file access not found");
        try
        {
            context.UserFileAccesses.Remove(access);
            context.SaveChanges();
        }
        catch (Exception)
        {
            throw new DatabaseUpdateException();
        }
    }
}