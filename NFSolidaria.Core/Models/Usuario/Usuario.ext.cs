using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Models;
using Common.Domain;
using NFSolidaria.Core.Filters;
using Common.Interfaces;
using Common.Domain.Interfaces;
using Common.Domain.CustomExceptions;
using Common.Domain.Enums;
using NFSolidaria.Core.Domain.Custom;

namespace NFSolidaria.Core.Domain
{
    [MetadataType(typeof(UsuarioValidation))]
    public partial class Usuario
    {
        private ICripto cripto;
        private string salt;

        private void DefineSalt()
        {
            this.salt = "NFSolidariaHash";
        }

        public Usuario()
        {
        }

        public virtual Entidade Entidade { get; set; }
        public virtual Cadastrador Cadastrador { get; set; }

        public Usuario(IRepository<Usuario> rep, ICache cache, ICripto cripto)
            : this(rep, cache)
        {
            this.cripto = cripto;
        }

        public Usuario Get(Usuario model)
        {
            return this.rep.GetAll(this.DataAgregation(new UsuarioFilter
            {
                QueryOptimizerBehavior = model.QueryOptimizerBehavior

            })).Where(_ => _.UsuarioId == model.UsuarioId).SingleOrDefault();
        }

        public IQueryable<Usuario> GetByFilters(UsuarioFilter filters, params Expression<Func<Usuario, object>>[] includes)
        {
            var queryBase = this.rep.GetAllAsNoTracking(this.DataAgregation(includes, filters));
            var queryFilter = queryBase;
            return this.SimpleFilters(filters, queryBase);
        }

        public IEnumerable<DataItem> GetDataItem(UsuarioFilter filters)
        {
            var dataList = this.GetByFilters(filters)
                .Select(_ => new DataItem
                {
                    Id = _.UsuarioId.ToString(),
                });

            return dataList.ToList();
        }

        public Common.Models.Summary GetSummary(IQueryable<Usuario> result)
        {
            return new Common.Models.Summary
            {
                Total = this.TotalCount
            };
        }

        public Usuario Save()
        {
            return Save(this);
        }

        public Usuario Save(Usuario model)
        {
            if (model.AttributeBehavior == "SignIn")
                return this.Login(model);

            if (model.AttributeBehavior == "EditPerfil")
                this.EditPerfil(model);

            model = SaveDefault(model);
            this.rep.Commit();

            if (model.AttributeBehavior == "SignUp")
                model.LastToken = this.AddCache(model);

            model.AttributeBehavior = string.Empty;

            return model;
        }

        private void EditPerfil(Usuario model)
        {
            var user = this.ValidateAuth(this.token, this.cache);

            if (!model.Email.IsSent())
                throw new CustomValidationException("Preencha o email");

            if (!model.Nome.IsSent())
                throw new CustomValidationException("Preencha o nome");

            var emailexistente = this.rep.GetAll()
                .Where(_ => _.Email == model.Email)
                .Where(_ => _.UsuarioId != user.UserId);
            if (emailexistente.IsAny())
                throw new CustomValidationException("Email já existente");

            if (!model.CPF_CNPJ.IsSent())
            {
                var cpfexistente = this.rep.GetAll()
                    .Where(_ => _.CPF_CNPJ == model.CPF_CNPJ)
                    .Where(_ => _.UsuarioId != user.UserId);
                if (cpfexistente.IsAny())
                    throw new CustomValidationException("CPF já existente");
            }
        }

        private Usuario Login(Usuario model, bool validate = true)
        {
            if (validate)
                model = this.Auth(model);

            var _token = this.AddCache(model);

            model.LastToken = _token;
            this.rep.Commit();
            return model;
        }

        private string AddCache(Usuario model)
        {
            var guid = Guid.NewGuid().ToString();
            var currentUser = new CurrentUser
            {
                UserId = model.UsuarioId
            };

            currentUser.SetUserInfo<UsuarioCache>(new UsuarioCache
            {
                UsuarioId = model.UsuarioId,
                Nome = model.Nome,
                RazaoSocial = model.RazaoSocial,
                Email = model.Email,
                LastToken = guid,
                Cidade = "teste",
                UF = "teste",
            });

            this.cache.Remove(guid);
            this.cache.Add(guid, currentUser);
            var teste = this.cache.Get(guid);
            return guid;
        }

        private Usuario Auth(Usuario model)
        {
            if (model.Email.IsNotSent())
                throw new CustomValidationException("Digite o e-mail.");

            if (model.SenhaMD5.IsNotSent())
                throw new CustomValidationException("Digite a senha.");

            model.SenhaMD5 = this.GeraSenhaMD5(model);
            var alvo = this.rep.GetAll().Where(_ => _.Email == model.Email).Where(_ => _.SenhaMD5 == model.SenhaMD5).SingleOrDefault();
            if (alvo.IsNull())
                throw new CustomValidationException("Senha ou email inválido");

            return alvo;
        }

        public IEnumerable<Usuario> Save(IEnumerable<Usuario> models)
        {
            var modelsInserted = new List<Usuario>();
            foreach (var item in models)
            {
                modelsInserted.Add(SaveDefault(item));
            }

            this.rep.Commit();
            return modelsInserted;
        }

        public void Delete(Usuario model)
        {
            this.cache.Remove(this.token);
        }

        private void SetInitialValues(Usuario model)
        {
            if (model.AttributeBehavior == "SignUp")
            {
                model.RazaoSocial = model.Nome;
                model.SenhaMD5 = this.GeraSenhaMD5(model);
            }

        }

        private string GeraSenhaMD5(Usuario model)
        {
            this.DefineSalt();
            cripto.Salt = this.salt;
            var passwordMD5 = cripto.Encrypt(model.SenhaMD5, TypeCripto.Hash128);
            return passwordMD5;
        }

        private void ValidationReletedClasses(Usuario model, Usuario modelOld)
        {
            if (model.AttributeBehavior == "SignUp")
            {
                var models = this.rep.GetAll().Where(_ => _.Email == model.Email);
                if (models.IsAny())
                    throw new CustomValidationException("E-mail já cadastrado.");
            }
        }

        private void DeleteCollectionsOnSave(Usuario model)
        {

        }

        protected virtual Expression<Func<Usuario, object>>[] DataAgregation(Filter filter)
        {
            return DataAgregation(new Expression<Func<Usuario, object>>[] { }, filter);
        }

        protected virtual Expression<Func<Usuario, object>>[] DataAgregation(Expression<Func<Usuario, object>>[] includes, Filter filter)
        {
            return includes;
        }


        public override void Dispose()
        {
            if (this.rep != null)
                this.rep.Dispose();
        }

        ~Usuario()
        {

        }

    }
}