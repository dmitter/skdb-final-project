CREATE TABLE `Positions`
(
    `PositionId` int unsigned PRIMARY KEY AUTO_INCREMENT,
    `Name`       varchar(255) NOT NULL UNIQUE,
    `HourlyRate` int          NOT NULL CHECK (`HourlyRate` >= 0 and `HourlyRate` <= 100)
);

CREATE TABLE `Tasks`
(
    `TaskId` int unsigned PRIMARY KEY AUTO_INCREMENT,
    `Name`   varchar(255) NOT NULL UNIQUE
);

CREATE TABLE `Employees`
(
    `EmployeeId` int unsigned PRIMARY KEY AUTO_INCREMENT,
    `Name`       varchar(255) NOT NULL UNIQUE,
    `PositionId` int unsigned NOT NULL,
    CONSTRAINT `FK_Employees_Positions_PositionId` FOREIGN KEY (`PositionId`) REFERENCES `Positions` (`PositionId`)
);

CREATE TABLE `TimeSpent`
(
    `TimeSpentId` int unsigned PRIMARY KEY AUTO_INCREMENT,
    `TaskId`      int unsigned NOT NULL,
    `EmployeeId`  int unsigned NOT NULL,
    `StartTime`   datetime(6)  NOT NULL,
    `EndTime`     datetime(6)  NOT NULL,
    CONSTRAINT `FK_TimeSpent_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `Employees` (`EmployeeId`) ON DELETE CASCADE,
    CONSTRAINT `FK_TimeSpent_Tasks_TaskId` FOREIGN KEY (`TaskId`) REFERENCES `Tasks` (`TaskId`) ON DELETE CASCADE
);

CREATE TABLE `TimesheetHistory`
(
    `EventId`    int unsigned PRIMARY KEY AUTO_INCREMENT,
    `EmployeeId` int unsigned NOT NULL,
    `TaskName`   text         NOT NULL,
    `StartTime`  datetime(6)  NOT NULL,
    `EndTime`    datetime(6)  NOT NULL,
    CONSTRAINT `FK_TimesheetHistory_Employees_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `Employees` (`EmployeeId`) ON DELETE CASCADE
);

CREATE TRIGGER `WriteHistory`
    AFTER DELETE
    ON TimeSpent
    FOR EACH ROW
    INSERT INTO `TimesheetHistory`
    SET `EmployeeId`=OLD.`EmployeeId`,
        `TaskName`=(SELECT `Name` FROM `Tasks` WHERE `TaskId` = OLD.`TaskId`),
        `StartTime`=OLD.`StartTime`,
        `EndTime`=OLD.`EndTime`;