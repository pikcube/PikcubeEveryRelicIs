using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Patches;

namespace PikcubeEveryRelicIs.PikcubeEveryRelicIsCode.Relics;

[Pool(typeof(SharedRelicPool))]
public class DerpingBell : DifferentEveryRelicIsRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        _ = TaskHelper.RunSafely(SwapBell());
    }

    private async Task SwapBell()
    {
        int index = Owner.Relics.IndexOf(this);
        await RelicCmd.Remove(this);
        ObtainRelicCmdPatch.AllowNext();
        await RelicCmd.Obtain(ModelDb.Relic<CallingBell>().ToMutable(), Owner, index);
    }
}