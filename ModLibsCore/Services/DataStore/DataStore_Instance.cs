﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Classes.Loadable;
using ModLibsCore.Libraries.Debug;


namespace ModLibsCore.Services.DataStore {
	/// @private
	public partial class DataStore : ILoadable {
		private IDictionary<object, (bool, object)> Data = new Dictionary<object, (bool, object)>();



		////////////////

		internal DataStore() { }

		void ILoadable.OnModsLoad() { }

		void ILoadable.OnPostModsLoad() { }

		void ILoadable.OnModsUnload() { }


		////////////////

		/// <summary></summary>
		public string Serialize() {
			return JsonConvert.SerializeObject( DataStore.GetAll(), Formatting.Indented );
		}
	}
}
