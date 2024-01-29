using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Sabio.Models.Requests;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Sabio.Web.Controllers;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.ExternalLinks;
using System.Runtime.InteropServices;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/links")]
    [ApiController]
    public class ExternalLinkApiController : BaseApiController
    {
        private IExternalLinkService _service = null;
        private IAuthenticationService<int> _authService = null;
        private ILookUpService _lookUpService = null;

        public ExternalLinkApiController(IExternalLinkService service
            , ILogger<ExternalLinkApiController> logger, IAuthenticationService<int> authService, ILookUpService lookUp) : base(logger)
        {
            _service = service;
            _authService = authService;
            _lookUpService = lookUp;
        }

        #region Delete
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        #endregion

        #region Get
        [HttpGet]
        public ActionResult<ItemsResponse<List<ExternalLink>>> GetSelectByCreatedBy()
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                List<ExternalLink> list = _service.GetSelectByCreatedBy(userId);

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<ExternalLink> { Items = list };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error:{ex.Message}");
            }

            return StatusCode(iCode, response);
        }

        #endregion


        #region Update
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(ExternalLinkUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        #endregion


        #region Insert/Create
        [HttpPost("createnewlink")]
        public ActionResult<ItemResponse<int>> Create(ExternalLinkAddRequest model)
        {
            ObjectResult result = null;

            try
            {

                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }
        #endregion

    }
}
