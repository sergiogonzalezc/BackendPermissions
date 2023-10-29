using MedicalWebScraping.Application.ConfiguracionApi;
using MedicalWebScraping.Application.Model;
using MedicalWebScraping.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalWebScraping.Application.Business
{
    public class ScheduleBusiness
    {
        private readonly Serilog.Core.Logger _logger;

        public ScheduleBusiness(Serilog.Core.Logger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary
        /// <param name="MedicalCenterId"></param>
        /// <param name="agendaList"></param>
        /// <param name="sucursalItemData"></param>
        /// <param name="speciality"></param>
        /// <param name="doctorName"></param>
        /// <param name="firsthourData"></param>
        /// <returns></returns>
        public async Task<(List<AgendaViewModel>, List<Especialidad>, List<Medico>)> InsertElement(ServiceProvider serviceProvider, int MedicalCenterId, List<AgendaViewModel> agendaList, List<Especialidad> _especialidadList, List<Medico> _medicoList, Sucursal sucursalItemData, string speciality, string doctorName, string firsthourData)
        {
            //private static List<Especialidad> _especialidadList = new List<Especialidad>();
            //private static List<Medico> _medicoList = new List<Medico>();

            // Si no está en la lista, lo agrega
            (List<Especialidad>, Especialidad) specialityOutPutList = ExtractSpeciality(_especialidadList, speciality);
            _especialidadList = specialityOutPutList.Item1;

            (List<Medico>, Medico) medicOutPutList = ExtractMedic(_medicoList, doctorName);
            _medicoList = medicOutPutList.Item1;

            AgendaViewModel _itemAgenda = new AgendaViewModel();
            //_itemAgenda.Id = new Guid.NewGuid().ToString();

            _itemAgenda.IdCentroMedico = MedicalCenterId;
            _itemAgenda.Sucursal = sucursalItemData;   // Si sucursal = 99 => "Telemedicina", sino es "Presencial"

            _itemAgenda.Especialidad = specialityOutPutList.Item2;
            _itemAgenda.Medico = medicOutPutList.Item2;

            try
            {
                if (!string.IsNullOrEmpty(firsthourData))
                {
                    _itemAgenda.ProximaHoraText = firsthourData;
                    _itemAgenda.ProximaHora = MedicalWebScraping.Common.Util.ConvertDate(firsthourData.Trim());
                    _itemAgenda.TieneHoraDisponible = true;
                }
                else
                {
                    _itemAgenda.ProximaHoraText = string.Empty;
                    _itemAgenda.ProximaHora = null;
                    _itemAgenda.TieneHoraDisponible = false;
                }

                if (agendaList == null)
                    agendaList = new List<AgendaViewModel>();

                agendaList.Add(_itemAgenda);

                ArgumentosCrearAgenda newAgenda = new ArgumentosCrearAgenda()
                {
                    IdCentroMedico = MedicalCenterId,
                    DataSucursal = _itemAgenda.Sucursal,
                    DataEspecialidad = _itemAgenda.Especialidad,
                    DataMedico = _itemAgenda.Medico,
                    ProximaHoraText = _itemAgenda.ProximaHoraText,
                    ProximaHora = _itemAgenda.ProximaHora,
                    TieneHoraDisponible = _itemAgenda.TieneHoraDisponible,
                    FechaLectura = DateTime.Now,
                    UsuarioCreacion = "ROBOT_AUTO",
                    Vigente = true
                };

                // Consume Api para insertar registro
                string url = "http://localhost:8000/api/";

                //string token = GetToken("13.997.721-1", FinWork.Business.Configuration.SecretKey.Value);

                //if (string.IsNullOrEmpty(token))
                //{
                //    throw new System.Exception("token de autorización no existe");
                //}
                if (true)
                {
                    //ScheduleApplication service = ScheduleApplication.
                    ScheduleApplication scheduleEngine = (ScheduleApplication)serviceProvider.GetService(typeof(MultiRobot.Application.Main.OC_BuyOrderApplication));

                }
                else
                {
                    Api apiAgenda = new Api(url, CallType.EnumCallType.Post);
                    HttpResponseMessage response = await apiAgenda.CallApi("Schedule/InsertSchedule", newAgenda);

                    if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                    {
                        _logger.Information($"Insertado OK - Sucursal Id [{sucursalItemData.Id}] Doctor [{doctorName} / {speciality} / {firsthourData}");
                        //Logger.Write(LogType.serverSite, System.Diagnostics.TraceLevel.Info, method, "DTE externo aceptado OK: AEC_ALERT_ID [" + aecAlert.Id + "] Code [" + response.StatusCode + "] Content [" + (response.Content == null ? "NULL" : "documento correcto" + "]"));

                        //return new Models.AECAlert
                        //{

                        //    Data = aecAlert,
                        //    Status = Business.Enumerators.EMessage.Message.Succes.ToString(),
                        //    SubStatus = Business.Enumerators.EMessage.Message.Succes.ToString(),
                        //    Success = true,
                        //    Message = "Factura aprobada con éxito",
                        //};
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            _logger.Error($"Respuesta Error (bad request)! Registro No Insertado! - Sucursal Id [{sucursalItemData.Id}] - " + doctorName + "/" + speciality + "/" + firsthourData + " - Code[" + response.StatusCode + "] Content[" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            _logger.Error($"Respuesta Error (no autorizado)!: Registro No Insertado! - Sucursal Id [{sucursalItemData.Id}] - " + doctorName + "/" + speciality + "/" + firsthourData + " - Code[" + response.StatusCode + "] Content[" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            _logger.Error($"Respuesta Error (InternalServerError)!: Registro No Insertado! - Sucursal Id [{sucursalItemData.Id}] - " + doctorName + "/" + speciality + "/" + firsthourData + " - Code[" + response.StatusCode + "] Content[" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));

                        }
                        //else
                        //Logger.Write(LogType.serverSite, System.Diagnostics.TraceLevel.Error, method, "Respuesta Error!: AEC_ALERT_ID [" + aecAlert.Id + "] Code [" + response.StatusCode + "] Content [" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));

                        //return new Models.AECAlert
                        //{

                        //    Data = aecAlert,
                        //    Status = Business.Enumerators.EMessage.Message.Succes.ToString(),
                        //    SubStatus = Business.Enumerators.EMessage.Message.Succes.ToString(),
                        //    Success = false,
                        //    Message = "Error al aprobar factura",
                        //};
                    }
                }

                //var resultado = await _servicioAgenda.Insertar(newAgenda);
                //if (resultado)
                //    _logger.Information("Insertado OK - " + doctorName.Text + "/" + specialidadMedico.Text + "/" + firsthourData.Text);
                //else
                //    _logger.Error("Registro No Insertado! - " + doctorName.Text + "/" + specialidadMedico.Text + "/" + firsthourData.Text);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error procesando registro -  Sucursal Id [{sucursalItemData.Id}] - " + doctorName + "/" + speciality + " - Fecha/Hora: " + firsthourData + " -STACK - " + ex.StackTrace?.ToString());
            }

            return (agendaList, _especialidadList, _medicoList);
        }


        private (List<Especialidad>, Especialidad) ExtractSpeciality(List<Especialidad> especialidadList, string newSpeciality)
        {
            // Si la lista está vacia, lo agrega y devuelve la lista.             
            // O también si la lista tiene elementos, pero el nuevo elemento no está en la lista, lo agrega y devuelve la lista
            if (especialidadList == null || !especialidadList.Any() || !especialidadList.Exists(x => x.Descripcion.Equals(newSpeciality)))
            {
                especialidadList = new List<Especialidad>();
                Especialidad newItem = new Especialidad() { Id = 1, Descripcion = newSpeciality, FechaCreacion = DateTime.Now };
                especialidadList.Add(newItem);
                return (especialidadList, newItem);
            }
            else   // Si ya existe el elemento dentro de la lista, retorna la misma lista ya existente y no lo agrega.
            {
                return (especialidadList, especialidadList.Where(x => x.Descripcion.Equals(newSpeciality)).OrderByDescending(x => x.FechaCreacion).Take(1).SingleOrDefault());
            }
        }


        private (List<Medico>, Medico) ExtractMedic(List<Medico> medicList, string doctorName)
        {
            // Si la lista está vacia, lo agrega y devuelve la lista.             
            // O también si la lista tiene elementos, pero el nuevo elemento no está en la lista, lo agrega y devuelve la lista
            if (medicList == null || !medicList.Any() || !medicList.Exists(x => x.NombreApellido.Equals(doctorName)))
            {
                medicList = new List<Medico>();
                Medico newItem = new Medico() { Id = 1, NombreApellido = doctorName, FechaCreacion = DateTime.Now };
                medicList.Add(newItem);
                return (medicList, newItem);
            }
            else   // Si ya existe el elemento dentro de la lista, retorna la misma lista ya existente y no lo agrega.
            {
                return (medicList, medicList.Where(x => x.NombreApellido.Equals(doctorName)).OrderByDescending(x => x.FechaCreacion).Take(1).SingleOrDefault());
            }
        }



    }
}
