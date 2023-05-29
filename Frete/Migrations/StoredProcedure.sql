IF OBJECT_ID('InsertCotacao', 'P') IS NOT NULL
    DROP PROCEDURE InsertCotacao;

CREATE PROCEDURE dbo.InsertCotacao
    @SellerCEP VARCHAR(50),
    @RecipientCEP VARCHAR(50),
    @ShippingServiceCode INT,
    @ShipmentInvoiceValue DECIMAL(10, 2),
    @Width VARCHAR(50),
    @Length VARCHAR(50),
    @Height VARCHAR(50),
    @Weight VARCHAR(50),
    @Quantity INT,
    @RecipientCountry VARCHAR(100),
    @DateLastUpdate DateTime,
    @InsertedId INT OUTPUT
AS
BEGIN
    -- Inserir o registro na tabela Cotacoes
    INSERT INTO [Frete].[dbo].[Cotacao] (SellerCEP, RecipientCEP, ShippingServiceCode, ShipmentInvoiceValue, Width, Length, Height, Weight, Quantity, RecipientCountry, DateLastUpdate)
    VALUES (@SellerCEP, @RecipientCEP, @ShippingServiceCode, @ShipmentInvoiceValue, @Width, @Length, @Height, @Weight, @Quantity, @RecipientCountry, @DateLastUpdate)

    -- Retornar o ID do registro inserido
    SET @InsertedId = SCOPE_IDENTITY()
END

---------------------------------------------------------------------------------------

IF OBJECT_ID('InsertShippingService', 'P') IS NOT NULL
    DROP PROCEDURE InsertShippingService;

CREATE PROCEDURE dbo.InsertShippingService
    @CotacaoId INT,
    @ServiceCode VARCHAR(50),
    @ServiceDescription VARCHAR(100),
    @Carrier VARCHAR(100),
    @CarrierCode VARCHAR(50),
    @ShippingPrice DECIMAL(10, 2),
    @DeliveryTime INT,
    @Error BIT,
    @Msg VARCHAR(100),
    @OriginalDeliveryTime INT,
    @OriginalShippingPrice DECIMAL(10, 2),
    @ResponseTime VARCHAR(50),
    @AllowBuyLabel BIT
AS
BEGIN
    -- Inserir o registro na tabela ShippingService
    INSERT INTO [Frete].[dbo].[ShippingService] (CotacaoId, ServiceCode, ServiceDescription, Carrier, CarrierCode, ShippingPrice, DeliveryTime, Error, Msg, OriginalDeliveryTime, OriginalShippingPrice, ResponseTime, AllowBuyLabel)
    VALUES (@CotacaoId, @ServiceCode, @ServiceDescription, @Carrier, @CarrierCode, @ShippingPrice, @DeliveryTime, @Error, @Msg, @OriginalDeliveryTime, @OriginalShippingPrice, @ResponseTime, @AllowBuyLabel)
END


---------------------------------------------------------------------------------------

ALTER TABLE [Frete].[dbo].[ShippingService]
ADD [CotacoesId] INT;

ALTER TABLE [Frete].[dbo].[ShippingService]
ADD CONSTRAINT [FK_ShippingService_Cotacoes]
FOREIGN KEY ([CotacoesId]) REFERENCES [Frete].[dbo].[Cotacoes] ([Id]);

