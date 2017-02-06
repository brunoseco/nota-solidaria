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
    public class CadastradorController : ApiController
    {
        private HttpResult<CadastradorDto> result;
		private HelperHttpLog httpLog;
		private CadastradorApp app;

        public CadastradorController()
        {
			this.httpLog = new HelperHttpLog();
            this.httpLog.LogRequestIni();
            this.result = new HttpResult<CadastradorDto>();
        }

				[ActionName("DefaultAction")]
		public HttpResponseMessage Get(int id)
		{
			var result = new HttpResult<CadastradorDto>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new CadastradorApp(token);
				var data = this.app.Get(new CadastradorDto { CadastradorId = id});
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
		public HttpResponseMessage Get([FromUri]CadastradorFilter filters)
        {
			var result = new HttpResult<CadastradorDto>();

            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new CadastradorApp(token);
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
		public HttpResponseMessage Post([FromBody]CadastradorDtoSpecialized model)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new CadastradorApp(token);
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
		public HttpResponseMessage Put([FromBody]CadastradorDtoSpecialized model)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new CadastradorApp(token);
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
		public HttpResponseMessage Delete([FromUri]CadastradorDto filters)
        {
            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new CadastradorApp(token);
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
        public HttpResponseMessage GetDataListCustom([FromUri]CadastradorFilter filters)
		{
			var result = new HttpResult<object>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new CadastradorApp(token);
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
        public HttpResponseMessage GetDataItem([FromUri]CadastradorFilter filters)
		{
			var result = new HttpResult<object>();

			try
			{
				var token = HelperAuth.GetHeaderToken();
				this.app = new CadastradorApp(token);
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
        public HttpResponseMessage GetTotalByFilters([FromUri]CadastradorFilter filters)
        {
            var result = new HttpResult<int>();

            try
            {
                var token = HelperAuth.GetHeaderToken();
                this.app = new CadastradorApp(token);
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
