using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.API
{
    public class HelperHttp
    {
        private string baseAddress;

        public HelperHttp(string baseAddress)
        {
            this.baseAddress = baseAddress;
        }

        public HttpResult<TReturn> Get<TReturn, TFilter>(string token, string resource, TFilter model, bool EnabledExceptions = true)
        {
            return Get<TReturn>(token, resource, model.ToQueryString(), EnabledExceptions);
        }
        
        public HttpResult<T> Get<T>(string resource, QueryStringParameter queryParameters = null)
        {
            return Get<T>(string.Empty, resource);
        }
        
        public HttpResult<T> Get<T>(string token, string resource, QueryStringParameter queryParameters = null, bool EnabledExceptions = true)
        {
            if (EnabledExceptions)
                return GetHttpClient<T>(token, resource, queryParameters);

            return GetWebClient<T>(token, resource, queryParameters);
        }

        public HttpResult<TResult> Post<T, TResult>(string resource, T model)
        {
            return Post<T, TResult>(string.Empty, resource, model);

        }

        public HttpResult<TResult> Post<T, TResult>(string token, string resource, T model)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("token", token);

                    var response = client.PostAsJsonAsync(resource, model).Result;
                    var result = response.Content.ReadAsAsync<HttpResult<TResult>>().Result;
                    return result;

                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<TResult>(resource, ex);
                }
            }

        }

        public HttpResult<TResult> Put<T, TResult>(string resource, T model)
        {
            return Put<T, TResult>(string.Empty, resource, model);
        }

        public HttpResult<TResult> Put<T, TResult>(string token, string resource, T model)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("token", token);

                    var response = client.PutAsJsonAsync(resource, model).Result;
                    var result = response.Content.ReadAsAsync<HttpResult<TResult>>().Result;
                    return result;

                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<TResult>(resource, ex);
                }
            }

        }

        public HttpResult<TResult> Delete<T, TResult>(string token, string resource, T model)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("token", token);

                    resource = MakeResource(resource, model.ToQueryString());


                    var response = client.DeleteAsync(resource).Result;
                    var result = response.Content.ReadAsAsync<HttpResult<TResult>>().Result;
                    return result;

                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<TResult>(resource, ex);
                }
            }



        }

        private HttpResult<TResult> MakeErrorHttpResult<TResult>(string resource, Exception ex)
        {
            return new HttpResult<TResult>().Error(string.Format("Erro ao acessar a API {0}{1}-Error: {2}", this.baseAddress, resource, ex.Message));
        }
        private HttpResult<TResult> MakeErrorHttpResult<TResult>(string resource, string message)
        {
            return new HttpResult<TResult>().Error(string.Format("Erro ao acessar a API {0}{1}-Message: {2}", this.baseAddress, resource, message));
        }
        private string MakeResource(string resource, QueryStringParameter queryParameters)
        {
            if (queryParameters != null)
            {
                var queryStringUrl = queryParameters.Get().ToQueryString();
                resource = String.Concat(resource, queryStringUrl);
            }
            return resource;
        }
        private HttpResult<T> GetWebClient<T>(string token, string resource, QueryStringParameter queryParameters)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = this.baseAddress;
                client.Headers.Add("token", token);
                client.Encoding = Encoding.UTF8;

                try
                {
                    resource = MakeResource(resource, queryParameters);

                    var result = client.DownloadString(resource);
                    return string.IsNullOrEmpty(result) ? default(HttpResult<T>) : JsonConvert.DeserializeObject<HttpResult<T>>(result);
                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<T>(resource, ex);
                }
            }
        }
        private HttpResult<T> GetHttpClient<T>(string token, string resource, QueryStringParameter queryParameters)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(this.baseAddress);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("token", token);

                    resource = MakeResource(resource, queryParameters);

                    var response = client.GetAsync(resource).Result;
                    var result = response.Content.ReadAsStringAsync();
                    var model = string.IsNullOrEmpty(result.Result) ? default(HttpResult<T>) : JsonConvert.DeserializeObject<HttpResult<T>>(result.Result);
                    return model;
                }
                catch (Exception ex)
                {
                    return MakeErrorHttpResult<T>(resource, ex);
                }
            }
        }

    }
}
