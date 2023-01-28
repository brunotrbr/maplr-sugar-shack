namespace maplr_api.Interfaces
{
    public interface IAuthenticate
    {
        bool ValidateCredentials(string username, string password);
    }
}
