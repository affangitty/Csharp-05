using System.Collections.Generic;

namespace SimpleNotificationSystem.Interfaces;

internal interface IRepository<K, T> where K : notnull where T : class
{
    T Create(T item);
    T? GetById(K key);
    List<T> GetAll();
    T? Update(K key, T item);
    T? Delete(K key);
}

