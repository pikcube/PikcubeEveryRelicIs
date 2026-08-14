using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(LargeCapsule), nameof(LargeCapsule.AfterObtained))]
public static class LargeCapsulePatch
{
    public static void Prefix()
    {
        AlwaysRelicManager.Freeze();
    }

    public static async Task Postfix(Task __task)
    {
        await __task;
        AlwaysRelicManager.Unfreeze();
    }
}