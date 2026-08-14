using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using Pikcube.Common.Extensions;
using Pikcube.Common.Utility;
using System.Reflection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(Neow), "GenerateInitialOptions")]
public static class NeowPatch
{
    public static IReadOnlyList<EventOption> Postfix(IReadOnlyList<EventOption> __result, Neow __instance)
    {
        if (!AlwaysRelicManager.IsRelicQueueEnabled || __instance.Owner is null)
        {
            return __result;
        }

        PrivateFieldWrapper<Player, List<RelicModel>> relicField = __instance.Owner.PrivateFieldWrapper<Player, List<RelicModel>>("_relics");
        if (relicField.Value is null)
        {
            return __result;
        }


        if (__instance.Owner.Relics.Any(IsRelicNeedy))
        {
            return
            [
                new EventOption(__instance, () => MyFunc(__instance.Owner, __instance, __result),
                    "PIKCUBE_EVERY_RELIC_ALWAYS_SETUP", [])
            ];
        }

        foreach (RelicModel relic in __instance.Owner.Relics.ToArray())
        {
            TaskHelper.RunSafely(RelicCmd.Replace(relic, AlwaysRelicManager.NextRelic().ToMutable()));
        }
        return __result;

    }

    private static bool IsRelicNeedy(RelicModel relic)
    {
        Type relicType = relic.GetType();
        return AccessTools.DeclaredMethod(relicType, "AfterObtained") is not null;
    }

    private static async Task MyFunc(Player p, Neow neow, IReadOnlyList<EventOption> readOnlyList)
    {
        foreach (RelicModel relic in p.Relics)
        {
            await relic.AfterObtained();
        }

        MethodInfo? method = AccessTools.DeclaredMethod(typeof(EventModel), "SetEventState", [typeof(LocString), typeof(IReadOnlyList<EventOption>)]);
        method.Invoke(neow, [neow.InitialDescription, readOnlyList]);
    }
}