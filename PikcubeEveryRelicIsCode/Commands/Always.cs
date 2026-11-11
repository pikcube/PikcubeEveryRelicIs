using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.Entities.Players;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Commands;

public class Always : AlwaysConsoleCmd
{
    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        return AlwaysRelicManager.RelicQueue.Count != 0 ? new CmdResult(true, $"Current queue: {string.Join('|', AlwaysRelicManager.RelicQueue.Select(r => r.Id.ToString()))}") : new CmdResult(true, "Queue empty");
    }

    public override string CmdName => "always_is";
    public override string Args => "";
    public override string Description => "Display the current queue";
    public override bool IsNetworked => false;
}