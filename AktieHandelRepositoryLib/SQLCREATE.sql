DROP TABLE IF EXISTS AktieHandel

CREATE TABLE AktieHandel (
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] varchar(255) NOT NULL,
	Amount int NOT NULL,
	ExchangePrice FLOAT NOT NULL,
		CONSTRAINT NonNegativePrice
		CHECK (ExchangePrice > 0),
		CONSTRAINT NonZeroAmount
		CHECK (Amount != 0)
);