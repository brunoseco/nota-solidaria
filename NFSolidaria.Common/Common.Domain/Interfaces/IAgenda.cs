using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Enums;

namespace Common.Domain.Interfaces
{
    public interface IAgenda
    {
        string NomeAluno { get; set; }
        string NomeUsuario { get; set; }
        DateTime DataNascimentoAluno { get; set; }
        DateTime DataNascimentoUsuario { get; set; }
        string EmailAluno { get; set; }
        string EmailUsuario { get; set; }
        UserRole PermissaoUsuario { get; set; }
        string NomeTurma { get; set; }
        string DescricaoAtividadeTurma { get; set; }
        DateTime DataAtividadeTurma { get; set; }
        string TrabalhoAtividadeTurma { get; set; }


        void InserirAluno();

        void InserirTurma();

        void InserirUsuario();

        void InserirAtividadeTurma();

    }
}
