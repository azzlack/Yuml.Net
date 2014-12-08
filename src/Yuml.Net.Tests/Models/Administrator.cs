namespace Yuml.Net.Test.Models
{
    using System.Collections.Generic;

    using global::Yuml.Net.Test.Interfaces;

    public class Administrator : User, IAdministrator
    {
        private IList<Role> roles;

        public Domain Domain { get; set; } 

        public IList<Role> Roles
        {
            get
            {
                return this.roles ?? (this.roles = new List<Role>());
            }

            set
            {
                this.roles = value;
            }
        }

        public void ChangePassword(User user, string password)
        {
            user.Password = password;
        }
    }
}