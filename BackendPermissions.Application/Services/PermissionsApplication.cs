using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackendPermissions.Application.ConfiguracionApi;
using BackendPermissions.Application.Interface;
using BackendPermissions.Application.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static BackendPermissions.Common.Enum;

namespace BackendPermissions.Application.Services
{
    public class PermissionsApplication : IPermissionsApplication
    {
        private IPermissionsRepository _permissionsRepository;
        private IConfiguration _configuration;

        public PermissionsApplication(IPermissionsRepository permissionsRepository, IConfiguration configuration)
        {
            _permissionsRepository = permissionsRepository;
            _configuration = configuration;
        }
           
        /// <summary>
        /// Inserta un registro de agenda en la BD
        /// </summary>
        /// <param name="argumentos"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> InsertPermissions(InputCreatePermission argumentos)
        {
            Permissions agenda = new Permissions
            {
                NombreEmpleado = argumentos.NombreEmpleado,
                ApellidoEmpleado = argumentos.ApellidoEmpleado,
                TipoPermiso = argumentos.TipoPermiso,
                FechaPermiso = argumentos.FechaPermiso,
            };

            bool resultado = await _permissionsRepository.InsertPermissions(agenda);
            if (!resultado)
                throw new Exception("No se pudo crear el registro, intente nuevamente");

            return resultado;
        }

        public async Task<List<PermissionsDTO>> GetPermissions()
        {
            List<PermissionsDTO> resultado = new List<PermissionsDTO>();
            resultado = await _permissionsRepository.GetPermissions();
            //if (resultado.Count == 0)
            //    throw new Exception("No existen datos.");

            return resultado;
        }
    }
}
