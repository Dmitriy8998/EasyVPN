using EasyVPN.Application.Common.Interfaces.Persistence;
using EasyVPN.Domain.Entities;

namespace EasyVPN.Infrastructure.Persistence;

public class ConnectionRepository : IConnectionRepository
{
    private static readonly List<Connection> _connections = new()
    {
        new Connection()
        {
            Id = Guid.Parse("00000001-0000-0000-0000-000000000000"), 
            ClientId = Guid.Parse("00000001-0000-0000-0000-000000000000"), 
            ServerId = Guid.Empty,
            ExpirationTime = DateTime.MinValue
        },
        new Connection()
        {
        Id = Guid.Parse("00000002-0000-0000-0000-000000000000"), 
        ClientId = Guid.Parse("00000001-0000-0000-0000-000000000000"), 
        ServerId = Guid.Empty,
        ExpirationTime = DateTime.MaxValue
    }
    };
    
    public Connection? Get(Guid id)
    {
        return _connections.SingleOrDefault(c => c.Id == id);
    }

    public IEnumerable<Connection> GetAll()
    {
        return _connections.AsEnumerable();
    }

    public void Add(Connection connection)
    {
        _connections.Add(connection);
    }

    public void Remove(Guid id)
    {
        _connections.RemoveAll(c => c.Id == id);
    }

    public void Update(Connection connection)
    {
        if (_connections.SingleOrDefault(c => c.Id == connection.Id) is not {} stateConnection)
            return;
        stateConnection.ClientId = connection.ClientId;
        stateConnection.ServerId = connection.ServerId;
        stateConnection.ExpirationTime = connection.ExpirationTime;
    }
}