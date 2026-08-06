using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Commands;

public class AlwaysClear : AbstractConsoleCmd
{
    public override CmdResult Process(Player? issuingPlayer, string[] args)
    {
        AlwaysRelicManager.RelicQueue.Clear();
        AlwaysRelicManager.ResetPos();
        return new CmdResult(true, "Queue empty");
    }

    public override string CmdName => "always_clear";
    public override string Args => "";
    public override string Description => "Reset Always Relic Queue";
    public override bool IsNetworked => true;
}