﻿using System;
using Terraria;
using Terraria.ModLoader;


namespace ModLibsCore.Libraries.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to log outputs.
	/// </summary>
	public partial class LogLibraries {
		private static bool CanOutputOnceMessage( string msg, bool repeatLog10, out string formattedMsg ) {
			Func<int, bool> outputWhen;
			if( repeatLog10 ) {
				outputWhen = (times) => (Math.Log10(times) % 1d) == 0;
			} else {
				outputWhen = (times) => times == 0;
			}

			return LogLibraries.CanOutputMessageWhen( msg, outputWhen, out formattedMsg );
		}


		////////////////

		/// <summary>
		/// Outputs a plain log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void LogOnce( string msg ) {
			if( LogLibraries.CanOutputOnceMessage(msg, true, out msg) ) {
				LogLibraries.Log( "~" + msg );
			}
		}

		/// <summary>
		/// Outputs an "alert" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void AlertOnce( string msg = "" ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, true, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context+" "+msg, false, out _ ) ) {
				return;
			}

			mymod.Logger.Warn( "~" + outMsg );	//was Error(...)
		}

		/// <summary>
		/// Outputs a "warning" log message "once" (or rather, once every log10 % 1 == 0 times).
		/// </summary>
		/// <param name="msg"></param>
		public static void WarnOnce( string msg = "" ) {
			ModLibsCoreMod mymod = ModLibsCoreMod.Instance;
			(string Context, string Info, string Full) fmtMsg = LogLibraries.FormatMessageFull( msg, 3 );

			string outMsg;
			LogLibraries.CanOutputOnceMessage( fmtMsg.Full, true, out outMsg );

			if( !LogLibraries.CanOutputOnceMessage( fmtMsg.Context + " " + msg, true, out _ ) ) {
				return;
			}

			mymod.Logger.Error( "~" + outMsg );	//was Fatal(...)
		}


		////////////////

		/// <summary>
		/// Resets a given "once" log, alert, or warn messages.
		/// </summary>
		/// <param name="msg"></param>
		public static void ResetOnceMessage( string msg ) {
			string fmtMsg = LogLibraries.FormatMessage( msg, 3 );
			var logLibs = ModContent.GetInstance<LogLibraries>();

			logLibs.UniqueMessages.Remove( "~" + msg );
			logLibs.UniqueMessages.Remove( "~" + fmtMsg );
		}
	}
}
