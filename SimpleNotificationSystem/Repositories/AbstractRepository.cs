using System.Collections.Generic;
using System.Linq;
using SimpleNotificationSystem.Interfaces;

namespace SimpleNotificationSystem.Repositories;

internal abstract class AbstractRepository<K, T> : IRepository<K, T> where K : notnull where T : class
{
    protected readonly Dictionary<K, T> _items = new();

    public abstract T Create(T item);

    public T? GetById(K key) => _items.TryGetValue(key, out var item) ? item : null;

    public List<T> GetAll() => _items.Values.ToList();

    public T? Update(K key, T item)
    {
        if (!_items.ContainsKey(key))
            return null;

        _items[key] = item;
        return item;
    }

    public T? Delete(K key)
    {
        if (!_items.TryGetValue(key, out var item))
            return null;

        _items.Remove(key);
        return item;
    }
}

