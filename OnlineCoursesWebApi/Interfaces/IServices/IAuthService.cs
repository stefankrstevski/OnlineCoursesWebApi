namespace OnlineCoursesWebApi.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> GetAuthTokenAsync();
    }
}
