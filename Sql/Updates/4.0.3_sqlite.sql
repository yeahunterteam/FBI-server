-- ----------------------------
-- Table structure for servers
-- ----------------------------
CREATE TABLE `servers` (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER,
ServerName VARCHAR(40) NOT NULL DEFAULT '',
Server TEXT,
Port INTEGER DEFAULT '6667',
SslType VARCHAR(5) DEFAULT 'false',
NickName VARCHAR(40) DEFAULT 'FBI',
NickName2 VARCHAR(40) DEFAULT '_FBI',
NickName3 VARCHAR(40) DEFAULT '__FBI',
UserName VARCHAR(40) DEFAULT 'FBI',
UserInfo VARCHAR(40) DEFAULT 'FBI IRC Bot',
UseNickServ VARCHAR(5) DEFAULT 'false',
NickServPassword TEXT,
UseHostServ VARCHAR(5) DEFAULT 'false',
HostServEnabled VARCHAR(5) DEFAULT 'false'
);
