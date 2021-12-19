﻿using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.DotNET.Extensions;


namespace ModLibsCore.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		/// <summary>
		/// Gets a given instance of a given CustomPlayerData class by its type and associated player `whoAmI` value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="playerWho"></param>
		/// <returns></returns>
		public static T GetPlayerData<T>( int playerWho ) where T : CustomPlayerData {
			return (T)CustomPlayerData.GetPlayerData( typeof(T), playerWho );
		}

		internal static CustomPlayerData GetPlayerData( Type plrDataType, int playerWho ) {
			CustomPlayerData singleton = ModContent.GetInstance<CustomPlayerData>();

			return singleton.PlayerWhoToTypeToTypeInstanceMap.Get2DOrDefault( playerWho, plrDataType );
		}
	}
}
