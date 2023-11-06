using BackendPermissions.Application.Model;
using Nest;

namespace BackendPermissions.Application.Interface
{
    public interface IPermissionsApplication
    {
        Task<bool> GetValidatePermission(InputValidatePermission input);

        public Task<ResultRequestPermissionDTO> RequestPermission(ElasticClient elasticClient, InputCreatePermission input);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> ModifyPermission(ElasticClient elasticClient, InputModifyPermission input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionsDTO>> GetPermissions(ElasticClient elasticClient);

        /// <summary>
        /// Validate if exists te same permission for the employee
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        Task<bool> ExistsPermissionByNameAndType(string name, string lastName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="permissionType"></param>
        /// <returns></returns>
        Task<PermissionsDTO> GetPermissionsByName(string name, string lastName);

        /// <summary>
        /// Get the permission type List filter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PermissionsDTO> GetPermissionsById(int id);

        /// <summary>
        /// Get the full permission type List
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionTypes>> GetPermissionTypes();

        /// <summary>
        /// Get the permission type List by Id
        /// </summary>
        /// <returns></returns>
        Task<PermissionTypes> GetPermissionTypeById(int id);

    }
}