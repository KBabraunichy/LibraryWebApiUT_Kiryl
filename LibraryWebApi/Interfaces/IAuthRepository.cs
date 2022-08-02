namespace LibraryWebApi.Interfaces
{
    public interface IAuthRepository<T, V>
    {
        public T Authenticate(V item);
        public Task<T> Registration(T item);
        public bool RegistrationCheck(T item);
        public string Generate(T item);
    }
}
