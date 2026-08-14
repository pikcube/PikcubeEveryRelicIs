using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(DrowningBeacon), "ClimbOption")]
public static class DrowningBeaconPatches
{
    public static async Task Postfix(Task __result, DrowningBeacon __instance)
    {
        await __result;
        FresnelLens? lens = __instance.Owner?.GetRelic<FresnelLens>();
        if (!AlwaysRelicManager.IsRelicQueueEnabled || lens is null || AlwaysRelicManager.RelicQueue.Contains(lens.CanonicalInstance))
        {
            return;
        }


        await RelicCmd.Replace(lens, AlwaysRelicManager.NextRelic().ToMutable());
    }
}