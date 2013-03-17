
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/FBI.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../Run/Debug/FBI.Framework.dll \
	../../Run/Debug/FBI.Api.dll
BUILD_DIR = ../../Run/Debug

FBI_EXE_MDB_SOURCE=../../Run/Debug/FBI.exe.mdb
FBI_EXE_MDB=$(BUILD_DIR)/FBI.exe.mdb
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
FBI_FRAMEWORK_DLL_SOURCE=../../Run/Debug/FBI.Framework.dll
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
ASSEMBLY = ../../Run/Release/FBI.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../Run/Release/FBI.Framework.dll \
	../../Run/Release/FBI.Api.dll
BUILD_DIR = ../../Run/Release

FBI_EXE_MDB=
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
FBI_FRAMEWORK_DLL_SOURCE=../../Run/Release/FBI.Framework.dll
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
ASSEMBLY = ../../Run/Debug_x64/FBI.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../Run/Debug_x64/FBI.Framework.dll \
	../../Run/Debug_x64/FBI.Api.dll
BUILD_DIR = ../../Run/Debug_x64

FBI_EXE_MDB_SOURCE=../../Run/Debug_x64/FBI.exe.mdb
FBI_EXE_MDB=$(BUILD_DIR)/FBI.exe.mdb
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
FBI_FRAMEWORK_DLL_SOURCE=../../Run/Debug_x64/FBI.Framework.dll
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
ASSEMBLY = ../../Run/Release_x64/FBI.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../Run/Release_x64/FBI.Framework.dll \
	../../Run/Release_x64/FBI.Api.dll
BUILD_DIR = ../../Run/Release_x64

FBI_EXE_MDB=
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
FBI_FRAMEWORK_DLL_SOURCE=../../Run/Release_x64/FBI.Framework.dll
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
	$(FBI_EXE_MDB) \
	$(MONO_POSIX_DLL) \
	$(FBI_FRAMEWORK_DLL) \
	$(FBI_FRAMEWORK_DLL_MDB) \
	$(SQLITE3_DLL) \
	$(FBI_DB3) \
	$(MYSQL_DATA_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(FBI_API_DLL) \
	$(FBI_API_DLL_MDB)  

BINARIES = \
	$(FBI)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Main.cs \
	FBI.cs \
	Console/Console.cs \
	Properties/AssemblyInfo.cs \
	Console/Commands/CommandHandler.cs \
	Console/Commands/CommandManager.cs \
	Console/Commands/CommandHelp.cs \
	Windows/Windows.cs \
	Linux/Linux.cs \
	Windows/CtrlType.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Console/Commands \
	Console \
	Windows \
	Linux \
	fbi.in 

REFERENCES =  \
	System \
	System.Data \
	System.Xml \
	System.Core

DLL_REFERENCES =  \
	../../Dependencies/Mono.Posix.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
FBI_FRAMEWORK_DLL = $(BUILD_DIR)/FBI.Framework.dll
SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
FBI_DB3 = $(BUILD_DIR)/FBI.db3
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
FBI_API_DLL = $(BUILD_DIR)/FBI.Api.dll
FBI = $(BUILD_DIR)/fbi

$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,FBI_DB3))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-wrapper,FBI,fbi,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
