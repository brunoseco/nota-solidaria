using Common.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using NFSolidaria.Core.Filters;
using NFSolidaria.Core.Dto;
using NFSolidaria.Core.Application;
using System.Threading.Tasks;

namespace NFSolidaria.Core.Api.Controllers
{
    public class UsuarioController : ApiController
    {
        private HttpResult<UsuarioDto> result;
		private HelperHttpLog httpLog;
		private UsuarioApp app;

        public UsuarioController()
        {
			this.httpLog = new HelperHttpLog();
            this.httpLog.LogRequestIni();
            this.result = new HttpResult<UsuarioDto>();
        }

				[ActionName("DefaultAction")]
		public HttpResponseMessage Get(int id)
		{
			var result = new HttpResult<UsuarioDto>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new UsuarioApp(token);
				var data = this.app.Get(new UsuarioDto { UsuarioId = id});
				this.app.Dispose();
				result.Success(data);
				return Request.CreateResponse(result.StatusCode, result);

			}
			catch (Exception ex)
			{
                result.ReturnCustomException(ex);
				return Request.CreateResponse(result.StatusCode, result);

			}

		}



		[ActionName("DefaultAction")]
		public HttpResponseMessage Get([FromUri]UsuarioFilter filters)
        {
			var result = new HttpResult<UsuarioDto>();

            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioApp(token);
				var searchResult = this.app.GetByFilters(filters);
                result.Summary = searchResult.Summary;
				result.Warnings = this.app.ValidationHelper.GetDomainWarning();
                result.Success(searchResult.DataList);
				this.app.Dispose();
                return Request.CreateResponse(result.StatusCode, result);

            }
            catch (Exception ex)
            {
                result.ReturnCustomException(ex, filters);
				return Request.CreateResponse(result.StatusCode, result);
            }
        }
						
		[ActionName("DefaultAction")]
		public HttpResponseMessage Post([FromBody]UsuarioDtoSpecialized model)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioApp(token);
                var returnModel = this.app.Save(model);
                this.app.Dispose();
				result.Warnings = this.app.ValidationHelper.GetDomainWarning();
				result.Confirms = this.app.ValidationHelper.GetDomainConfirms();
                result.Success(returnModel);
				this.httpLog.LogSerialize(model);
				return Request.CreateResponse(result.StatusCode, result);

            }
            catch (Exception ex)
            {
                result.ReturnCustomException(ex, model);
				return Request.CreateResponse(result.StatusCode, result);
            }

        }

		
		[ActionName("DefaultAction")]
		public HttpResponseMessage Put([FromBody]UsuarioDtoSpecialized model)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioApp(token);
                var returnModel = this.app.SavePartial(model);
                this.app.Dispose();
				result.Warnings = this.app.ValidationHelper.GetDomainWarning();
				result.Confirms = this.app.ValidationHelper.GetDomainConfirms();
                result.Success(returnModel);
				this.httpLog.LogSerialize(model);
				return Request.CreateResponse(result.StatusCode, result);

            }
            catch (Exception ex)
            {
                result.ReturnCustomException(ex, model);
				return Request.CreateResponse(result.StatusCode, result);
            }

        }
		
		[ActionName("DefaultAction")]
		public HttpResponseMessage Delete([FromUri]UsuarioDto filters)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioApp(token);
                this.app.Delete(filters);
				this.result.Warnings = this.app.ValidationHelper.GetDomainWarning();
                var result = this.result.Success();
                this.app.Dispose();
				return Request.CreateResponse(result.StatusCode, result);

            }
            catch (Exception ex)
            {
                result.ReturnCustomException(ex, filters);
				return Request.CreateResponse(result.StatusCode, result);
            }
        }
		
		[ActionName("GetDataListCustom")]
        public HttpResponseMessage GetDataListCustom([FromUri]UsuarioFilter filters)
		{
			var result = new HttpResult<object>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new UsuarioApp(token);
                var searchResult = this.app.GetDataListCustom(filters);
                result.Summary = searchResult.Summary;
				result.Warnings = this.app.ValidationHelper.GetDomainWarning();
                result.Success(searchResult.DataList);
				this.app.Dispose();
                return Request.CreateResponse(result.StatusCode, result);

			}
			catch (Exception ex)
			{
				result.ReturnCustomException(ex, filters);
				return Request.CreateResponse(result.StatusCode, result);
			}

		}
		
		[ActionName("GetDataItem")]
        public HttpResponseMessage GetDataItem([FromUri]UsuarioFilter filters)
		{
			var result = new HttpResult<object>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new UsuarioApp(token);
                var data = this.app.GetDataItem(filters);
				this.app.Dispose();
				result.Warnings = this.app.ValidationHelper.GetDomainWarning();
                result.Success(data);
				return Request.CreateResponse(result.StatusCode, result);

			}
			catch (Exception ex)
			{
				result.ReturnCustomException(ex, filters);
				return Request.CreateResponse(result.StatusCode, result);
			}

		}

		[ActionName("GetTotalByFilters")]
        public HttpResponseMessage GetTotalByFilters([FromUri]UsuarioFilter filters)
        {
            var result = new HttpResult<int>();

            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioApp(token);
                var searchResult = this.app.GetTotalByFilters(filters);
				result.Warnings = this.app.ValidationHelper.GetDomainWarning();
                result.Success(searchResult);
                this.app.Dispose();
                return Request.CreateResponse(result.StatusCode, result);

            }
            catch (Exception ex)
            {
                result.ReturnCustomException(ex, filters);
				return Request.CreateResponse(result.StatusCode, result);
            }
        }

		protected override void Dispose(bool disposing)
        {
			if (this.app.IsNotNull()) this.app.Dispose();
            this.httpLog.LogRequestEnd();
            base.Dispose(disposing);
        }
    }
}
