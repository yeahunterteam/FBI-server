/* SQLite.sql */

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "channel"
-- ----------------------------
CREATE TABLE "channels" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Functions VARCHAR(500) DEFAULT ',log:on,rejoin:on,commands:on',
Channel VARCHAR(20),
Password VARCHAR(30),
Enabled VARCHAR(5) DEFAULT 'false',
Error TEXT,
Language VARCHAR(4) DEFAULT 'enUS'
);

-- ----------------------------
-- Table structure for "localized_console_command"
-- ----------------------------
CREATE TABLE "localized_console_command" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for localized_console_command_help
-- ----------------------------
CREATE TABLE "localized_console_command_help" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for localized_console_warning
-- ----------------------------
CREATE TABLE "localized_console_warning" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4) DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Table structure for "schumix"
-- ----------------------------
CREATE TABLE "schumix" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
FunctionName VARCHAR(20),
FunctionStatus VARCHAR(3)
);

-- ----------------------------
-- Table structure for "uptime"
-- ----------------------------
CREATE TABLE "uptime" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Date TEXT,
Uptime TEXT,
Memory INTEGER
);
