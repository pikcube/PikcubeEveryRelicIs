using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace PikcubeEveryRelicIs
{
    [ModInitializer(nameof(Initialize))]
    public partial class MainFile : Node
    {
        private static int Position
        {
            get;
            set => field = value % Math.Max(RelicQueue.Count, 1);
        } = 0;

        public static List<RelicModel> RelicQueue { get; set; } = [];
        public static bool IsRelicQueueEnabled => RelicQueue.Count > 0;

        public const string ModId = "PikcubeEveryRelicIs"; //Used for resource filepath

        public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

        public static void Initialize()
        {
            Harmony harmony = new(ModId);
            harmony.PatchAll();
        }

        public static RelicModel NextRelic(bool moveNext = true)
        {
            if (!IsRelicQueueEnabled)
            {
                return ModelDb.Relic<Circlet>();
            }

            RelicModel relic = RelicQueue[Position];
            if (moveNext)
            {
                ++Position;
            }

            return relic;
        }

        public static void ResetPos()
        {
            Position = 0;
        }
    }
}
