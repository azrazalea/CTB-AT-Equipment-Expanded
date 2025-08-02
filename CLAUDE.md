# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

CTB-AT Equipment Mod is a RimWorld compatibility mod that bridges "Colonist Tech Background" and "Arcane Technology" mods. It modifies equipment restrictions based on pawn tech levels, allowing certain equipment to be used without research based on the pawn's technological background.

## Build Commands

### VS Code / Visual Studio Build (Recommended)

The mod now uses a modern .NET SDK project structure:

```bash
# Build with dotnet CLI
dotnet build Source/CTB-AT-Equipment.csproj

# Or in VS Code
# Press Ctrl+Shift+B to build
```

This compiles to both `1.5/Assemblies/` and `1.6/Assemblies/` directories.

### Legacy Mono Build

The original build script is still available:

```bash
cd Source
./compile
```

Dependencies:
- Harmony (via NuGet package)
- TechBackground.dll (from Colonist Tech Background mod)
- Arcane_Technology.dll (from Arcane Technology mod)
- RimWorld core assemblies

## Architecture Overview

### Core Components

1. **Harmony Patches**: The mod uses Harmony library to patch RimWorld methods at runtime
   - `HarmonyPatches.cs`: Entry point that registers all patches
   - `EquipmentUtility_Patch.cs`: Patches `EquipmentUtility.CanEquip` to modify equipment restrictions
   - `IsResearchLocked_Patch.cs`: Patches `Base.IsResearchLocked` from Arcane Technology mod

2. **Equipment Rules System**:
   - `ItemLevelRule.cs`: Defines tech level requirements for equipment (with/without research)
   - `EquipmentRestrictions.cs`: Static dictionary storing all equipment rules
   - `EquipmentRestrictionsPatcher.cs`: XML PatchOperation that loads rules from `EquipmentRules.xml`

3. **Patch Logic Flow**:
   - XML patches load equipment rules into memory via `EquipmentRestrictionsPatcher`
   - Harmony patches intercept equipment checks and modify tech levels temporarily
   - Uses `TechLevelMatcher` (from dependency mods) to check pawn compatibility
   - Manipulates `GearAssigner.exemptProjects` to bypass research requirements

### Key Concepts

- **Tech Levels**: Neolithic, Medieval, Industrial, Spacer, etc.
- **Pawn Tech Background**: Trait degree determines what tech level equipment they can use
- **Dual Requirements**: Each item has two tech levels - one without research, one with research
- Equipment restrictions are applied through runtime patching, not XML modifications

## Version Compatibility

Currently supports RimWorld 1.5. To update for 1.6:
1. Update `<supportedVersions>` in About.xml
2. Create new version folder structure if needed
3. Verify compatibility with updated dependency mods
4. Test Harmony patches against RimWorld 1.6 changes