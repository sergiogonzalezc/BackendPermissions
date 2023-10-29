using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackendPermissions.Application.ConfiguracionApi
{
    public class Api
    {
        public CallType.EnumCallType Call
        {
            get;
            set;
        }

        private string EndPoint
        {
            get;
            set;
        }

        public Api(string endPonit, CallType.EnumCallType callType)
        {
            EndPoint = endPonit;

            Call = callType;
        }

        /// <summary>
        /// Ejecuta el llamado asíncrona a una API
        /// </summary>
        /// <param name="shortUrl"></param>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <param name="logginRequestParam">0: No logea parametros ni llamado a api / 1: Logea llamado a la API con todos los PERO Sin el atributo Content / 2: Logea llamado a la API con todos los parámetros incluyedo el atributo Content</param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> CallApi(string shortUrl, object? content = null, string token = null, int logginRequestParam = 0, string methodName = "INFO", FormUrlEncodedContent FormData = null)
        {
            string url = string.Empty;
            string typeCall = string.Empty;
            string jsonObject = string.Empty;

            try
            {
                HttpResponseMessage response;

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(5);

                    url = string.Format("{0}{1}", EndPoint, shortUrl);

                    client.BaseAddress = new Uri(EndPoint);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    HttpRequestMessage request = new HttpRequestMessage(CallType.EnumCallType.Post.Equals(Call) ? HttpMethod.Post : HttpMethod.Get, url);

                    if (FormData != null)
                    {
                        request.Content = FormData;
                    }
                    else
                    {
                        if (content != null)
                        {
                            jsonObject = JsonConvert.SerializeObject(content);

                            request.Content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                        }
                    }



                    // SGC 24-05-2021 - Registra en log el llamado a la API
                    //if (logginRequestParam != 0)
                    //RecordParamRequest(request, client, logginRequestParam, "CallApi --> " + methodName); // SGC 21-01-2021 - Registra los parámetros de entrada en el log

                    if (CallType.EnumCallType.Get.Equals(Call))
                    {
                        typeCall = "GetAsync";
                        response = await client.GetAsync(url);
                    }
                    else
                    {
                        typeCall = "PostAsync";
                        response = await client.PostAsync(url, request.Content);

                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                // SGC 24-05-2021 - Se logea el error del llamado de la api, y URL y Endpoint
                //Logger.Write(LogType.serverSite, ex, "ERROR", "CallApi --> " + methodName + " - Error consumiendo Api [" + url + "] logginRequestParam [" + logginRequestParam + "] typeCall [" + typeCall + "] jsonObject [" + jsonObject + "]");

                throw;
            }
        }

        /// <summary>
        /// SGC 24-05-2021 - Registra los parámetros de un request a una API
        /// </summary>
        /// <param name="request"></param>
        /// <param name="restClient"></param>
        /// <param name="logginRequestParam">0: No logea parametros ni llamado a api / 1: Logea llamado a la API con todos los PERO Sin el atributo Content / 2: Logea llamado a la API con todos los parámetros incluyedo el atributo Content</param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        //private bool RecordParamRequest(HttpRequestMessage request, HttpClient restClient, int logginRequestParam = 0, string methodName = "INFO")
        //{
        //    try
        //    {
        //        if (request != null)
        //        {
        //            if (logginRequestParam != 0) // logea
        //            {
        //                // SGC 24-05-2021 - Logea todo los parámetros PERO si el atributo Content
        //                if (logginRequestParam == 1)
        //                {
        //                    var requestToLog = new
        //                    {
        //                        method = request.Method.ToString(),
        //                        uri = request.RequestUri,
        //                    };

        //                    Logger.Write(LogType.serverSite, System.Diagnostics.TraceLevel.Info, methodName, "Call API_C1: Request --> [" + JsonConvert.SerializeObject(requestToLog) + "]");

        //                }
        //                else
        //                {
        //                    // SGC 24-05-2021 - Logea todo los parámetros incluido el atributo Content
        //                    if (request.Content != null)
        //                    {
        //                        var requestToLog = new
        //                        {
        //                            content = (request.Content != null ? request.Content.ToString() : ""),
        //                            method = request.Method.ToString(),
        //                            uri = request.RequestUri,
        //                        };

        //                        Logger.Write(LogType.serverSite, System.Diagnostics.TraceLevel.Info, methodName, "Call API_C2: Request --> [" + JsonConvert.SerializeObject(requestToLog) + "]");

        //                    }
        //                    else
        //                    {
        //                        var requestToLog = new
        //                        {
        //                            method = request.Method.ToString(),
        //                            uri = request.RequestUri,
        //                        };

        //                        Logger.Write(LogType.serverSite, System.Diagnostics.TraceLevel.Info, methodName, "Call API_C3: Request --> [" + JsonConvert.SerializeObject(requestToLog) + "]");
        //                    }
        //                }
        //            }

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Write(LogType.serverSite, ex, "ERROR LOG REQUEST API_C", "Error logeado parametros");
        //    }

        //    return false;
        //}
    }

    public class CallType
    {
        public enum EnumCallType
        {
            Get,
            Post,
            Put
        }
    }

}
