USE [EnterpriseLogging]
GO

/****** Object:  Table [dbo].[EnterpriseLog]    Script Date: 2/16/2018 1:33:56 PM ******/
DROP TABLE [dbo].[EnterpriseLog]
GO

/****** Object:  Table [dbo].[EnterpriseLog]    Script Date: 2/16/2018 1:33:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EnterpriseLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
    [HostName] [varchar](50) NOT NULL,
	[ApplicationName] [varchar](50) NOT NULL,
	[MemberName] [varchar](100) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[CorrelationId] [uniqueidentifier] NULL,
	[Message] [varchar](4000) NULL,
	[Exception] [varchar](max) NULL,
	[SourceFilePath] varchar(1024) NULL,
	[SourceLineNumber] int,
	[Level] [varchar](50) NOT NULL
) ON [PRIMARY]
GO


