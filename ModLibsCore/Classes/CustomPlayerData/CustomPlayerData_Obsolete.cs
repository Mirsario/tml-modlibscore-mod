﻿using System;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsCore.Classes.PlayerData {
	/// <summary>
	/// An alternative to ModPlayer for basic per-player, per-game data storage and Update use.
	/// </summary>
	public partial class CustomPlayerData : ILoadable {
		/// @private
		[Obsolete("use `OnEnter<T>(bool, object)`", true)]
		protected virtual void OnEnter( object data ) { }
	}
}
