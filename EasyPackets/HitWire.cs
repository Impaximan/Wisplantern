using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wisplantern.EasyPackets
{
    public readonly struct HitWire : IEasyPacket<HitWire>
    {
        public readonly int X;
        public readonly int Y;

        public HitWire(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public HitWire Deserialise(BinaryReader reader, in SenderInfo sender)
        {
            return new HitWire(reader.ReadInt32(), reader.ReadInt32());
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }
    }

    public readonly struct HitWireHandler : IEasyPacketHandler<HitWire>
    {
        void IEasyPacketHandler<HitWire>.Receive(in HitWire packet, in SenderInfo sender, ref bool handled)
        {
            typeof(Terraria.Wiring).GetMethod("HitWireSingle", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[]
            {
                    packet.X,
                    packet.Y,
            });
            handled = true;
        }
    }
}
