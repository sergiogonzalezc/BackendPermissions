namespace BackendPermissions.Api.Model
{
    public class Error
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public string? InformacionAdicional { get; set; }
    }

}
