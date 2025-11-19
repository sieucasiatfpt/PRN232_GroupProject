namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(string username, string email, string password, string role);
        Task<string> LoginAsync(string email, string password);
    }
}
