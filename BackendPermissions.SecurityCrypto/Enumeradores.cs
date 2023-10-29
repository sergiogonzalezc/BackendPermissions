using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalWebScraping.SecurityCrypto
{
    public class Enumeradores
    {
        /*
         * IdTipoLicencia	Descripcion
            CL_ADV	Licencia Cloud Avanzada
            CL_BAS	Licencia Cloud Básica
            CL_STA	Licencia Cloud Standar
            DEMO_CL_ADV	Licencia Demo Cloud Avanzada
            DEMO_SA_ADV	Licencia Demo Stand-Alone Avanzada
            DESA	Licencia de Desarrollo
            SA_ADV	Licencia Stand-Alone Avanzada
            SA_BAS	Licencia Stand-Alone Basica
            SA_STA	Licencia Stand-Alone Standar
         */

        public enum EnumTiposLicencia
        {
            CL_ADV,
            CL_BAS,
            CL_STA,
            DEMO_CL_ADV,
            DEMO_SA_ADV,
            DESA,
            SA_ADV,
            SA_BAS,
            SA_STA
        }
    }
}
