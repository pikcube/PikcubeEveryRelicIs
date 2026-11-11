using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Commands;

public class AlwaysSetPosition : AlwaysConsoleCmd
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

        foreach (RelicModel relic in relics)
        {
            if (!AlwaysRelicManager.RelicQueue.Contains(relic))
            {
                continue;
            }

            while (AlwaysRelicManager.NextRelic() != relic)
            {
            }

            AlwaysRelicManager.DecPos();
        }

        return new CmdResult(true, $"Next relic is {AlwaysRelicManager.NextRelic(false).Id}");
    }

    public static RelicModel? GetRelicById(string id)
    {
        id = id.ToUpperInvariant();
        List<RelicModel> list = [.. ModelDb.AllRelics.Where(r => r.Id.Entry.Contains(id))];
        return list.FirstOrDefault(r => r.Id.Entry == id) ?? list.FirstOrDefault(r => r.Id.Entry.StartsWith(id)) ?? list.FirstOrDefault();
    }

    public override string CmdName => "always_position";
    public override string Args => "<relic-id:string>";

    public override string Description =>
        "Set the next relic to be obtained in the relic queue (in case of accidental desync)";

    public override bool IsNetworked => false;

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
        return CompleteArgument([.. AlwaysRelicManager.RelicQueue.Select(r => r.Id.Entry)], [], args[0], matchPredicate: (Func<string, string, bool>)((candidate, partial) => candidate.Contains(partial, StringComparison.OrdinalIgnoreCase)));
    }
}