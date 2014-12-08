namespace Yuml.Net.Test.Interfaces
{
    using global::Yuml.Net.Test.Models;

    public interface IAdministrator
    {
        void ChangePassword(User user, string password);
    }
}