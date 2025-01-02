using application.dtos;
using application.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController(IFileService fileService): ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "DocumentGet")]
    public ActionResult<(UpdateOrGetFileDto, AddOrGetUserFileAccessDto)> GetFile([FromQuery] string fileId, [FromQuery] string userId)
    {
        var result = fileService.GetFile(fileId, userId);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize]
    public ActionResult PostFile(AddFileDto file)
    {   
        fileService.AddFile(file);
        return Ok();
    }

    [HttpPut]
    [Authorize(Policy = "DocumentChange")]
    public ActionResult PutFile(UpdateOrGetFileDto file)
    {
        var result = fileService.UpdateFile(file);
        return result ? Ok() : NotFound();
    }

    [HttpGet("access")]
    [Authorize]
    public ActionResult<AddOrGetUserFileAccessDto> GetUserFileAccess([FromQuery] string fileId, [FromQuery] string userId)
    {
        var result = fileService.GetUserFileAccess(fileId, userId);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("access")]
    [Authorize(Policy = "FileAccess")]
    public ActionResult PostUserFileAccess(AddOrGetUserFileAccessDto userFileAccess)
    {
        fileService.AddUserFileAccess(userFileAccess);
        return Ok();
    }
}
