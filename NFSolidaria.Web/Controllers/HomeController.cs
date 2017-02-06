using ProjetoRankingNFE.Models;
using ProjetoRankingNFE.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjetoRankingNFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly EntidadeRepository _entidadeRepository = new EntidadeRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }

        public ActionResult Ranking()
        {
            var entidade = _entidadeRepository.ObterDadosPorAno();
            return View(entidade);
        }

        public ActionResult Contato()
        {            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contato(EmailSendViewModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email de: {0} ({1})</p><strong>Mensagem:</strong><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("x"));  // replace with valid value   
                message.Subject = "Assunto: Mensagem NFSOLIDARIO";
                message.Body = string.Format(body, model.Nome, model.Email, model.Mensagem);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Contato");
                }
            }
            return View(model);
        }

        public JsonResult MostrarDados(int mes)
        {
            var entidade = _entidadeRepository.ObterDadosPorMes(mes);
            return Json(entidade, JsonRequestBehavior.AllowGet);
        }
    }
}