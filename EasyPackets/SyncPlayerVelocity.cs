using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisplantern.EasyPackets
{
    public readonly struct SyncPlayerVelocity : IEasyPacket<SyncPlayerVelocity>
    {
        public readonly float vX;
        public readonly float vY;
        public readonly int player;

        public SyncPlayerVelocity(float vX, float vY, int player)
        {
            this.vX = vX;
            this.vY = vY;
            this.player = player;
        }

        public SyncPlayerVelocity Deserialise(BinaryReader reader, in SenderInfo sender)
        {
            return new SyncPlayerVelocity(reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt32());
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(vX);
            writer.Write(vY);
            writer.Write(player);
        }
    }

    public readonly struct SyncPlayerVelocityHandler : IEasyPacketHandler<SyncPlayerVelocity>
    {
        void IEasyPacketHandler<SyncPlayerVelocity>.Receive(in SyncPlayerVelocity packet, in SenderInfo sender, ref bool handled)
        {
            Main.player[packet.player].velocity = new Vector2(packet.vX, packet.vY);
            handled = true;
        }
    }

    public readonly struct SyncPlayerTeleport : IEasyPacket<SyncPlayerTeleport>
    {
        public readonly float X;
        public readonly float Y;
        public readonly int player;
        public readonly int type;

        public SyncPlayerTeleport(float X, float Y, int player, int type)
        {
            this.X = X;
            this.Y = Y;
            this.player = player;
            this.type = type;
        }

        public SyncPlayerTeleport Deserialise(BinaryReader reader, in SenderInfo sender)
        {
            return new SyncPlayerTeleport(reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt32(), reader.ReadInt32());
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(player);
            writer.Write(type);
        }
    }

    public readonly struct SyncPlayerTeleportHandler : IEasyPacketHandler<SyncPlayerTeleport>
    {
        void IEasyPacketHandler<SyncPlayerTeleport>.Receive(in SyncPlayerTeleport packet, in SenderInfo sender, ref bool handled)
        {
            Main.player[packet.player].Teleport(new Vector2(packet.X, packet.Y), packet.type);
            handled = true;
        }
    }
}
