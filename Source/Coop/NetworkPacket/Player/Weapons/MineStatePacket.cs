using Comfort.Common;
using EFT;
using StayInTarkov.Coop.Matchmaker;
using System.IO;
using System.Linq;
using UnityEngine;
using static StayInTarkov.Networking.SITSerialization;

namespace StayInTarkov.Coop.NetworkPacket.Player.Weapons
{
    public sealed class MineStatePacket : BasePlayerPacket
    {
        public Vector3 Position { get; set; }
        public bool IsActive { get; set; }

        public MineStatePacket() : base("", nameof(MineStatePacket)) { }

        public override byte[] Serialize()
        {
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(ms);
            WriteHeaderAndProfileId(writer);
            writer.Write(Position);
            writer.Write(IsActive);

            return ms.ToArray();
        }

        public override ISITPacket Deserialize(byte[] bytes)
        {
            using BinaryReader reader = new BinaryReader(new MemoryStream(bytes));
            ReadHeaderAndProfileId(reader);
            Position = Vector3Utils.Deserialize(reader);
            IsActive = reader.ReadBoolean();
            return this;
        }

        public override void Process()
        {
            if (SITMatchmaking.IsServer || SITMatchmaking.IsSinglePlayer) return;

            var _gameWorld = Singleton<GameWorld>.Instance;
            var _bridgeMines = _gameWorld.MineManager.Mines;

            foreach (var mine in _bridgeMines.Where(mine => mine.transform.parent.gameObject.name == "Directional_mines_LHZONE"))
            {
                float distance = Vector3.Distance(Position, mine.gameObject.transform.position);
                if (distance <= 1.0)
                {
                    mine.SetArmed(IsActive);
                }
            }
        }
    }
}
