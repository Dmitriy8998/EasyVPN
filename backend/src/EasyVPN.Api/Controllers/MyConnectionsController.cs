using System.Security.Claims;
using EasyVPN.Api.Common;
using EasyVPN.Application.Vpn.Commands.CreateConnection;
using EasyVPN.Application.Vpn.Queries.GetConfig;
using EasyVPN.Application.Vpn.Queries.GetConnections;
using EasyVPN.Contracts.Connections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyVPN.Api.Controllers;

[Route("my/connections")]
[Authorize(Roles = Roles.Client)]
public class MyConnectionsController : ApiController
{
    private readonly ISender _sender;

    public MyConnectionsController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetConnections()
    {
        if (GetCurrentId() is not { } clientId)
            return Forbid();
        
        var getConnectionsResult = 
            await _sender.Send(new GetConnectionsQuery(clientId));
        
        return getConnectionsResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateConnection(CreateConnectionRequest request)
    {
        if (GetCurrentId() is not { } clientId)
            return Forbid();
        
        var createConnectionResult = 
            await _sender.Send(new CreateConnectionCommand(
                clientId,
                request.ServerId));
        
        return createConnectionResult.Match(
            _ => Ok(),
            errors => Problem(errors));
    }
    
    [HttpGet("{connectionId:guid}/config")]
    public async Task<IActionResult> GetConnectionConfig([FromRoute] Guid connectionId)
    {
        if (GetCurrentId() is not { } clientId)
            return Forbid();
        
        var configResult = await _sender.Send(new GetConfigQuery(connectionId));
        return configResult.Match(
            result => result.ClientId == clientId 
                ? Ok(new ConnectionConfigResponse(result.ClientId, result.Config))
                : Forbid(),
            errors => Problem(errors));
    }

    private Guid? GetCurrentId()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id is not null && Guid.TryParse(id, out var guid))
            return guid;
        
        return null;
    }
}