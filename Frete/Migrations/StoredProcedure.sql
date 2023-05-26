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
