﻿using System.Net;
using api.Models;
using api.SchemaFilters;
using application.dtos;
using application.services;
using application.transformers;
using core.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController(IFileService fileService) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "DocumentGet")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
    public ActionResult<GetFileDto> GetFile(
        [FromQuery, CustomDescription("Must be a valid GUID")]
        string fileId,
        [FromQuery, CustomDescription("Must be a valid GUID")]
        string userId
    )
    {
        var dto = new GetFileOrAccessInputDto()
        {
            UserId = userId,
            FileId = fileId
        };
        var result = fileService.GetFile(dto);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "DocumentAdd")]
    public ActionResult<PostFileResultDto> PostFile(AddFileDto file)
    {
        var res = fileService.AddFile(file);
        return Ok(res);
    }

    [HttpGet("access")]
    [Authorize(Policy = "DocumentGetUserFileAccess")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
    public ActionResult<AddOrGetUserFileAccessDto> GetUserFileAccess(
        [FromQuery, CustomDescription("Must be a valid GUID")]
        string fileId,
        [FromQuery, CustomDescription("Must be a valid GUID")]
        string userId
    )
    {
        var dto = new GetFileOrAccessInputDto()
        {
            UserId = userId,
            FileId = fileId
        };

        var result = fileService.GetUserFileAccess(dto);
        return Ok(result);
    }

    [HttpPost("access")]
    [Authorize(Policy = "FileAccess")]
    [ProducesResponseType(typeof(ErrorMessageResponse), (int)HttpStatusCode.Conflict)]
    public ActionResult PostUserFileAccess(AddOrGetUserFileAccessDto userFileAccess)
    {
        fileService.AddUserFileAccess(userFileAccess);
        return Ok();
    }

    [HttpPost("accesses")]
    [Authorize(Policy = "ListAccessPolicy")]
    public ActionResult PostUserFileAccesses(List<AddOrGetUserFileAccessDto> userFileAccessDtos)
    {
        fileService.AddUserFileAccesses(userFileAccessDtos);
        return Ok();
    }


    [HttpDelete("access")]
    [Authorize(Policy = "DeleteAccess")]
    public ActionResult DeleteUserFileAccess(
        [FromQuery, CustomDescription("Must be a valid GUID")]
        string fileId,
        [FromQuery, CustomDescription("Must be a valid GUID")]
        string userId
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


    [HttpGet("fileOptions/{id}")]
    [Authorize(Policy = "OwnsResourcePolicy")]
    public ActionResult<List<FileOptionDto>> GetFileOptions(string id)
    {
        var optionDtos = fileService.GetFileOptionDtos(id);
        return Ok(optionDtos);
    }
  
    [HttpGet("allFileAccess/{id}")]
    [Authorize(Policy = "GetAllFileAccessPolicy" )]
    public ActionResult<FileAccessDto> GetAllUserFileAccess(string id)
    {
        var userFileAccess = fileService.GetAllUserFileAccessDto(id);
        return Ok(userFileAccess);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteFile(string id)
    {
        fileService.DeleteFile(id);
        return Ok();
    }
}