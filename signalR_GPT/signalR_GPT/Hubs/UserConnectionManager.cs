using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalR_GPT.Hubs
{
    public class UserConnectionManager
    {
        //    private readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

        //    public void AddConnection(string userId, string connectionId)
        //    {
        //        lock (_userConnections)
        //        {
        //            if (_userConnections.ContainsKey(userId))
        //            {
        //                _userConnections[userId] = connectionId;
        //            }
        //            else
        //            {
        //                _userConnections.Add(userId, connectionId);
        //            }
        //        }
        //    }

        //    public void RemoveConnection(string connectionId)
        //    {
        //        lock (_userConnections)
        //        {
        //            var userId = _userConnections.FirstOrDefault(x => x.Value == connectionId).Key;
        //            if (userId != null)
        //            {
        //                _userConnections.Remove(userId);
        //            }
        //        }
        //    }

        //    public string GetConnectionId(string userId)
        //    {
        //        lock (_userConnections)
        //        {
        //            if (_userConnections.TryGetValue(userId, out string connectionId))
        //            {
        //                return connectionId;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //}
        private readonly Dictionary<string, List<string>> _userConnections;

        public UserConnectionManager()
        {
            _userConnections = new Dictionary<string, List<string>>();
        }

        public void AddConnection(string userId, string connectionId)
        {
            lock (_userConnections)
            {
                if (_userConnections.ContainsKey(userId))
                {
                    var connections = _userConnections[userId];
                    connections.Add(connectionId);
                }
                else
                {
                    _userConnections.Add(userId, new List<string> { connectionId });
                }
            }
        }

        public void RemoveConnection(string userId, string connectionId)
        {
            lock (_userConnections)
            {
                if (_userConnections.ContainsKey(userId))
                {
                    var connections = _userConnections[userId];
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _userConnections.Remove(userId);
                    }
                }
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            lock (_userConnections)
            {
                if (_userConnections.ContainsKey(userId))
                {
                    return _userConnections[userId];
                }
                return new List<string>();
            }
        }
    }
}