using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;
using RimWorld;
using SimpleSidearms.rimworld;
using DArcaneTechnology;

namespace CTB_AT_Equipment
{
	[StaticConstructorOnStartup]
	public static class SimpleSidearmsPatch
	{
		static SimpleSidearmsPatch()
		{
			try
			{
				var harmony = new Harmony("ctb-at-equipment.simplesidearms");
				
				// Check if Simple Sidearms is loaded
				bool simpleSidearmsLoaded = LoadedModManager.RunningModsListForReading.Any(x => 
					x.Name.ToLower().Contains("simple sidearms") || 
					x.PackageId.ToLower().Contains("simplesidearms"));
				
				if (simpleSidearmsLoaded)
				{
					Log.Message("CTB-AT Equipment: Simple Sidearms detected, applying compatibility patch");
					
					// Remove Arcane Technology's patch first
					var arcaneTechHarmony = new Harmony("io.github.dametri.arcanetechnology");
					var targetType = AccessTools.TypeByName("PeteTimesSix.SimpleSidearms.Utilities.StatCalculator");
					if (targetType != null)
					{
						var isValidSidearmMethod = AccessTools.Method(targetType, "isValidSidearm");
						if (isValidSidearmMethod != null)
						{
							arcaneTechHarmony.Unpatch(isValidSidearmMethod, HarmonyPatchType.Postfix, "io.github.dametri.arcanetechnology");
							Log.Message("CTB-AT Equipment: Removed Arcane Technology's SimpleSidearms patch");
						}
						
						// Apply our patch to CanPickupSidearmType which has pawn context
						var targetMethod = AccessTools.Method(targetType, "CanPickupSidearmType");
						var patchMethod = AccessTools.Method(typeof(SimpleSidearmsPatch), "CanPickupSidearmType_Postfix");
						
						if (targetMethod != null && patchMethod != null)
						{
							harmony.Patch(targetMethod, null, new HarmonyMethod(patchMethod));
							Log.Message("CTB-AT Equipment: Simple Sidearms patch applied successfully");
						}
						else
						{
							Log.Warning("CTB-AT Equipment: Could not find Simple Sidearms target method");
						}
					}
					else
					{
						Log.Warning("CTB-AT Equipment: Could not find Simple Sidearms StatCalculator type");
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error($"CTB-AT Equipment: Error patching Simple Sidearms: {ex}");
			}
		}
		
		public static void CanPickupSidearmType_Postfix(ThingDefStuffDefPair sidearmType, Pawn pawn, ref bool __result, ref string errString)
		{
			// Only check if SimpleSidearms said yes - we need to potentially block it
			if (!__result) return;
			
			try
			{
				var thingDef = sidearmType.thing;
				if (thingDef == null) return;
				
				// First check: Is this research-locked? (Arcane Technology's logic)
				if (Base.IsResearchLocked(thingDef))
				{
					// It's research-locked, but check if this pawn's tech background allows it
					var checkInfo = EquipmentChecker.CheckEquipment(thingDef, pawn);
					
					if (checkInfo.result == EquipmentCheckResult.CanUseWithoutResearch)
					{
						// Pawn can use without research - allow it
						__result = true;
						errString = null;
					}
					else
					{
						// Pawn cannot use without research - block it (Arcane Technology's behavior)
						__result = false;
						errString = "DUnknownTechnology".Translate();
					}
				}
				// If not research-locked, let SimpleSidearms' original decision stand
			}
			catch (Exception ex)
			{
				Log.Error($"CTB-AT Equipment: Error in Simple Sidearms postfix: {ex}");
			}
		}
	}
}