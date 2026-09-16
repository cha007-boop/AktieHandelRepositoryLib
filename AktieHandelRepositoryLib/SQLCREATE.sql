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

INSERT INTO AktieHandel ([Name], Amount, ExchangePrice)
VALUES ('Microsoft', 100, 120.0),
	   ('Apple', 50, 80.0),
	   ('Amazon', 25, 350.0),
	   ('Broad Index', 300, 200.0);