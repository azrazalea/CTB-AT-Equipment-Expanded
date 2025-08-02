using System;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using DArcaneTechnology;
using TechBackground;

namespace CTB_AT_Equipment
{
	[HarmonyPatch(typeof(Base), "IsResearchLocked", new Type[] {typeof(ThingDef), typeof(Pawn)} )]
	public class IsResearchLocked
	{
		[HarmonyPrefix]
		static bool Prefix(ref EquipmentCheckInfo __state, ref bool __result, ThingDef thingDef, Pawn pawn)
		{
			if (pawn == null) return true;
			
			// if we don't have a backstory, then we are not a real pawn, but an insect or a bot
			// allow them to construct everything they can
			if (pawn.story == null) return true;

			__state = EquipmentChecker.CheckEquipment(thingDef, pawn);
			
			if (__state.result == EquipmentCheckResult.CannotUse)
			{
				__result = true; // Item is locked for this pawn
				return false; // Skip original method
			}
			
			if (__state.result != EquipmentCheckResult.NoRules)
			{
				EquipmentChecker.ApplyTemporaryChanges(thingDef, __state);
			}
			
			return true;
		}
		
	    [HarmonyPostfix]
		static void Postfix(ref EquipmentCheckInfo __state, ref bool __result, ThingDef thingDef, Pawn pawn)
		{
			if (__state != null && __state.result != EquipmentCheckResult.NoRules)
			{
				EquipmentChecker.RevertTemporaryChanges(thingDef, __state);
			}
		}
	}
}
