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
    public class UsuarioEntidadeFavoritaController : ApiController
    {
        private HttpResult<UsuarioEntidadeFavoritaDto> result;
		private HelperHttpLog httpLog;
		private UsuarioEntidadeFavoritaApp app;

        public UsuarioEntidadeFavoritaController()
        {
			this.httpLog = new HelperHttpLog();
            this.httpLog.LogRequestIni();
            this.result = new HttpResult<UsuarioEntidadeFavoritaDto>();
        }

		

		[ActionName("DefaultAction")]
		public HttpResponseMessage Get([FromUri]UsuarioEntidadeFavoritaFilter filters)
        {
			var result = new HttpResult<UsuarioEntidadeFavoritaDto>();

            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioEntidadeFavoritaApp(token);
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
		public HttpResponseMessage Post([FromBody]UsuarioEntidadeFavoritaDtoSpecialized model)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioEntidadeFavoritaApp(token);
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
		public HttpResponseMessage Put([FromBody]UsuarioEntidadeFavoritaDtoSpecialized model)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioEntidadeFavoritaApp(token);
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
		public HttpResponseMessage Delete([FromUri]UsuarioEntidadeFavoritaDto filters)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioEntidadeFavoritaApp(token);
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
        public HttpResponseMessage GetDataListCustom([FromUri]UsuarioEntidadeFavoritaFilter filters)
		{
			var result = new HttpResult<object>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new UsuarioEntidadeFavoritaApp(token);
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
        public HttpResponseMessage GetDataItem([FromUri]UsuarioEntidadeFavoritaFilter filters)
		{
			var result = new HttpResult<object>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new UsuarioEntidadeFavoritaApp(token);
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
        public HttpResponseMessage GetTotalByFilters([FromUri]UsuarioEntidadeFavoritaFilter filters)
        {
            var result = new HttpResult<int>();

            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new UsuarioEntidadeFavoritaApp(token);
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
