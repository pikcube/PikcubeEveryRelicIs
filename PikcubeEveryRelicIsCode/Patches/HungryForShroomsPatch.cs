using System.Data;
using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(HungryForMushrooms), "GenerateInitialOptions")]
public static class HungryForShroomsPatch
{
    public static IReadOnlyList<EventOption> Postfix(IReadOnlyList<EventOption> __result, HungryForMushrooms __instance)
    {
        if (!AlwaysRelicManager.IsRelicQueueEnabled)
        {
            return __result;
        }

        List<EventOption> newOptions = [];

        RelicModel newRelic = AlwaysRelicManager.NextRelic();
        newOptions.AddRange(__result.Select(option => EventOption.FromRelic(newRelic.ToMutable(), __instance, OnChosen(option.Relic, newRelic.ToMutable(), __instance.Owner ?? throw new NoNullAllowedException(), __instance), $"{StringHelper.Slugify(__instance.GetType().Name)}.pages.INITIAL.options.{newRelic.Id.Entry}")));

        return newOptions.AsReadOnly();
    }

    private static Func<Task> OnChosen(RelicModel? relic, RelicModel newRelic, Player p,
        HungryForMushrooms hungryForMushrooms)
    {
        return ChosenFunc;
        
        async Task ChosenFunc()
        {
            MethodInfo setEventState = AccessTools.DeclaredMethod(typeof(EventModel), "SetEventFinished");

            await RelicCmd.Obtain(newRelic, p);
            switch (relic)
            {
                case BigMushroom:
                    setEventState.Invoke(hungryForMushrooms, [new LocString(hungryForMushrooms.LocTable, "HUNGRY_FOR_MUSHROOMS.pages.BIG_MUSHROOM.description")]);
                    break;
                case FragrantMushroom:
                    setEventState.Invoke(hungryForMushrooms, [new LocString(hungryForMushrooms.LocTable, "HUNGRY_FOR_MUSHROOMS.pages.FRAGRANT_MUSHROOM.description")]);
                    break;
            }
        }
    }
}