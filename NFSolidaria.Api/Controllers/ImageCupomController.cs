using Common.API;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using NFSolidaria.Core.Dto;
using NFSolidaria.Core.Application;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using Common.Domain.CustomExceptions;
using Common.OCR;
using System.Drawing;
using System.Text.RegularExpressions;

namespace NFSolidaria.Core.Api.Controllers
{
    public class ImageCupomController : ApiController
    {
        private HttpResult<CupomDto> result;
        private HelperHttpLog httpLog;

        public ImageCupomController()
        {
            this.httpLog = new HelperHttpLog();
            this.httpLog.LogRequestIni();
            this.result = new HttpResult<CupomDto>();
        }

        [ActionName("DefaultAction")]
        public HttpResponseMessage Post([FromBody]CupomDto model)
        {
            try
            {
                var resposta = string.Empty;

                if (model.Imagem1.IsAny())
                    resposta = this.ConverteBytesParaTexto(model.Imagem1, resposta);

                if (model.Imagem2.IsAny())
                    resposta = this.ConverteBytesParaTexto(model.Imagem2, resposta);

                var alvo = new CupomDto();

                var palavras = this.MontaArrayPalavras(resposta);

                alvo.CNPJEmissor = this.FindByWord("CNPJ", palavras);
                alvo.COO = this.FindByWord("COO", palavras);
                alvo.DataCompra = this.FindDate(palavras);
                alvo.Valor = this.FindMoneyValue(palavras);

                result.Success(alvo);

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                result.ReturnCustomException(ex);
                return Request.CreateResponse(result.StatusCode, result);
            }
        }

        private decimal FindMoneyValue(string[] palavras)
        {
            var result = default(decimal);
            var str = string.Empty;

            var index = Array.FindIndex(palavras, _ => _.Contains("R$"));
            if (index.IsSent())
            {
                str = palavras[index + 1];
                Decimal.TryParse(str, out result);
            }

            return result;
        }

        private DateTime FindDate(string[] palavras)
        {
            var result = default(DateTime);
            foreach (var item in palavras)
            {
                if (item.Length == 10)
                    DateTime.TryParse(item, out result);
            }

            return result;
        }

        private string FindByWord(string word, string[] palavras)
        {
            var result = string.Empty;

            var index = Array.FindIndex(palavras, _ => _.Contains(word));
            result = palavras[index + 1];

            return result;
        }

        private string ConverteBytesParaTexto(byte[] array, string resposta)
        {
            var image = this.ParseBytesToImage(array);
            var ocr = new OCR(image);
            resposta += ocr.GetText();
            return resposta;
        }

        private string[] MontaArrayPalavras(string resposta)
        {
            resposta = resposta.Replace(@"\n", " ");

            var list = new List<string>();
            var splited = Regex.Split(resposta.ToUpper(), @"\s");
            foreach (var item in splited)
            {
                if (item != "" && item != " ")
                    list.Add(item);
            }

            return list.ToArray();
        }

        public Image ParseBytesToImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    return Image.FromStream(ms);
            }
            catch (Exception)
            {
                throw new CustomValidationException("Arquivo enviado não é uma imagem");
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.httpLog.LogRequestEnd();
            base.Dispose(disposing);
        }
    }
}
