/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * 
 * Schumix is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Schumix is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Schumix.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Management;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Schumix.API;
using Schumix.API.Functions;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Utilities
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0);
		private const long TicksSince1970 = 621355968000000000; // .NET ticks for 1970
		private readonly object WriteLock = new object();
		private const int TicksPerSecond = 10000;
		private Utilities() {}

		public DateTime GetUnixTimeStart()
		{
			return UnixTimeStart;
		}

		public string GetUrl(string url)
		{
			lock(WriteLock)
			{
				return GetUrl(url, string.Empty, string.Empty);
			}
		}

		public string GetUrl(string url, string args)
		{
			lock(WriteLock)
			{
				return GetUrl(url, args, string.Empty);
			}
		}

		public string GetUrl(string url, string args, string noencode)
		{
			lock(WriteLock)
			{
				if(args != string.Empty && noencode == string.Empty)
					url = url + HttpUtility.UrlEncode(args);
				else if(args != string.Empty && noencode != string.Empty)
					url = url + HttpUtility.UrlEncode(args) + noencode;

				var request = (HttpWebRequest)WebRequest.Create(url);
				request.AllowAutoRedirect = true;
				request.UserAgent = Consts.SchumixUserAgent;
				request.Referer = Consts.SchumixReferer;

				int length = 0;
				byte[] buf = new byte[1024];
				var sb = new StringBuilder();
				var response = (HttpWebResponse)request.GetResponse();
				var stream = response.GetResponseStream();

				while((length = stream.Read(buf, 0, buf.Length)) != 0)
				{
					buf = Encoding.Convert(Encoding.GetEncoding(response.CharacterSet), Encoding.UTF8, buf);
					sb.Append(Encoding.UTF8.GetString(buf, 0, length));
				}

				response.Close();
				return sb.ToString();
			}
		}

		public void DownloadFile(string url, string filename)
		{
			using(var client = new WebClient())
			{
				client.Headers.Add("referer", Consts.SchumixReferer);
				client.Headers.Add("user-agent", Consts.SchumixUserAgent);
				client.DownloadFile(url, filename);
			}
		}

		public string GetRandomString()
		{
			string path = Path.GetRandomFileName();
			return path.Replace(SchumixBase.Point.ToString(), string.Empty);
		}

		public string Sha1(string value)
		{
			if(value.IsNull())
				throw new ArgumentNullException("value");

			var x = new SHA1CryptoServiceProvider();
			var data = Encoding.UTF8.GetBytes(value);
			data = x.ComputeHash(data);
//#if !MONO
			//x.Dispose();
//#endif
			var ret = string.Empty;

			for(var i = 0; i < data.Length; i++)
				ret += data[i].ToString("x2").ToLower();

			return ret;
		}

		public string Md5(string value)
		{
			if(value.IsNull())
				throw new ArgumentNullException("value");

			var x = new MD5CryptoServiceProvider();
			var data = Encoding.UTF8.GetBytes(value);
			data = x.ComputeHash(data);
//#if !MONO
			//x.Dispose();
//#endif
			var ret = string.Empty;

			for(var i = 0; i < data.Length; i++)
				ret += data[i].ToString("x2").ToLower();

			return ret;
		}

		public string MD5File(string fileName)
		{
			if(fileName.IsNull())
				throw new ArgumentNullException("fileName");

			byte[] retVal;

			using(var file = new FileStream(fileName, FileMode.Open))
			{
				var md5 = new MD5CryptoServiceProvider();
				retVal = md5.ComputeHash(file);
//#if !MONO
				//md5.Dispose();
//#endif
			}

			var sb = new StringBuilder();

			if(!retVal.IsNull())
			{
				for(var i = 0; i < retVal.Length; i++)
					sb.Append(retVal[i].ToString("x2"));
			}

			return sb.ToString();
		}

		public bool IsPrime(long x)
		{
			x = Math.Abs(x);

			if(x == 1 || x == 0)
				return false;

			if(x == 2)
				return true;

			if(x % 2 == 0)
				return false;

			bool p = true;

			for(var i = 3; i <= Math.Floor(Math.Sqrt(x)); i += 2)
			{
				if(x % i == 0)
				{
					p = false;
					break;
				}
			}

			return p;
		}

		public string GetPlatform()
		{
			string platform = string.Empty;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					platform = "Windows";
					break;
				case PlatformID.Unix:
					platform = "Linux";
					break;
				case PlatformID.MacOSX:
					platform = "MacOSX";
					break;
				case PlatformID.Xbox:
					platform = "Xbox";
					break;
				default:
					platform = "Unknown";
					break;
			}

			return platform;
		}

		/// <summary>
		/// Returns the name of the operating system running on this computer.
		/// </summary>
		/// <returns>A string containing the the operating system name.</returns>
		public string GetOSName()
		{
			var Info = Environment.OSVersion;
			string Name = string.Empty;
			
			switch(Info.Platform)
			{
				case PlatformID.Win32Windows:
				{
					switch(Info.Version.Minor)
					{
						case 0:
						{
							Name = "Windows 95";
							break;
						}
						case 10:
						{
							if(Info.Version.Revision.ToString() == "2222A")
								Name = "Windows 98 Second Edition";
							else
								Name = "Windows 98";

							break;
						}
						case 90:
						{
							Name = "Windows Me";
							break;
						}
					}

					break;
				}
				case PlatformID.Win32NT:
				{
					switch(Info.Version.Major)
					{
						case 3:
						{
							Name = "Windows NT 3.51";
							break;
						}
						case 4:
						{
							Name = "Windows NT 4.0";
							break;
						}
						case 5:
						{
							if(Info.Version.Minor == 0)
								Name = "Windows 2000";
							else if(Info.Version.Minor == 1)
								Name = "Windows XP";
							else if(Info.Version.Minor == 2)
								Name = "Windows Server 2003";
							break;
						}
						case 6:
						{
							if(Info.Version.Minor == 0)
								Name = "Windows Vista";
							else if(Info.Version.Minor == 1)
								Name = "Windows 7";
							break;
						}
					}

					break;
				}
				case PlatformID.WinCE:
				{
					Name = "Windows CE";
					break;
				}
				case PlatformID.Unix:
				{
					Name = "Linux " + Info.Version;
					break;
				}
				case PlatformID.MacOSX:
				{
					Name = "MacOSX";
					break;
				}
				case PlatformID.Xbox:
				{
					Name = "Xbox";
					break;
				}
				default:
				{
					Name = "Unknown";
					break;
				}
			}

			return Name;
		}

		public PlatformType GetPlatformType()
		{
			PlatformType platform = PlatformType.None;
			var pid = Environment.OSVersion.Platform;

			switch(pid)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					platform = PlatformType.Windows;
					break;
				case PlatformID.Unix:
					platform = PlatformType.Linux;
					break;
				case PlatformID.MacOSX:
					platform = PlatformType.MacOSX;
					break;
				case PlatformID.Xbox:
					platform = PlatformType.Xbox;
					break;
				default:
					platform = PlatformType.None;
					break;
			}

			return platform;
		}

		public string GetVersion()
		{
			return Schumix.Framework.Config.Consts.SchumixVersion;
		}

		public string GetFunctionUpdate()
		{
			string functions = string.Empty;

			foreach(var function in Enum.GetNames(typeof(IChannelFunctions)))
			{
				if(function == IChannelFunctions.Log.ToString() || function == IChannelFunctions.Rejoin.ToString() ||
					function == IChannelFunctions.Commands.ToString())
					functions += SchumixBase.Comma + function.ToString().ToLower() + SchumixBase.Colon + SchumixBase.On;
				else
					functions += SchumixBase.Comma + function.ToString().ToLower() + SchumixBase.Colon + SchumixBase.Off;
			}

			return functions;
		}

		public string SqlEscape(string text)
		{
			if(text.IsNull() || text.IsEmpty())
				return string.Empty;
			
			if(SQLiteConfig.Enabled)
			{
				var split = text.Split('\'');
				if(split.Length > 1)
				{
					int x = 0;
					var sb = new StringBuilder();
					
					foreach(var s in split)
					{
						x++;
						
						if(s.Length == 0 && x != split.Length)
							sb.Append(@"''");
						else if(s.Length == 1)
						{
							if(s.Substring(0, 1) != @"'")
							{
								sb.Append(s);
								sb.Append(@"''");
							}
							else
								sb.Append(@"' ''");
						}
						else
						{
							int i = 0;
							string ss = s;
							
							for(;;)
							{
								if(ss.Length > 0 && ss.Substring(ss.Length-1) != @"'")
								{
									if(ss.Length-1 > 0)
										sb.Append(ss.Substring(0, ss.Length));
									
									for(int a = 0; a < i; a++)
										sb.Append(@"'");
									
									if(x != split.Length && i % 2 == 0)
										sb.Append(@"''");
									else if(x != split.Length)
										sb.Append(@" ''");
									
									break;
								}
								else if(ss.Length <= 0)
								{
									for(int a = 0; a < i; a++)
										sb.Append(@"'");
									
									if(x != split.Length && i % 2 == 0)
										sb.Append(@"''");
									else if(x != split.Length)
										sb.Append(@" ''");
									
									break;
								}
								
								i++;
								ss = ss.Remove(ss.Length-1);
							}
						}
					}
					
					text = sb.ToString();
				}
			}
			else
			{
				var split = text.Split('\'');
				if(split.Length > 1)
				{
					int x = 0;
					var sb = new StringBuilder();
					
					foreach(var s in split)
					{
						x++;
						
						if(s.Length == 0 && x != split.Length)
							sb.Append(@"\'");
						else if(s.Length == 1)
						{
							if(s.Substring(0, 1) != @"\")
							{
								sb.Append(s);
								sb.Append(@"\'");
							}
							else
								sb.Append(@"\ \'");
						}
						else
						{
							int i = 0;
							string ss = s;
							
							for(;;)
							{
								if(ss.Length > 0 && ss.Substring(ss.Length-1) != @"\")
								{
									if(ss.Length-1 > 0)
										sb.Append(ss.Substring(0, ss.Length));
									
									for(int a = 0; a < i; a++)
										sb.Append(@"\");
									
									if(x != split.Length && i % 2 == 0)
										sb.Append(@"\'");
									else if(x != split.Length)
										sb.Append(@" \'");
									
									break;
								}
								else if(ss.Length <= 0)
								{
									for(int a = 0; a < i; a++)
										sb.Append(@"\");
									
									if(x != split.Length && i % 2 == 0)
										sb.Append(@"\'");
									else if(x != split.Length)
										sb.Append(@" \'");
									
									break;
								}
								
								i++;
								ss = ss.Remove(ss.Length-1);
							}
						}
					}
					
					text = sb.ToString();
				}
			}
			
			return text;
		}

		/// <summary>
		///   Gets the cpu brand string.
		/// </summary>
		/// <returns>
		///   The CPU brand string.
		/// </returns>
		public string GetCpuId()
		{
			if(GetPlatformType() == PlatformType.Windows)
			{
				var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
				return (from ManagementObject mo in mos.Get() select (Regex.Replace(Convert.ToString(mo["Name"]), @"\s+", SchumixBase.Space.ToString()))).FirstOrDefault();
			}
			else if(GetPlatformType() == PlatformType.Linux)
			{
				var reader = new StreamReader("/proc/cpuinfo");
				string content = reader.ReadToEnd();
				reader.Close();
				reader.Dispose();
				var getBrandRegex = new Regex(@"model\sname\s:\s*(?<first>.+\sCPU)\s*(?<second>.+)", RegexOptions.IgnoreCase);

				if(!getBrandRegex.IsMatch(content))
				{
					// not intel
					var amdRegex = new Regex(@"model\sname\s:\s*(?<cpu>.+)");

					if(!amdRegex.IsMatch(content))
						return sLConsole.Other("Notfound");

					var amatch = amdRegex.Match(content);
					string amd = amatch.Groups["cpu"].ToString();
					return amd;
				}

				var match = getBrandRegex.Match(content);
				string cpu = (match.Groups["first"].ToString() + SchumixBase.Space + match.Groups["second"].ToString());
				return cpu;
			}

			return sLConsole.Other("Notfound");
		}

		/// <summary>
		///   The current unix time.
		/// </summary>
		public double UnixTime
		{
			get
			{
				var elapsed = (DateTime.UtcNow - UnixTimeStart);
				return elapsed.TotalSeconds;
			}
		}

		/// <summary>
		/// Converts ticks to miliseconds.
		/// </summary>
		/// <param name="ticks"></param>
		/// <returns></returns>
		public int ToMilliSecondsInt(int ticks)
		{
			return ticks/TicksPerSecond;
		}

		/// <summary>
		///   Gets the system uptime.
		/// </summary>
		/// <returns>the system uptime in milliseconds</returns>
		public long GetSystemTime()
		{
			return (long)Environment.TickCount;
		}

		/// <summary>
		///   Gets the time since the Unix epoch.
		/// </summary>
		/// <returns>the time since the unix epoch in seconds</returns>
		public long GetEpochTime()
		{
			return (long)((DateTime.UtcNow.Ticks - TicksSince1970)/TimeSpan.TicksPerSecond);
		}

		/// <summary>
		/// Gets the date time from unix time.
		/// </summary>
		/// <param name="unixTime">The unix time.</param>
		/// <returns></returns>
		public DateTime GetDateTimeFromUnixTime(long unixTime)
		{
			return UnixTimeStart.AddSeconds(unixTime);
		}

		/// <summary>
		/// Gets the UTC time from seconds.
		/// </summary>
		/// <param name="seconds">The seconds.</param>
		/// <returns></returns>
		public DateTime GetUTCTimeSeconds(long seconds)
		{
			return UnixTimeStart.AddSeconds(seconds);
		}

		/// <summary>
		/// Gets the UTC time from millis.
		/// </summary>
		/// <param name="millis">The millis.</param>
		/// <returns></returns>
		public DateTime GetUTCTimeMillis(long millis)
		{
			return UnixTimeStart.AddMilliseconds(millis);
		}

		/// <summary>
		///   Gets the system uptime.
		/// </summary>
		/// <remarks>
		///   Even though this returns a long, the original value is a 32-bit integer,
		///   so it will wrap back to 0 after approximately 49 and half days of system uptime.
		/// </remarks>
		/// <returns>the system uptime in milliseconds</returns>
		public long GetSystemTimeLong()
		{
			return (long)Environment.TickCount;
		}

		/// <summary>
		///   Gets the time between the Unix epich and a specific <see cref = "DateTime">time</see>.
		/// </summary>
		/// <returns>the time between the unix epoch and the supplied <see cref = "DateTime">time</see> in seconds</returns>
		public long GetEpochTimeFromDT()
		{
			return GetEpochTimeFromDT(DateTime.Now);
		}

		/// <summary>
		///   Gets the time between the Unix epich and a specific <see cref = "DateTime">time</see>.
		/// </summary>
		/// <param name = "time">the end time</param>
		/// <returns>the time between the unix epoch and the supplied <see cref = "DateTime">time</see> in seconds</returns>
		public long GetEpochTimeFromDT(DateTime time)
		{
			return (long)((time.Ticks - TicksSince1970)/10000000L);
		}

		public void CreateDirectory(string Name)
		{
			if(!Directory.Exists(Name))
				Directory.CreateDirectory(Name);
		}

		public void CreateFile(string Name)
		{
			if(!File.Exists(Name))
				new FileStream(Name, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
		}

		public string GetUserName()
		{
			return Environment.UserName;
		}

		public string GetSpecialDirectory(string data)
		{
			if(GetPlatformType() == PlatformType.Windows)
			{
				string text = data.ToLower();

				if(text.Length >= "$home".Length && text.Substring(0, "$home".Length) == "$home")
				{
					if(data.Contains("/") && data.Substring(data.IndexOf("/")).Length > 1 && data.Substring(data.IndexOf("/")).Substring(0, 1) == "/")
						return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + data.Substring(data.IndexOf("/")+1);
					else if(data.Contains(@"\") && data.Substring(data.IndexOf(@"\")).Length > 1 && data.Substring(data.IndexOf(@"\")).Substring(0, 1) == @"\")
						return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + data.Substring(data.IndexOf(@"\")+1);
					else
						return data;
				}
				if(text.Length >= "$localappdata".Length && text.Substring(0, "$localappdata".Length) == "$localappdata")
				{
					if(data.Contains("/") && data.Substring(data.IndexOf("/")).Length > 1 && data.Substring(data.IndexOf("/")).Substring(0, 1) == "/")
						return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + data.Substring(data.IndexOf("/")+1);
					else if(data.Contains(@"\") && data.Substring(data.IndexOf(@"\")).Length > 1 && data.Substring(data.IndexOf(@"\")).Substring(0, 1) == @"\")
						return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + data.Substring(data.IndexOf(@"\")+1);
					else
						return data;
				}
				else
					return data;
			}
			else if(GetPlatformType() == PlatformType.Linux)
			{
				string text = data.ToLower();
				return (text.Length >= "$home".Length && text.Substring(0, "$home".Length) == "$home") ?
					"/home/" + GetUserName() + "/" + data.Substring(data.IndexOf("/")+1) : data;
			}
			else
				return data;
		}

		public bool IsSpecialDirectory(string dir)
		{
			dir = dir.ToLower();

			if(dir.Contains("$home"))
				return true;
			else if(GetPlatformType() == PlatformType.Windows && dir.Contains("$localappdata"))
				return true;
			else
				return false;
		}

		public string DirectoryToSpecial(string dir, string file)
		{
			if(GetPlatformType() == PlatformType.Windows)
			{
				if(dir.Length > 2 && dir.Substring(1, 2) == @":\")
					return string.Format(@"{0}\{1}", dir, file);
				else if(dir.Length > 2 && dir.Substring(0, 2) == "//")
					return string.Format("//{0}/{1}", dir, file);
				else
					return string.Format("./{0}/{1}", dir, file);
			}
			else if(GetPlatformType() == PlatformType.Linux)
				return (dir.Length > 0 && dir.Substring(0, 1) == "/") ? string.Format("{0}/{1}", dir, file) : string.Format("./{0}/{1}", dir, file);
			else
				return string.Format("{0}/{1}", dir, file);
		}

		public string GetDirectoryName(string data)
		{
			if(GetPlatformType() == PlatformType.Windows)
			{
				var split = data.Split('\\');
				return split.Length > 1 ? split[split.Length-1] : data;
			}
			else if(GetPlatformType() == PlatformType.Linux)
			{
				var split = data.Split('/');
				return split.Length > 1 ? split[split.Length-1] : data;
			}
			else
				return data;
		}

		public void CreatePidFile(string Name)
		{
			string pidfile = Name;

			if(!pidfile.Contains(".pid"))
			{
				if(pidfile.Contains(".xml"))
					pidfile = pidfile.Remove(pidfile.IndexOf(".xml")) + ".pid";
				else if(pidfile.Contains(".yml"))
					pidfile = pidfile.Remove(pidfile.IndexOf(".yml")) + ".pid";
				else
					pidfile = pidfile + ".pid";
			}

			pidfile = DirectoryToSpecial(LogConfig.LogDirectory, pidfile);
			SchumixBase.PidFile = pidfile;
			RemovePidFile();
			CreateFile(pidfile);
			var file = new StreamWriter(pidfile, true) { AutoFlush = true };
			file.WriteLine("{0}", Process.GetCurrentProcess().Id);
			file.Close();
		}

		public void RemovePidFile()
		{
			if(File.Exists(SchumixBase.PidFile))
				File.Delete(SchumixBase.PidFile);
		}
	}
}