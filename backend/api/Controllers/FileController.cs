using System.Net;
using api.Models;
using api.SchemaFilters;
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
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
    public ActionResult<GetFileDto> GetFile(
        [FromQuery, CustomDescription("Must be a valid GUID")] string fileId, 
        [FromQuery, CustomDescription("Must be a valid GUID")] string userId
        )
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
        var res = fileService.AddFile(file);
        return Ok(res);
    }

    [HttpPut]
    [Authorize(Policy = "DocumentChange")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
    public ActionResult PutFile(FileDto file)
    {
        var result = fileService.UpdateFile(file);
        return result ? Ok() : NotFound();
    }

    [HttpGet("access")]
    [Authorize(Policy = "DocumentGetUserFileAccess")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
    public ActionResult<AddOrGetUserFileAccessDto> GetUserFileAccess(
        [FromQuery, CustomDescription("Must be a valid GUID")] string fileId, 
        [FromQuery, CustomDescription("Must be a valid GUID")] string userId
        )
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
    [ProducesResponseType(typeof(ErrorMessageResponse), (int)HttpStatusCode.Conflict)]
    public ActionResult PostUserFileAccess(AddOrGetUserFileAccessDto userFileAccess)
    {
        fileService.AddUserFileAccess(userFileAccess);
        return Ok();
    }
    
    [HttpDelete("access")]
    //todo auth
    public ActionResult DeleteUserFileAccess(
        [FromQuery, CustomDescription("Must be a valid GUID")] string fileId, 
        [FromQuery, CustomDescription("Must be a valid GUID")] string userId
        )
    {
        var dto = new GetFileOrAccessInputDto()
        {
            UserId = userId,
            FileId = fileId
        };
        
        fileService.DeleteUserFileAccess(dto);
        return Ok();
    }
}
