using Cna.Erp.Core.Infrastructure.ORM.Contexto;
using Common.Infrastructure.Log;
using InteractivePreGeneratedViews;
using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.MappingViews;
using System.IO;

//[assembly: DbMappingViewCacheType(typeof(DbContextCore), typeof(MappingViewCacheCoreGenereted))]
public class MappingViewCacheCoreGenereted : DbMappingViewCache
{

    public static void LoadPreCompiledViews()
    {
		ViewCacheFactory();
    }

	private static void ViewCacheFactory()
    {
		var log = FactoryLog.GetInstace();
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"MappingViewCacheCoreGenereted.xml");
        var ctx = new DbContextCore(log);
		InteractiveViews.SetViewCacheFactory(ctx, new FileViewCacheFactory(path));
    }

    public override string MappingHashValue
    {
        get { return "145a24638dc705f0e86acd383145948d4d948f58169e657398cc0ec142e66b7d"; }
    }

    public override DbMappingView GetView(EntitySetBase extent)
    {

		var viewname = string.Format("{0}.{1}", extent.EntityContainer.Name, extent.Name);

        if (viewname == "CodeFirstDatabase.Cadastrador") {  return CodeFirstDatabase_Cadastrador(); }
        if (viewname == "DbContextCore.Cadastradors") {  return DbContextCore_Cadastradors(); }
        if (viewname == "CodeFirstDatabase.Cupom") {  return CodeFirstDatabase_Cupom(); }
        if (viewname == "CodeFirstDatabase.Entidade") {  return CodeFirstDatabase_Entidade(); }
        if (viewname == "CodeFirstDatabase.UsuarioEntidadeFavorita") {  return CodeFirstDatabase_UsuarioEntidadeFavorita(); }
        if (viewname == "CodeFirstDatabase.Usuario") {  return CodeFirstDatabase_Usuario(); }
        if (viewname == "DbContextCore.Cupoms") {  return DbContextCore_Cupoms(); }
        if (viewname == "DbContextCore.Entidades") {  return DbContextCore_Entidades(); }
        if (viewname == "DbContextCore.UsuarioEntidadeFavoritas") {  return DbContextCore_UsuarioEntidadeFavoritas(); }
        if (viewname == "DbContextCore.Usuarios") {  return DbContextCore_Usuarios(); }
        if (viewname == "CodeFirstDatabase.EntidadeCadastrador") {  return CodeFirstDatabase_EntidadeCadastrador(); }
        if (viewname == "DbContextCore.EntidadeCadastradors") {  return DbContextCore_EntidadeCadastradors(); }


		return null;

        
    }

	private static DbMappingView CodeFirstDatabase_Cadastrador() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Cadastrador
        [CodeFirstDatabaseSchema.Cadastrador](T1.Cadastrador_Id, T1.Cadastrador_Ativo)
    FROM (
        SELECT 
            T.CadastradorId AS Cadastrador_Id, 
            T.Ativo AS Cadastrador_Ativo, 
            True AS _from0
        FROM DbContextCore.Cadastradors AS T
    ) AS T1"); 
}
private static DbMappingView DbContextCore_Cadastradors() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Cadastradors
        [NFSolidaria.Core.Infrastructure.ORM.Contexto.Cadastrador](T1.Cadastrador_CadastradorId, T1.Cadastrador_Ativo)
    FROM (
        SELECT 
            T.Id AS Cadastrador_CadastradorId, 
            T.Ativo AS Cadastrador_Ativo, 
            True AS _from0
        FROM CodeFirstDatabase.Cadastrador AS T
    ) AS T1"); 
}
private static DbMappingView CodeFirstDatabase_Cupom() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Cupom
        [CodeFirstDatabaseSchema.Cupom](T1.Cupom_Id, T1.Cupom_ChaveAcesso, T1.Cupom_DataCompra, T1.Cupom_COO, T1.Cupom_TipoNota, T1.Cupom_Valor, T1.Cupom_EntidadeId, T1.Cupom_UsuarioId, T1.Cupom_CadastradorId, T1.Cupom_CNPJEmissor, T1.Cupom_Situacao, T1.Cupom_DataLancamento, T1.Cupom_DataProcessamento, T1.Cupom_Imagem1, T1.Cupom_Imagem2)
    FROM (
        SELECT 
            T.CupomId AS Cupom_Id, 
            T.ChaveAcesso AS Cupom_ChaveAcesso, 
            T.DataCompra AS Cupom_DataCompra, 
            T.COO AS Cupom_COO, 
            T.TipoNota AS Cupom_TipoNota, 
            T.Valor AS Cupom_Valor, 
            T.EntidadeId AS Cupom_EntidadeId, 
            T.UsuarioId AS Cupom_UsuarioId, 
            T.CadastradorId AS Cupom_CadastradorId, 
            T.CNPJEmissor AS Cupom_CNPJEmissor, 
            T.Situacao AS Cupom_Situacao, 
            T.DataLancamento AS Cupom_DataLancamento, 
            T.DataProcessamento AS Cupom_DataProcessamento, 
            T.Imagem1 AS Cupom_Imagem1, 
            T.Imagem2 AS Cupom_Imagem2, 
            True AS _from0
        FROM DbContextCore.Cupoms AS T
    ) AS T1"); 
}
private static DbMappingView CodeFirstDatabase_Entidade() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Entidade
        [CodeFirstDatabaseSchema.Entidade](T1.Entidade_Id, T1.Entidade_IdentificadorNFP, T1.Entidade_Ativo)
    FROM (
        SELECT 
            T.EntidadeId AS Entidade_Id, 
            T.IdentificadorNFP AS Entidade_IdentificadorNFP, 
            T.Ativo AS Entidade_Ativo, 
            True AS _from0
        FROM DbContextCore.Entidades AS T
    ) AS T1"); 
}
private static DbMappingView CodeFirstDatabase_UsuarioEntidadeFavorita() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing UsuarioEntidadeFavorita
        [CodeFirstDatabaseSchema.UsuarioEntidadeFavorita](T1.UsuarioEntidadeFavorita_UsuarioId, T1.UsuarioEntidadeFavorita_EntidadeId)
    FROM (
        SELECT 
            T.UsuarioId AS UsuarioEntidadeFavorita_UsuarioId, 
            T.EntidadeId AS UsuarioEntidadeFavorita_EntidadeId, 
            True AS _from0
        FROM DbContextCore.UsuarioEntidadeFavoritas AS T
    ) AS T1"); 
}
private static DbMappingView CodeFirstDatabase_Usuario() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Usuario
        [CodeFirstDatabaseSchema.Usuario](T1.Usuario_Id, T1.[Usuario.CPF_CNPJ], T1.Usuario_Nome, T1.Usuario_RazaoSocial, T1.Usuario_Email, T1.Usuario_DataNascimento, T1.Usuario_SenhaMD5, T1.Usuario_Cidade, T1.Usuario_UF, T1.Usuario_LastToken)
    FROM (
        SELECT 
            T.UsuarioId AS Usuario_Id, 
            T.CPF_CNPJ AS [Usuario.CPF_CNPJ], 
            T.Nome AS Usuario_Nome, 
            T.RazaoSocial AS Usuario_RazaoSocial, 
            T.Email AS Usuario_Email, 
            T.DataNascimento AS Usuario_DataNascimento, 
            T.SenhaMD5 AS Usuario_SenhaMD5, 
            T.Cidade AS Usuario_Cidade, 
            T.UF AS Usuario_UF, 
            T.LastToken AS Usuario_LastToken, 
            True AS _from0
        FROM DbContextCore.Usuarios AS T
    ) AS T1"); 
}
private static DbMappingView DbContextCore_Cupoms() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Cupoms
        [NFSolidaria.Core.Infrastructure.ORM.Contexto.Cupom](T1.Cupom_CupomId, T1.Cupom_ChaveAcesso, T1.Cupom_DataCompra, T1.Cupom_COO, T1.Cupom_TipoNota, T1.Cupom_Valor, T1.Cupom_EntidadeId, T1.Cupom_UsuarioId, T1.Cupom_CadastradorId, T1.Cupom_CNPJEmissor, T1.Cupom_Situacao, T1.Cupom_DataLancamento, T1.Cupom_DataProcessamento, T1.Cupom_Imagem1, T1.Cupom_Imagem2)
    FROM (
        SELECT 
            T.Id AS Cupom_CupomId, 
            T.ChaveAcesso AS Cupom_ChaveAcesso, 
            T.DataCompra AS Cupom_DataCompra, 
            T.COO AS Cupom_COO, 
            T.TipoNota AS Cupom_TipoNota, 
            T.Valor AS Cupom_Valor, 
            T.EntidadeId AS Cupom_EntidadeId, 
            T.UsuarioId AS Cupom_UsuarioId, 
            T.CadastradorId AS Cupom_CadastradorId, 
            T.CNPJEmissor AS Cupom_CNPJEmissor, 
            T.Situacao AS Cupom_Situacao, 
            T.DataLancamento AS Cupom_DataLancamento, 
            T.DataProcessamento AS Cupom_DataProcessamento, 
            T.Imagem1 AS Cupom_Imagem1, 
            T.Imagem2 AS Cupom_Imagem2, 
            True AS _from0
        FROM CodeFirstDatabase.Cupom AS T
    ) AS T1"); 
}
private static DbMappingView DbContextCore_Entidades() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Entidades
        [NFSolidaria.Core.Infrastructure.ORM.Contexto.Entidade](T1.Entidade_EntidadeId, T1.Entidade_IdentificadorNFP, T1.Entidade_Ativo)
    FROM (
        SELECT 
            T.Id AS Entidade_EntidadeId, 
            T.IdentificadorNFP AS Entidade_IdentificadorNFP, 
            T.Ativo AS Entidade_Ativo, 
            True AS _from0
        FROM CodeFirstDatabase.Entidade AS T
    ) AS T1"); 
}
private static DbMappingView DbContextCore_UsuarioEntidadeFavoritas() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing UsuarioEntidadeFavoritas
        [NFSolidaria.Core.Infrastructure.ORM.Contexto.UsuarioEntidadeFavorita](T1.UsuarioEntidadeFavorita_UsuarioId, T1.UsuarioEntidadeFavorita_EntidadeId)
    FROM (
        SELECT 
            T.UsuarioId AS UsuarioEntidadeFavorita_UsuarioId, 
            T.EntidadeId AS UsuarioEntidadeFavorita_EntidadeId, 
            True AS _from0
        FROM CodeFirstDatabase.UsuarioEntidadeFavorita AS T
    ) AS T1"); 
}
private static DbMappingView DbContextCore_Usuarios() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing Usuarios
        [NFSolidaria.Core.Infrastructure.ORM.Contexto.Usuario](T1.Usuario_UsuarioId, T1.[Usuario.CPF_CNPJ], T1.Usuario_Nome, T1.Usuario_RazaoSocial, T1.Usuario_Email, T1.Usuario_DataNascimento, T1.Usuario_SenhaMD5, T1.Usuario_Cidade, T1.Usuario_UF, T1.Usuario_LastToken)
    FROM (
        SELECT 
            T.Id AS Usuario_UsuarioId, 
            T.CPF_CNPJ AS [Usuario.CPF_CNPJ], 
            T.Nome AS Usuario_Nome, 
            T.RazaoSocial AS Usuario_RazaoSocial, 
            T.Email AS Usuario_Email, 
            T.DataNascimento AS Usuario_DataNascimento, 
            T.SenhaMD5 AS Usuario_SenhaMD5, 
            T.Cidade AS Usuario_Cidade, 
            T.UF AS Usuario_UF, 
            T.LastToken AS Usuario_LastToken, 
            True AS _from0
        FROM CodeFirstDatabase.Usuario AS T
    ) AS T1"); 
}
private static DbMappingView CodeFirstDatabase_EntidadeCadastrador() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing EntidadeCadastrador
        [CodeFirstDatabaseSchema.EntidadeCadastrador](T1.EntidadeCadastrador_EntidadeId, T1.EntidadeCadastrador_CadastradorId)
    FROM (
        SELECT 
            T.EntidadeId AS EntidadeCadastrador_EntidadeId, 
            T.CadastradorId AS EntidadeCadastrador_CadastradorId, 
            True AS _from0
        FROM DbContextCore.EntidadeCadastradors AS T
    ) AS T1"); 
}
private static DbMappingView DbContextCore_EntidadeCadastradors() 
{ 
	return new DbMappingView(@"
    SELECT VALUE -- Constructing EntidadeCadastradors
        [NFSolidaria.Core.Infrastructure.ORM.Contexto.EntidadeCadastrador](T1.EntidadeCadastrador_EntidadeId, T1.EntidadeCadastrador_CadastradorId)
    FROM (
        SELECT 
            T.EntidadeId AS EntidadeCadastrador_EntidadeId, 
            T.CadastradorId AS EntidadeCadastrador_CadastradorId, 
            True AS _from0
        FROM CodeFirstDatabase.EntidadeCadastrador AS T
    ) AS T1"); 
}



}

