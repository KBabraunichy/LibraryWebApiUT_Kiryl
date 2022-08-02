public interface IRepository<T>
{
    public Task<IEnumerable<T>> GetObjectList();
    public Task<T> GetObject(int id);
    public Task<T> Create(T item);
    public Task<T> Update(T item);
    public Task<T> Delete(int id);

}