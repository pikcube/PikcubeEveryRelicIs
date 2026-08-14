using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(TrashHeap), "Relics", MethodType.Getter)]
public static class TrashHeapPatch
{
    public static RelicModel[] Postfix(RelicModel[] __result)
    {
        return AlwaysRelicManager.IsRelicQueueEnabled ? [AlwaysRelicManager.NextRelic(false)] : __result;
    }
}