using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Merchant;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

[HarmonyPatch(typeof(MerchantRelicEntry), "OnTryPurchase")]
public class MerchantPurchasePatch
{
    public static void Prefix()
    {
        ObtainRelicCmdPatch.AllowNext();
    }
}