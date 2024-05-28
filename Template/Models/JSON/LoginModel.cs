using System;
namespace Template.Models.JSON
{
    public class LoginModel
    {
        public string email { get; set; }
        public string appVersion { get; set; }
        public string password { get; set; }


        public LoginModel()
        {
        }

        public LoginModel(string email, string appVersion, String password) : this()
        {
            this.email = email;
            this.appVersion = appVersion;
            this.password = password;
        }
    }
}
