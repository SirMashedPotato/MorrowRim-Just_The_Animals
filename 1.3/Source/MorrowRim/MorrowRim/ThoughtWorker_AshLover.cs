using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
    public class ThoughtWorker_AshLover : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            return p.Awake() && !p.Position.Roofed(p.Map) && IsAshStorm(p);
        }

        public bool IsAshStorm(Pawn p)
        {
            WeatherDef weatherDef = p.Map.weatherManager.curWeather;
            var props = WeatherProperties.Get(weatherDef);
            return props != null && props.isAshStorm;
        }
    }
}
