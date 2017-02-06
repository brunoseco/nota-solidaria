using NFSolidaria.Core.Dto;
using NFSolidaria.Core.Filters;
using NFSolidaria.Enum;
using NFSolidaria.Integration.Application;
using NFSolidaria.Integration.Application.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFSolidaria.Integration.ConsoleApp
{
    class Program
    {
        public static IWebDriver driver = new ChromeDriver();

        static void Main(string[] args)
        {
            AutoMapperConfigCore.RegisterMappings();

            Console.WriteLine("-----PROCESSO INICIADO-----");

            driver.Manage().Window.Maximize();

            var entidades = EntidadesComCuponsPendentes();
            var erros = 0;
            var processadas = 0;

            foreach (var item in entidades)
            {
                ExecutaLogin(item);
                RedirecionaParaPaginaEscolha();
                SelecionaEntidade(item);
                PreparaTelaCadastro();

                var index = 0;

                foreach (var cupom in item.CollectionCupom)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                    Thread.Sleep(1000);

                    var elCNPJ = driver.FindElement(By.XPath("/html/body/form[@id='aspnetForm']/div[@id='site']/div[@id='ConteudoPrincipal']/div[@class='containerConteudo LarguraFixa75']/div[@id='pnlForm']/fieldset/div[@id='divDocSemChave']/fieldset/div[@id='divCNPJEstabelecimento']/input"));
                    elCNPJ.Clear();
                    elCNPJ.SendKeys(cupom.CNPJEmissor);

                    var selectElement = new SelectElement(driver.FindElement(By.XPath("/html/body/form[@id='aspnetForm']/div[@id='site']/div[@id='ConteudoPrincipal']/div[@class='containerConteudo LarguraFixa75']/div[@id='pnlForm']/fieldset/div[@id='divDocSemChave']/fieldset/div[@id='divddlTpNota']/select[@id='ddlTpNota']")));
                    selectElement.SelectByValue((int)cupom.TipoNota == (int)ETipoNota.CupomFiscal ? "CF" : "NF");

                    var data = (DateTime)cupom.DataCompra;
                    var elData = driver.FindElement(By.XPath("/html/body/form[@id='aspnetForm']/div[@id='site']/div[@id='ConteudoPrincipal']/div[@class='containerConteudo LarguraFixa75']/div[@id='pnlForm']/fieldset/div[@id='divDocSemChave']/fieldset/div[@id='divtxtDtNota']/input"));
                    elData.Clear();
                    var novadata = data.ToShortDateString().Replace("/", "");
                    elData.SendKeys(novadata);

                    var elCOO = driver.FindElement(By.XPath("/html/body/form[@id='aspnetForm']/div[@id='site']/div[@id='ConteudoPrincipal']/div[@class='containerConteudo LarguraFixa75']/div[@id='pnlForm']/fieldset/div[@id='divDocSemChave']/fieldset/div[@id='divtxtNrNota']/input"));
                    elCOO.Clear();
                    elCOO.SendKeys(cupom.COO);

                    string valor = Convert.ToString(cupom.Valor);
                    var elValor = driver.FindElement(By.XPath("/html/body/form[@id='aspnetForm']/div[@id='site']/div[@id='ConteudoPrincipal']/div[@class='containerConteudo LarguraFixa75']/div[@id='pnlForm']/fieldset/div[@id='divDocSemChave']/fieldset/div[@id='divtxtVlNota']/input"));
                    elValor.Clear();
                    elValor.SendKeys(valor);

                    if (index == 0)
                        Thread.Sleep(10000);
                    else
                        Thread.Sleep(1000);

                    var btn = driver.FindElement(By.XPath("/html/body/form[@id='aspnetForm']/div[@id='site']/div[@id='ConteudoPrincipal']/div[@class='containerConteudo LarguraFixa75']/div[@class='line100Padded alinharCentro']/input[@id='btnSalvarNota']"));
                    btn.Click();

                    var els = driver.FindElements(By.Id("lblErro"));
                    if (els == null || els.Count() == 0)
                    {
                        SetaCupomComo(ESituacaoCupom.Processado, (int)cupom.CupomId);
                        index++;
                        processadas++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("-----LANÇADO COM SUCESSO >> COO " + cupom.COO);

                    }
                    else
                    {
                        if (!driver.FindElement(By.Id("lblErro")).Text.Contains("Captcha"))
                        {
                            SetaCupomComo(ESituacaoCupom.Erro, (int)cupom.CupomId);
                            index = 0;
                        }

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("-----LANÇADO COM ERRO >> COO " + cupom.COO + " - " + driver.FindElement(By.Id("lblErro")).Text);

                        js.ExecuteScript("return document.getElementById('lblErro').remove();");
                        erros++;
                    }
                }
            }

            driver.Dispose();

            Thread.Sleep(5000);
            Console.ResetColor();
            Console.WriteLine("-----PROCESSO CONCLUÍDO COM SUCESSO-----");
            Console.WriteLine(string.Format("-----NOTAS LANÇADAS {0} | NOTAS COM ERROS {1}-----", processadas, erros));
            Console.WriteLine("-----PRESSIONE UMA TECLA PARA SAIR-----");
            Console.ReadLine();

        }

        private static void PreparaTelaCadastro()
        {
            var modais = driver.FindElements(By.ClassName("ui-icon-closethick"));
            foreach (var item in modais)
                if (item.Displayed)
                    item.Click();
        }

        private static void SelecionaEntidade(dynamic item)
        {
            var entidadeEl = driver.FindElement(By.Id("ddlEntidadeFilantropica"));
            var selectElement = new SelectElement(entidadeEl);
            selectElement.SelectByValue(item.IdentificadorNFP);
            driver.FindElement(By.Id("ctl00_ConteudoPagina_btnNovaNota")).Click();
        }

        private static void RedirecionaParaPaginaEscolha()
        {
            driver.Navigate().GoToUrl("https://www.nfp.fazenda.sp.gov.br/EntidadesFilantropicas/ListagemNotaEntidade.aspx");
            driver.FindElement(By.Id("ctl00_ConteudoPagina_btnOk")).Click();
        }

        private static void ExecutaLogin(dynamic item)
        {
            driver.Navigate().GoToUrl("https://www.nfp.fazenda.sp.gov.br/login.aspx");
            driver.FindElement(By.Id("UserName")).SendKeys(item.Cadastrador.CPF);
            driver.FindElement(By.Id("Password")).SendKeys(item.Cadastrador.Pass);
            driver.FindElement(By.Id("Login")).Click();
        }

        private static List<dynamic> EntidadesComCuponsPendentes()
        {
            var token = "";
            using (ConfigContainer.BeginLifetimeScope())
            {
                var app = new EntidadeApp(token);
                var result = app.GetDataListCustom(new EntidadeFilter()
                {
                    Ativo = true,
                    ComUsuarioCadastrador = true,
                    ComCupomPendente = true
                });

                app.Dispose();
                return result;
            }
        }

        private static void SetaCupomComo(ESituacaoCupom status, int id)
        {
            var token = "";
            using (ConfigContainer.BeginLifetimeScope())
            {
                var app = new CupomApp(token);
                var result = app.SavePartial(new CupomDtoSpecialized()
                {
                    CupomId = id,
                    Situacao = (int)status,
                    DataProcessamento = DateTime.Now
                });

                app.Dispose();
            }
        }

    }
}
