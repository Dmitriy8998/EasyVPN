using EasyVPN.Api.Common;
using EasyVPN.Application.Vpn.Commands.CreateConnection;
using EasyVPN.Application.Vpn.Queries.GetConfig;
using EasyVPN.Application.Vpn.Queries.GetConnections;
using EasyVPN.Contracts.Connections;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyVPN.Api.Controllers;

[Route("connections")]
[Authorize(Roles = Roles.Administrator)]
public class ConnectionsController : ApiController
{
    private readonly ISender _sender;

    public ConnectionsController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> GetConnections([FromQuery] Guid? clientId)
    {
        var getConnectionsResult = 
            await _sender.Send(new GetConnectionsQuery(clientId));
        
        return getConnectionsResult.Match(
            result => Ok(result),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateConnection(
        CreateConnectionRequest request, [FromQuery] Guid clientId)
    {
        var createConnectionResult = 
            await _sender.Send(new CreateConnectionCommand(
                clientId,
                request.ServerId,
                request.CountDays));
        
        return createConnectionResult.Match(
            _ => Ok(),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}/config")]
    public async Task<IActionResult> GetConnectionConfig([FromRoute] Guid id)
    {
        var configResult = 
            await _sender.Send(new GetConfigQuery(id));
        return configResult.Match(
        result => Ok(new ConnectionConfigResponse(result.ClientId, result.Config)),
        errors => Problem(errors));
    }
}