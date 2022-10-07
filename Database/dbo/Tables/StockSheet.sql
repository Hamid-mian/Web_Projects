CREATE TABLE [dbo].[StockSheet] (
    [Name]           VARCHAR (50) NOT NULL,
    [PricePerUnit]   INT          NULL,
    [used]           FLOAT (53)   NULL,
    [available]      FLOAT (53)   NULL,
    [availablePrice] FLOAT (53)   NULL,
    [usedPrice]      FLOAT (53)   NULL,
    PRIMARY KEY CLUSTERED ([Name] ASC)
);

