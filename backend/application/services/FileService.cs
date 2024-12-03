using application.ports;
using core.models;
using File = core.models.File;

namespace application.services;

public class FileService(IFilePort filePort): IFileService
{
    
}