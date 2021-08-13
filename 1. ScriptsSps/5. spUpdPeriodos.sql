USE [SistemaSolar]
GO

/****** Object:  StoredProcedure [dbo].[spUpdPeriodos]    Script Date: 12/08/2021 6:16:54 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Wilson Camacho>
-- Create date: <12/08/2021>
-- Description:	<Actualiza los periodos del clima por dia.>
-- =============================================
CREATE PROCEDURE [dbo].[spUpdPeriodos]
	-- Add the parameters for the stored procedure here
	@Dia int,
	@Clima VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Periodo
	SET Clima = @Clima
	WHERE Dia = @Dia
END
GO


