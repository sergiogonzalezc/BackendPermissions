using BackendPermissions.Application.Model;

namespace BackendPermissions.Application.Interface
{
    public interface IPermissionsApplication
    {
        Task<bool> RequestPermission(InputRequestPermission input);

        Task<bool> InsertNewPermission(InputCreatePermission input);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> ModifyPermission(InputModifyPermission input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionsDTO>> GetPermissions();

        /// <summary>
        /// Validate if exists te same permission for the employee
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        Task<bool> ExistsPermissionByNameAndType(string name, string lastName, int permissionType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        Task<PermissionsDTO> GetPermissionsByName(string name, string lastName, int permissionType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PermissionsDTO> GetPermissionsById(int id);
    }
}