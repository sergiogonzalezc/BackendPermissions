using BackendPermissions.Application.Model;

namespace BackendPermissions.Application.Interface
{
    public interface IPermissionsRepository
    {
        public Task<bool> InsertPermissions(Permissions argumentos);

        public Task<List<PermissionsDTO>> GetPermissions();      
    }
}