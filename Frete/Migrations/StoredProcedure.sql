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
AS
BEGIN
    INSERT INTO Frete (CepOrigem, CepDestino, CodigoServicoEnvio, ValorRemessa, Largura, Comprimento, Altura, Peso, Quantidade)
    VALUES (@CepOrigem, @CepDestino, @CodigoServicoEnvio, @ValorRemessa, @Largura, @Comprimento, @Altura, @Peso, @Quantidade)
END