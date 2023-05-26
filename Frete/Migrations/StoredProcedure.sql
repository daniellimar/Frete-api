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
(
    @ServiceCode VARCHAR(50),
    @ServiceDescription VARCHAR(100),
    @Carrier VARCHAR(100),
    @CarrierCode VARCHAR(50),
    @ShippingPrice DECIMAL(10, 2),
    @DeliveryTime INT,
    @Error BIT,
    @Msg VARCHAR(100),
    @OriginalDeliveryTime VARCHAR(50),
    @OriginalShippingPrice VARCHAR(50),
    @ResponseTime VARCHAR(50),
    @AllowBuyLabel BIT
)
AS
BEGIN
    INSERT INTO ShippingService
    (
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
    VALUES
    (
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
