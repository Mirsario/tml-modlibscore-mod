﻿using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Services.Hooks.LoadHooks;


namespace ModLibsCore.Helpers.Debug {
	/// @private
	public partial class LogHelpers : ILoadable {
		private static object MyLock { get; } = new object();



		////////////////
		
		private DateTime StartTimeBase;
		private double StartTime;

		private IDictionary<string, int> UniqueMessages = new Dictionary<string, int>();



		////////////////

		internal LogHelpers() { }

		void ILoadable.OnModsLoad() {
			this.Reset();
		}

		void ILoadable.OnPostModsLoad() {
			LoadHooks.AddWorldUnloadEachHook( this.OnWorldExit );
		}

		void ILoadable.OnModsUnload() { }


		////////////////

		private void OnWorldExit() {
			this.Reset();
		}

		internal void Reset() {
			this.StartTimeBase = DateTime.UtcNow;
			this.StartTime = DateTime.UtcNow.Subtract( new DateTime( 1970, 1, 1, 0, 0, 0 ) ).TotalSeconds;
		}

		////////////////

		internal string GetLogPath() {
			string basePath = Logging.LogDir + Path.DirectorySeparatorChar;
			string fullPath = basePath + "History" + Path.DirectorySeparatorChar;

			Directory.CreateDirectory( basePath );
			Directory.CreateDirectory( fullPath );

			return fullPath;
		}

		internal string GetHourlyLogFileName() {
			DateTime currHour = this.StartTimeBase;
			currHour.AddMinutes( -currHour.Minute );
			currHour.AddSeconds( -currHour.Second );
			currHour.AddMilliseconds( -currHour.Millisecond );

			string when = currHour.ToString( "MM-dd, HH" );

			return "Log Any " + when + ".txt";
		}

		internal string GetModalLogFileName() {
			string mode = "";

			if( Main.dedServ ) {
				mode += "Dedi ";
			}
			
			switch( Main.netMode ) {
			case 0:
				mode += "Single";
				break;
			case 1:
				mode += "Client";
				break;
			case 2:
				mode += "Server";
				break;
			default:
				mode += "Unknown Mode";
				break;
			}

			//string when = this.StartTimeBase.ToString( "MM-dd, HH.mm.ss" );
			string when = this.StartTimeBase.ToString( "MM-dd, HH.mm" );

			return "Log " + mode + " " + when + ".txt";
		}


		////////////////

		internal void OutputDirect( string fileName, string logEntry ) {
			lock( LogHelpers.MyLock ) {
				string path = this.GetLogPath();

				try {
					using( StreamWriter writer = File.AppendText( path + fileName ) ) {
						writer.WriteLine( logEntry );
					}
				} catch( Exception e ) {
					ModLibsCoreMod.Instance.Logger.Info( "FALLBACK LOGGER (" + e.GetType().Name + "; " + fileName + ") - " + logEntry );
				}
			}
		}
	}
}
