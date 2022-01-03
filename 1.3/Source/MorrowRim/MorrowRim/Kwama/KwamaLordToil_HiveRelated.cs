using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace MorrowRim.Kwama
{
    public abstract class KwamaLordToil_HiveRelated : LordToil
    {
		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003200 RID: 12800 RVA: 0x00115BB5 File Offset: 0x00113DB5
		private KwamaLordToil_HiveRelatedData Data
		{
			get
			{
				return (KwamaLordToil_HiveRelatedData)this.data;
			}
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x00115BC2 File Offset: 0x00113DC2
		public KwamaLordToil_HiveRelated()
		{
			this.data = new KwamaLordToil_HiveRelatedData();
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x00115BD5 File Offset: 0x00113DD5
		protected void FilterOutUnspawnedHives()
		{
			this.Data.assignedHives.RemoveAll((KeyValuePair<Pawn, KwamaNest> x) => x.Value == null || !x.Value.Spawned);
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x00115C08 File Offset: 0x00113E08
		protected KwamaNest GetHiveFor(Pawn pawn)
		{
			KwamaNest hive;
			if (this.Data.assignedHives.TryGetValue(pawn, out hive))
			{
				return hive;
			}
			hive = this.FindClosestHive(pawn);
			if (hive != null)
			{
				this.Data.assignedHives.Add(pawn, hive);
			}
			return hive;
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x00115C4C File Offset: 0x00113E4C
		private KwamaNest FindClosestHive(Pawn pawn)
		{
			return (KwamaNest)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.MorrowRim_KwamaNest), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 30f, (Thing x) => x.Faction == pawn.Faction, null, 0, 30, false, RegionType.Set_Passable, false);
		}
	}
}
