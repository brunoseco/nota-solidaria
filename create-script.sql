
CREATE DATABASE NFSolidaria
GO
USE [NFSolidaria]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 06/01/2016 19:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CPF_CNPJ] [varchar](14) NULL,
	[Nome] [varchar](150) NOT NULL,
	[RazaoSocial] [varchar](150) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[DataNascimento] [datetime] NULL,
	[SenhaMD5] [varchar](32) NOT NULL,
	[Cidade] [varchar](100) NULL,
	[UF] [varchar](2) NULL,
	[LastToken] [varchar](200) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Entidade]    Script Date: 06/01/2016 19:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Entidade](
	[Id] [int] NOT NULL,
	[IdentificadorNFP] [varchar](100) NULL,
	[Ativo] [bit] NOT NULL,
 CONSTRAINT [PK_Entidade] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cadastrador]    Script Date: 06/01/2016 19:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cadastrador](
	[Id] [int] NOT NULL,
	[Ativo] [bit] NOT NULL,
 CONSTRAINT [PK_Cadastrador] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioEntidadeFavorita]    Script Date: 06/01/2016 19:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioEntidadeFavorita](
	[UsuarioId] [int] NOT NULL,
	[EntidadeId] [int] NOT NULL,
 CONSTRAINT [PK_UsuarioEntidadeFavorita] PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC,
	[EntidadeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntidadeCadastrador]    Script Date: 06/01/2016 19:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntidadeCadastrador](
	[EntidadeId] [int] NOT NULL,
	[CadastradorId] [int] NOT NULL,
 CONSTRAINT [PK_EntidadeCadastrador] PRIMARY KEY CLUSTERED 
(
	[EntidadeId] ASC,
	[CadastradorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cupom]    Script Date: 06/01/2016 19:44:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cupom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChaveAcesso] [varchar](100) NULL,
	[DataCompra] [datetime] NOT NULL,
	[COO] [varchar](20) NOT NULL,
	[TipoNota] [int] NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[EntidadeId] [int] NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[CadastradorId] [int] NULL,
	[CNPJEmissor] [varchar](25) NOT NULL,
	[Situacao] [int] NOT NULL,
	[DataLancamento] [datetime] NOT NULL,
	[DataProcessamento] [datetime] NULL,
	[Imagem1] [image] NULL,
	[Imagem2] [image] NULL,
 CONSTRAINT [PK_Cupom] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_Cadastrador_Usuario]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[Cadastrador]  WITH CHECK ADD  CONSTRAINT [FK_Cadastrador_Usuario] FOREIGN KEY([Id])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[Cadastrador] CHECK CONSTRAINT [FK_Cadastrador_Usuario]
GO
/****** Object:  ForeignKey [FK_Cupom_Cadastrador]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[Cupom]  WITH CHECK ADD  CONSTRAINT [FK_Cupom_Cadastrador] FOREIGN KEY([CadastradorId])
REFERENCES [dbo].[Cadastrador] ([Id])
GO
ALTER TABLE [dbo].[Cupom] CHECK CONSTRAINT [FK_Cupom_Cadastrador]
GO
/****** Object:  ForeignKey [FK_Cupom_Entidade]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[Cupom]  WITH CHECK ADD  CONSTRAINT [FK_Cupom_Entidade] FOREIGN KEY([EntidadeId])
REFERENCES [dbo].[Entidade] ([Id])
GO
ALTER TABLE [dbo].[Cupom] CHECK CONSTRAINT [FK_Cupom_Entidade]
GO
/****** Object:  ForeignKey [FK_Cupom_Usuario]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[Cupom]  WITH CHECK ADD  CONSTRAINT [FK_Cupom_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[Cupom] CHECK CONSTRAINT [FK_Cupom_Usuario]
GO
/****** Object:  ForeignKey [FK_Entidade_Usuario]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[Entidade]  WITH CHECK ADD  CONSTRAINT [FK_Entidade_Usuario] FOREIGN KEY([Id])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[Entidade] CHECK CONSTRAINT [FK_Entidade_Usuario]
GO
/****** Object:  ForeignKey [FK_EntidadeCadastrador_Cadastrador]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[EntidadeCadastrador]  WITH CHECK ADD  CONSTRAINT [FK_EntidadeCadastrador_Cadastrador] FOREIGN KEY([CadastradorId])
REFERENCES [dbo].[Cadastrador] ([Id])
GO
ALTER TABLE [dbo].[EntidadeCadastrador] CHECK CONSTRAINT [FK_EntidadeCadastrador_Cadastrador]
GO
/****** Object:  ForeignKey [FK_EntidadeCadastrador_Entidade]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[EntidadeCadastrador]  WITH CHECK ADD  CONSTRAINT [FK_EntidadeCadastrador_Entidade] FOREIGN KEY([EntidadeId])
REFERENCES [dbo].[Entidade] ([Id])
GO
ALTER TABLE [dbo].[EntidadeCadastrador] CHECK CONSTRAINT [FK_EntidadeCadastrador_Entidade]
GO
/****** Object:  ForeignKey [FK_UsuarioEntidadeFavorita_Entidade]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[UsuarioEntidadeFavorita]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioEntidadeFavorita_Entidade] FOREIGN KEY([EntidadeId])
REFERENCES [dbo].[Entidade] ([Id])
GO
ALTER TABLE [dbo].[UsuarioEntidadeFavorita] CHECK CONSTRAINT [FK_UsuarioEntidadeFavorita_Entidade]
GO
/****** Object:  ForeignKey [FK_UsuarioEntidadeFavorita_Usuario]    Script Date: 06/01/2016 19:44:45 ******/
ALTER TABLE [dbo].[UsuarioEntidadeFavorita]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioEntidadeFavorita_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[UsuarioEntidadeFavorita] CHECK CONSTRAINT [FK_UsuarioEntidadeFavorita_Usuario]
GO

INSERT INTO Usuario
	VALUES ('123456789123', 'Entidade Amigos dos Autistas', 'Entidade Amigos dos Autistas', 'entidade@teste.com.br', NULL, '', 'Sorocaba', 'SP', NULL)
DECLARE @ultimo INT
SET @ultimo = (SELECT TOP 1
		Id
	FROM Usuario
	ORDER BY 1 DESC)
INSERT INTO Entidade
	VALUES (@ultimo, '', 1)