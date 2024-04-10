using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using LiteNetLib.Utils;
using StayInTarkov.AkiSupport.Airdrops;
using StayInTarkov.Configuration;
using StayInTarkov.Coop.Components.CoopGameComponents;
using StayInTarkov.Coop.Factories;
using StayInTarkov.Coop.Players;
using StayInTarkov.Networking;
using UnityEngine;

namespace StayInTarkov.Coop.NetworkPacket.Player
{
    public sealed class PingPacket : BasePlayerPacket
    {
        public PingPacket() : base("", nameof(PingPacket))
        {
            PingLocation = default;
            PingType = PingFactory.EPingType.Point;
            Nickname = null;
            PingColor = Color.white;
        }

        public Vector3 PingLocation { get; set; }
        public PingFactory.EPingType PingType { get; set; }
        public Color PingColor { get; set; }
        public string Nickname { get; set; }

        public override ISITPacket Deserialize(byte[] bytes)
        {
            using BinaryReader reader = new BinaryReader(new MemoryStream(bytes));
            ReadHeaderAndProfileId(reader);
            ProfileId = reader.ReadString();
            PingLocation = SITSerialization.Vector3Utils.Deserialize(reader);
            PingType = (PingFactory.EPingType)reader.ReadByte();
            PingColor = SITSerialization.ColorUtils.Deserialize(reader);
            Nickname = reader.ReadString();

            return this;
        }

        public override byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(ms);
            WriteHeaderAndProfileId(writer);
            writer.Write(ProfileId);
            SITSerialization.Vector3Utils.Serialize(writer, PingLocation);
            writer.Write((byte)PingType);
            SITSerialization.ColorUtils.Serialize(writer, PingColor);
            writer.Write(Nickname);

            return ms.ToArray();
        }

        protected override void Process(CoopPlayerClient client)
        {
            if (client.IsYourPlayer) return;

            bool usePingSystem =  PluginConfigSettings.Instance.CoopSettings.UsePingSystem;
            if (usePingSystem)
            {
                var registeredPlayers = Singleton<GameWorld>.Instance.RegisteredPlayers;
                foreach (CoopPlayer player in registeredPlayers)
                {
                    if (SITGameComponent.TryGetCoopGameComponent(out var coopGameComponent))
                    {
                        var targetIsAI = !coopGameComponent.ProfileIdsUser.Contains(player.ProfileId);

                        if (player.ProfileId != ProfileId && !targetIsAI)
                        {
                            StayInTarkovHelperConstants.Logger.LogDebug($"PingPacket:{player.ProfileId}:{ProfileId}:{nameof(Process)}");
                            player.ReceivePing(PingLocation, PingType, PingColor, Nickname);
                        }
                    }
                }
            }
        }
    }
}
