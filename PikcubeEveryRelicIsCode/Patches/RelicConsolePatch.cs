using HarmonyLib;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(RelicConsoleCmd), nameof(RelicConsoleCmd.Process))]
public class RelicConsolePatch
{
    public static void Prefix()
    {
        ObtainRelicCmdPatch.AllowNext();
    }
}