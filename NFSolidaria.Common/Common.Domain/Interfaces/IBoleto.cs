using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IBoleto
    {
        /// <returns>Caminho físico do arquivo gerado</returns>
        string GerarBoletos(IEnumerable<IBoletoDto> cobrancas);

        string GerarRemessa(int codigoBanco, decimal taxaBoleto, int numeroArquivoRemessa, IEnumerable<IBoletoDto> cobrancas);

        IEnumerable<IDetalheRetornoDto> ImportarArquivoRetorno(int layout, int codigoBanco, Stream arquivo);

        IBoletoDto NovoDto();
    }

    public interface IBoletoDto 
    {
        // CEDENTE

        string CedenteCpfCnpj { get; set; }
        string CedenteNome { get; set; }
        string CedenteAgencia { get; set; }
        string CedenteDigitoAgencia { get; set; }
        string CedenteContaCorrente { get; set; }
        string CedenteDigitoConta { get; set; }

        string CedenteUF { get; set; }
        string CedenteCEP { get; set; }
        string CedenteEndereco { get; set; }
        string CedenteCidade { get; set; }
        string CedenteBairro { get; set; }
        string CedenteComplemento { get; set; }
        string CedenteEmail { get; set; }

        // Contrato
        string CodigoBanco { get; set; }
        string CodigoCarteira { get; set; }
        string CodigoCedente { get; set; }

        // SACADO

        string SacadoCpfCnpj { get; set; }
        string SacadoNome { get; set; }

        string SacadoUF { get; set; }
        string SacadoCEP { get; set; }
        string SacadoEndereco { get; set; }
        string SacadoCidade { get; set; }
        string SacadoBairro { get; set; }
        string SacadoComplemento { get; set; }
        string SacadoEmail { get; set; }

        // Ordem de Cobranca
        string NossoNumero { get; set; }

        DateTime DataVencimento { get; set; }
        decimal Valor { get; set; }
        decimal PorcentagemMultaPorAtraso { get; set; }
        decimal PorcentagemJurosMoraMes { get; set; }

        IEnumerable<string> Instrucoes { get; set; }

        string LocalDePagamento { get; set; }

        // Layout
        string Layout { get; set; }
    }

    public interface IDetalheRetornoDto
    {
        string Registro { get; set; }
        DateTime DataOcorrencia { get; set; }
        string NossoNumero { get; set; }
        decimal ValorPago { get; set; }
        decimal DescontoTaxa { get; set; }
        decimal JurosMora { get; set; }
    }

    public interface IBaixaBoletoDto 
    {
        string ConfirmationResponseBehavior { get; set; }
        string NossoNumero { get; set; }
        DateTime DataOcorrencia { get; set; }
        decimal ValorPago { get; set; }
        decimal DescontoTaxa { get; set; }
        decimal JurosMora { get; set; }
        int ContaCorrenteId { get; set; }
    }
}
