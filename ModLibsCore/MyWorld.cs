﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModLibsCore.Classes.Errors;
using ModLibsCore.Libraries.Debug;
using ModLibsCore.Libraries.XNA;
using ModLibsCore.Services.Hooks.Draw;
using ModLibsCore.Internals.Logic;


namespace ModLibsCore {
	/// @private
	class ModLibsWorld : ModWorld {
		public override void PreUpdate() {
			var logic = ModContent.GetInstance<WorldLogic>();

			if( logic != null ) {
				if( Main.netMode == NetmodeID.SinglePlayer ) { // Single
					logic.PreUpdateSingle();
				} else if( Main.netMode == NetmodeID.Server ) {
					logic.PreUpdateServer();
				}
			}
		}


		////////////////

		public override void PostDrawTiles() {
			//DataStore.Add( DebugLibraries.GetCurrentContext()+"_A", 1 );
			Player player = Main.LocalPlayer;
			if( player == null ) { return; }
			RasterizerState rasterizer = Main.gameMenu ||
					(double)player.gravDir == 1.0 ?
					RasterizerState.CullCounterClockwise : RasterizerState.CullClockwise;

			bool _;
			XNASpritebatchLibraries.DrawBatch(
				( sb ) => {
					DebugLibraries.DrawAllRects( sb );

					ModContent.GetInstance<DrawHooksInternal>()?.RunPostDrawTilesActions();
				},
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				Main.DefaultSamplerState,
				DepthStencilState.None,
				rasterizer,
				(Effect)null,
				Main.GameViewMatrix.TransformationMatrix,
				out _
			);
			//DataStore.Add( DebugLibraries.GetCurrentContext()+"_B", 1 );
		}
	}
}
