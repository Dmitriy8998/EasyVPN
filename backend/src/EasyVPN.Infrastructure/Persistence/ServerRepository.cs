using EasyVPN.Application.Common.Interfaces.Persistence;
using EasyVPN.Domain.Entities;

namespace EasyVPN.Infrastructure.Persistence;

public class ServerRepository : IServerRepository
{
    private static readonly List<Server> _servers = new();
    
    public Server? Get(Guid id)
    {
        return _servers.SingleOrDefault(s => s.Id == id);
    }
}