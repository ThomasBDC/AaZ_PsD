using AaZ_PsD.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace AaZ_PsD.Repository
{
    public class UsersRepository : BaseRepository
    {
        public UsersRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public List<UserModel> getAllUsers()
        {
            var users = new List<UserModel>();

            //Faire une requête en BDD et transformer ça en objet.

            var cnn = this.OpenConnexion();

            string sql = @"SELECT 
                 u.iduser, u.surname, u.forename, u.mail, u.telephone, u.dateembauche,
                 r.idrole, r.name
                FROM 
                users u
                inner join role r on r.idrole = u.idrole";

            var cmd = new MySqlCommand(sql, cnn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var user = new UserModel()
                {
                    IdUser = Convert.ToInt16(reader["iduser"]),
                    Surname = reader["surname"].ToString(),
                    Forename = reader["forename"].ToString(),
                    Mail = reader["mail"].ToString(),
                    Telephone = reader["telephone"].ToString(),
                    DateEmbauche = DateTime.Parse(reader["dateembauche"].ToString()),
                    Role = new Role()
                    {
                        IdRole = Convert.ToInt16(reader["idrole"]),
                        Name = reader["name"].ToString(),
                    }
                };

                users.Add(user);
            }

            return users;
        }
    }
}
