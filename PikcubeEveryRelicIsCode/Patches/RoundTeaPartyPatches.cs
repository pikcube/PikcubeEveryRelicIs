using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(RoundTeaParty), "EnjoyTea")]
public class RoundTeaPartyPatches
{
    public static async Task Postfix(Task __result, RoundTeaParty __instance)
    {
        await __result;
        RoyalPoison? poison = __instance.Owner?.GetRelic<RoyalPoison>();
        if (!AlwaysRelicManager.IsRelicQueueEnabled || poison is null || AlwaysRelicManager.RelicQueue.Contains(poison.CanonicalInstance))
        {
            return;
        }

        
        await RelicCmd.Replace(poison, AlwaysRelicManager.NextRelic().ToMutable());
    }
}