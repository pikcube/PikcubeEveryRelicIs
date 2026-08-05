using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Relics;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(Player), "PopulateStartingRelics")]
public class ModifyStartingRelicPatch
{
    public static bool IsOverrideOn { get; set; }

    public static void Prefix()
    {
        IsOverrideOn = true;
    }

    public static void Postfix()
    {
        IsOverrideOn = false;
    }
}

[HarmonyPatch]
public static class StartingRelicPatch
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        foreach (Type? type in AccessTools.AllTypes()
                     .Where(t => t.IsSubclassOf(typeof(CharacterModel)) && !t.IsAbstract))
        {
            MethodInfo? method = type.GetProperty("StartingRelics",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetMethod;

            if (method != null && method.DeclaringType != typeof(CardModel))
            {
                yield return method;
            }
        }
    }

    public static IReadOnlyList<RelicModel> Postfix(IReadOnlyList<RelicModel> __result)
    {
        if (!MainFile.IsRelicQueueEnabled)
        {
            return __result;
        }
        return ModifyStartingRelicPatch.IsOverrideOn ? [.. __result.Select(_ =>
        {
            RelicModel relic = MainFile.NextRelic();
            if (relic is CallingBell)
            {
                relic = ModelDb.Relic<DerpingBell>();
            }
            return relic;
        })] : __result;
    }
}