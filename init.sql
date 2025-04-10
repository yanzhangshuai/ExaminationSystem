CREATE DATABASE IF NOT EXISTS exam_sym DEFAULT CHARACTER SET utf8mb4;

USE exam_sym;

CREATE TABLE `Student` (
   `Id`             INT             NOT NULL AUTO_INCREMENT,
   `Name`           VARCHAR(20)     NOT NULL,
   CONSTRAINT `PK_Student` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;
