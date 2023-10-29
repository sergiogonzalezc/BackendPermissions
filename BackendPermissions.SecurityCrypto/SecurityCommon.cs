using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MedicalWebScraping.SecurityCrypto
{
    public class SecurityCommon
    {
        //private static readonly byte[] arregloClave = ASCIIEncoding.ASCII.GetBytes("SGCSoft@");
        private static readonly byte[] arregloClave = ASCIIEncoding.ASCII.GetBytes("Dhg$3g@k");

        /// <summary>
        /// Método encargado de encriptar un texto, utilizando algoritmo DES (Data Encription Standard)
        /// </summary>
        /// <param name="cadenaAEncriptar"></param>
        public static string Encriptar(string cadenaAEncriptar)
        {
            if (String.IsNullOrEmpty(cadenaAEncriptar))
            {
                throw new ArgumentNullException("Para encriptar texto no puede ser nulo.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(arregloClave, arregloClave), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(cadenaAEncriptar);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Método encargado de desencriptar un texto, utilizando algoritmo DES (Data Encription Standard)
        /// </summary>
        /// <param name="cadenaADesencriptar"></param>
        /// <returns></returns>
        public static string Desencriptar(string cadenaADesencriptar)
        {
            if (String.IsNullOrEmpty(cadenaADesencriptar))
            {
                throw new ArgumentNullException("La cadena a desencriptar no puede ser nulo");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cadenaADesencriptar));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(arregloClave, arregloClave), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }


        /// <summary>
        /// Método encargado de obtener los datos de una licencia a raiz de una licencia NO encriptada
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static LicenceObject GetLicenceKey(string Input)
        {
            LicenceObject salida = null;

            try
            {
                if (string.IsNullOrEmpty(Input))
                    return null;
                else
                {
                    string[] elementos = Input.Split('-');

                    if (elementos == null)
                        return null;
                    else
                    {
                        salida = new LicenceObject();

                        /* 
                         * Estructura de una licencia, por ejemplo DESA-0001-U9999-M9999-C9999-V0-MR0000
                         * Separadas por guión:
                         * 
                         * Segmento número 1: Tipo de licencia:
                         *  
                            CL_ADV	    Licencia Cloud Avanzada
                            CL_BAS	    Licencia Cloud Básica
                            CL_STA	    Licencia Cloud Standar
                            DEMO_CL_ADV	Licencia Demo Cloud Avanzada
                            DEMO_SA_ADV	Licencia Demo Stand-Alone Avanzada
                            DESA	    Licencia de Desarrollo
                            SA_ADV	    Licencia Stand-Alone Avanzada
                            SA_BAS	    Licencia Stand-Alone Basica
                            SA_STA	    Licencia Stand-Alone Standar
                 
                         * Segmento número 2: Número de licencia.
                         * 
                         * Segmento número 3: Concurrencia (número de usuarios conectados con la misma liencia) (minimo 1, máximo 9999)
                         * 
                         * Segmento número 4: Meses de vigencia (minimo 0, máximo 9999 meses)
                         * 
                         * Segmento número 5: Comunidades soportadas por licencia (mínimo 1, máximo 9999)
                         * 
                         * Segmento número 6: Solicita validación al servidor en cada login (0 o 1). Por defecto es 0.
                         *  
                         * Segmento número 7: Códigos de características o funciones (opcional)
                         * 
                         **/
                        salida.Tipo = (Enumeradores.EnumTiposLicencia)Enum.Parse(typeof(Enumeradores.EnumTiposLicencia), elementos[0]);

                        // valida quer sean valores numéricos
                        if (!IsNumeric(elementos[1]) || !IsNumeric(elementos[2].Substring(1)) || !IsNumeric(elementos[3].Substring(1)) || !IsNumeric(elementos[4].Substring(1)) || !IsNumeric(elementos[5].Substring(1)))
                            return null;

                        salida.Numero = Convert.ToInt32(elementos[1]);
                        salida.UsuariosConcurrentes = Convert.ToInt32(elementos[2].Substring(1));

                        if (salida.UsuariosConcurrentes <= 0 || salida.UsuariosConcurrentes > 9999)
                            return null;

                        salida.MesesVigencias = Convert.ToInt32(elementos[3].Substring(1));

                        if (salida.MesesVigencias <= 0 || salida.MesesVigencias > 9999)
                            return null;

                        salida.NumeroComunidades = Convert.ToInt32(elementos[4].Substring(1));

                        if (salida.NumeroComunidades <= 0 || salida.NumeroComunidades > 9999)
                            return null;

                        if (Convert.ToInt32(elementos[5].Substring(1)) == 1)
                            salida.RequereValidacion = true;
                        else if (Convert.ToInt32(elementos[5].Substring(1)) == 0)
                            salida.RequereValidacion = false;
                        else
                            return null;

                        if (elementos[6] != null)
                            salida.CodigosFunciones = elementos[6];
                        else
                            salida.CodigosFunciones = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                //_bitacora.Error(ex, "Error al convertir objeto de licencia [" + Input + "]");
                throw ex;
            }

            return salida;
        }

        /// <summary>
        /// SGC 13-11-2016: Método encargado de obtener los datos de una licencia a raiz de una licencia Encriptada
        /// </summary>
        /// <param name="InputEncripted"></param>
        /// <returns></returns>
        public static LicenceObject GetLicenceKeyEncriptada(string InputEncripted)
        {
            string entrada = Desencriptar(InputEncripted);

            return GetLicenceKey(entrada);
        }

        /// <summary>
        /// Método que devuelve TRUE si la licencia es válida (estructura)
        /// </summary>
        /// <param name="InputEncripted"></param>
        /// <returns></returns>
        public static bool IsLicenceValid(string InputEncripted)
        {
            if (GetLicenceKeyEncriptada(InputEncripted) != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// SGC 13-11-2016: Método encargado de calcular la fecha fin de vigencia de una licencia
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static DateTime? GetLicenciaFechaFin(DateTime FechaInicio, string LicenceKey)
        {
            LicenceObject licencia = null;

            try
            {
                if (string.IsNullOrEmpty(LicenceKey))
                    return null;
                else
                {
                    licencia = GetLicenceKeyEncriptada(LicenceKey);

                    if (licencia != null)
                    {
                        if (licencia.MesesVigencias > 0)
                        {
                            DateTime dtFin;

                            if (FechaInicio < DateTime.MinValue)
                            {
                                throw new Exception("Error: fecha de inicio inválida!");
                                //return null;
                            }

                            if (FechaInicio.AddMonths(licencia.MesesVigencias) > DateTime.MaxValue)
                                dtFin = DateTime.MaxValue;
                            else
                                dtFin = FechaInicio.AddMonths(licencia.MesesVigencias);

                            return dtFin;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (licencia != null)
                    throw new Exception("Error calculando fecha fin de licencia. Fecha Inicio [" + FechaInicio + "] Meses por sumar [" + licencia.MesesVigencias + "]");
                else
                    throw new Exception("Error calculando fecha fin de licencia. Fecha Inicio [" + FechaInicio + "]");
            }

            return null;
        }

        /// <summary>
        /// Método que retorna TRUE si la expresion es NUMERICA
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        private static System.Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime || Expression is Boolean)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
    }
}
