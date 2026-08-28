namespace RunningAnalytics.Application.Services;

public interface IService<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid? id);
    Task<T> AddAsync(T obj);
    Task<bool> UpdateAsync(Guid id, T obj);
    Task<bool> DeleteAsync(T obj);
}