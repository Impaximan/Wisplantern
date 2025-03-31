using Terraria.WorldBuilding;
using System.Threading;
using System;
using ReLogic.Content;
using Terraria.Audio;
using Wisplantern.Globals.GItems;
using Terraria.GameContent.Creative;
using Wisplantern.Globals.GNPCs;
using Wisplantern.ModPlayers;

namespace Wisplantern
{
    public static class WispUtils
    {
        public static void InvokeOnMainThread(Action action)
        {
            if (!AssetRepository.IsMainThread)
            {
                ManualResetEvent evt = new(false);

                Main.QueueMainThreadAction(() => {
                    action();
                    evt.Set();
                });

                evt.WaitOne();
            }
            else
                action();
        }

        /// <summary>
        /// Registers that an accessory is equipped.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="item"></param>
        public static void AddAccessoryEffect(this Player player, Item item)
        {
            player.GetModPlayer<AccessoryPlayer>().accessoryEffects.Add(item.type);
        }

        public static void MarkAsHuntingWeapon(this Item item)
        {
            if (item.TryGetGlobalItem(out HuntingItem hItem))
            {
                hItem.huntingWeapon = true;
            }
        }

        public static void MarkAsShepherd(this Item item)
        {
            if (item.TryGetGlobalItem(out AggravatingItem aItem))
            {
                aItem.markedAsManipulative = true;
            }
        }

        /// <summary>
        /// Checks if a certain accessory is equipped according to this player's AccessoryPlayer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool AccessoryActive<T>(this Player player) where T : ModItem
        {
            return player.AccessoryActive(ModContent.ItemType<T>());
        }

        /// <summary>
        /// Checks if a certain accessory is equipped according to this player's AccessoryPlayer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool AccessoryActive(this Player player, int type)
        {
            return player.GetModPlayer<AccessoryPlayer>().accessoryEffects.Contains(type);
        }

        public static void DoManaRechargeEffect(this Player player)
        {
            if (player.whoAmI == Main.myPlayer) SoundEngine.PlaySound(SoundID.MaxMana);
            for (int i = 0; i < 5; i++)
            {
                int num3 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                Main.dust[num3].noLight = true;
                Main.dust[num3].noGravity = true;
                Dust obj = Main.dust[num3];
                obj.velocity *= 0.5f;
            }
        }

        public static void DoBattleArtRechargeEffect(this Player player)
        {
            SoundStyle soundStyle = SoundID.MaxMana;
            soundStyle.Pitch -= 0.5f;
            if (player.whoAmI == Main.myPlayer) SoundEngine.PlaySound(soundStyle);
            for (int i = 0; i < 5; i++)
            {
                int num3 = Dust.NewDust(player.position, player.width, player.height, 45, 0f, 0f, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                Main.dust[num3].noLight = true;
                Main.dust[num3].noGravity = true;
                Dust obj = Main.dust[num3];
                obj.velocity *= 0.5f;
            }
        }

        public static bool TileCanBeLush(int i, int j)
        {
            if (WorldGen.TileEmpty(i + 1, j) || !Main.tileSolid[Main.tile[i + 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j) || !Main.tileSolid[Main.tile[i - 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1) || !Main.tileSolid[Main.tile[i, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1) || !Main.tileSolid[Main.tile[i, j - 1].TileType])
            {
                return true;
            }
            //else if (WorldGen.TileEmpty(i + 1, j + 1) || !Main.tileSolid[Main.tile[i + 1, j + 1].TileType])
            //{
            //    return true;
            //}
            //else if (WorldGen.TileEmpty(i + 1, j - 1) || !Main.tileSolid[Main.tile[i + 1, j - 1].TileType])
            //{
            //    return true;
            //}
            //else if (WorldGen.TileEmpty(i - 1, j + 1) || !Main.tileSolid[Main.tile[i - 1, j + 1].TileType])
            //{
            //    return true;
            //}
            //else if (WorldGen.TileEmpty(i - 1, j - 1) || !Main.tileSolid[Main.tile[i - 1, j - 1].TileType])
            //{
            //    return true;
            //}

            return false;
        }

        public static bool TileCanBeGrass(int i, int j)
        {
            if (WorldGen.TileEmpty(i + 1, j) || !Main.tileSolid[Main.tile[i + 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j) || !Main.tileSolid[Main.tile[i - 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1) || !Main.tileSolid[Main.tile[i, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1) || !Main.tileSolid[Main.tile[i, j - 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j + 1) || !Main.tileSolid[Main.tile[i + 1, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j - 1) || !Main.tileSolid[Main.tile[i + 1, j - 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j + 1) || !Main.tileSolid[Main.tile[i - 1, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j - 1) || !Main.tileSolid[Main.tile[i - 1, j - 1].TileType])
            {
                return true;
            }

            return false;
        }

        public static bool TileShouldBeSnow(int i, int j)
        {
            if (Main.tile[i, j - 1].TileType == TileID.VanityTreeSakura || Main.tile[i, j - 1].TileType == TileID.VanityTreeYellowWillow || Main.tile[i, j - 1].TileType == TileID.Sunflower)
            {
                return false;
            }
            if (WorldGen.TileEmpty(i + 1, j) || !Main.tileSolid[Main.tile[i + 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j) || !Main.tileSolid[Main.tile[i - 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1) || !Main.tileSolid[Main.tile[i, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1) || !Main.tileSolid[Main.tile[i, j - 1].TileType])
            {
                return true;
            }

            return false;
        }

        public static Vector2 FindGroundUnder(this Vector2 position)
        {
            Vector2 returned = position;
            while (!WorldUtils.Find(returned.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                returned.Y++;
            }

            return returned;
        }

        public static Vector2 FindGroundUnder(this Vector2 position, int type)
        {
            Vector2 returned = position;

            int i = 5000;
            while (Main.tile[(int)(returned.X / 16), (int)(returned.Y / 16)].TileType != type)
            {
                returned.Y++;

                i--;
                if (i <= 0)
                {
                    return Vector2.Zero;
                }
            }

            return returned;
        }

        public static Vector2 FindCeilingAbove(this Vector2 position)
        {
            Vector2 returned = position;
            while (!WorldUtils.Find(returned.ToTileCoordinates(), Searches.Chain(new Searches.Up(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                returned.Y--;
            }

            return returned;
        }

        public static Vector2 FindCeilingAbove(this Vector2 position, int type)
        {
            Vector2 returned = position;

            int i = 5000;
            while (Main.tile[(int)(returned.X / 16), (int)(returned.Y / 16)].TileType != type)
            {
                returned.Y--;

                i--;
                if (i <= 0)
                {
                    return Vector2.Zero;
                }
            }

            return returned;
        }

        public static void SetAsBattleArtItem(this Item item, BattleArt battleArt)
        {
            item.GetGlobalItem<BattleArtItem>().isBattleArtItem = true;
            item.GetGlobalItem<BattleArtItem>().battleArtItemBattleArt = battleArt;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[item.type] = 1;
            item.maxStack = 20;
        }

        public static void SetScholarlyDescription(this Item item, string description)
        {
            item.GetGlobalItem<ScholarsItem>().scholarsDescription = description;
        }

        /// <summary>
        /// Gradually aggravates this NPC. Returns true if the NPC was fully aggravated: otherwise, returns false.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="amount"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="critChance"></param>
        /// <param name="player"></param>
        /// <param name="combatText"></param>
        /// <returns></returns>
        public static bool Aggravate(this NPC npc, float amount, int damage, float knockback, int critChance, Player player, Item item, bool combatText = true, bool fromPacket = false)
        {
            if (!fromPacket && Main.netMode != NetmodeID.SinglePlayer) Wisplantern.instance.SendPacket(new EnemyAggravated(amount, npc.whoAmI, damage, knockback, critChance, player.whoAmI, combatText), toClient: -1, ignoreClient: player.whoAmI, forward: true);

            if (InfightingNPC.infightingBlacklist.Contains(npc.type) || npc.dontTakeDamage || (npc.realLife != -1 && npc.realLife != npc.whoAmI))
            {
                return false;
            }

            InfightingNPC iNPC = npc.GetGlobalNPC<InfightingNPC>();

            if (iNPC.aggravated)
            {
                return false;
            }

            amount = amount * 100 / npc.life;
            if (npc.HasBuff(BuffID.Confused))
            {
                amount *= 1.5f;
            }
            if (amount > 1f)
            {
                amount = 1f;
            }
            if (!iNPC.aggravated)
            {
                iNPC.aggravation += amount;
                CombatText.NewText(npc.getRect(), Color.MediumPurple * 0.5f, Math.Round((double)amount * 100, 1).ToString() + "%", false, false);
            }
            if (iNPC.aggravation >= 1f)
            {
                if (!iNPC.aggravated)
                {
                    CombatText.NewText(npc.getRect(), Color.Crimson, "Aggravated!", true, false);
                    SoundEngine.PlaySound(SoundID.Item113, npc.Center);

                    //Redirect projectile NPCs
                    if (NPCID.Sets.ProjectileNPC[npc.type])
                    {
                        float distance = 1000f;

                        NPC target = null;

                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC potentialTarget = Main.npc[i];
                            float dist = npc.Distance(potentialTarget.Center);

                            if (potentialTarget.active && !potentialTarget.friendly && dist < distance && potentialTarget != npc && !NPCID.Sets.ProjectileNPC[potentialTarget.type])
                            {
                                target = potentialTarget;
                                distance = dist;
                            }
                        }

                        if (target != null)
                        {
                            npc.velocity = npc.DirectionTo(target.Center) * npc.velocity.Length();
                        }
                    }
                }

                iNPC.aggravation = 1f;
                iNPC.aggravated = true;
                iNPC.infightPlayer = player.whoAmI;
                iNPC.infightCritChance = critChance;
                iNPC.infightDamage = damage;
                iNPC.infightKnockback = knockback;
                iNPC.infightItem = item;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC other = Main.npc[i];

                    if (other.active && other.realLife == npc.whoAmI)
                    {
                        InfightingNPC otherINPC = other.GetGlobalNPC<InfightingNPC>();

                        otherINPC.aggravation = 1f;
                        otherINPC.aggravated = true;
                        otherINPC.infightPlayer = player.whoAmI;
                        otherINPC.infightCritChance = critChance;
                        otherINPC.infightDamage = damage;
                        otherINPC.infightKnockback = knockback;
                        otherINPC.infightItem = item;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gradually aggravates this NPC. Returns true if the NPC was fully aggravated: otherwise, returns false.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="amount"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="critChance"></param>
        /// <param name="player"></param>
        /// <param name="combatText"></param>
        /// <returns></returns>
        public static bool Aggravate(this NPC npc, Item item, Player player)
        {
            return item.AggravateNPC(npc, player);
        }

        /// <summary>
        /// Gradually aggravates an NPC. Returns true if the NPC was fully aggravated: otherwise, returns false.
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="amount"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <param name="critChance"></param>
        /// <param name="player"></param>
        /// <param name="combatText"></param>
        /// <returns></returns>
        public static bool AggravateNPC(this Item item, NPC npc, Player player)
        {
            return npc.Aggravate(item.GetGlobalItem<AggravatingItem>().manipulativePower, player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.GetWeaponCrit(item), player, item);
        }

        public static void SetManipulativePower(this Item item, float amount)
        {
            item.GetGlobalItem<AggravatingItem>().manipulativePower = amount;
        }

        public static void SetCharisma(this Item item, int amount)
        {
            item.GetGlobalItem<AggravatingItem>().charisma = amount;
        }

        public static int EvenSimplerStrikeNPC(this NPC npc, Player player, Item item, int damage, float knockback, int hitDirection)
        {
            int dealtDamage = npc.SimpleStrikeNPC(damage, hitDirection, Main.rand.NextBool(player.GetWeaponCrit(item), 100), knockback, item.DamageType, true, player.luck);
            return dealtDamage;
        }

        public static void SmokeBomb(this Player player, int time, bool fromNet = false)
        {
            if (!fromNet && Main.netMode != NetmodeID.SinglePlayer) Wisplantern.instance.SendPacket(new SmokeBomb(player.whoAmI, time), ignoreClient: player.whoAmI, forward: true);
            player.GetModPlayer<ManipulativePlayer>().smokeBombTime = time;
            player.SmokeBombEffect();
        }

        public static void SmokeBombEffect(this Player player)
        {
            SoundStyle style = SoundID.DoubleJump;
            style.Pitch += 0.25f;
            style.Volume *= 2f;
            SoundEngine.PlaySound(style, player.Center);
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(player.position, player.width, player.height, DustID.Smoke);
                Main.dust[d].velocity += new Vector2(player.velocity.X, player.velocity.Y - Main.rand.Next(10));
            }
        }

        public static void GainCharisma(this Player player, int amount = 1)
        {
            ManipulativePlayer mPlayer = player.GetModPlayer<ManipulativePlayer>();
            mPlayer.charisma += amount;

            if (mPlayer.charisma > mPlayer.MaxCharisma)
            {
                mPlayer.charisma = mPlayer.MaxCharisma;
            }

            CombatText.NewText(player.getRect(), new Color(252, 156, 80), amount);
        }
    }
}
