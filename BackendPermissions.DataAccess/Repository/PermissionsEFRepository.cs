using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using BackendPermissions.Application.Model;
using BackendPermissions.Application.Interface;
using AutoMapper;
using BackendPermissions.Infraestructura.Repositor;
using BackendPermissions.Infraestructura.Data;
using static BackendPermissions.Common.Enum;

namespace BackendPermissions.Infraestructura.Repository
{
    public class PermissionsEFRepository : IPermissionsRepository
    {
        private readonly string _cadenaConexion;
        private readonly IConfiguration _configuracion;
        private DBContextBackendPermissions _dataBaseDBContext;
        private Mapper _mapper;
        private readonly IConnectionFactory _connectionFactory;

        public PermissionsEFRepository(IConfiguration configuracion)
        {
            _configuracion = configuracion;
            _cadenaConexion = _configuracion.GetConnectionString("cadenaConexion");

            var opcionesDBContext = new DbContextOptionsBuilder<DBContextBackendPermissions>();
            //opcionesDBContext.UseMySQL(_cadenaConexion);
            opcionesDBContext.UseSqlServer(_cadenaConexion);

            _dataBaseDBContext = new DBContextBackendPermissions(opcionesDBContext.Options);

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<PermissionsEF, Permissions>().ReverseMap();
                cfg.CreateMap<PermissionTypesEF, PermissionTypes>().ReverseMap();
            }
            );

            _mapper = new Mapper(config);

        }

        #region Permission

        public async Task<bool> InsertPermissions(Permissions input)
        {
            try
            {
                PermissionsEF permissionBD = _mapper.Map<PermissionsEF>(input);

                _dataBaseDBContext.Permissions.Add(permissionBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ModifyPermissions(Permissions input)
        {
            try
            {
                PermissionsEF? permissionBD = await _dataBaseDBContext.Permissions.FirstOrDefaultAsync(m => m.Id == input.Id);
                {
                    permissionBD.NombreEmpleado = input.NombreEmpleado;
                    permissionBD.ApellidoEmpleado = input.ApellidoEmpleado;
                    permissionBD.TipoPermiso = input.TipoPermiso;
                    permissionBD.FechaPermiso = input.FechaPermiso;
                }

                _dataBaseDBContext.Permissions.Update(permissionBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Se ejecuta lectura asíncrona de permisos
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionsDTO>> GetPermissions()
        {
            List<PermissionsEF>? dataBD = await _dataBaseDBContext.Permissions.ToListAsync();
            List<PermissionsDTO> outPutList = new List<PermissionsDTO>();

            // Se mapea el objeto
            List<Permissions> result = _mapper.Map<List<Permissions>>(dataBD);

            foreach (var permission in result)
            {
                PermissionsDTO item = new PermissionsDTO();
                item.Id = permission.Id;
                item.NombreEmpleado = permission.NombreEmpleado;
                item.ApellidoEmpleado = permission.ApellidoEmpleado;
                item.FechaPermiso = permission.FechaPermiso;
                item.TipoPermisoCode = permission.TipoPermiso;
                item.TipoPermisoDesc = _dataBaseDBContext.PermissionTypes.Where(x => x.Id.Equals(permission.TipoPermiso)).Select(x => x.Descripcion).SingleOrDefault();

                outPutList.Add(item);
            }

            return outPutList;
        }

        /// <summary>
        /// Validate if exists te same permission for the employee
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public async Task<bool> ExistsPermissionByNameAndType(string name, string lastName, int permissionType)
        {
            List<PermissionsEF>? dataBD = await _dataBaseDBContext.Permissions.Where(x => x.NombreEmpleado.ToUpper().Equals(name.ToUpper())
                                                                                    && x.ApellidoEmpleado.ToUpper().Equals(lastName.ToUpper())
                                                                                    && x.TipoPermiso.Equals(permissionType)
                                                                                    ).ToListAsync();
            if (dataBD == null || dataBD.Count == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public async Task<PermissionsDTO> GetPermissionsByName(string name, string lastName, int permissionType)
        {
            // Take the last record 
            PermissionsEF? dataBD = await _dataBaseDBContext.Permissions.Where(x => x.NombreEmpleado.ToUpper().Equals(name.ToUpper())
                                                                                    && x.ApellidoEmpleado.ToUpper().Equals(lastName.ToUpper())
                                                                                    && x.TipoPermiso.Equals(permissionType)
                                                                                    ).OrderByDescending(x => x.Id).Take(1).SingleOrDefaultAsync();

            if (dataBD == null)
                return null;

            // Se mapea el objeto
            Permissions result = _mapper.Map<Permissions>(dataBD);

            PermissionsDTO item = new PermissionsDTO();
            item.Id = result.Id;
            item.NombreEmpleado = result.NombreEmpleado;
            item.ApellidoEmpleado = result.ApellidoEmpleado;
            item.FechaPermiso = result.FechaPermiso;
            item.TipoPermisoCode = result.TipoPermiso;
            item.TipoPermisoDesc = _dataBaseDBContext.PermissionTypes.Where(x => x.Id.Equals(result.TipoPermiso)).Select(x => x.Descripcion).SingleOrDefault();

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PermissionsDTO> GetPermissionsById(int id)
        {
            // Take the last record 
            PermissionsEF? dataBD = await _dataBaseDBContext.Permissions.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (dataBD == null)
                return null;

            // Se mapea el objeto
            Permissions result = _mapper.Map<Permissions>(dataBD);

            PermissionsDTO item = new PermissionsDTO();
            item.Id = result.Id;
            item.NombreEmpleado = result.NombreEmpleado;
            item.ApellidoEmpleado = result.ApellidoEmpleado;
            item.FechaPermiso = result.FechaPermiso;
            item.TipoPermisoCode = result.TipoPermiso;
            item.TipoPermisoDesc = _dataBaseDBContext.PermissionTypes.Where(x => x.Id.Equals(result.TipoPermiso)).Select(x => x.Descripcion).SingleOrDefault();

            return item;
        }

        /// <summary>
        /// Get the Permission Type By Id
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionTypeDTO> GetPermissionTypeById(int id)
        {
            PermissionTypesEF? dataBD = await _dataBaseDBContext.PermissionTypes.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (dataBD == null)
                return null;

            // Se mapea el objeto
            PermissionTypes result = _mapper.Map<PermissionTypes>(dataBD);

            PermissionTypeDTO item = new PermissionTypeDTO();
            item.Id = result.Id;
            item.Descripcion = result.Descripcion;

            return item;
        }

        #endregion

    }
}