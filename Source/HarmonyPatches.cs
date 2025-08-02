using System;
using HarmonyLib;
using RimWorld;
using Verse;
using System.Reflection;


namespace CTB_AT_Equipment
{
	[StaticConstructorOnStartup]
	static class HarmonyPatches
	{
		static HarmonyPatches()
		{
			Log.Message("CTB-AT Equipment: Starting Harmony initialization");
			try
			{
				Harmony harmony = new Harmony("azrazalea.CTB_AT_Equipment");
				harmony.PatchAll();
				Log.Message("CTB-AT Equipment: Harmony patches applied successfully");
			}
			catch (System.Exception e)
			{
				Log.Error("CTB-AT Equipment: Failed to apply Harmony patches: " + e.ToString());
			}
		}
	}

}
