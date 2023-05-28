IF OBJECT_ID('InsertCotacao', 'P') IS NOT NULL
    DROP PROCEDURE InsertCotacao;

CREATE PROCEDURE InsertCotacao
    @SellerCEP VARCHAR(50),
    @RecipientCountry VARCHAR(50),
    @ShippingServiceCode VARCHAR(50),
    @Width DECIMAL(18,2),
    @ShipmentInvoiceValue DECIMAL(18,2),
    @Length DECIMAL(18,2),
    @Height DECIMAL(18,2),
    @Weight DECIMAL(18,2),
    @Quantity INT,
    @RecipientCEP VARCHAR(50)
AS
BEGIN
    INSERT INTO [dbo].[Cotacoes] (
        [SellerCEP],
        [RecipientCountry],
        [ShippingServiceCode],
        [Width],
        [ShipmentInvoiceValue],
        [Length],
        [Height],
        [Weight],
        [Quantity],
        [RecipientCEP]
    )
    VALUES (
        @SellerCEP,
        @RecipientCountry,
        @ShippingServiceCode,
        @Width,
        @ShipmentInvoiceValue,
        @Length,
        @Height,
        @Weight,
        @Quantity,
        @RecipientCEP
    )
END


IF OBJECT_ID('InsertShippingService', 'P') IS NOT NULL
    DROP PROCEDURE InsertShippingService;

CREATE PROCEDURE InsertShippingService
    @ServiceCode NVARCHAR(50),
    @ServiceDescription NVARCHAR(50),
    @Carrier NVARCHAR(50),
    @CarrierCode NVARCHAR(50),
    @ShippingPrice DECIMAL(10, 2),
    @DeliveryTime NVARCHAR(50),
    @Error BIT,
    @Msg NVARCHAR(MAX),
    @OriginalDeliveryTime NVARCHAR(50),
    @OriginalShippingPrice DECIMAL(10, 2),
    @ResponseTime NVARCHAR(50),
    @AllowBuyLabel BIT
AS
BEGIN
    SET @Msg = ISNULL(@Msg, 'FALSE') -- Define 'FALSE' se @Msg for NULL

    INSERT INTO ShippingService (
        ServiceCode,
        ServiceDescription,
        Carrier,
        CarrierCode,
        ShippingPrice,
        DeliveryTime,
        Error,
        Msg,
        OriginalDeliveryTime,
        OriginalShippingPrice,
        ResponseTime,
        AllowBuyLabel
    )
    VALUES (
        @ServiceCode,
        @ServiceDescription,
        @Carrier,
        @CarrierCode,
        @ShippingPrice,
        @DeliveryTime,
        @Error,
        @Msg,
        @OriginalDeliveryTime,
        @OriginalShippingPrice,
        @ResponseTime,
        @AllowBuyLabel
    )
END
