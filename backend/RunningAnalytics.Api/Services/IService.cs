using System;

namespace RunningAnalytics.Api.Services;

public interface IService<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int? id);
    Task AddAsync(T obj);
    //Task UpdateAsync(int id, T obj);
    //Task DeleteAsync(T obj);
}