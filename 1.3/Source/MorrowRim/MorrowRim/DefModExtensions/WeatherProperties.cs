using Verse;
using RimWorld;

namespace MorrowRim
{
    class WeatherProperties : DefModExtension
    {
        //generic
        public bool reduceLightLevels = false;
        public bool increaseHeatLevels = false; //doesn't do anything at this point
        public float reduceLightLevelsFactor = 0.5f;
        public float increaseHeatLevelsFactor = 1.5f;

        //ash
        public bool isAshStorm = false;

        //blight

        //fire
        public bool isFireStorm = false;
        public bool setFireToPawns = false;
        public bool setFireToPlants = false;
        public float burnDamage = 1f;
        public float fireSize = 1f;

        public static WeatherProperties Get(Def def)
        {
            return def.GetModExtension<WeatherProperties>();
        }
    }
}
