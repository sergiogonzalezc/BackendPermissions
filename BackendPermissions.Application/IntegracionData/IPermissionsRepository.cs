using BackendPermissions.Application.Model;

namespace BackendPermissions.Application.Interface
{
    public interface IPermissionsRepository
    {
        public Task<bool> InsertPermissions(Permissions input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> ModifyPermissions(Permissions input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<List<PermissionsDTO>> GetPermissions();

        /// <summary>
        /// Validate if exists te same permission for the employee
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public Task<bool> ExistsPermissionByNameAndType(string name, string lastName, int permissionType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public Task<PermissionsDTO> GetPermissionsByName(string name, string lastName, int permissionType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PermissionsDTO> GetPermissionsById(int id);

        /// <summary>
        /// Get the Permission Type By Id
        /// </summary>
        /// <returns></returns>
        public Task<PermissionTypeDTO> GetPermissionTypeById(int id);
    }
}