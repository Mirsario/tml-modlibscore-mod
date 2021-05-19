﻿using ModLibsCore.Classes.Loadable;
using System;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.ModLoader;


namespace ModLibsCore.Services.Debug.CustomHotkeys {
	/// @private
	public partial class CustomHotkeys : ILoadable {
		private readonly ModHotKey Key1;
		private readonly ModHotKey Key2;

		private readonly IDictionary<string, Action> Key1Actions = new Dictionary<string, Action>();
		private readonly IDictionary<string, Action> Key2Actions = new Dictionary<string, Action>();



		////////////////

		internal CustomHotkeys() {
			this.Key1 = ModLibsCoreMod.Instance.RegisterHotKey( "Custom Hotkey 1", "K" );
			this.Key2 = ModLibsCoreMod.Instance.RegisterHotKey( "Custom Hotkey 2", "L" );
		}

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }

		////////////////

		internal void ProcessTriggers( TriggersSet triggersSet ) {
			if( this.Key1.JustPressed ) {
				foreach( Action act in this.Key1Actions.Values ) {
					act();
				}
			}
			if( this.Key2.JustPressed ) {
				foreach( Action act in this.Key2Actions.Values ) {
					act();
				}
			}
		}
	}
}
