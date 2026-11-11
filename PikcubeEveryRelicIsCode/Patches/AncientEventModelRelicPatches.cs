using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(AncientEventModel), "RelicOption", [typeof(RelicModel), typeof(string), typeof(string)])]
public class AncientEventModelRelicPatches
{
    private static AncientEventModel? _mostRecent = null;
    public static void Prefix(AncientEventModel __instance, ref RelicModel relic,
        string pageName = "INITIAL", string? customDonePage = null)
    {
        if (!AlwaysRelicManager.IsRelicQueueEnabled)
        {
            return;
        }

        if (__instance == _mostRecent)
        {
            AlwaysRelicManager.DecPos();
        }
        else if (__instance is Neow)
        {
            AlwaysRelicManager.ResetPos();
            AlwaysRelicManager.NextRelic();
        }
        _mostRecent = __instance;
        relic = AlwaysRelicManager.NextRelic().ToMutable();
    }
}