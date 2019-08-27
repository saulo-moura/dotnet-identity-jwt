using DotnetJwtAuth.Entities;

namespace DotnetJwtAuth.Interfaces
{
    public interface IAuthenticate
    {
         ApplicationUser Authenticate(string username, string password);
    }
}