/* MySql.sql */

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for channels
-- ----------------------------
CREATE TABLE `channels` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Functions` varchar(500) NOT NULL default ',log:on,rejoin:on,commands:on',
  `Channel` varchar(20) NOT NULL default '',
  `Password` varchar(30) NOT NULL default '',
  `Enabled` varchar(5) NOT NULL default 'false',
  `Error` text NOT NULL,
  `Language` varchar(4) NOT NULL default 'enUS',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for localized_console_command
-- ----------------------------
CREATE TABLE `localized_console_command` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_console_command_help
-- ----------------------------
CREATE TABLE `localized_console_command_help` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_console_warning
-- ----------------------------
CREATE TABLE `localized_console_warning` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for schumix
-- ----------------------------
CREATE TABLE `schumix` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `FunctionName` varchar(20) NOT NULL default '',
  `FunctionStatus` varchar(3) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for uptime
-- ----------------------------
CREATE TABLE `uptime` (
  `Id` int(100) unsigned NOT NULL auto_increment,
  `Date` text NOT NULL,
  `Uptime` text NOT NULL,
  `Memory` int(20) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
