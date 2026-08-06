using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(RelicGrabBag), "PullFromBack", typeof(RelicRarity), typeof(Func<RelicModel, bool>), typeof(IRunState))]
public static class RelicGrabBagBackPatch
{
    public static RelicModel? Postfix(RelicModel? __result, RelicRarity rarity,
        Func<RelicModel, bool> filter,
        IRunState runState)
    {
        if (!AlwaysRelicManager.IsRelicQueueEnabled)
        {
            return __result;
        }
        return AlwaysRelicManager.NextRelic(false);
    }
}

[HarmonyPatch(typeof(RelicGrabBag), "PullFromFront", typeof(RelicRarity), typeof(Func<RelicModel, bool>), typeof(IRunState))]
public static class RelicGrabBagFrontPatch
{
    public static RelicModel? Postfix(RelicModel? __result, RelicRarity rarity,
        Func<RelicModel, bool> filter,
        IRunState runState)
    {
        if (!AlwaysRelicManager.IsRelicQueueEnabled)
        {
            return __result;
        }
        return AlwaysRelicManager.NextRelic();
    }
}