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

CREATE TABLE `LoginUsers` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Username` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_LoginUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

-- Insert sample accounts
INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
VALUES 
(UUID(), 'John', 'Doe', 'john.doe@example.com', 1000.00, NOW(), NOW()),
(UUID(), 'Jane', 'Smith', 'jane.smith@example.com', 2500.50, NOW(), NOW()),
(UUID(), 'Alice', 'Johnson', 'alice.johnson@example.com', 750.25, NOW(), NOW()),
(UUID(), 'Bob', 'Brown', 'bob.brown@example.com', 1200.00, NOW(), NOW()),
(UUID(), 'Charlie', 'Davis', 'charlie.davis@example.com', 3000.00, NOW(), NOW()),
(UUID(), 'David', 'Evans', 'david.evans@example.com', 500.00, NOW(), NOW()),
(UUID(), 'Eve', 'Foster', 'eve.foster@example.com', 800.00, NOW(), NOW()),
(UUID(), 'Frank', 'Green', 'frank.green@example.com', 1500.00, NOW(), NOW()),
(UUID(), 'Grace', 'Harris', 'grace.harris@example.com', 2000.00, NOW(), NOW()),
(UUID(), 'Hank', 'Ivy', 'hank.ivy@example.com', 250.00, NOW(), NOW());

-- Insert sample account histories
INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
VALUES 
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 0), NOW(), 100.00, 'Deposit'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 1), NOW(), 200.00, 'Withdrawal'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 2), NOW(), 300.00, 'Deposit'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 3), NOW(), 400.00, 'Withdrawal'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 4), NOW(), 500.00, 'Deposit'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 5), NOW(), 600.00, 'Withdrawal'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 6), NOW(), 700.00, 'Deposit'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 7), NOW(), 800.00, 'Withdrawal'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 8), NOW(), 900.00, 'Deposit'),
((SELECT `Id` FROM `Accounts` LIMIT 1 OFFSET 9), NOW(), 1000.00, 'Withdrawal');

-- Insert sample login users
INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
VALUES 
(UUID(), 'user1', 'passwordhash1'),
(UUID(), 'user2', 'passwordhash2'),
(UUID(), 'user3', 'passwordhash3'),
(UUID(), 'user4', 'passwordhash4'),
(UUID(), 'user5', 'passwordhash5'),
(UUID(), 'user6', 'passwordhash6'),
(UUID(), 'user7', 'passwordhash7'),
(UUID(), 'user8', 'passwordhash8'),
(UUID(), 'user9', 'passwordhash9'),
(UUID(), 'user10', 'passwordhash10');

-- Insert migration history
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES 
('20250219095045_InitialCreate', '8.0.13'),
('20250220123123_AddLoginUser', '8.0.13');

COMMIT;
