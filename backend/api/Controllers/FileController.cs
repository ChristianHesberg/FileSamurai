using application.dtos;
using application.errors;
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
    public ActionResult<GetFileDto> GetFile([FromQuery] string fileId, [FromQuery] string userId)

    {
        var dto = new GetFileOrAccessInputDto()
        {
            UserId = userId,
            FileId = fileId
        };
        var result = fileService.GetFile(dto);
        return result == null ? NotFound() : Ok(result);
    }
    

    [HttpPost]
    [Authorize(Policy = "DocumentAdd")]
    public ActionResult<PostFileResultDto> PostFile(AddFileDto file)
    {
        fileService.AddFile(file);
        return Ok();
    }

    [HttpPut]
    [Authorize(Policy = "DocumentChange")]
    public ActionResult PutFile(FileDto file)
    {
        var result = fileService.UpdateFile(file);
        return result ? Ok() : NotFound();
    }

    [HttpGet("access")]
    [Authorize(Policy = "DocumentGetUserFileAccess")]

    public ActionResult<AddOrGetUserFileAccessDto> GetUserFileAccess([FromQuery] string userId, [FromQuery] string fileId)
    {
        var dto = new GetFileOrAccessInputDto()
        {
            UserId = userId,
            FileId = fileId
        };
        var result = fileService.GetUserFileAccess(dto);
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
