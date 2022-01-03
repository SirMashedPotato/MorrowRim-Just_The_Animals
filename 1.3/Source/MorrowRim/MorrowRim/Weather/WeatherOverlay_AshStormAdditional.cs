using System;
using UnityEngine;
using Verse;

namespace MorrowRim
{
	[StaticConstructorOnStartup]
	public class WeatherOverlay_AshStormAdditional : SkyOverlay
	{
		public WeatherOverlay_AshStormAdditional()
		{
			this.worldOverlayMat = AshStormWorld;
			base.OverlayColor = new Color(64f, 55f, 57f);
			this.worldOverlayPanSpeed1 = 0.045f;
			this.worldOverlayPanSpeed2 = 0.045f;
			this.worldPanDir1 = new Vector2(1f, 1f);
			this.worldPanDir2 = new Vector2(1f, 1f);
		}

		private static readonly Material AshStormWorld = MatLoader.LoadMat("Weather/SnowOverlayWorld", -1);
	}
}
