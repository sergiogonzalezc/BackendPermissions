using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        /// Validate if exists te same permission for the employee
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> RequestPermission(InputRequestPermission input)
        {
            bool result = await ExistsPermissionByNameAndType(input.NombreEmpleado, input.ApellidoEmpleado);

            if (result)
                return result;
            else
                throw new Exception(string.Format("Permisson does not exists for employee {0}!", input.NombreEmpleado));
        }

        /// <summary>
        /// Insert a un record of Permission in BD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResultInsertPermissionDTO> InsertNewPermission(InputCreatePermission input)
        {
            bool result = false;
            Permissions newPermission = new Permissions
            {
                NombreEmpleado = input.NombreEmpleado,
                ApellidoEmpleado = input.ApellidoEmpleado,
                TipoPermiso = input.TipoPermiso,
                FechaPermiso = Convert.ToDateTime(input.FechaPermiso),
            };

            if (!ExistsPermissionByNameAndType(newPermission.NombreEmpleado, newPermission.ApellidoEmpleado).Result)
            {
                result = await _permissionsRepository.InsertPermissions(newPermission);
                if (!result)
                {
                    return new ResultInsertPermissionDTO
                    {
                        Success = true,
                        ErrorMessage = "No se pudo crear el registro, intente nuevamente"
                    };
                }                

                return new ResultInsertPermissionDTO
                {
                    Success = true,
                    ErrorMessage = null
                };
            }
            else
                return new ResultInsertPermissionDTO
                {
                    Success = false,
                    ErrorMessage = "Registro duplicado!"
                };
            
        }



        /// <summary>
        /// Modifica un registro de Permission en la BD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ModifyPermission(InputModifyPermission input)
        {
            bool result = false;
            Permissions dataPermission = new Permissions
            {
                Id = input.Id,
                NombreEmpleado = input.NombreEmpleado,
                ApellidoEmpleado = input.ApellidoEmpleado,
                TipoPermiso = input.TipoPermiso.Value,
                FechaPermiso = Convert.ToDateTime(input.FechaPermiso),
            };

            var item = await _permissionsRepository.GetPermissionsById(dataPermission.Id);

            if (item == null)
            {
                throw new Exception(string.Format("Record cannot modify! Record {0} was not found!", input.Id));
            }
            else
            {
                // We validate the Permission Type Id
                var permissionTypeObject = await _permissionsRepository.GetPermissionTypeById(input.TipoPermiso.Value);
                if (permissionTypeObject != null)
                {
                    result = await _permissionsRepository.ModifyPermissions(dataPermission);
                }
                else
                    throw new Exception(string.Format("Cannot modify! Record {0} was an invalid permission type Id {1}!", input.Id, input.TipoPermiso));
            }

            return result;
        }



        public async Task<List<PermissionsDTO>> GetPermissions()
        {
            List<PermissionsDTO> resultado = new List<PermissionsDTO>();
            resultado = await _permissionsRepository.GetPermissions();
            //if (resultado.Count == 0)
            //    throw new Exception("No existen datos.");

            return resultado;
        }

        /// <summary>
        /// Validate if exists te same permission for the employee
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public async Task<bool> ExistsPermissionByNameAndType(string name, string lastName)
        {
            return await _permissionsRepository.ExistsPermissionByNameAndType(name, lastName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public Task<PermissionsDTO> GetPermissionsByName(string name, string lastName)
        {
            return _permissionsRepository.GetPermissionsByName(name, lastName);
        }

        /// <summary>
        /// Get the permission type List filter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PermissionsDTO> GetPermissionsById(int id)
        {
            return _permissionsRepository.GetPermissionsById(id);
        }

        /// <summary>
        /// Get the full permission type List
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionTypes>> GetPermissionTypes()
        {
            return await _permissionsRepository.GetPermissionTypes();
        }


        /// <summary>
        /// Get the permission type List by Id
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionTypes> GetPermissionTypeById(int id)
        {
            return await _permissionsRepository.GetPermissionTypeById(id);
        }
    }
}
