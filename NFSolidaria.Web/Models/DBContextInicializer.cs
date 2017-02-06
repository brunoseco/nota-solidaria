using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjetoRankingNFE.Models
{
    public class DBContextInicializer : CreateDatabaseIfNotExists<DBContexto>
    {
        protected override void Seed(DBContexto context)
        {
            IList<Usuario> usuarioDefault = new List<Usuario>();
            usuarioDefault.Add(new Usuario()
            {
                CPF_CNPJ = "123456789",
                Nome = "Entidade 1",
                RazaoSocial = "Razão Teste 1",
                Email = "email@teste.com",
                DataNascimento = DateTime.Now,
                SenhaMD5 = "teste"
            });

            usuarioDefault.Add(new Usuario()
            {
                CPF_CNPJ = "123456789",
                Nome = "Entidade 2",
                RazaoSocial = "Razão Teste 2",
                Email = "email@teste.com",
                DataNascimento = DateTime.Now,
                SenhaMD5 = "teste"
            });

            usuarioDefault.Add(new Usuario()
            {
                CPF_CNPJ = "123456789",
                Nome = "Entidade 2",
                RazaoSocial = "Razão Teste 2",
                Email = "email@teste.com",
                DataNascimento = DateTime.Now,
                SenhaMD5 = "teste"
            });

            //// // // Entidade Seed
            IList<Entidade> entidadeDefault = new List<Entidade>();

            entidadeDefault.Add(new Entidade()
            {
                Id = 1,
                IdentificadorNFP = "Entidade 1",
                Ativo = true
            });

            entidadeDefault.Add(new Entidade()
            {
                Id = 2,
                IdentificadorNFP = "Entidade 2",
                Ativo = true
            });

            entidadeDefault.Add(new Entidade()
            {
                Id = 3,
                IdentificadorNFP = "Entidade 3",
                Ativo = true
            });


            // // // Cupom Seed
            IList<Cupom> cupomDefault = new List<Cupom>();

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 4, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 110.50m,
                EntidadeId = 1,
                UsuarioId = 1,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 5, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 110.50m,
                EntidadeId = 1,
                UsuarioId = 1,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 3, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 110.50m,
                EntidadeId = 1,
                UsuarioId = 1,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 4, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 210.50m,
                EntidadeId = 2,
                UsuarioId = 2,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 5, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 50.50m,
                EntidadeId = 2,
                UsuarioId = 2,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 3, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 310.50m,
                EntidadeId = 2,
                UsuarioId = 2,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 4, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 130.50m,
                EntidadeId = 3,
                UsuarioId = 3,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 5, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 30.50m,
                EntidadeId = 3,
                UsuarioId = 3,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });

            cupomDefault.Add(new Cupom()
            {
                DataCompra = new DateTime(2016, 3, 15),
                COO = "COO 1",
                TipoNota = 1,
                Valor = 230.50m,
                EntidadeId = 3,
                UsuarioId = 3,
                CNPJEmissor = "12345678910",
                Situacao = 2,
                DataLancamento = DateTime.Now
            });


            foreach(Usuario usu in usuarioDefault)
            {
                context.Usuario.Add(usu);
                context.SaveChanges();
            }

            foreach(Entidade ent in entidadeDefault)
            {
                context.Entidade.Add(ent);
                context.SaveChanges();
            }

            foreach(Cupom cup in cupomDefault)
            {
                context.Cupom.Add(cup);
                context.SaveChanges();
            }
        }
    }
}