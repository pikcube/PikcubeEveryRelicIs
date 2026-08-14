using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using LostWispRelic = MegaCrit.Sts2.Core.Models.Relics.LostWisp;
using LostWispEvent = MegaCrit.Sts2.Core.Models.Events.LostWisp;


namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

public static class AllGenericObtainRelicPatches
{
    [HarmonyPatch(typeof(WelcomeToWongos), "BuyMysteryBox")]
    [HarmonyPostfix]
    public static async Task BuyBoxPatch(Task __result, WelcomeToWongos __instance)
    {
        await __result;
        await MaybeReplaceAsync<WongosMysteryTicket>(__instance.Owner);
    }

    [HarmonyPatch(typeof(WelcomeToWongos), "CheckObtainWongoBadge")]
    [HarmonyPostfix]
    public static async Task CheckObtainPatch(Task __result, WelcomeToWongos __instance)
    {
        await __result;
        await MaybeReplaceAsync<WongoCustomerAppreciationBadge>(__instance.Owner);
    }

    [HarmonyPatch(typeof(WarHistorianRepy), "UnlockCage")]
    [HarmonyPostfix]
    public static async Task UnlockCagePatch(Task __result, WarHistorianRepy __instance)
    {
        await __result;
        await MaybeReplaceAsync<HistoryCourse>(__instance.Owner);
    }

    [HarmonyPatch(typeof(TeaMaster), "BoneTea")]
    [HarmonyPostfix]
    public static async Task BoneTeaPatch(Task __result, TeaMaster __instance)
    {
        await __result;
        await MaybeReplaceAsync<BoneTea>(__instance.Owner);
    }

    [HarmonyPatch(typeof(TeaMaster), "EmberTea")]
    [HarmonyPostfix]
    public static async Task EmberTeaPatch(Task __result, TeaMaster __instance)
    {
        await __result;
        await MaybeReplaceAsync<EmberTea>(__instance.Owner);
    }

    [HarmonyPatch(typeof(TeaMaster), "TeaOfDiscourtesy")]
    [HarmonyPostfix]
    public static async Task TeaOfDiscourtesyPatch(Task __result, TeaMaster __instance)
    {
        await __result;
        await MaybeReplaceAsync<TeaOfDiscourtesy>(__instance.Owner);
    }

    [HarmonyPatch(typeof(SunkenStatue), "GrabSword")]
    [HarmonyPostfix]
    public static async Task GrabSwordPatch(Task __result, SunkenStatue __instance)
    {
        await __result;
        await MaybeReplaceAsync<SwordOfStone>(__instance.Owner);
    }

    [HarmonyPatch(typeof(RoomFullOfCheese), "Search")]
    [HarmonyPostfix]
    public static async Task SearchPatch(Task __result, RoomFullOfCheese __instance)
    {
        await __result;
        await MaybeReplaceAsync<ChosenCheese>(__instance.Owner);
    }

    [HarmonyPatch(typeof(LostWispEvent), "Claim")]
    [HarmonyPostfix]
    public static async Task SearchPatch(Task __result, LostWispEvent __instance)
    {
        await __result;
        await MaybeReplaceAsync<LostWispRelic>(__instance.Owner);
    }


    [HarmonyPatch(typeof(GraveOfTheForgotten), "Accept")]
    [HarmonyPostfix]
    public static async Task AcceptPatch(Task __result, GraveOfTheForgotten __instance)
    {
        await __result;
        await MaybeReplaceAsync<ForgottenSoul>(__instance.Owner);
    }


    [HarmonyPatch(typeof(ColossalFlower), "ObtainPollinousCore")]
    [HarmonyPostfix]
    public static async Task ObtainPollinousCorePatch(Task __result, ColossalFlower __instance)
    {
        await __result;
        await MaybeReplaceAsync<PollinousCore>(__instance.Owner);
    }


    private static async Task MaybeReplaceAsync<T>(Player? owner) where T : RelicModel
    {
        T? original = owner?.GetRelic<T>();

        if (!AlwaysRelicManager.IsRelicQueueEnabled || 
            original is null ||
            AlwaysRelicManager.RelicQueue.Contains(original))
        {
            return;
        }

        await RelicCmd.Replace(original, AlwaysRelicManager.NextRelic().ToMutable());
    }
}