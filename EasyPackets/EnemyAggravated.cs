using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisplantern.EasyPackets
{
    public readonly struct EnemyAggravated : IEasyPacket<EnemyAggravated>
    {
        public readonly float amount;
        public readonly int enemy;
        public readonly int damage;
        public readonly float knockback;
        public readonly int critChance;
        public readonly int player;
        public readonly bool text;

        public EnemyAggravated(float amount, int enemy, int damage, float knockback, int critChance, int player, bool text)
        {
            this.amount = amount;
            this.enemy = enemy;
            this.damage = damage;
            this.knockback = knockback;
            this.critChance = critChance;
            this.player = player;
            this.text = text;
        }

        public EnemyAggravated Deserialise(BinaryReader reader, in SenderInfo sender)
        {
            return new EnemyAggravated(reader.ReadSingle(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadSingle(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadBoolean());
        }

        public void Serialise(BinaryWriter writer)
        {
            writer.Write(amount);
            writer.Write(enemy);
            writer.Write(damage);
            writer.Write(knockback);
            writer.Write(critChance);
            writer.Write(player);
            writer.Write(text);
        }
    }

    public readonly struct EnemyAggravatedHandler : IEasyPacketHandler<EnemyAggravated>
    {
        void IEasyPacketHandler<EnemyAggravated>.Receive(in EnemyAggravated packet, in SenderInfo sender, ref bool handled)
        {
            Main.npc[packet.enemy].Aggravate(packet.amount, packet.damage, packet.knockback, packet.critChance, Main.player[packet.player], Main.player[packet.player].HeldItem, packet.text, true);
            handled = true;
        }
    }
}
