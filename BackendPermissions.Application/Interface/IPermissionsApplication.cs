using BackendPermissions.Application.Model;

namespace BackendPermissions.Application.Interface
{
    public interface IPermissionsApplication
    {
        Task<bool> InsertPermissions(InputCreatePermission input);

        Task<List<PermissionsDTO>> GetPermissions();      
    }
}