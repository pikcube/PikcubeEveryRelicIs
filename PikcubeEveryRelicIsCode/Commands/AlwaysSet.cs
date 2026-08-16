using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Commands;

public class AlwaysSet : AlwaysConsoleCmd
{
    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        if (args.Length == 0)
        {
            return new CmdResult(false, "You need to specify a relic");
        }

        List<string> errors = [];
        List<RelicModel> relics = [];

        foreach (string relicId in args)
        {
            RelicModel? relic = GetRelicById(relicId);
            if (relic is null)
            {
                errors.Add($"Relic not found: {relicId}");
            }
            else
            {
                relics.Add(relic);
            }
        }

        if (errors.Count != 0)
        {
            return new CmdResult(false, string.Join('\n', errors));
        }

        AlwaysRelicManager.RelicQueue.Clear();
        AlwaysRelicManager.RelicQueue.AddRange(relics);
        AlwaysRelicManager.ResetPos();
        return new CmdResult(true, $"Current queue: {string.Join('|', AlwaysRelicManager.RelicQueue.Select(r => r.Id.ToString()))}");

    }

    public static RelicModel? GetRelicById(string id)
    {
        id = id.ToUpperInvariant();
        List<RelicModel> list = [.. ModelDb.AllRelics.Where(r => r.Id.Entry.Contains(id))];
        return list.FirstOrDefault(r => r.Id.Entry == id) ?? list.FirstOrDefault(r => r.Id.Entry.StartsWith(id)) ?? list.FirstOrDefault();
    }

    public override string CmdName => "always_set";
    public override string Args => "<relic-id:string>";
    public override string Description => "Set a relic to the Always Relic queue.";
    public override bool IsNetworked => true;
    public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
    {
        if (args.Length > 1)
        {
            return new CompletionResult
            {
                Type = CompletionType.Argument,
                ArgumentContext = CmdName
            };
        }
        return CompleteArgument([.. ModelDb.AllRelics.Select(r => r.Id.Entry)], [], args[0], matchPredicate: (Func<string, string, bool>)((candidate, partial) => candidate.Contains(partial, StringComparison.OrdinalIgnoreCase)));
    }
}