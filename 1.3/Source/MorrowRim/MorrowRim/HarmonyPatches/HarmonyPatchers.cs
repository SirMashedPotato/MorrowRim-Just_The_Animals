using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System.Reflection;
using Verse;
using System;
using System.Linq;
using System.Collections.Generic;
using Verse.AI;
using Verse.AI.Group;
using System.Text;

namespace MorrowRim
{

    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.MorrowRim");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    //Postfix to alter light levels for certain biomes or weather
    //now gets info from  DefModExtension: WeatherProperties
    [HarmonyPatch(typeof(GenCelestial))]
    [HarmonyPatch("CelestialSunGlow")]
    [HarmonyPatch(new Type[] { typeof(Map), typeof(int) })]
    public static class GenCelestial_CelestialSunGlow_Patch
    {
        [HarmonyPostfix]
        public static void AshStormLighting(ref Map map, ref float __result)
        {
            if (ModSettings_Utility.MorrowRim_SettingEnableLightingEffects())
            {
                var pm = WeatherProperties.Get(map.Biome);
                if (pm != null && pm.reduceLightLevels && map.weatherManager.curWeather != WeatherDefOf.Clear)
                {
                    __result *= pm.reduceLightLevelsFactor;
                }
                else
                {
                    var pw = WeatherProperties.Get(map.weatherManager.curWeather);
                    if (pw != null && pw.reduceLightLevels)
                    {
                        __result *= pw.reduceLightLevelsFactor;
                    }
                }
            }
        }
    }

    //Postfix to force certain plants to grow on certain terrains
    //Original function checks what plants can grow at c
    //Used to be part of AshSwamp, but moved here for future use
    //now gets info from DefModExtension: PlantProperties
    [HarmonyPatch(typeof(WildPlantSpawner))]
    [HarmonyPatch("CalculatePlantsWhichCanGrowAt")]
    public static class WildPlantSpawner_CalculatePlantsWhichCanGrowAt_Patch
    {
        [HarmonyPostfix]
        public static void GrowOnTerrain(IntVec3 c, List<ThingDef> outPlants, Map ___map)
        {
            List<ThingDef> PlantsToRemove = new List<ThingDef>();
            foreach(ThingDef plant in outPlants)
            {
                var pp = PlantProperties.Get(plant);
                if (pp != null && pp.forceOntoTerrain)
                {
                    TerrainDef terrainDef = c.GetTerrain(___map);
                    if (!pp.whitelist && pp.terrainList.Contains(terrainDef) || (pp.whitelist && !pp.terrainList.Contains(terrainDef)))
                    {
                        //outPlants.Remove(plant);
                        PlantsToRemove.Add(plant);
                    }
                }
            }
            //remove them this way to avoid nullRef
            foreach(ThingDef plant in PlantsToRemove)
            {
                outPlants.Remove(plant);
            }
        }

        private static readonly List<string> plantsList = new List<string>{ "MorrowRim_EmporerParasol", "MorrowRim_Trama", "MorrowRim_MuckSponge", "MorrowRim_Marshmerrow" };
        //Used for a setting in the mod settings menu
        //Prevents plants from the above list spawning in non-MorrowRim biomes
        [HarmonyPostfix]
        public static void GrowInBiome(List<ThingDef> outPlants, Map ___map)
        {
            List<ThingDef> PlantsToRemove = new List<ThingDef>();
            if (ModSettings_Utility.CheckBiomesPatch() && !ModSettings_Utility.MorrowRim_SettingBiomeEnablePlantsOutside())
            {
                foreach (ThingDef plant in outPlants)
                {
                    if (!___map.Biome.defName.Contains("MorrowRim_") && plantsList.Contains(plant.defName))
                    {
                        PlantsToRemove.Add(plant);
                    }
                }
            }
            //remove them this way to avoid nullRef
            foreach (ThingDef plant in PlantsToRemove)
            {
                outPlants.Remove(plant);
            }
        }
    }

    //postifx to spawn plants faster after an ash storm
    [HarmonyPatch(typeof(WeatherDecider))]
    [HarmonyPatch("StartNextWeather")]
    public static class WeatherDecider_StartNextWeather_Patch
    {
        [HarmonyPrefix]
        public static bool IncreaseGrowthAfterAshStorm(Map ___map)
        {
            if (ModSettings_Utility.MorrowRim_SettingAshRegrowth())
            {
                if (___map.weatherManager.curWeather.GetModExtension<WeatherProperties>() != null
                && ___map.weatherManager.curWeather.GetModExtension<WeatherProperties>().isAshStorm)
                {
                    for (int i = 0; i != 20000; i++)
                    {
                        ___map.wildPlantSpawner.WildPlantSpawnerTick();
                    }
                }
            }
            return true;
        }
    }

    //patch to edit terrain gen in ashlands
    //removes gravel and sand, replacing them with the appropriate alternative
    [HarmonyPatch(typeof(GenStep_Terrain))]
    [HarmonyPatch("TerrainFrom")]
    public static class GenStep_Terrain_TerrainFrom_Patch
    {
        [HarmonyPostfix]
        public static void ReplaceTerrainWithAsh(Map map, ref TerrainDef __result)
        {
            switch(map.Biome.defName)
            {
                /* all have the same terrain */
                case "MorrowRim_Ashlands":
                case "MorrowRim_AshSwamp":
                case "MorrowRim_Grazelands":
                case "MorrowRim_TelvanniForest":
                    if (__result == TerrainDefOf.Gravel)
                    {
                        __result = TerrainDef.Named("MorrowRim_StonyAsh");
                    }
                    if (__result == TerrainDefOf.Sand)
                    {
                        __result = TerrainDef.Named("MorrowRim_SandyAsh");
                    }
                    return;
                case "MorrowRim_BlightedAshlands":
                    if (__result == TerrainDefOf.Gravel || __result == TerrainDefOf.Sand)
                    {
                        __result = TerrainDef.Named("MorrowRim_BlightedStonyAsh");
                    }
                    return;
                case "MorrowRim_VolcanicAshlands":
                    if (__result == TerrainDefOf.Gravel)
                    {
                        __result = TerrainDef.Named("MorrowRim_VolcanicGravel");
                    }
                    if (__result == TerrainDefOf.Sand)
                    {
                        __result = TerrainDef.Named("MorrowRim_VolcanicSand");
                    }
                    if (ModSettings_Utility.MorrowRim_SettingBiomeSwitchToDumbLava() && __result.defName == "RG_Lava")
                    {
                        __result = TerrainDef.Named("MorrowRimRG_LavaDumb");
                    }
                    return;
                default:
                    return;
            }
        }
    }

    //remove sand from beaches generated in specific biomes
    //TODO make setting
    [HarmonyPatch(typeof(BeachMaker))]
    [HarmonyPatch("BeachTerrainAt")]
    public static class BeachMaker_BeachTerrainAt_Patch
    {
        [HarmonyPostfix]
        public static void ReplaceTerrainWithAsh(BiomeDef biome, ref TerrainDef __result)
        {
            if (biome.defName == "MorrowRim_AshSwamp" && ModSettings_Utility.MorrowRim_SettingBiomeDisableSwampBeaches())
            {
                if (__result == TerrainDefOf.Sand || __result == TerrainDefOf.Ice)
                {
                    __result = null;
                }
            }
        }
    }

    //fix rich soil on volcanic ashlands
    [HarmonyPatch(typeof(MapGenerator))]
    [HarmonyPatch("GenerateMap")]
    public static class MapGenerator_GenerateMap_Patch
    {
        [HarmonyPostfix]
        public static void ReplaceTerrainWithAsh(ref Map __result)
        {
            if (__result.Biome.defName == "MorrowRim_VolcanicAshlands")
            {
                TerrainDef terrainDef = DefDatabase<TerrainDef>.GetNamed("MorrowRim_VolcanicGravel");
                TerrainGrid terrainGrid = __result.terrainGrid;
                foreach (IntVec3 cell in __result.AllCells)
                {
                    if (cell.GetTerrain(__result).defName == "SoilRich")
                    {
                        terrainGrid.SetTerrain(cell, terrainDef);
                    }
                }
            }
        }
    }

    //TODO really need to make this an actual gen step
    [HarmonyPatch(typeof(GenStep_RockChunks))]
    [HarmonyPatch("Generate")]
    public static class GenStep_ScatterSiltStrider
    {
        [HarmonyPrefix]
        public static bool scatterSiltStrider(Map map)
        {
            if(map.Biome.defName == "MorrowRim_BlightedAshlands")
            {
                int numToSpawn = Rand.Range(3, 5);
                //int numToSpawn = 1;
                IntVec3[] vec = new IntVec3[map.AllCells.Count()];
                vec = map.AllCells.InRandomOrder().ToArray();
                for (int i = 0; i < vec.Length; i++)
                {
                    if (numToSpawn != 0)
                    {
                        if (map.terrainGrid.TerrainAt(vec[i]).defName == "MorrowRim_BlightedAsh")
                        {
                            //generate random shell
                            Thing thing;

                            switch (Rand.Range(1, 3))
                            {
                                case 1:
                                    thing = ThingMaker.MakeThing(ThingDefOf.MorrowRim_SiltStriderShell_a);
                                    break;
                                case 2:
                                    thing = ThingMaker.MakeThing(ThingDefOf.MorrowRim_SiltStriderShell_b);
                                    break;
                                case 3:
                                    thing = ThingMaker.MakeThing(ThingDefOf.MorrowRim_SiltStriderShell_c);
                                    break;
                                default:
                                    thing = ThingMaker.MakeThing(ThingDefOf.MorrowRim_SiltStriderShell_a);
                                    break;
                            }

                            CellRect cellRect = GenAdj.OccupiedRect(vec[i], thing.Rotation, thing.def.size);
                            if (cellRect.InBounds(map))
                            {
                                GenSpawn.Spawn(thing, vec[i], map);
                                numToSpawn--;
                            }
                        }
                    }
                    else return true;
                }
            }
            return true;
        }
    }


    //patch to modify moisture pump behaviour in MorrowRim biomes
    [HarmonyPatch(typeof(CompTerrainPumpDry))]
    [HarmonyPatch("GetTerrainToDryTo")]
    public static class CompTerrainPumpDry_GetTerrainToDryTo_Patch
    {
        [HarmonyPostfix]
        public static void ReplaceWithAsh(Map map, TerrainDef terrainDef, ref TerrainDef __result)
        {
            if (terrainDef.driesTo == TerrainDef.Named("Gravel"))
            {
                switch (map.Biome.defName)
                {
                    case "MorrowRim_Ashlands":
                    case "MorrowRim_AshSwamp":
                    case "MorrowRim_Grazelands":
                    case "MorrowRim_TelvanniForest":
                        __result = TerrainDef.Named("MorrowRim_StonyAsh");
                        return;
                    case "MorrowRim_BlightedAshlands":
                        __result = TerrainDef.Named("MorrowRim_BlightedStonyAsh");
                        return;
                    case "MorrowRim_VolcanicAshlands":
                        __result = TerrainDef.Named("MorrowRim_VolcanicGravel");
                        return;
                    case "MorrowRim_GlaciatedAshlands":
                        __result = TerrainDef.Named("MorrowRim_StonyAsh");
                        return;
                    default:
                        return;
                }
                return;
            }
            if (terrainDef.driesTo == TerrainDef.Named("Soil"))
            {
                switch (map.Biome.defName)
                {
                    case "MorrowRim_Ashlands":
                    case "MorrowRim_AshSwamp":
                    case "MorrowRim_Grazelands":
                    case "MorrowRim_TelvanniForest":
                        __result = TerrainDef.Named("MorrowRim_Ash");
                        return;
                    case "MorrowRim_BlightedAshlands":
                        __result = TerrainDef.Named("MorrowRim_BlightedAsh");
                        return;
                    case "MorrowRim_VolcanicAshlands":
                        __result = TerrainDef.Named("MorrowRim_VolcanicAsh");
                        return;
                    case "MorrowRim_GlaciatedAshlands":
                        __result = TerrainDef.Named("MorrowRim_Ash");
                        return;
                    default:
                        return;
                }
                return;
            }
        }
    }


    //patch to send away traders during ash storms
    [HarmonyPatch(typeof(Trigger_PawnExperiencingDangerousTemperatures))]
    [HarmonyPatch("ActivateOn")]

    public static class Trigger_Ashbuildup
    {
        [HarmonyPostfix]
        public static void ActivateOn_Ash(Lord lord, TriggerSignal signal, ref bool __result)
        {
            if (!__result)
            {
                for (int i = 0; i < lord.ownedPawns.Count; i++)
                {
                    Pawn pawn = lord.ownedPawns[i];
                    if (pawn.Spawned && !pawn.Dead && !pawn.Downed)
                    {
                        Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorrowRim_AshBuildup, false);
                        if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.05)
                        {
                            __result = true;
                            return;
                        }
                    }
                }
            }
            return;
        }
    }

    //patch to prevent traders and such entirely during ash storms
    [HarmonyPatch(typeof(IncidentWorker_PawnsArrive))]
    [HarmonyPatch("CanFireNowSub")]

    public static class IncidentWorker_PawnsArrive_CanFireNowSub_Patch
    {
        [HarmonyPostfix]
        public static void PreventVisitorsDuringAshStorms(IncidentParms parms, ref bool __result)
        {
            if (ModSettings_Utility.MorrowRim_SettingAshPreventVisitors()) 
            {
                if (__result)
                {
                    Map map = (Map)parms.target;
                    var pw = WeatherProperties.Get(map.weatherManager.curWeather);
                    if (pw != null && pw.isAshStorm)
                    {
                        __result = false;
                        return;
                    }
                }
            }
        }
    }

    //prefix to spawn kwama nests instead of insectoid hives on map gen in ashlands
    [HarmonyPatch(typeof(GenStep_CaveHives))]
    [HarmonyPatch("Generate")]

    public static class Gen_KwamaNest_NotInsectoidHive
    {
        [HarmonyPrefix]
        public static bool Gen_KwamaNest(Map map, GenStepParams parms)
        {
            //advanced version, chance based on proximity to egg mine
            if (ModSettings_Utility.MorrowRim_SettingKwamaEnableGen())
            {
                WorldGrid worldGrid = Find.WorldGrid;
                WorldObjectsHolder worldObjectsHolder = Find.WorldObjects;
                List<Settlement> kwamaSettlements = new List<Settlement>(worldObjectsHolder.SettlementBases.Where(x => x.Faction.def == FactionDefOf.MorrowRim_Kwama));
                float distance = -1f;
                for (int i = 0; i != kwamaSettlements.Count; i++)
                {
                    float distance2 = worldGrid.ApproxDistanceInTiles(map.Tile, kwamaSettlements[i].Tile);
                    if (distance == -1 || distance > distance2) distance = distance2;
                }
                //try to spawn in kwama nests
                if (map.Biome.defName == "MorrowRim_Ashlands" || map.Biome.defName == "MorrowRim_Grazelands"
                    || map.Biome.defName == "MorrowRim_VolcanicAshlands" || map.Biome.defName == "MorrowRim_AshSwamp") distance /= 2;   //doubles probability for certain biomes
                if (map.Biome.defName == "MorrowRim_BlightedAshlands") distance *= 2;   //halves probability if the biome is blighted ashlands
                                                                                        //Log.Message("Checking for spawn chance of kwama nests, distance from nearest nest is: " + distance);
                if (Rand.Range(0, distance + 1) <= ModSettings_Utility.MorrowRim_SettingKwamaMinDistance())
                {
                    Kwama.GenStep_KwamaNest kwamaNest = new Kwama.GenStep_KwamaNest();
                    kwamaNest.Generate(map, parms);
                    return false;
                }
            }
            return true;
        }
    }

    //patch to spawn kwama settlements on impassable tiles
    //also limits to certain biomes
    [HarmonyPatch(typeof(TileFinder))]
    [HarmonyPatch("RandomSettlementTileFor")]
    public static class KwamaSettlementSpawnerPatch
    {
        [HarmonyPrefix]
        public static bool SettlementPatch(ref int __result, Faction faction)
        {
            if (faction?.def == FactionDefOf.MorrowRim_Kwama)
            {
                __result = DoTheThing(faction);
                return false;
            }
            return true;
        }

        public static int DoTheThing(Faction faction)
        {
            for (int i = 0; i < 500; i++)
            {
                int num;
                if ((from _ in Enumerable.Range(0, 100) select Rand.Range(0, Find.WorldGrid.TilesCount)).TryRandomElementByWeight(delegate (int x)
                {
                    Tile tile = Find.WorldGrid[x];
                    if (!tile.biome.canBuildBase || !tile.biome.implemented || tile.hilliness != Hilliness.Impassable)
                    {
                        return 0f;
                    }
                    if (biomeDefNames.Contains(tile.biome.defName))
                    {
                        if(tile.biome.defName == biomeDefNames[0]) return tile.biome.settlementSelectionWeight * 2;
                        return tile.biome.settlementSelectionWeight;
                    }

                    return 0f;
                }, out num))
                    if (CheckTileIsValid(num, null))
                    {
                        return num;
                    }
            }
            Log.Error("Failed to find faction base tile for " + faction, false);
            return 0;
        }

        public static bool CheckTileIsValid(int tile, StringBuilder reason = null)
        {
            Tile tile2 = Find.WorldGrid[tile];
            if (!tile2.biome.canBuildBase)
            {
                if (reason != null)
                {
                    reason.Append("CannotLandBiome".Translate(tile2.biome.LabelCap));
                }
                return false;
            }
            if (!tile2.biome.implemented)
            {
                if (reason != null)
                {
                    reason.Append("BiomeNotImplemented".Translate() + ": " + tile2.biome.LabelCap);
                }
                return false;
            }
            /*if (tile2.hilliness == Hilliness.Impassable)
            {
                if (reason != null)
                {
                    reason.Append("CannotLandImpassableMountains".Translate());
                }
                return false;
            }*/
            Settlement settlement = Find.WorldObjects.SettlementBaseAt(tile);
            if (settlement != null)
            {
                if (reason != null)
                {
                    if (settlement.Faction == null)
                    {
                        reason.Append("TileOccupied".Translate());
                    }
                    else if (settlement.Faction == Faction.OfPlayer)
                    {
                        reason.Append("YourBaseAlreadyThere".Translate());
                    }
                    else
                    {
                        reason.Append("BaseAlreadyThere".Translate(settlement.Faction.Name));
                    }
                }
                return false;
            }
            if (Find.WorldObjects.AnySettlementBaseAtOrAdjacent(tile))
            {
                if (reason != null)
                {
                    reason.Append("FactionBaseAdjacent".Translate());
                }
                return false;
            }
            if (Find.WorldObjects.AnyMapParentAt(tile) || Current.Game.FindMap(tile) != null || Find.WorldObjects.AnyWorldObjectOfDefAt(WorldObjectDefOf.AbandonedSettlement, tile))
            {
                if (reason != null)
                {
                    reason.Append("TileOccupied".Translate());
                }
                return false;
            }
            return true;
        }

        static readonly string[] biomeDefNames = { "MorrowRim_Ashlands", "MorrowRim_AshSwamp", "MorrowRim_VolcanicAshlands", BiomeDefOf.TemperateForest.defName, BiomeDefOf.TropicalRainforest.defName, BiomeDefOf.AridShrubland.defName };
    }

    [HarmonyPatch(typeof(Faction))]
    class GetRidOfLeaderError
    {
        [HarmonyPostfix]
        [HarmonyPatch("ShouldHaveLeader", MethodType.Getter)]
        static void GetRidOfLeaderError_Patch(ref Faction __instance, ref bool __result)
        {
            if (__instance.def.defName == "MorrowRim_Kwama") __result = false;
        }
    }

    /* extinction settings */

    [HarmonyPatch(typeof(BiomeDef))]
    [HarmonyPatch("CommonalityOfAnimal")]
    public static class BiomeDef_CommonalityOfAnimal_Patch
    {
        [HarmonyPostfix]
        public static void ExtinctionPatch(PawnKindDef animalDef, ref float __result)
        {
            if (animalDef == PawnKindDefOf.MorrowRim_CliffRacer && ModSettings_Utility.MorrowRim_SettingEnableCliffRacerExtinction()||
                animalDef == PawnKindDefOf.MorrowRim_SiltStrider && ModSettings_Utility.MorrowRim_SettingEnableSiltStriderExtinction())
            {
                __result = 0f;
            }
        }
    }

    [HarmonyPatch(typeof(BiomeDef))]
    [HarmonyPatch("IsPackAnimalAllowed")]
    public static class BiomeDef_IsPackAnimalAllowed_Patch
    {
        [HarmonyPostfix]
        public static void ExtinctionPatch(ThingDef pawn, ref bool __result)
        {
            if (pawn == ThingDefOf.MorrowRim_SiltStrider && ModSettings_Utility.MorrowRim_SettingEnableSiltStriderExtinction())
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(BiomeDef))]
    [HarmonyPatch("CommonalityOfDisease")]
    public static class BiomeDef_CommonalityOfDisease_Patch
    {
        [HarmonyPostfix]
        public static void ExtinctionPatch(IncidentDef diseaseInc, ref float __result)
        {
            if (diseaseInc.defName == "MorrowRim_Disease_Corprus" && ModSettings_Utility.MorrowRim_SettingEnableCorprusExtinction())
            {
                __result = 0f;
            }
        }
    }
}