/*
 * This file is part of FBI.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * 
 * FBI is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * FBI is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with FBI.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FBI.Framework.Config;

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.

[assembly: AssemblyTitle("FBI")]
[assembly: AssemblyDescription(Consts.FBIDescription)]
[assembly: AssemblyConfiguration(Consts.FBIConfiguration)]
[assembly: AssemblyCompany(Consts.FBICompany)]
[assembly: AssemblyProduct(Consts.FBIProduct)]
[assembly: AssemblyCopyright(Consts.FBICopyright)]
[assembly: AssemblyTrademark(Consts.FBITrademark)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(Consts.FBIVersion)]
[assembly: AssemblyFileVersion(Consts.FBIFileVersion)]