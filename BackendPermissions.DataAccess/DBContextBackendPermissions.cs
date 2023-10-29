//using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using BackendPermissions.Infraestructura.Repository;
using BackendPermissions.Infraestructura.Repositor;
using BackendPermissions.Application.Model;

namespace BackendPermissions.Infraestructura
{
    public class DBContextBackendPermissions : DbContext
    {
        private string _cadenaConexion { get; set; }

        public DbSet<PermissionsEF> Permissions { get; set; }

        public DbSet<PermissionTypesEF> PermissionTypes { get; set; }

        private DBContextBackendPermissions()
        {
            _cadenaConexion = String.Empty;
        }

        public DBContextBackendPermissions(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }
        public DBContextBackendPermissions(DbContextOptions opciones) : base(opciones)
        {

            _cadenaConexion = Database.GetDbConnection().ConnectionString ?? String.Empty;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_cadenaConexion);
            }
        }
    }
}