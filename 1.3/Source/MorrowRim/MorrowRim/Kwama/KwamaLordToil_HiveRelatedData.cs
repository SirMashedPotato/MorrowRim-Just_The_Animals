using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using RimWorld;

namespace MorrowRim.Kwama
{
    class KwamaLordToil_HiveRelatedData : LordToilData
    {
		// Token: 0x06003205 RID: 12805 RVA: 0x00115CB8 File Offset: 0x00113EB8
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, KwamaNest> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, KwamaNest>(ref this.assignedHives, "assignedHives", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, KwamaNest> x) => x.Value == null);
			}
		}

		// Token: 0x04001B21 RID: 6945
		public Dictionary<Pawn, KwamaNest> assignedHives = new Dictionary<Pawn, KwamaNest>();
	}
}
