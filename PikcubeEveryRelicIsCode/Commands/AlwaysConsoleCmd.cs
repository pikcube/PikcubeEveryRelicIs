using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.RestSite;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Commands;

public abstract class AlwaysConsoleCmd : AbstractConsoleCmd
{
    public static List<AlwaysConsoleCmd> AllCommands { get; } = [];
    protected AlwaysConsoleCmd()
    {
        AllCommands.Add(this);
    }
}