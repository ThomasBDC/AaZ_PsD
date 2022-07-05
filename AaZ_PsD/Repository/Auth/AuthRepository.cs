using AaZ_PsD.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace AaZ_PsD.Repository
{
    public class AuthRepository : BaseRepository
    {
        public AuthRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public void SignUp(UserModel userModel, string passwordSalt, string passwordHash)
        {
            var roles = new List<Role>();

            //Faire une requête en BDD et transformer ça en objet.

            var cnn = this.OpenConnexion();

            string sql = @"INSERT INTO `users`
                    (
                    surname,
                    forename,
                    mail,
                    password,
                    passwordkey,
                    telephone,
                    dateembauche,
                    daterenvoi,
                    idrole)
                    VALUES
                    (
                    @surname,
                    @forename,
                    @mail,
                    @password,
                    @passwordkey,
                    @telephone,
                    @dateembauche,
                    @daterenvoi,
                    @idrole);
            ";

            var cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@surname", userModel.Surname);
            cmd.Parameters.AddWithValue("@forename", userModel.Forename);
            cmd.Parameters.AddWithValue("@mail", userModel.Mail);
            cmd.Parameters.AddWithValue("@password", passwordHash);
            cmd.Parameters.AddWithValue("@passwordkey", passwordSalt);
            cmd.Parameters.AddWithValue("@telephone", userModel.Telephone);
            cmd.Parameters.AddWithValue("@dateembauche", userModel.DateEmbauche);
            cmd.Parameters.AddWithValue("@daterenvoi", userModel.DateRenvoi);
            cmd.Parameters.AddWithValue("@idrole", userModel.Role.IdRole);

            cmd.ExecuteNonQuery();
        }

        public UserModel GetUser(string mail)
        {
            var cnn = this.OpenConnexion();

            string sql = @"
                SELECT 
                    u.iduser,
                    u.surname,
                    u.forename,
                    u.mail,
                    u.telephone,
                    u.dateembauche,
                    u.daterenvoi,
                    u.subjectid,
                    u.passwordkey,
                    u.password,
                    r.idrole,
                    r.name
                FROM users u
                INNER JOIN role r ON r.idrole = u.idrole
                where u.mail = @mail
            ";

            var cmd = new MySqlCommand(sql, cnn);
            cmd.Parameters.AddWithValue("@mail", mail);

            var reader = cmd.ExecuteReader();

            UserModel user = null;

            if (reader.Read())
            {
                user = new UserModel()
                {
                    IdUser = Convert.ToInt16(reader["iduser"]),
                    Surname = reader["surname"].ToString(),
                    Forename = reader["forename"].ToString(),
                    Mail = reader["mail"].ToString(),
                    Password = reader["password"].ToString(),
                    Telephone = reader["telephone"].ToString(),
                    PasswordKey = reader["passwordkey"].ToString(),
                    Role = new Role()
                    {
                        IdRole = Convert.ToInt16(reader["idrole"]),
                        Name = reader["name"].ToString()
                    }
                };
            }

            return user;
        }
    }
}
