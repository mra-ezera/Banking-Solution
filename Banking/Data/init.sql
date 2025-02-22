CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE IF NOT EXISTS `Accounts` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Surname` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Balance` decimal(18,2) NOT NULL,
    `DateCreated` datetime NOT NULL,
    `DateModified` datetime NOT NULL,
    CONSTRAINT `PK_Accounts` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE IF NOT EXISTS `AccountHistories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AccountId` char(36) COLLATE ascii_general_ci NOT NULL,
    `TransactionDate` datetime NOT NULL,
    `Amount` decimal(18,2) NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AccountHistories` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AccountHistories_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AccountHistories_AccountId` ON `AccountHistories` (`AccountId`);

CREATE TABLE IF NOT EXISTS `LoginUsers` (
    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
    `Username` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_LoginUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

-- Insert sample accounts if not exists
INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'John', 'Doe', 'john.doe@example.com', 1000.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'john.doe@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Jane', 'Smith', 'jane.smith@example.com', 2500.50, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'jane.smith@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Alice', 'Johnson', 'alice.johnson@example.com', 750.25, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'alice.johnson@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Bob', 'Brown', 'bob.brown@example.com', 1200.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'bob.brown@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Charlie', 'Davis', 'charlie.davis@example.com', 3000.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'charlie.davis@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'David', 'Evans', 'david.evans@example.com', 500.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'david.evans@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Eve', 'Foster', 'eve.foster@example.com', 800.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'eve.foster@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Frank', 'Green', 'frank.green@example.com', 1500.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'frank.green@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Grace', 'Harris', 'grace.harris@example.com', 2000.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'grace.harris@example.com');

INSERT INTO `Accounts` (`Id`, `Name`, `Surname`, `Email`, `Balance`, `DateCreated`, `DateModified`)
SELECT UUID(), 'Hank', 'Ivy', 'hank.ivy@example.com', 250.00, NOW(), NOW()
WHERE NOT EXISTS (SELECT 1 FROM `Accounts` WHERE `Email` = 'hank.ivy@example.com');

-- Insert sample account histories if not exists
INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'john.doe@example.com'), NOW(), 100.00, 'Deposit'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'john.doe@example.com') AND `Description` = 'Deposit');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'jane.smith@example.com'), NOW(), 200.00, 'Withdrawal'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'jane.smith@example.com') AND `Description` = 'Withdrawal');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'alice.johnson@example.com'), NOW(), 300.00, 'Deposit'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'alice.johnson@example.com') AND `Description` = 'Deposit');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'bob.brown@example.com'), NOW(), 400.00, 'Withdrawal'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'bob.brown@example.com') AND `Description` = 'Withdrawal');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'charlie.davis@example.com'), NOW(), 500.00, 'Deposit'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'charlie.davis@example.com') AND `Description` = 'Deposit');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'david.evans@example.com'), NOW(), 600.00, 'Withdrawal'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'david.evans@example.com') AND `Description` = 'Withdrawal');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'eve.foster@example.com'), NOW(), 700.00, 'Deposit'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'eve.foster@example.com') AND `Description` = 'Deposit');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'frank.green@example.com'), NOW(), 800.00, 'Withdrawal'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'frank.green@example.com') AND `Description` = 'Withdrawal');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'grace.harris@example.com'), NOW(), 900.00, 'Deposit'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'grace.harris@example.com') AND `Description` = 'Deposit');

INSERT INTO `AccountHistories` (`AccountId`, `TransactionDate`, `Amount`, `Description`)
SELECT (SELECT `Id` FROM `Accounts` WHERE `Email` = 'hank.ivy@example.com'), NOW(), 1000.00, 'Withdrawal'
WHERE NOT EXISTS (SELECT 1 FROM `AccountHistories` WHERE `AccountId` = (SELECT `Id` FROM `Accounts` WHERE `Email` = 'hank.ivy@example.com') AND `Description` = 'Withdrawal');

-- Insert sample login users if not exists
INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user1', 'passwordhash1'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user1');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user2', 'passwordhash2'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user2');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user3', 'passwordhash3'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user3');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user4', 'passwordhash4'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user4');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user5', 'passwordhash5'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user5');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user6', 'passwordhash6'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user6');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user7', 'passwordhash7'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user7');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user8', 'passwordhash8'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user8');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user9', 'passwordhash9'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user9');

INSERT INTO `LoginUsers` (`Id`, `Username`, `PasswordHash`)
SELECT UUID(), 'user10', 'passwordhash10'
WHERE NOT EXISTS (SELECT 1 FROM `LoginUsers` WHERE `Username` = 'user10');

-- Insert migration history if not exists
INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
SELECT '20250219095045_InitialCreate', '8.0.13'
WHERE NOT EXISTS (SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250219095045_InitialCreate');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
SELECT '20250220123123_AddLoginUser', '8.0.13'
WHERE NOT EXISTS (SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250220123123_AddLoginUser');

COMMIT;
