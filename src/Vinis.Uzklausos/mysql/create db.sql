
CREATE SCHEMA `vinis` ;


CREATE TABLE `vinis`.`Uzklausos` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `tipas` VARCHAR(10) NOT NULL,
  `iniciatorius` INT NOT NULL,
  `gavejas` INT NOT NULL,
  `skyrius` INT NOT NULL,
  `kategorija` INT NOT NULL,
  `vykdytojas` INT NULL,
  `ivykdyti_iki` DATETIME NOT NULL,
  `prioritetas` VARCHAR(10) NULL,
  `pavadinimas` VARCHAR(255) NOT NULL,
  `tekstas` TEXT NOT NULL,
  PRIMARY KEY (`id`));

CREATE TABLE `vinis`.`UzklausuEiga` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `uzklausos_id` INT NOT NULL,
  `data` DATETIME NOT NULL,
  `turinys` TEXT NOT NULL,
  PRIMARY KEY (`id`));

ALTER TABLE `vinis`.`Uzklausos` 
ADD COLUMN `statusas` VARCHAR(15) NULL AFTER `tekstas`;

UPDATE Uzklausos
SET statusas = 'Iregistruota'
WHERE  Uzklausos.statusas is null;

ALTER TABLE `vinis`.`Uzklausos` 
CHANGE COLUMN `statusas` `statusas` VARCHAR(15) NOT NULL ;

CREATE SCHEMA `asmenys` ;

CREATE TABLE `asmenys`.`asmenys` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `sistema_id` INT NULL,
  `vardas_pavarde_pavadinimas` VARCHAR(255) NOT NULL,
  `asmens_kodas` BIGINT NOT NULL,
  `istaiga` VARCHAR(255) NULL,
  `telefonas` VARCHAR(30) NULL,
  `el_pastas` VARCHAR(255) NULL,
  `papildoma_informacija` TEXT NULL,
  `sukurimo_data` DATETIME NOT NULL,
  `atnaujinimo_data` DATETIME NOT NULL,
  PRIMARY KEY (`id`));


