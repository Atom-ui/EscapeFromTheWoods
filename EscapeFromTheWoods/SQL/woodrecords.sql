﻿CREATE TABLE [dbo].[woodrecords]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [woodid] INT NOT NULL, 
    [treeid] INT NOT NULL, 
    [x] FLOAT NOT NULL, 
    [y] FLOAT NOT NULL
)
