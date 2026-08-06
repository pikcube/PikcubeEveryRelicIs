using HarmonyLib;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(AncientEventModel), "GenerateInitialOptionsWrapper")]
public class GenerateInitialOptionsWrapperPatch
{
    public static void Prefix(AncientEventModel __instance)
    {
        if (__instance.Owner is null || __instance.Owner.Relics.Count > 0)
        {
            return;
        }

        int count = __instance.Owner.Character.StartingRelics.Count;
        for (int n = 0; n < count; ++n)
        {
            RelicModel relicModel = AlwaysRelicManager.NextRelic().ToMutable();
            ObtainRelicWithoutSoftlocking(relicModel, __instance.Owner);
        }
    }

    public static void ObtainRelicWithoutSoftlocking(RelicModel relic, Player player, int index = -1)
    {
        relic.AssertMutable();
        player.RunState.CurrentMapPointHistoryEntry?.GetEntry(player.NetId).RelicChoices.Add(new ModelChoiceHistoryEntry(relic.Id, true));
        player.AddRelicInternal(relic, index);
        if (!relic.IsStackable)
        {
            player.RelicGrabBag.Remove(relic);
            player.RunState.SharedRelicGrabBag.Remove(relic);
        }
        if (LocalContext.IsMe(player))
        {
            NRun.Instance?.GlobalUi.RelicInventory.AnimateRelic(relic);
            NDebugAudioManager.Instance?.Play("relic_get.mp3");
            SaveManager.Instance.MarkRelicAsSeen(relic);
        }
        relic.FloorAddedToDeck = player.RunState.TotalFloor;
        TaskHelper.RunSafely(relic.AfterObtained());
    }
}