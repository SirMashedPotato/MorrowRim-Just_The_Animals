using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection.Emit;
using System.Linq;
using System.Text;

namespace MorrowRim
{
    public class WeatherEvent_AshBlight : WeatherEvent
    {
        float current = 0;
        public WeatherEvent_AshBlight(Map map) : base(map)
        {
        }

        public override bool Expired
        {
            get
            {
                return this.age > this.duration;
            }
        }

        public override void FireEvent()
        {
            if (!ModSettings_Utility.MorrowRim_SettingEnableBlightEffects()) return;


            //for plants
            IntVec3[] vecs = new IntVec3[map.AllCells.Where(x => WeatherUtilityBlight.IsValidCell(x, this.map)).Count()];
            vecs = map.AllCells.Where(x => WeatherUtilityAsh.IsValidCell(x, this.map)).InRandomOrder().ToArray();
            List<Plant> plants = new List<Plant>();
            foreach (IntVec3 cell in vecs)
            {
                Plant p = DoBlightToPlant(cell);
                if(p != null)
                {
                    plants.Add(p);
                }
                if (plants.Count >= ModSettings_Utility.MorrowRim_SettingBlightPlantNumber()) break;
            }
            if (!plants.NullOrEmpty())
            {
                Find.LetterStack.ReceiveLetter("MorrowRim_LetterLabelPlantBlight".Translate(), FormatLetterText("MorrowRim_LetterPlantBlight".Translate(), plants), LetterDefOf.ThreatSmall, plants, null, null);
            }

            //for pawns
            List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawns = new List<Pawn>();
            for (int i = 0; i != allPawnsSpawned.Count(); i++)
            {
                if (WeatherUtilityBlight.IsValidTarget(allPawnsSpawned[i]))
                {
                    if (DoBlightToPawn(allPawnsSpawned[i]))
                    {
                        pawns.Add(allPawnsSpawned[i]);
                    }
                    if (pawns.Count >= ModSettings_Utility.MorrowRim_SettingBlightAnimalNumber()) break;
                }
            }
            if (!pawns.NullOrEmpty())
            {
                Find.LetterStack.ReceiveLetter("MorrowRim_LetterLabelAnimalBlight".Translate(), FormatLetterText("MorrowRim_LetterAnimalBlight".Translate(), pawns), LetterDefOf.ThreatSmall, pawns, null, null);
            }
        }

        public bool DoBlightToPawn(Pawn p)
        {
            if (WeatherUtilityBlight.CheckAnimalBlight(p) && WeatherUtilityBlight.CheckAnimalBlightScaling(p))
            {
                WeatherUtilityBlight.TriggerAnimalBlight(p);
                return true;
            }
            return false;
        }

        public Plant DoBlightToPlant(IntVec3 vec)
        {
            List<Thing> thingList = vec.GetThingList(this.map);
            foreach (Thing t in thingList)
            {
                if (WeatherUtilityBlight.IsActualPlant(t))
                {
                    Plant p = t as Plant;
                    if (WeatherUtilityBlight.CheckPlantBlight(p))
                    {
                        WeatherUtilityBlight.TriggerPlantBlight(p);
                        return p;
                    }
                }
            }
            return null;
        }

        public string FormatLetterText(String letter, List<Pawn> pawns)
        {

            StringBuilder stringBuilder = new StringBuilder();
            //setup list text
            for (int i = 0; i < pawns.Count; i++)
            {
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.AppendLine();
                }
                stringBuilder.Append("  - " + pawns[i].LabelNoCountColored.Resolve());
            }

            return letter + stringBuilder.ToString();
        }

        public string FormatLetterText(String letter, List<Plant> plants)
        {

            StringBuilder stringBuilder = new StringBuilder();
            //setup list text
            for (int i = 0; i < plants.Count; i++)
            {
                if (stringBuilder.Length != 0)
                {
                    stringBuilder.AppendLine();
                }
                stringBuilder.Append("  - " + plants[i].LabelNoCount);
            }

            return letter + stringBuilder.ToString();
        }

        public override void WeatherEventTick()
        {
            this.age++;
        }

        //variables
        private int age;
        private int duration;
    }
}
