using System;
using RimWorld;
using Verse;
using TechBackground;
using DArcaneTechnology;

namespace CTB_AT_Equipment
{
	public enum EquipmentCheckResult
	{
		NoRules,           // No custom rules apply
		CanUseWithoutResearch,  // Pawn can use without research
		CanUseWithResearch,     // Pawn can use with research
		CannotUse               // Pawn cannot use even with research
	}

	public class EquipmentCheckInfo
	{
		public EquipmentCheckResult result;
		public string exemptResearchDefName;
		public TechLevel originalTechLevel;
		public TechLevel adjustedTechLevel;
		public string cantReason;
	}

	public static class EquipmentChecker
	{
		public static EquipmentCheckInfo CheckEquipment(ThingDef thingDef, Pawn pawn)
		{
			var info = new EquipmentCheckInfo
			{
				result = EquipmentCheckResult.NoRules,
				originalTechLevel = thingDef.techLevel,
				exemptResearchDefName = null,
				cantReason = null
			};

			// Check if CTB equipment effects are enabled
			if (!TechBackground_Settings.AffectEquipping)
			{
				return info;
			}

			// Check if we have custom rules for this item
			if (!EquipmentRestrictions.itemList.ContainsKey(thingDef.defName))
			{
				return info;
			}

			var rule = EquipmentRestrictions.itemList.TryGetValue(thingDef.defName);
			if (rule == null)
			{
				return info;
			}

			// Get pawn's tech level
			var traitDef = TraitDef.Named("TechBackground");
			if (traitDef == null)
			{
				return info;
			}

			int pawnTechLevel = 0;
			if (pawn.story?.traits?.HasTrait(traitDef) == true)
			{
				var trait = pawn.story.traits.GetTrait(traitDef);
				pawnTechLevel = trait.Degree;
			}

			// Check if pawn can use without research
			bool canUseWithoutResearch = TechLevelMatcher.Match(pawnTechLevel, rule.techLevelNoResearch);

			if (canUseWithoutResearch)
			{
				info.result = EquipmentCheckResult.CanUseWithoutResearch;
				info.adjustedTechLevel = rule.techLevelNoResearch;

				// Check if we need to add research exemption
				ResearchProjectDef researchDef;
				if (Base.thingDic.TryGetValue(thingDef, out researchDef))
				{
					if (!GearAssigner.exemptProjects.Contains(researchDef.defName))
					{
						info.exemptResearchDefName = researchDef.defName;
					}
				}
			}
			else
			{
				// Check if pawn can use with research
				bool canUseWithResearch = TechLevelMatcher.Match(pawnTechLevel, rule.techLevelResearched);
				
				if (canUseWithResearch)
				{
					info.result = EquipmentCheckResult.CanUseWithResearch;
					info.adjustedTechLevel = rule.techLevelResearched;
				}
				else
				{
					info.result = EquipmentCheckResult.CannotUse;
					info.cantReason = "Colonist's tech background is too low to use this item.";
				}
			}

			return info;
		}

		public static void ApplyTemporaryChanges(ThingDef thingDef, EquipmentCheckInfo info)
		{
			if (info.result == EquipmentCheckResult.NoRules) return;

			// Apply tech level change
			thingDef.techLevel = info.adjustedTechLevel;

			// Add research exemption if needed
			if (info.exemptResearchDefName != null)
			{
				GearAssigner.exemptProjects.Add(info.exemptResearchDefName);
			}
		}

		public static void RevertTemporaryChanges(ThingDef thingDef, EquipmentCheckInfo info)
		{
			if (info.result == EquipmentCheckResult.NoRules) return;

			// Revert tech level
			thingDef.techLevel = info.originalTechLevel;

			// Remove research exemption if we added it
			if (info.exemptResearchDefName != null)
			{
				GearAssigner.exemptProjects.Remove(info.exemptResearchDefName);
			}
		}
	}
}