using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wisplantern.EasyPackets
{
    public readonly struct SmokeBomb : IEasyPacket<SmokeBomb>
    {
        public readonly int Player;
        public readonly int Time;

        public SmokeBomb(int Player, int Time)
        {
            this.Player = Player;
            this.Time = Time;
        }

        public SmokeBomb Deserialise(BinaryReader reader, in SenderInfo sender)
        {
            return new SmokeBomb(reader.ReadInt32(), reader.ReadInt32());
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(Player);
            writer.Write(Time);
        }
    }

    public readonly struct SmokeBombHandler : IEasyPacketHandler<SmokeBomb>
    {
        void IEasyPacketHandler<SmokeBomb>.Receive(in SmokeBomb packet, in SenderInfo sender, ref bool handled)
        {
            Main.player[packet.Player].SmokeBomb(packet.Time, true);
            handled = true;
        }
    }
}
