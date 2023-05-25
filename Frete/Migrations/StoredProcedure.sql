IF OBJECT_ID('AdicionarFrete', 'P') IS NOT NULL
    DROP PROCEDURE AdicionarFrete;


  CREATE PROCEDURE dbo.AdicionarFrete
    @CepOrigem VARCHAR(50),
    @CepDestino VARCHAR(50),
    @CodigoServicoEnvio VARCHAR(50),
    @ValorRemessa DECIMAL(18, 2),
    @Largura DECIMAL(18, 2),
    @Comprimento DECIMAL(18, 2),
    @Altura DECIMAL(18, 2),
    @Peso DECIMAL(18, 2),
    @Quantidade INT
    @DataUltimaAtualizacao DateTime
AS
BEGIN
    INSERT INTO Frete (CepOrigem, CepDestino, CodigoServicoEnvio, ValorRemessa, Largura, Comprimento, Altura, Peso, Quantidade, DataUltimaAtualizacao)
    VALUES (@CepOrigem, @CepDestino, @CodigoServicoEnvio, @ValorRemessa, @Largura, @Comprimento, @Altura, @Peso, @Quantidade, @DataUltimaAtualizacao)
END