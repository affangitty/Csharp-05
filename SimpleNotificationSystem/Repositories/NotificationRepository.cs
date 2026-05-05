using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNotificationSystem.Models;

namespace SimpleNotificationSystem.Repositories;

internal class NotificationRepository : AbstractRepository<int, Notification>
{
    private int _lastId = 0;

    public override Notification Create(Notification item)
    {
        var id = ++_lastId;
        _items[id] = item;
        return item;
    }

    public int CreateWithId(Notification item)
    {
        var id = ++_lastId;
        _items[id] = item;
        return id;
    }

    public IReadOnlyList<int> GetIds() => _items.Keys.OrderBy(k => k).ToList();
}

