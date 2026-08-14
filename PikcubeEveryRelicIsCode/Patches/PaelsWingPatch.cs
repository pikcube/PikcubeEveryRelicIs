using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using Pikcube.Common.Extensions;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(PaelsWing), nameof(PaelsWing.OnSacrifice))]
public static class PaelsWingPatch
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

[HarmonyPatch(typeof(PaelsWing), nameof(PaelsWing.TryModifyCardRewardAlternatives))]
public static class PaelsWingsStaggerPatch
{
    public static readonly CardRewardAlternative GlobalSacrifice = new("SACRIFICE", DoSacAll,
        PostAlternateCardRewardAction.EndSelectionAndCompleteReward);

    private static async Task DoSacAll()
    {
        RunState? runState = RunManager.Instance.PrivatePropertyWrapper<RunManager, RunState>("State").Value;
        if (runState is null)
        {
            return;
        }

        foreach (PaelsWing relic in runState.Players.SelectMany(p => p.Relics).OfType<PaelsWing>().ToArray())
        {
            await relic.OnSacrifice();
        }
    }

    public static bool Prefix(ref bool __result, PaelsWing __instance, Player player, CardReward cardReward, List<CardRewardAlternative> alternatives)
    {
        if (player.RunState.Players.Count > 1)
        {
            return true;
        }

        if (player.Relics.OfType<PaelsWing>().Count() <= 1)
        {
            return true;
        }

        if (alternatives.Contains(GlobalSacrifice))
        {
            __result = false;
            return false;
        }

        alternatives.Add(GlobalSacrifice);
        __result = true;
        return false;
    }
}