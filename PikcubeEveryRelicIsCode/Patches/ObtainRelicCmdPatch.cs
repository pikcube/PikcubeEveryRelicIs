using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(RelicCmd), nameof(RelicCmd.Obtain), [typeof(RelicModel), typeof(Player), typeof(int)])]
public static class ObtainRelicCmdPatch
{
    private static int IgnoreCount { get; set; }
    public static void Prefix(ref RelicModel relic, Player player, int index = -1)
    {
        if (!MainFile.IsRelicQueueEnabled)
        {
            IgnoreCount = 0;
            return;
        }

        if (IgnoreCount > 0)
        {
            --IgnoreCount;
            return;
        }

        RelicModel model = relic;
        if (MainFile.RelicQueue.Any(r => r.CanonicalInstance == model.CanonicalInstance))
        {
            return;
        }

        relic = MainFile.NextRelic().ToMutable();
    }

    public static void AllowNext()
    {
        ++IgnoreCount;
    }
}