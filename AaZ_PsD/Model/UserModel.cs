using System;

namespace AaZ_PsD.Model
{
    public class UserModel
    {
        public int IdUser { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PasswordKey { get; set; }
        public string Telephone { get; set; }
        public DateTime DateEmbauche { get; set; }
        public DateTime DateRenvoi { get; set; }
        public string SubjectId { get; set; }
        public Role Role { get; set; }

    }
}
