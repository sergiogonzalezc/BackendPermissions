using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using static MedicalWebScraping.Common.Enum;
using MedicalWebScraping.Application.Model;
using Microsoft.Extensions.Configuration;

namespace MedicalWebScraping.Application.Business
{
    public class LoginBusiness
    {
        public LoginBusiness()
        {

        }

        public static string GetToken(IConfiguration configuration, string email, string secretKey)
        {
            List<Claim> ListClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("email", email),
                //new Claim(MultiGarantias2.Business.Enumerators.EnumClaims.EClaims.rutUsuario.ToString(), rutUsuario ?? string.Empty)
            };

            SymmetricSecurityKey key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            EncryptingCredentials encryptingCredentials = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);

            var jwt = configuration.GetSection("JwtConfig").Get<JwtConfig>();

            JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                subject: new ClaimsIdentity(ListClaims),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(24),
                issuedAt: DateTime.Now,
                signingCredentials: creds,
                encryptingCredentials: encryptingCredentials,
                claimCollection: ListClaims.ToDictionary(k => k.Type, v => (object)v.Value)
                );

            string encryptedJWT = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return encryptedJWT;
        }

        /// <summary>
        /// Genera login anonimo (login que usa el sitio web)
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="email"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static Usuario GetLogin(IConfiguration configuration, string email, string encodedPass, string secretKey)
        {
            //rutEmpresa = Helper.HelperClass.FormatRut(rutEmpresa);

            //CuentaAcceso cuentaAcceso = CuentaAcceso.Get(context, rutEmpresa);

            //Proveedor proveedor = Proveedor.Get(context, rutEmpresa);


            //if (cuentaAcceso == null || proveedor == null)
            //{
            //    return new User
            //    {
            //        StatusUser = StatusUser.UserDoesNotExist,
            //        Token = null
            //    };
            //}

            if (!email.Equals("sergio.gonzalez.c@gmail.com"))
            {
                return new Usuario
                {
                    StatusUser = StatusUser.UserDoesNotExist,
                    Token = null
                };
            }

            if (!encodedPass.Equals("g8u7Lkps5Vfgj4iu7Rkj7vPzfRCmOOnU"))
            {
                return new Usuario
                {
                    StatusUser = StatusUser.UserDoesNotExist,
                    Token = null
                };
            }

            //string millionRut = proveedor.Rut.Substring(0, proveedor.Rut.IndexOf("."));

            //bool isNaturalPerson = Convert.ToInt32(millionRut) <= 50;

            //if (cuentaAcceso == null || proveedor == null)
            //{
            //    return new User
            //    {
            //        StatusUser = StatusUser.UserDoesNotExist,
            //        Token = null
            //    };
            //}

            //if (!cuentaAcceso.Habilitado)
            //{
            //    return new User
            //    {
            //        StatusUser = StatusUser.UserDisabled,
            //        Token = null
            //    };
            //}

            // Registra su ultimo acceso
            //cuentaAcceso.UltimaFechaIngreso = DateTime.Now;
            //cuentaAcceso.Save(context);
            //context.SaveChanges();

            return new Usuario
            {
                Nombre = "",
                Fono = "",
                Email = "",
                Direccion = "",
                FirstLogin = true,

                Token = GetToken(configuration, email, secretKey),
                StatusUser = StatusUser.ValidUser,

            };
        }


        /// <summary>
        /// Genera login anonimo (login que usa el sitio web)
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="email"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static Usuario Register(IConfiguration configuration, string email, string encodedPass)
        {
            //rutEmpresa = Helper.HelperClass.FormatRut(rutEmpresa);
                        
            //Proveedor proveedor = Proveedor.Get(context, rutEmpresa);


            //if (cuentaAcceso == null || proveedor == null)
            //{
            //    return new User
            //    {
            //        StatusUser = StatusUser.UserDoesNotExist,
            //        Token = null
            //    };
            //}

            return new Usuario
            {
                Nombre = "",
                Fono = "",
                Email = "",
                Direccion = "",
                FirstLogin = true,                
                StatusUser = StatusUser.ValidUser,
            };
        }
    }
}
