using LudeonTK;
using RimWorld;
using Verse;

namespace CTB_AT_Equipment
{
    /// <summary>
    /// Debug actions for CTB-AT Equipment Mod
    /// Provides tools to set colonist tech backgrounds for testing
    /// </summary>
    internal class CTBATDebugActions
    {
        #region Tech Background Debug Actions

        /// <summary>
        /// Sets the selected pawn's tech background to Tribal (degree 0)
        /// </summary>
        /// <param name="pawn">The pawn to modify</param>
        [DebugAction("CTB-AT Equipment", "Set Tech Background: Tribal", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SetTechBackgroundTribal(Pawn pawn)
        {
            SetTechBackground(pawn, 0, "Tribal");
        }

        /// <summary>
        /// Sets the selected pawn's tech background to Industrial (degree 1)
        /// </summary>
        /// <param name="pawn">The pawn to modify</param>
        [DebugAction("CTB-AT Equipment", "Set Tech Background: Industrial", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SetTechBackgroundIndustrial(Pawn pawn)
        {
            SetTechBackground(pawn, 1, "Industrial");
        }

        /// <summary>
        /// Sets the selected pawn's tech background to Spacer (degree 2)
        /// </summary>
        /// <param name="pawn">The pawn to modify</param>
        [DebugAction("CTB-AT Equipment", "Set Tech Background: Spacer", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SetTechBackgroundSpacer(Pawn pawn)
        {
            SetTechBackground(pawn, 2, "Spacer");
        }

        /// <summary>
        /// Sets the selected pawn's tech background to Transcendent (degree 3)
        /// </summary>
        /// <param name="pawn">The pawn to modify</param>
        [DebugAction("CTB-AT Equipment", "Set Tech Background: Transcendent", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SetTechBackgroundTranscendent(Pawn pawn)
        {
            SetTechBackground(pawn, 3, "Transcendent");
        }

        /// <summary>
        /// Removes the tech background trait from the selected pawn
        /// </summary>
        /// <param name="pawn">The pawn to modify</param>
        [DebugAction("CTB-AT Equipment", "Remove Tech Background", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void RemoveTechBackground(Pawn pawn)
        {
            if (pawn?.story?.traits == null)
            {
                Log.Warning("CTB-AT Equipment: Cannot modify tech background - pawn has no story or traits");
                return;
            }

            TraitDef techBackgroundDef = TraitDef.Named("TechBackground");
            if (techBackgroundDef == null)
            {
                Log.Warning("CTB-AT Equipment: TechBackground trait not found");
                return;
            }

            if (pawn.story.traits.HasTrait(techBackgroundDef))
            {
                Trait existingTrait = pawn.story.traits.GetTrait(techBackgroundDef);
                pawn.story.traits.RemoveTrait(existingTrait);
                Log.Message($"CTB-AT Equipment: Removed tech background from {pawn.Name.ToStringShort}");
            }
            else
            {
                Log.Message($"CTB-AT Equipment: {pawn.Name.ToStringShort} doesn't have a tech background trait");
            }
        }

        /// <summary>
        /// Shows the current tech background of the selected pawn
        /// </summary>
        /// <param name="pawn">The pawn to check</param>
        [DebugAction("CTB-AT Equipment", "Show Tech Background", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void ShowTechBackground(Pawn pawn)
        {
            if (pawn?.story?.traits == null)
            {
                Log.Message($"CTB-AT Equipment: {pawn?.Name?.ToStringShort ?? "Unknown pawn"} has no story or traits");
                return;
            }

            TraitDef techBackgroundDef = TraitDef.Named("TechBackground");
            if (techBackgroundDef == null)
            {
                Log.Message("CTB-AT Equipment: TechBackground trait definition not found");
                return;
            }

            if (pawn.story.traits.HasTrait(techBackgroundDef))
            {
                Trait trait = pawn.story.traits.GetTrait(techBackgroundDef);
                string levelName = GetTechLevelName(trait.Degree);
                Log.Message($"CTB-AT Equipment: {pawn.Name.ToStringShort} has tech background: {levelName} (degree {trait.Degree})");
            }
            else
            {
                Log.Message($"CTB-AT Equipment: {pawn.Name.ToStringShort} has no tech background trait");
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Sets a pawn's tech background to the specified degree
        /// </summary>
        /// <param name="pawn">The pawn to modify</param>
        /// <param name="degree">The tech level degree (0-3)</param>
        /// <param name="levelName">The name of the tech level for logging</param>
        private static void SetTechBackground(Pawn pawn, int degree, string levelName)
        {
            if (pawn?.story?.traits == null)
            {
                Log.Warning("CTB-AT Equipment: Cannot set tech background - pawn has no story or traits");
                return;
            }

            TraitDef techBackgroundDef = TraitDef.Named("TechBackground");
            if (techBackgroundDef == null)
            {
                Log.Warning("CTB-AT Equipment: TechBackground trait not found");
                return;
            }

            // Remove existing tech background trait if present
            if (pawn.story.traits.HasTrait(techBackgroundDef))
            {
                Trait existingTrait = pawn.story.traits.GetTrait(techBackgroundDef);
                pawn.story.traits.RemoveTrait(existingTrait);
            }

            // Add new tech background trait with specified degree
            Trait newTrait = new Trait(techBackgroundDef, degree);
            pawn.story.traits.GainTrait(newTrait);

            Log.Message($"CTB-AT Equipment: Set {pawn.Name.ToStringShort}'s tech background to {levelName} (degree {degree})");
        }

        /// <summary>
        /// Gets the display name for a tech level degree
        /// </summary>
        /// <param name="degree">The tech level degree</param>
        /// <returns>The display name</returns>
        private static string GetTechLevelName(int degree)
        {
            return degree switch
            {
                0 => "Tribal",
                1 => "Industrial", 
                2 => "Spacer",
                3 => "Transcendent",
                _ => $"Unknown ({degree})"
            };
        }

        #endregion
    }
}