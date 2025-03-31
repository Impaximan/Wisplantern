using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;

namespace Wisplantern.EasyPackets
{
    public readonly struct SpawnDecoy : IEasyPacket<SpawnDecoy>
    {
        public readonly float X;
        public readonly float Y;
        public readonly int player;
        public readonly int crit;
        public readonly int damage;
        public readonly int lifeMax;
        public readonly int type;
        public readonly float dX;
        public readonly float dY;

        public SpawnDecoy(float X, float Y, int player, int crit, int damage, int lifeMax, int type, float dX, float dY)
        {
            this.X = X;
            this.Y = Y;
            this.player = player;
            this.crit = crit;
            this.damage = damage;
            this.lifeMax = lifeMax;
            this.type = type;
            this.dX = dX;
            this.dY = dY;
        }

        public SpawnDecoy Deserialise(BinaryReader reader, in SenderInfo sender)
        {
            return new SpawnDecoy(reader.ReadSingle(), reader.ReadSingle(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadSingle(), reader.ReadSingle());
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(player);
            writer.Write(crit);
            writer.Write(damage);
            writer.Write(lifeMax);
            writer.Write(type);
            writer.Write(dX);
            writer.Write(dY);
        }
    }

    public readonly struct SpawnDecoyHandler : IEasyPacketHandler<SpawnDecoy>
    {
        void IEasyPacketHandler<SpawnDecoy>.Receive(in SpawnDecoy packet, in SenderInfo sender, ref bool handled)
        {
            Player player = Main.player[packet.player];
            int n = NPC.NewNPC(new EntitySource_ItemUse_WithAmmo(player, player.HeldItem, 0), (int)packet.X, (int)packet.Y, packet.type);
            Main.npc[n].position.Y = player.position.Y + player.height - Main.npc[n].height;
            Main.npc[n].velocity = new Vector2(packet.dX, packet.dY);
            Main.npc[n].ai[0] = player.whoAmI;
            Main.npc[n].ai[1] = packet.crit;
            Main.npc[n].damage = packet.damage;
            Main.npc[n].lifeMax = packet.lifeMax;
            Main.npc[n].life = Main.npc[n].lifeMax;
            handled = true;
        }
    }
}
