USE [SistemaSolar]
GO

/****** Object:  StoredProcedure [dbo].[spSelPeriodosPorId]    Script Date: 12/08/2021 6:16:33 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Wilson Camacho>
-- Create date: <12/08/2021>
-- Description:	<Consulta por id los periodos del clima.>
-- =============================================
CREATE PROCEDURE [dbo].[spSelPeriodosPorId] 
	-- Add the parameters for the stored procedure here
	@Dia int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Dia, Clima
	FROM Periodo
	WHERE Dia = @Dia
END
GO


