using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BackendPermissions.Application.Model;
using BackendPermissions.Infraestructura;
using BackendPermissions.Application.Interface;
using BackendPermissions.Api.Model;
using static BackendPermissions.Common.Enum;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Utilities;
using System.Configuration;
using BackendPermissions.Application.Business;
using MySqlX.XDevAPI.Common;
using BackendPermissions.Common;
using System.ComponentModel.Design;
using MediatR;
using BackendPermissions.Application.Querys;
using Microsoft.AspNetCore.Cors;
using BackendPermissions.Application.Commands;
using BackendPermissions.Application.Handlers;
using Nest;
using MySqlX.XDevAPI;
using System;
using Confluent.Kafka;

namespace BackendPermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsApplication _permissionsService;
        private readonly ILogger<PermissionsController> _logger;
        private readonly IMediator _mediator;
        private readonly ElasticClient _elasticClient;

        private BackendPermissions.Api.Model.Error err = new BackendPermissions.Api.Model.Error
        {
            Codigo = StatusCodes.Status400BadRequest
        };

        public PermissionsController(IPermissionsApplication permissionsServices, ILogger<PermissionsController> logger, IMediator mediator, ElasticClient elasticClient)
        {
            _permissionsService = permissionsServices;
            _logger = logger;
            //_producer = producer;
            _mediator = mediator;
            _elasticClient = elasticClient;
        }


        [HttpGet]
        [Route("GetPermissions")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.Permissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionsModel> GetPermissions()
        {
            string nameMethod = nameof(GetPermissions);
            try
            {
                _logger.LogInformation("Start...");

                //var response = _elasticClient.Indices.Create(IndexName,
                //    index => index.Map<ElasticsearchDocument>(
                //        x => x.AutoMap()
                //    ));

                // Implement a CQRS for query/command responsibility segregation
                var query = new GetPermissionsQuerys();
                List<PermissionsDTO> result = await _mediator.Send(query);

                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Get.ToString());

                var finalResult = new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    DataList = result.ToList(),
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        [HttpGet]
        [Route("GetPermissionById")]
        [ProducesResponseType(typeof(BackendPermissions.Application.Model.Permissions), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionsModel> GetPermissionById(int id)
        {
            string nameMethod = nameof(GetPermissions);
            try
            {
                _logger.LogInformation("Start...");
                
                // Implement a CQRS for query/command responsibility segregation
                var query = new GetPermissionByIdQuerys(id);
                PermissionsDTO result = await _mediator.Send(query);

                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Get.ToString());

                var finalResult = new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    Data = result,
                    DataList = null,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionsModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// Validate a Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ValidatePermission")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ValidatePermisionModel> ValidatePermission([FromBody] InputRequestPermission input)
        {
            string nameMethod = nameof(ValidatePermission);

            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new GetValidatePermissionQuery(input.NombreEmpleado, input.ApellidoEmpleado);
                bool permissionStatus = await _mediator.Send(query);

                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Request.ToString());

                var finalResult = new ValidatePermisionModel
                {
                    Success = permissionStatus,
                    Message = permissionStatus ? "ok" : "error",
                    Data = null,
                    DataList = null,
                };

                return finalResult;
            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new ValidatePermisionModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;

                return new ValidatePermisionModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        /// <summary>
        /// Modify Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ModifyPermission")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyPermission([FromBody] InputModifyPermission input)
        {
            string nameMethod = nameof(ModifyPermission);

            try
            {
                // Implement a CQRS for query/command responsibility segregation
                var query = new ModifyPermissionCommand(input);
                bool result = await _mediator.Send(query);

                // log message in kafka
                _ = ProducerEventKafka.SendProducerEvent(Guid.NewGuid().ToString(), Common.Enum.CallType.Modify.ToString());

                _logger.LogInformation($"Modifiy: {result}");

                return Ok(result);
            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;
                return StatusCode(StatusCodes.Status206PartialContent, err);
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                return BadRequest(err);
            }
        }

        /// <summary>
        /// Get the full permission type List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionTypes")]
        [ProducesResponseType(typeof(List<BackendPermissions.Application.Model.PermissionTypes>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionTypesModel> GetPermissionTypes()
        {
            string nameMethod = nameof(GetPermissionTypes);
            try
            {
                _logger.LogInformation("Start...");

                // Implement a CQRS for query/command responsibility segregation
                var query = new GetPermissionTypesQuerys();
                List<PermissionTypes> result = await _mediator.Send(query);

                var finalResult = new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    DataList = result,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        /// <summary>
        /// Get the permission type List by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPermissionTypeById")]
        [ProducesResponseType(typeof(BackendPermissions.Application.Model.PermissionTypes), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<PermissionTypesModel> GetPermissionTypeById(int id)
        {
            string nameMethod = nameof(GetPermissionTypes);
            try
            {
                _logger.LogInformation("Start...");

                // Implement a CQRS for query/command responsibility segregation
                var query = new GetPermissionTypeByIdQuerys(id);
                PermissionTypes result = await _mediator.Send(query);

                var finalResult = new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Succes.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Succes.ToString(),
                    Success = true,
                    Message = "ok",
                    Data = result,
                };

                return finalResult;

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameMethod, "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = arEx.Message
                };
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameMethod, "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                //return BadRequest(err);

                return new PermissionTypesModel
                {
                    Status = Common.Enum.EnumMessage.Error.ToString(),
                    SubStatus = Common.Enum.EnumMessage.Error.ToString(),
                    Success = false,
                    Message = ex.Message
                };
            }

        }

        /// <summary>
        /// Insert a new Permission
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertNewPermission")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> InsertNewPermission([FromBody] InputCreatePermission input)
        {
            try
            {
                var asyncIndexResponse = await _elasticClient.IndexDocumentAsync(input);

                var searchResponse = _elasticClient.Search<PermissionsDTO>(s => s
                                        .AllIndices()
                                        .From(0)
                                        .Size(10)
                                        .Query(q => q
                                             .Match(m => m
                                                .Field(f => f.NombreEmpleado)
                                                .Query("Luis")
                                             )
                                        )
                                    );

                var people = searchResponse.Documents;

                // Implement a CQRS for query/command responsibility segregation

                var query = new InsertNewPermissionCommand(input);
                ResultInsertPermissionDTO result = await _mediator.Send(query);

                if (result == null)
                    return BadRequest(result);

                _logger.LogInformation($"Insert New Permission: {result}");

                return Ok(result);

            }
            catch (ArgumentException arEx)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, arEx, nameof(InsertNewPermission), "Error!");

                err.Codigo = StatusCodes.Status206PartialContent;
                err.Mensaje = arEx.Message;
                err.InformacionAdicional = arEx.ParamName;
                return StatusCode(StatusCodes.Status206PartialContent, err);
            }
            catch (Exception ex)
            {
                ServiceLog.Write(Common.Enum.LogType.WebSite, ex, nameof(InsertNewPermission), "Error!");

                err.Mensaje = ex.Message;
                err.InformacionAdicional = ex.GetBaseException().Message;
                return BadRequest(err);
            }
        }
    }
}
