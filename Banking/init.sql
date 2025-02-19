CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Accounts` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Surname` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Balance` decimal(18,2) NOT NULL,
    `DateCreated` datetime NOT NULL,
    `DateModified` datetime NOT NULL,
    CONSTRAINT `PK_Accounts` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AccountHistories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `TransactionDate` datetime NOT NULL,
    `Amount` decimal(18,2) NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AccountHistories` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AccountHistories_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AccountHistories_AccountId` ON `AccountHistories` (`AccountId`);

-- Insert sample accounts
INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
VALUES 
(UUID(), 'John', 'Doe', 'john.doe@example.com', 1000.00, NOW(), NOW()),
(UUID(), 'Jane', 'Smith', 'jane.smith@example.com', 2500.50, NOW(), NOW()),
(UUID(), 'Alice', 'Johnson', 'alice.johnson@example.com', 750.25, NOW(), NOW());

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250219095045_InitialCreate', '8.0.13');

COMMIT;