using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Commands;

public abstract class AlwaysConsoleCmd : AbstractConsoleCmd
{
    public static List<AlwaysConsoleCmd> AllCommands { get; } = [];
    protected AlwaysConsoleCmd()
    {
        AllCommands.Add(this);
    }
}