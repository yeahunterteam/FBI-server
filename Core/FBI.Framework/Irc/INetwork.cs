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
using System.IO;
using System.Collections.Generic;

namespace FBI.Framework.Irc
{
	/// <summary>
	///     A Network class része. Ezzel elérhető bárhonnét eggyes része.
	/// </summary>
	public sealed class INetwork
	{
		/// <summary>
		///     Üzenet küldés az irc szerver felé.
		/// </summary>
		public static Dictionary<string, StreamWriter> WriterList = new Dictionary<string, StreamWriter>();
	}
}