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
            cmd.Parameters.AddWithValue("@forename", userModel.Surname);
            cmd.Parameters.AddWithValue("@mail", userModel.Mail);
            cmd.Parameters.AddWithValue("@password", passwordHash);
            cmd.Parameters.AddWithValue("@passwordkey", passwordSalt);
            cmd.Parameters.AddWithValue("@telephone", userModel.Telephone);
            cmd.Parameters.AddWithValue("@dateembauche", userModel.DateEmbauche);
            cmd.Parameters.AddWithValue("@daterenvoi", userModel.DateRenvoi);
            cmd.Parameters.AddWithValue("@idrole", userModel.Role.IdRole);

            cmd.ExecuteNonQuery();
        }
    }
}
