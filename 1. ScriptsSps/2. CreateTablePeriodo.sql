USE [SistemaSolar]
GO

/****** Object:  Table [dbo].[Periodo]    Script Date: 12/08/2021 6:13:42 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Periodo](
	[Dia] [int] NOT NULL,
	[Clima] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Periodo] PRIMARY KEY CLUSTERED 
(
	[Dia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dia a mostrar' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Periodo', @level2type=N'COLUMN',@level2name=N'Dia'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo de Clima' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Periodo', @level2type=N'COLUMN',@level2name=N'Clima'
GO


