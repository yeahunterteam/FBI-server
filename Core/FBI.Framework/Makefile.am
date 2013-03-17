
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/FBI.Framework.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/FBI.Api.dll
BUILD_DIR = ../../Run/Debug

FBI_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/FBI.Framework.dll.mdb
FBI_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/FBI.Framework.dll.mdb
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
FBI_DB3_SOURCE=../../Sql/FBI.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
FBI_API_DLL_SOURCE=../../Run/Debug/FBI.Api.dll
FBI_API_DLL_MDB_SOURCE=../../Run/Debug/FBI.Api.dll.mdb
FBI_API_DLL_MDB=$(BUILD_DIR)/FBI.Api.dll.mdb

endif

if ENABLE_RELEASE_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/FBI.Framework.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/FBI.Api.dll
BUILD_DIR = ../../Run/Release

FBI_FRAMEWORK_DLL_MDB=
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
FBI_DB3_SOURCE=../../Sql/FBI.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
FBI_API_DLL_SOURCE=../../Run/Release/FBI.Api.dll
FBI_API_DLL_MDB=

endif

if ENABLE_DEBUG_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug_x64/FBI.Framework.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug_x64/FBI.Api.dll
BUILD_DIR = ../../Run/Debug_x64

FBI_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug_x64/FBI.Framework.dll.mdb
FBI_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/FBI.Framework.dll.mdb
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
FBI_DB3_SOURCE=../../Sql/FBI.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
FBI_API_DLL_SOURCE=../../Run/Debug_x64/FBI.Api.dll
FBI_API_DLL_MDB_SOURCE=../../Run/Debug_x64/FBI.Api.dll.mdb
FBI_API_DLL_MDB=$(BUILD_DIR)/FBI.Api.dll.mdb

endif

if ENABLE_RELEASE_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release_x64/FBI.Framework.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release_x64/FBI.Api.dll
BUILD_DIR = ../../Run/Release_x64

FBI_FRAMEWORK_DLL_MDB=
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
FBI_DB3_SOURCE=../../Sql/FBI.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
FBI_API_DLL_SOURCE=../../Run/Release_x64/FBI.Api.dll
FBI_API_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(FBI_FRAMEWORK_DLL_MDB) \
	$(SQLITE3_DLL) \
	$(FBI_DB3) \
	$(MYSQL_DATA_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(FBI_API_DLL) \
	$(FBI_API_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(FBI_FRAMEWORK_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Config/Config.cs \
	Config/DefaultConfig.cs \
	Singleton.cs \
	FBIBase.cs \
	ConsoleLog.cs \
	Properties/AssemblyInfo.cs \
	Exceptions/SingletonException.cs \
	Database/SQLite.cs \
	Database/DatabaseManager.cs \
	Database/MySql.cs \
	Utilities.cs \
	Timer.cs \
	Localization/LocalizationManager.cs \
	Localization/LocalizationConsole.cs \
	Extensions/GeneralExtensions.cs \
	Network/Opcodes.cs \
	Network/FBIPacket.cs \
	Runtime.cs \
	CrashDumper.cs \
	PlatformType.cs \
	Config/Consts.cs \
	Config/FBIConfig.cs \
	Config/MySqlConfig.cs \
	Config/SQLiteConfig.cs \
	Config/LogConfig.cs \
	Config/LocalizationConfig.cs \
	Config/ServerConfig.cs \
	Config/YamlConfig.cs \
	Config/CrashConfig.cs \
	Config/ShutdownConfig.cs \
	Database/Cache/CacheDB.cs \
	Database/Cache/LocalizedConsoleCommand.cs \
	Database/Cache/LocalizedConsoleCommandHelp.cs \
	Database/Cache/LocalizedConsoleWarning.cs \
	Database/Cache/Channels.cs \
	Clean/CleanManager.cs \
	Clean/CleanConfig.cs \
	Clean/CleanDatabase.cs \
	Sender/Sender.cs \
	Sender/SendMessage.cs \
	Config/CleanConfig.cs \
	Network/ServerListener.cs \
	Network/ServerPacketHandler.cs \
	Network/ServerPacketHandlerDelegate.cs \
	Config/IRCConfig.cs \
	IrcServer.cs \
	ServerList.cs \
	Log/Log.cs \
	Log/LogType.cs \
	Irc/ChannelInfo.cs \
	Irc/ConnectionType.cs \
	Irc/IrcBase.cs \
	Irc/IrcMethod.cs \
	Irc/MessageHandler.cs \
	Irc/Network.cs \
	Irc/NickInfo.cs \
	Irc/PublicMessageHandler.cs \
	Irc/ReplyCode.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	../../Dependencies/sqlite3.dll \
	../../Sql/FBI.db3 \
	Database/Cache \
	Clean \
	fbi.framework.pc.in 

REFERENCES =  \
	System \
	System.Data \
	System.Xml \
	System.Web \
	System.Management \
	System.Core \
	System.Net \
	System.Runtime.Serialization

DLL_REFERENCES =  \
	../../Dependencies/MySql.Data.dll \
	../../Dependencies/System.Data.SQLite.dll \
	../../Dependencies/YamlDotNet.Core.dll \
	../../Dependencies/YamlDotNet.RepresentationModel.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
FBI_DB3 = $(BUILD_DIR)/FBI.db3
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
FBI_API_DLL = $(BUILD_DIR)/FBI.Api.dll
FBI_FRAMEWORK_PC = $(BUILD_DIR)/fbi.framework.pc

$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,FBI_DB3))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-wrapper,FBI_FRAMEWORK_PC,fbi.framework.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
