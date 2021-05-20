﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NetSerializer;


namespace ModLibsCore.Libraries.DotNET.Serialization {
	sealed class HashSetSerializer : IStaticTypeSerializer {
		private static MethodInfo GetGenericWriter( Type serializerType, Type genericType ) {
			var mis = serializerType
				.GetMethods( BindingFlags.Static | BindingFlags.Public )
				.Where( mi => mi.IsGenericMethod && mi.Name == "WritePrimitive" );

			foreach( var mi in mis ) {
				ParameterInfo[] parameters = mi.GetParameters();
				if( parameters.Length != 3 ) {
					continue;
				}

				if( parameters[1].ParameterType != typeof( Stream ) ) {
					continue;
				}

				var paramType = parameters[2].ParameterType;
				if( paramType.IsGenericType == false ) {
					continue;
				}

				var genParamType = paramType.GetGenericTypeDefinition();
				if( genericType == genParamType ) {
					return mi;
				}
			}

			return null;
		}

		private static MethodInfo GetGenericReader( Type serializerType, Type genericType ) {
			var mis = serializerType
				.GetMethods( BindingFlags.Static | BindingFlags.Public )
				.Where( mi => mi.IsGenericMethod && mi.Name == "ReadPrimitive" );

			foreach( var mi in mis ) {
				ParameterInfo[] parameters = mi.GetParameters();
				if( parameters.Length != 3 ) {
					continue;
				}

				if( parameters[1].ParameterType != typeof( Stream ) ) {
					continue;
				}

				var paramType = parameters[2].ParameterType;
				if( paramType.IsByRef == false ) {
					continue;
				}

				paramType = paramType.GetElementType();
				if( paramType.IsGenericType == false ) {
					continue;
				}

				var genParamType = paramType.GetGenericTypeDefinition();
				if( genericType == genParamType ) {
					return mi;
				}
			}

			return null;
		}


		////

		public static void WritePrimitive<TValue>(
					Serializer serializer,
					Stream stream,
					HashSet<TValue> value ) {
			var vArray = new TValue[ value.Count ];

			int i = 0;
			foreach( TValue val in value ) {
				vArray[i++] = val;
			}

			serializer.Serialize( stream, vArray );
		}

		public static void ReadPrimitive<TValue>(
					Serializer serializer,
					Stream stream,
					out HashSet<TValue> value ) {
			var vArray = (TValue[])serializer.Deserialize( stream );
			value = new HashSet<TValue>();

			foreach( TValue val in vArray ) {
				value.Add( val );
			}
		}



		////////////////

		/// <summary>
		/// Indicates whether a given type is handled by this serializer.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Handles( Type type ) {
			if( !type.IsGenericType ) {
				return false;
			}

			return type.GetGenericTypeDefinition() == typeof( HashSet<> );
		}

		/// <summary>
		/// Indicates what types are generated by this serializer.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IEnumerable<Type> GetSubtypes( Type type ) {
			// HashSet<V> is stored as V[]

			Type[] genArgs = type.GetGenericArguments();

			return new Type[] { genArgs[0].MakeArrayType() };
		}

		/// <summary>
		/// Gets the writer method for the given serializeable type.
		/// </summary>
		/// <param name="serializableType"></param>
		/// <returns></returns>
		public MethodInfo GetStaticWriter( Type serializableType ) {
			if( !serializableType.IsGenericType ) {
				throw new Exception();
			}

			Type genericType = serializableType.GetGenericTypeDefinition();
			MethodInfo writer = HashSetSerializer.GetGenericWriter( this.GetType(), genericType );
			writer = writer.MakeGenericMethod( serializableType.GetGenericArguments() );

			return writer;
		}

		/// <summary>
		/// Gets the reader method for the given serializeable type.
		/// </summary>
		/// <param name="serializableType"></param>
		/// <returns></returns>
		public MethodInfo GetStaticReader( Type serializableType ) {
			if( !serializableType.IsGenericType ) {
				throw new Exception();
			}

			Type genericType = serializableType.GetGenericTypeDefinition();
			MethodInfo reader = HashSetSerializer.GetGenericReader( this.GetType(), genericType );
			reader = reader.MakeGenericMethod( serializableType.GetGenericArguments() );

			return reader;
		}
	}
}