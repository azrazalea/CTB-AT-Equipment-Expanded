# CTB-AT Equipment Mod

A RimWorld compatibility mod that bridges "Colonist Tech Background" and "Arcane Technology" mods, allowing pawns to use equipment based on their technological background.

## Features

- Modifies equipment restrictions based on pawn tech levels
- Allows certain equipment to be used without research based on the pawn's technological background
- Supports both RimWorld 1.5 and 1.6

## Dependencies

- **Colonist Tech Background** by aossi
- **Arcane Technology** by dame
- **Harmony** (included via NuGet)

## Building from Source

### Prerequisites

- Visual Studio 2022 or VS Code with C# extension
- .NET Framework 4.8 SDK
- RimWorld installed

### Build Instructions

1. Clone or download this repository
2. Update the mod dependency paths in `Source/CTB-AT-Equipment.csproj`:
   - Update the paths to `TechBackground.dll` and `Arcane_Technology.dll` to match your local mod installation
3. Open the solution in Visual Studio or VS Code
4. Build the solution (Ctrl+Shift+B in VS Code)
5. The mod DLL will be compiled to both `1.5/Assemblies/` and `1.6/Assemblies/`

### Alternative Build Method (Linux/Mac)

If you have Mono installed, you can use the original build script:

```bash
cd Source
./compile
```

## Installation

1. Subscribe to the mod dependencies on Steam Workshop
2. Place this mod in your RimWorld Mods folder
3. Enable the mod in RimWorld's mod manager after its dependencies

## License

GPL-3.0 (see Source/gpl-3.0.txt)