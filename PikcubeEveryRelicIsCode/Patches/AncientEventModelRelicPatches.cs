using HarmonyLib;
using MegaCrit.Sts2.Core.Models;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(AncientEventModel), "RelicOption", [typeof(RelicModel), typeof(string), typeof(string)])]
public class AncientEventModelRelicPatches
{
    public static void Prefix(AncientEventModel __instance, ref RelicModel relic,
        string pageName = "INITIAL", string? customDonePage = null)
    {
        if (!AlwaysRelicManager.IsRelicQueueEnabled)
        {
            return;
        }
        relic = AlwaysRelicManager.NextRelic().ToMutable();
    }
}