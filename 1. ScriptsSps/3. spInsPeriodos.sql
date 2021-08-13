USE [SistemaSolar]
GO

/****** Object:  StoredProcedure [dbo].[spInsPeriodos]    Script Date: 12/08/2021 6:16:10 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Wilson Camacho>
-- Create date: <11/08/2021>
-- Description:	<Inserta los periodos del clima por dia.>
-- =============================================
CREATE PROCEDURE [dbo].[spInsPeriodos]
	-- Add the parameters for the stored procedure here
	@Dia int,
	@Clima VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Periodo(Dia, Clima)
	VALUES (@Dia, @Clima)

END
GO


