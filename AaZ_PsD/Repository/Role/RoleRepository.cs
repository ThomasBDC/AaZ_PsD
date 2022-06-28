using AaZ_PsD.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace AaZ_PsD.Repository
{
    public class RoleRepository : BaseRepository
    {
        public RoleRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public List<Role> getAllRoles()
        {
            var roles = new List<Role>();

            //Faire une requête en BDD et transformer ça en objet.

            var cnn = this.OpenConnexion();

            string sql = "select idrole, name, description from role";

            var cmd = new MySqlCommand(sql, cnn);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var role = new Role()
                {
                    IdRole = Convert.ToInt16(reader["idrole"]),
                    Name = reader["name"].ToString(),
                    Description = reader["description"].ToString()
                };

                roles.Add(role);
            }

            return roles;
        }
    }
}
