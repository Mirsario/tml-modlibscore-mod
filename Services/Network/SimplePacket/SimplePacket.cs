﻿using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NetSerializer;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.DotNET.Reflection;
using ModLibsCore.Libraries.DotNET.Serialization;


namespace ModLibsCore.Services.Network.SimplePacket {
	/// <summary>
	/// Provides functions to neatly send data (via. ModPacket) to server, clients, or both. Abstracts away serialization.
	/// </summary>
	public partial class SimplePacket : ModSystem {
		private IDictionary<int, Type> PayloadCodeToType = new Dictionary<int, Type>();
		private IDictionary<Type, int> PayloadTypeToCode = new Dictionary<Type, int>();
		private IDictionary<int, Serializer> PayloadCodeToSerializer = new Dictionary<int, Serializer>();



		////////////////

		public override void Load() {
			IList<Type> payloadTypes = ReflectionLibraries
				.GetAllAvailableSubTypesFromMods( typeof(SimplePacketPayload) )
				.OrderBy( t => t.Namespace + "." + t.Name )
				.ToList();
			var settings = new Settings {
				CustomTypeSerializers = new ITypeSerializer[] { new HashSetSerializer() }
			};

			int i = 0;
			foreach( Type payloadType in payloadTypes.ToArray() ) {
				if( !this.ValidateSerializeable(payloadType, payloadType, new HashSet<Type>(), out string result) ) {
					LogLibraries.Warn( payloadType.Name+" not serializeable: "+result );
					continue;
				}

				this.PayloadCodeToType[i] = payloadType;
				this.PayloadTypeToCode[payloadType] = i;
				this.PayloadCodeToSerializer[i] = new Serializer( new Type[] { payloadType }, settings );
				i++;
			}
		}

		////

		private bool ValidateSerializeable(
					Type basePayloadType,
					Type payloadType,
					ISet<Type> alreadyValidated,
					out string result ) {
			//if( !payloadType.IsSerializable ) {
			//	result = "Invalid payload type "+payloadType.Name+" "+"(in "+payloadType.Assembly.GetName().Name+")";
			//	return false;
			//}

			alreadyValidated.Add( payloadType );

			foreach( FieldInfo field in payloadType.GetFields() ) {
				//if( !field.FieldType.IsSerializable || field.IsNotSerialized ) {
				//	result = "Invalid payload type "+payloadType.Name+"; field "+field.Name+" not serializeable "
				//		+"(in "+payloadType.Assembly.GetName().Name+")";
				//	return false;
				//}

				if( alreadyValidated.Contains(field.FieldType) ) {
					continue;
				}

				//if( !field.FieldType.IsValueType ) {
				//	if( !this.ValidateSerializeable(basePayloadType, field.FieldType, alreadyValidated, out result) ) {
				//		return false;
				//	}
				//}
			}

			result = null;
			return true;
		}
	}
}
