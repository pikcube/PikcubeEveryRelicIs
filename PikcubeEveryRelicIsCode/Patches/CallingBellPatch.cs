using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(CallingBell), "AfterObtained")]
public class CallingBellPatch
{
    public static void Prefix()
    {
        AlwaysRelicManager.Freeze();
    }

    public static async Task Postfix(Task __result)
    {
        await __result;
        AlwaysRelicManager.Unfreeze();
    }
}