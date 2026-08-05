using HarmonyLib;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(EventOption), nameof(EventOption.FromRelic), [typeof(RelicModel), typeof(EventModel), typeof(Func<Task>), typeof(string)])]
public class EventOptionPatch
{
    public static void Prefix(ref RelicModel relic, EventModel eventModel, ref Func<Task>? onChosen, string textKey)
    {
        if (!MainFile.IsRelicQueueEnabled)
        {
            return;
        }

        relic = MainFile.NextRelic(false).ToMutable();
    }
}