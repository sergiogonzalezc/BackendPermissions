using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalWebScraping.SecurityCrypto
{
    public class LicenceObject
    {
        public int Numero { get; set; }
        public Enumeradores.EnumTiposLicencia Tipo { get; set; }
        public int UsuariosConcurrentes { get; set; }
        public int NumeroComunidades { get; set; }
        public int MesesVigencias { get; set; }
        public bool RequereValidacion { get; set; }
        public string CodigosFunciones { get; set; }
    }
}
