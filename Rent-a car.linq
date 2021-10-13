<Query Kind="SQL">
  <Connection>
    <ID>f3865f73-49b9-43be-84fb-604035c8066c</ID>
    <Persist>true</Persist>
    <Server>.\SQLEXPRESS</Server>
    <Database>seminar</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

CREATE DATABASE seminar

CREATE TABLE AUTOMOBIL (
	Reg_Oznaka VARCHAR(10) PRIMARY KEY,
	Model VARCHAR(30),
	Marka_vozila VARCHAR(20),
	Starost INT,
	Cijena DECIMAL(6,2),
	Kilometri INT,
	Datum_Registracije DATETIME,
	Iznajmljen VARCHAR(2)
	);

CREATE TABLE KLIJENT(
	OIB VARCHAR(11) PRIMARY KEY,
	Ime VARCHAR(20),
	Prezime VARCHAR(20),
	Adresa VARCHAR(50),
	Broj_telefona VARCHAR(15),
	Broj_vozacke_dozvole VARCHAR(20) UNIQUE	
);

CREATE TABLE ZAPOSLENIK(
	OIB VARCHAR(11) PRIMARY KEY,
	Ime VARCHAR(20),
	Prezime VARCHAR(20),
	Adresa VARCHAR(50),
	Broj_telefona VARCHAR(15)
);

CREATE TABLE UGOVOR(
	Broj_racuna INT PRIMARY KEY,
	Vrijeme_pocetka DATETIME,
	Vrijeme_zavrsetka DATETIME,
	Iznos_Placanja DECIMAL(6,2),
	Registracija VARCHAR(10),
	OIB_zaposlenika VARCHAR(11),
	OIB_klijenta VARCHAR(11),
	
	CONSTRAINT ugovor_fk_automobil FOREIGN KEY(Registracija) REFERENCES AUTOMOBIL(Reg_Oznaka),
	CONSTRAINT ugovor_fk_zaposlenik FOREIGN KEY(OIB_zaposlenika) REFERENCES ZAPOSLENIK(OIB),
	CONSTRAINT ugovor_fk_klijent FOREIGN KEY(OIB_klijenta) REFERENCES KLIJENT(OIB)
);

INSERT INTO AUTOMOBIL VALUES('os123dd', '3', 'Tesla', '17', '20', '132419', '12.1.2020', 'NE')
INSERT INTO AUTOMOBIL VALUES('os122dd', 'Corsa', 'Opel', '2', '22', '99000', '2.13.2020', 'DA')
INSERT INTO AUTOMOBIL VALUES('os123ff', 'Golf', 'VW', '5', '18', '110000', '11.11.2020', 'DA')
INSERT INTO AUTOMOBIL VALUES('os222fg', '323', 'MAZDA', '6', '16', '12999', '1.17.2020', 'NE')
INSERT INTO AUTOMOBIL VALUES('os234tf', '6', 'MAZDA', '4', '16', '12999', '4.17.2020', 'NE')
INSERT INTO AUTOMOBIL VALUES('os356mt', 'vanquish', 'Aston Martin', '4', '40', '12999', '6.17.2020', 'DA')
INSERT INTO AUTOMOBIL VALUES('os978ii', 'Passat', 'VW', '3', '16', '12999', '2.18.2020', 'NE')
INSERT INTO AUTOMOBIL VALUES('os888jk', '325', 'BMW', '11', '16', '12999', '1.17.2020', 'DA')
INSERT INTO AUTOMOBIL VALUES('os666fu', 'A6', 'AUDI', '9', '16', '12999', '3.7.2020', 'NE')
INSERT INTO AUTOMOBIL VALUES('os567jo', 'XF', 'Jaguar', '6', '16', '12999', '10.1.2020', 'NE')



SELECT *FROM AUTOMOBIL

INSERT INTO KLIJENT VALUES('12345678910','Marko', 'Marulic', '11.street Osijek' , '099222333444', '1234567812')
INSERT INTO KLIJENT VALUES('12345678911','Ivan', 'Ivicc', '12.street Osijek', '099222333256444', '123233367813')
INSERT INTO KLIJENT VALUES('12345678912','Sinan', 'Zeljezo', '29.street Zagreb', '09922267833444', '1234567814')
INSERT INTO KLIJENT VALUES('12345678913','Fabijan', 'Fabijanovic', '12.street Split', '0994442332444', '123233367815')
INSERT INTO KLIJENT VALUES('12345678914','Bruno', 'Bergic', '44.street Zadar', '099244433444', '1234567816')
INSERT INTO KLIJENT VALUES('12345678915','Ana', 'Sokol', '67.street Zadar', '0992222343244444', '123233367817')
INSERT INTO KLIJENT VALUES('12345678916','Silvijo', 'Cool', '99.street Osijek', '099223423443444', '1234567818')
INSERT INTO KLIJENT VALUES('12345678917','Mario', 'Marunic', '122.street Dubrovnik', '0992223242444', '123233367819')


SELECT *FROM KLIJENT

INSERT INTO ZAPOSLENIK VALUES('12345678913', 'Zaposlenik', 'Zaposlenkovic', '29. Str Osijek', '09988887777')
INSERT INTO ZAPOSLENIK VALUES('12345678914', 'Lucky', 'Luke', '29.Str Osijek', '09988887778')
INSERT INTO ZAPOSLENIK VALUES('12345678915', 'Zaposlenik', 'Zaposlenkovic', '11. Str Osijek', '0998833387777')

SELECT *FROM ZAPOSLENIK

INSERT INTO UGOVOR VALUES('12345', '2.2.2020', '2.19.2020', '0', 'os123dd', '12345678913', '1234567812')
INSERT INTO UGOVOR VALUES('12346', '1.2.2020', '1.19.2020', '0', 'os567jo', '12345678913', '12345678910')
INSERT INTO UGOVOR VALUES('12347', '2.21.2020', '2.22.2020', '0', 'os356mt', '12345678913', '12345678913')
INSERT INTO UGOVOR VALUES('12349', '10.16.2020', '10.26.2020', '0', 'os123dd', '12345678914', '12345678917')
INSERT INTO UGOVOR VALUES('123451', '3.20.2020', '3.21.2020', '0', 'os123ff', '12345678914', '12345678911')
INSERT INTO UGOVOR VALUES('123452', '12.11.2020', '12.13.2020', '0', 'os978ii', '12345678915', '12345678916')
INSERT INTO UGOVOR VALUES('123453', '9.12.2020', '9.19.2020', '0', 'os567jo', '12345678915', '12345678916')

SELECT *FROM UGOVOR

CREATE FUNCTION vrijeme_iznajmmljeno (@broj_racuna INT) 
RETURNS INT
BEGIN 
DECLARE @vrijeme INT;
DECLARE @datum_pocetka DATETIME;
DECLARE @datum_kraja DATETIME;
SET @datum_pocetka = (SELECT Vrijeme_pocetka FROM UGOVOR WHERE Broj_racuna = @broj_racuna);
SET @datum_kraja = (SELECT Vrijeme_zavrsetka FROM UGOVOR WHERE Broj_racuna = @broj_racuna);
SET @vrijeme = (SELECT DATEDIFF(day,@datum_pocetka,@datum_kraja)); 
RETURN @vrijeme
END

DROP FUNCTION vrijeme_iznajmmljeno

SELECT dbo.vrijeme_iznajmmljeno('12345')

CREATE PROCEDURE cijena_najma (@broj_racuna INT)
AS
DECLARE @vrijeme INT;
DECLARE @cijena INT;
DECLARE @cijena_najma INT;
SET @vrijeme = (SELECT dbo.vrijeme_iznajmmljeno(@broj_racuna));
SET @cijena = (SELECT Cijena FROM AUTOMOBIL,UGOVOR WHERE UGOVOR.Broj_racuna = @broj_racuna AND AUTOMOBIL.Reg_Oznaka = UGOVOR.Registracija);
SET @cijena_najma = @vrijeme * @cijena;
UPDATE UGOVOR SET Iznos_Placanja = @cijena_najma WHERE Broj_racuna = @broj_racuna;
PRINT @cijena_najma;
RETURN(0)

exec cijena_najma @broj_racuna = 12345


DROP PROCEDURE cijena_najma 

CREATE FUNCTION vrijeme_za_registraciju (@reg VARCHAR(10))
RETURNS VARCHAR(50)
BEGIN 
DECLARE @vrijeme INT;
DECLARE @datum_reg DATETIME;
DECLARE @poruka VARCHAR (50);
SET @datum_reg = (SELECT Datum_Registracije FROM AUTOMOBIL WHERE Reg_Oznaka = @reg);
SET @vrijeme = (SELECT DATEDIFF(day,@datum_reg,GETDATE())); 
IF @vrijeme > 365
SET @poruka = ('Registracija za vozilo ' + @reg + ' Je istekla');
ELSE
BEGIN
SET @vrijeme = 365-@vrijeme;
SET @poruka = ( 'Registracija za vozilo ' + @reg + ' Istice za ' + CAST(@vrijeme AS VARCHAR) + ' Dana');
END
RETURN @poruka
END

DROP FUNCTION vrijeme_za_registraciju

SELECT dbo.vrijeme_za_registraciju('os123ff')

CREATE PROCEDURE Unos_Klijenta (@OIB VARCHAR(11), @Ime VARCHAR(20),@Prezime VARCHAR(20), @Adresa VARCHAR(50), @Br_tel VARCHAR(15), @Broj_vozacke VARCHAR(20))
AS
INSERT INTO KLIJENT VALUES('@OIB','@Ime','@Prezime','@Adresa','@Br_tel','@Broj_vozacke');
RETURN(0)

CREATE PROCEDURE Unos_Novog_Zaposlenika (@OIB VARCHAR(11), @Ime VARCHAR(20),@Prezime VARCHAR(20), @Adresa VARCHAR(50), @Br_tel VARCHAR(15))
AS
INSERT INTO ZAPOSLENIK VALUES('@OIB','@Ime','@Prezime','@Adresa','@Br_tel');
RETURN(0)

CREATE PROCEDURE Unos_Novog_Automobila (@Reg VARCHAR(10), @Model VARCHAR(30), @Marka VARCHAR(20), @Starost INT, @Cijena DECIMAL(6,2), @Kilometri INT, @Dat_reg DATETIME, @Iznajmljen VARCHAR(2))
AS
INSERT INTO AUTOMOBIL VALUES('@Reg','@Model','@Marka','@Starost','@Cijena','@Kilometri','@Dat_reg','@Iznajmljen');
RETURN(0)

CREATE PROCEDURE Unos_Novog_Ugovora (@Broj_rac INT, @Vrijeme_pocetka DATETIME, @Vrijeme_zavrsetka DATETIME, @Iznos_plac DECIMAL(6,2), @Reg VARCHAR(10), @OIB_zapo VARCHAR(11), @Oib_kli VARCHAR(11))
AS
INSERT INTO UGOVOR VALUES('@Broj_rac','@Vrijeme_pocetka','@Vrijeme_zavrsetka','@Iznos_plac','@Reg','@OIB_zapo','@Oib_kli');
RETURN(0)

CREATE PROCEDURE Izbrisi_automobil (@Reg VARCHAR(10))
AS
DELETE FROM AUTOMOBIL WHERE Reg_Oznaka = @Reg;
RETURN(0)

exec Izbrisi_automobil @Reg = os122ff



