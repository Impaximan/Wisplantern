using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;

namespace Wisplantern.EasyPackets
{
    public readonly struct CaneCircle : IEasyPacket<CaneCircle>
    {
        public readonly float X;
        public readonly float Y;
        public readonly float radius;
        public readonly int type;
        public readonly float dX;
        public readonly float dY;

        public CaneCircle(float X, float Y, float radius, int type, float dX, float dY)
        {
            this.X = X;
            this.Y = Y;
            this.radius = radius;
            this.type = type;
            this.dX = dX;
            this.dY = dY;
        }

        public CaneCircle Deserialise(BinaryReader reader, in SenderInfo sender)
        {
            return new CaneCircle(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt32(), reader.ReadSingle(), reader.ReadSingle());
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(radius);
            writer.Write(type);
            writer.Write(dX);
            writer.Write(dY);
        }
    }

    public readonly struct CaneCircleHandler : IEasyPacketHandler<CaneCircle>
    {
        void IEasyPacketHandler<CaneCircle>.Receive(in CaneCircle packet, in SenderInfo sender, ref bool handled)
        {
            for (int i = 0; i < 150; i++)
            {
                Vector2 dustPos = Main.rand.NextVector2CircularEdge(packet.radius, packet.radius) + new Vector2(packet.X, packet.Y);
                if (!Collision.SolidCollision(dustPos + new Vector2(packet.dX, packet.dY) * 8f, 1, 1) && !Collision.SolidCollision(dustPos, 1, 1))
                {
                    int dust = Dust.NewDust(dustPos, 0, 0, packet.type);
                    Main.dust[dust].velocity = new Vector2(packet.dX, packet.dY);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLightEmittence = true;
                    Main.dust[dust].noLight = true;
                }
            }

            SoundStyle sound = SoundID.Item8;
            sound.PitchVariance = 0.15f;
            sound.MaxInstances = 4;
            sound.SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest;
            SoundEngine.PlaySound(sound, new Vector2(packet.X, packet.Y));

            handled = true;
        }
    }
}
