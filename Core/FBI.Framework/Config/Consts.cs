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

namespace FBI.Framework.Config
{
	public static class Consts
	{
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public const string FBIDescription = "FBI IRC bot";
#if DEBUG
		public const string FBIConfiguration = "Debug";
#else
		public const string FBIConfiguration = "Release";
#endif
		public const string FBICompany = "FBI Productions";
		public const string FBIProduct = "FBI";
		public const string FBICopyright = "Copyright (C) 2010-2012 Megax <http://megax.yeahunter.hu/>";
		public const string FBITrademark = "GNU General Public License";
		public const string FBIVersion = "4.0.3";
		public const string FBIFileVersion = "4.0.3.0";
		public const string FBIProgrammedBy = "Csaba Jakosa (Megax)";
		public const string FBIDevelopers = "Csaba Jakosa (Megax), Twl, Jackneill, Invisible";
		public const string FBIWebsite = "https://github.com/FBI/FBI2";
		public static string FBIUserAgent = FBIBase.Title + FBIBase.Space + sUtilities.GetVersion() + " / .NET " + Environment.Version;
		public const string FBIReferer = "http://yeahunter.hu";
	}
}