using maplr_api.Interfaces;

namespace maplr_api.Authentication
{
    public class Authenticate : IAuthenticate
    {
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("maplr") && password.Equals("maplr");
        }
    }
}
