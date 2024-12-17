using application.dtos;
using application.services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController(IFileService fileService): ControllerBase
{
    [HttpGet]
    public ActionResult<GetFileDto> GetFile([FromQuery] string fileId, [FromQuery] string userId)
    {
        var result = fileService.GetFile(fileId, userId);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public ActionResult<PostFileResultDto> PostFile(AddFileDto file)
    {
        var res = fileService.AddFile(file);
        return Ok(res);
    }

    [HttpPut]
    public ActionResult PutFile(FileDto file)
    {
        var result = fileService.UpdateFile(file);
        return result ? Ok() : NotFound();
    }

    [HttpGet("access")]
    public ActionResult<AddOrGetUserFileAccessDto> GetUserFileAccess([FromQuery] string fileId, [FromQuery] string userId)
    {
        var result = fileService.GetUserFileAccess(fileId, userId);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("access")]
    public ActionResult PostUserFileAccess(AddOrGetUserFileAccessDto userFileAccess)
    {
        fileService.AddUserFileAccess(userFileAccess);
        return Ok();
    }
}
