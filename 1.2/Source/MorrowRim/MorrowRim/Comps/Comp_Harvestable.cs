using System;
using Verse;
using RimWorld;

namespace MorrowRim
{
	// Token: 0x02000D0F RID: 3343
	public class Comp_Harvestable : CompHasGatherableBodyResource
	{
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.harvestIntervalDays;
			}
		}
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.harvestAmount;
			}
		}

		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.harvestDef;
			}
		}

		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		public CompProperties_Harvestable Props
		{
			get
			{
				return (CompProperties_Harvestable)this.props;
			}
		}

		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return pawn == null || pawn.ageTracker.CurLifeStage.shearable;
			}
		}

		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "MorrowRim_shedGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
