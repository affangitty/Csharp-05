using SimpleNotificationSystem.Models;

namespace SimpleNotificationSystem.Repositories;

internal class UserRepository : AbstractRepository<string, User>
{
    public override User Create(User item)
    {
        _items[item.Email] = item;
        return item;
    }
}

