CREATE DATABASE dbCheckPecas
ON PRIMARY(
NAME = dbCheckPecas,
FILENAME = 'D:\BD\dbCheckPecas.mdf',
SIZE = 10MB,
MAXSIZE = 30MB,
FILEGROWTH = 10%
);
GO
USE dbCheckPecas;
GO
CREATE TABLE tblUsuarios
(
	codigo INT PRIMARY KEY IDENTITY (10,1),
	loginUsuario VARCHAR(50) NOT NULL,
	senhaUsuario VARCHAR(20) NOT NULL
);
CREATE TABLE tblPecas
(
	codigo INT PRIMARY KEY IDENTITY (1001,1),
	nomePeca VARCHAR(50) NOT NULL,
	prejuizo MONEY NOT NULL
);

CREATE TABLE tblRegistros
(
	codUsuario INT NOT NULL,
	codPecas INT NOT NULL,
	dtRegistro DATETIME NOT NULL,
	aprovadas INT NOT NULL,
	reprovadas INT NOT NULL,
	produzidas INT NOT NULL,
	prejuizo MONEY NOT NULL,
	email VARCHAR(50)
);

ALTER TABLE tblRegistros
ADD CONSTRAINT fk_codUsuario
FOREIGN KEY(codUsuario) 
REFERENCES tblUsuarios(codigo);

ALTER TABLE tblRegistros
ADD CONSTRAINT fk_codPecas
FOREIGN KEY(codPecas) 
REFERENCES tblPecas(codigo);

ALTER TABLE tblRegistros
ADD CONSTRAINT pk_registro
PRIMARY KEY(codUsuario, codPecas);

BULK
INSERT tblUsuarios
FROM 'D:\BD\usuarios.txt'
WITH
(FIRSTROW = 2,
FIELDTERMINATOR = ';',
ROWTERMINATOR = '\n',
CODEPAGE = '1252');

BULK
INSERT tblPecas
FROM 'D:\BD\pecas.txt'
WITH
(FIRSTROW = 2,
FIELDTERMINATOR = ';',
ROWTERMINATOR = '\n',
CODEPAGE = '1252');

SELECT * FROM tblPecas;


SELECT loginUsuario, senhaUsuario FROM tblUsuarios WHERE loginUsuario = 'admin@sistema.com' AND senhaUsuario = 'nimda';
