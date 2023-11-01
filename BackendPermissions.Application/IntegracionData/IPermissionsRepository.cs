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
        public Task<bool> ExistsPermissionByNameAndType(string name, string lastName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        public Task<PermissionsDTO> GetPermissionsByName(string name, string lastName);

        /// <summary>
        /// Get the permission type List filter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PermissionsDTO> GetPermissionsById(int id);


        /// <summary>
        /// Get the full permission type List
        /// </summary>
        /// <returns></returns>
        public Task<List<PermissionTypes>> GetPermissionTypes();

        /// <summary>
        /// Get the Permission Type By Id
        /// </summary>
        /// <returns></returns>
        public Task<PermissionTypes> GetPermissionTypeById(int id);

    }
}