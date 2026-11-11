using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using Pikcube.Common.Code.Utility;
using PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Commands;

namespace PikcubeEveryRelicIs;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{

    public const string ModId = "PikcubeEveryRelicIs"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();

        BetterHooks.AfterOneTimeInitialization += BetterHooks_AfterOneTimeInitialization;
    }

    private static void BetterHooks_AfterOneTimeInitialization()
    {
        AlwaysRelicManager.RelicQueue.Clear();
        AlwaysRelicManager.LoadOrDefault();
    }
}

public class AlwaysRelicManager() : CustomSingletonModel(HookType.Run), ICreatingNewRunListener
{
    static AlwaysRelicManager()
    {
        new EventSpawnManager().RegisterRule<DollRoom>(_ => !IsRelicQueueEnabled);
    }
    private static int Position
    {
        get;
        set => field = value % Math.Max(RelicQueue.Count, 1);
    } = -1;

    public static List<RelicModel> RelicQueue { get; set; } = [];
    public static bool IsRelicQueueEnabled => RelicQueue.Count > 0 && IsQueueFrozen == 0;
    public static int IsQueueFrozen { get; set; }

    public static RelicModel NextRelic(bool moveNext = true)
    {
        if (!IsRelicQueueEnabled)
        {
            return ModelDb.Relic<Circlet>();
        }

        RelicModel relic = RelicQueue[Position];
        if (!moveNext)
        {
            return relic;
        }

        ++Position;
        Save();

        return relic;
    }

    public static void ResetPos()
    {
        Position = 0;
        Save();
    }

    private static void Save()
    {
        GodotFileIo godotFile = new(UserDataPathProvider.GetAccountScopedBasePath("pikcube/everyrelic"));
        godotFile.WriteFile("everyRelicState.txt", $"{Position}\n{string.Join('\n', RelicQueue.Select(r => r.Id.ToString()))}");
    }
    public static void LoadOrDefault()
    {
        GodotFileIo godotFile = new(UserDataPathProvider.GetAccountScopedBasePath("pikcube/everyrelic"));
        string body = godotFile.ReadFile("everyRelicState.txt") ?? string.Empty;
        if (string.IsNullOrEmpty(body))
        {
            return;
        }

        string[] lines = body.Split('\n');
        try
        {
            foreach (string line in lines)
            {
                if (int.TryParse(line, out int pos))
                {
                    Position = pos;
                    continue;
                }

                RelicModel? relic = AlwaysAdd.GetRelicById(line);
                if (relic is null)
                {
                    continue;
                }
                RelicQueue.Add(relic);
            }
        }
        catch (Exception e)
        {
            MainFile.Logger.Error(e.Message);
        }
        Save();
    }

    public void AfterCreatingNewRun(RunState runState, IReadOnlyList<Player> players, IReadOnlyList<ActModel> acts, IReadOnlyList<ModifierModel> modifiers,
        GameMode gameMode, int ascensionLevel, string seed)
    {
        ResetPos();
    }

    public static void Freeze()
    {
        ++IsQueueFrozen;
    }

    public static void Unfreeze()
    {
        --IsQueueFrozen;
    }
}