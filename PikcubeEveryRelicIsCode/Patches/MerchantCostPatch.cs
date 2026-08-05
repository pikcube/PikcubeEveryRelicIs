using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch]
public class MerchantCostPatch
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        MethodInfo? onPlayMethod = typeof(RelicModel).GetProperty(nameof(RelicModel.MerchantCost))?.GetMethod;
        if (onPlayMethod is not null)
        {
            yield return onPlayMethod;
        }

        foreach (Type? type in AccessTools.AllTypes()
                     .Where(t => t.IsSubclassOf(typeof(RelicModel)) && !t.IsAbstract))
        {
            MethodInfo? method = type.GetProperty(nameof(RelicModel.MerchantCost),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetMethod;

            if (method != null && method.DeclaringType != typeof(CardModel))
            {
                yield return method;
            }
        }
    }

    public static int Postfix(int __result, RelicModel __instance)
    {
        if (__result < 500)
        {
            return __result;
        }

        return __instance.Rarity switch
        {
            RelicRarity.None => 1,
            RelicRarity.Starter => 150,
            RelicRarity.Common => 175,
            RelicRarity.Uncommon => 225,
            RelicRarity.Rare => 275,
            RelicRarity.Shop => 200,
            RelicRarity.Event => 300,
            RelicRarity.Ancient => 300,
            _ => __result
        };
    }
}