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
                var agendaBD = _mapper.Map<PermissionsEF>(input);

                _dataBaseDBContext.Permissions.Add(agendaBD);
                bool resultado = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return resultado;
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
            List<Permissions> resultado = _mapper.Map<List<Permissions>>(dataBD);

            foreach (var permission in resultado)
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

        #endregion

    }
}