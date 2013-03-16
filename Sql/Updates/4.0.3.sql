-- ----------------------------
-- Table structure for servers
-- ----------------------------
CREATE TABLE `servers` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` int(10) NOT NULL,
  `ServerName` varchar(40) NOT NULL DEFAULT '',
  `Server` text NOT NULL,
  `Port` int(10) NOT NULL DEFAULT '6667',
  `SslType` varchar(5) NOT NULL DEFAULT 'false',
  `NickName` varchar(40) NOT NULL DEFAULT 'FBI',
  `NickName2` varchar(40) NOT NULL DEFAULT '_FBI',
  `NickName3` varchar(40) NOT NULL DEFAULT '__FBI',
  `UserName` varchar(40) NOT NULL DEFAULT 'FBI',
  `UserInfo` varchar(40) NOT NULL DEFAULT 'FBI IRC Bot',
  `UseNickServ` varchar(5) NOT NULL DEFAULT 'false',
  `NickServPassword` text NOT NULL,
  `UseHostServ` varchar(5) NOT NULL DEFAULT 'false',
  `HostServEnabled` varchar(5) NOT NULL DEFAULT 'false',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
