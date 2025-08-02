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
    [HarmonyPatch(typeof(EquipmentUtility), "CanEquip", new Type[] {typeof(Thing), typeof(Pawn), typeof(string), typeof(bool)}, new ArgumentType[] {ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal})]
	public class CanEquip
	{
		[HarmonyPrefix]
		static bool Prefix(ref EquipmentCheckInfo __state, ref bool __result, Thing thing, Pawn pawn, ref string cantReason, bool checkBonded = true)
		{
			// if we don't have a backstory, then we are not a real pawn, but an insect or a bot
			// allow them to construct everything they can
			if (pawn.story == null) return true;

			__state = EquipmentChecker.CheckEquipment(thing.def, pawn);
			
			if (__state.result == EquipmentCheckResult.CannotUse)
			{
				cantReason = __state.cantReason;
				__result = false;
				return false; // Skip original method
			}
			
			if (__state.result != EquipmentCheckResult.NoRules)
			{
				EquipmentChecker.ApplyTemporaryChanges(thing.def, __state);
			}
			
			return true;
		}
		
	    [HarmonyPostfix]
		static void Postfix(ref EquipmentCheckInfo __state, ref bool __result, Thing thing, Pawn pawn, ref string cantReason, bool checkBonded = true)
		{
			if (__state != null && __state.result != EquipmentCheckResult.NoRules)
			{
				EquipmentChecker.RevertTemporaryChanges(thing.def, __state);
			}
		}
	}
}
