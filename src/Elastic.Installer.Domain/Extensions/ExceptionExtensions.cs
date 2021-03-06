﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Installer.Domain.Extensions
{
	public static class ExceptionExtensions
	{
		public static void ToConsole(this Exception e, string prefix) =>
			ElasticsearchConsole.WriteLine(ConsoleColor.Red, $"{prefix}: {e}");

		public static void ToEventLog(this Exception e, string source)
		{
			if (!EventLog.SourceExists(source))
				EventLog.CreateEventSource(source, "Application");
			EventLog.WriteEntry(source, e.ToString(), EventLogEntryType.Error);
		}
	}
}
