using Common.Models;
using Common.Domain;
using Common.Domain.CustomExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Infrastructure.Log;
using Newtonsoft.Json;


namespace Common.API
{
    public abstract class HttpResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public Summary Summary { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<string> Warnings { get; set; }
        public IEnumerable<ValidationConfirm> Confirms { get; set; }

    }
    public class HttpResult<T> : HttpResult
    {

        public HttpResult()
        {
            base.Summary = new Summary();
        }

        public IEnumerable<T> DataList { get; set; }
        public T Data { get; set; }


        public HttpResult<T> Success(T data)
        {
            this.StatusCode = HttpStatusCode.OK;
            this.Data = data;
            return this;

        }

        public HttpResult<T> Success(IEnumerable<T> dataList)
        {
            this.StatusCode = HttpStatusCode.OK;
            this.DataList = dataList;
            return this;

        }

        public HttpResult<T> Success()
        {
            this.StatusCode = HttpStatusCode.OK;
            return this;
        }

        public HttpResult<T> Error(IList<ValidationResult> erros)
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
            this.Errors = erros.Select(_ => _.ErrorMessage);

            return this;
        }

        public HttpResult<T> Error(string erro)
        {
            this.StatusCode = HttpStatusCode.InternalServerError;
            this.Errors = new List<string> { erro };
            return this;
        }

        private HttpResult<T> BadRequest(string erro)
        {
            this.StatusCode = HttpStatusCode.BadRequest;
            this.Errors = new List<string> { erro };
            return this;
        }

        private HttpResult<T> NotFound(string erro)
        {
            this.StatusCode = HttpStatusCode.NotFound;
            this.Errors = new List<string> { erro };
            return this;
        }

        private HttpResult<T> AlreadyExists(string erro)
        {
            this.StatusCode = HttpStatusCode.Conflict;
            this.Errors = new List<string> { erro };
            return this;
        }

        private HttpResult<T> NotAuthorized(string erro)
        {
            this.StatusCode = HttpStatusCode.Unauthorized;
            this.Errors = new List<string> { erro };
            return this;
        }

        public HttpResult<T> ReturnCustomException(Exception ex, object model = null)
        {

            if ((ex as CustomNotFoundException).IsNotNull())
            {
                return this.NotFound(ex.Message);
            }

            if ((ex as CustomBadRequestException).IsNotNull())
            {
                return this.BadRequest(ex.Message);
            }

            if ((ex as CustomNotAutorizedException).IsNotNull())
            {
                return this.NotAuthorized(ex.Message);
            }

            if ((ex as CustomAlreadyExistsException).IsNotNull())
            {
                return this.AlreadyExists(ex.Message);
            }

            if ((ex as CustomValidationException).IsNotNull())
            {
                var customEx = ex as CustomValidationException;
                return this.Error(customEx.Errors);
            }

            var erroMessage = ex.Message;
            if (model.IsNotNull())
            {
                var modelSerialization = JsonConvert.SerializeObject(model);
                erroMessage = string.Format("{0} - [{1}]", ex.Message, modelSerialization);
            }
            FactoryLog.GetInstace().Error(erroMessage, ex);
            return ExceptionWithInner(ex);

        }

        private HttpResult<T> ExceptionWithInner(Exception ex)
        {
            if (ex.InnerException.IsNotNull())
            {
                if (ex.InnerException.InnerException.IsNotNull())
                    return this.Error(string.Format("InnerException: {0}", ex.InnerException.InnerException.Message));
                else
                    return this.Error(string.Format("InnerException: {0}", ex.InnerException.Message));
            }
            else
            {
                return this.Error(string.Format("Exception: {0}", ex.Message));
            }
        }

    }
}
