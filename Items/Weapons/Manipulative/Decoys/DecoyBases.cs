﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Wisplantern.Globals.GNPCs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Wisplantern.Items.Weapons.Manipulative.Decoys
{
    abstract class DecoyItem : ModItem
    {
        /// <summary>
        /// The type of NPC this will spawn.
        /// </summary>
        public virtual int DecoyType => NPCID.None;

        /// <summary>
        /// The decoy cooldown in seconds caused by the item.
        /// </summary>
        public virtual int CooldownSeconds => 30;

        /// <summary>
        /// The default max HP of the decoy.
        /// </summary>
        public virtual int DefaultHP => 10;

        /// <summary>
        /// The name of the decoy.
        /// </summary>
        public virtual string DecoyName => "YELL AT THE MOD DEV FOR NOT PUTTING THE DECOY NAME LMAO";

        /// <summary>
        /// The standard SetDefaults, DON'T USE THIS FOR DECOY ITEMS. Use SetStats instead.
        /// </summary>
        public override void SetDefaults()
        {
            Item.shoot = 1;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.noMelee = true;
            Item.damage = 1;
            SetStats();
        }

        public override bool? CanHitNPC(Player player, NPC target)
        {
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        /// <summary>
        /// Use this instead of SetDefaults. Don't set Item.shoot;
        /// </summary>
        public virtual void SetStats()
        {

        }

        /// <summary>
        /// When overriding for decoy items, use base.CanUseItem(player) instead of true.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.GetGlobalNPC<InfightingNPC>().decoy)
                    {
                        npc.life = 1;
                        npc.StrikeNPC(npc.lifeMax, 0f, 0, true);
                    }
                }
                Item.stack++;
                return true;
            }
            if (player.HasBuff<Buffs.DecoyCooldown>())
            {
                return false;
            }
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.GetGlobalNPC<InfightingNPC>().decoy)
                {
                    return false;
                }
            }
            return base.CanUseItem(player);
        }

        /// <summary>
        /// Don't override this for decoy items unless you want to have custom spawning behavior.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        /// <returns></returns>
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                player.SmokeBomb(120);
                player.AddBuff(ModContent.BuffType<Buffs.DecoyCooldown>(), CooldownSeconds * 60);
                int n = NPC.NewNPC(source, (int)player.Center.X, (int)player.Center.Y, DecoyType);
                Main.npc[n].position.Y = player.position.Y + player.height - Main.npc[n].height;
                Main.npc[n].ai[0] = player.whoAmI;
                Main.npc[n].ai[1] = player.GetWeaponCrit(Item);
                Main.npc[n].damage = damage;
                Main.npc[n].lifeMax = DefaultHP;
                Main.npc[n].life = Main.npc[n].lifeMax;
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage");
            if (index != -1)
            {
                tooltips.Insert(index + 1, new TooltipLine(Mod, "DecoyCooldown", CooldownSeconds + " second cooldown"));
            }
            index = tooltips.FindIndex(x => x.Name == "Consumable");
            if (index != -1)
            {
                tooltips.Insert(index + 1, new TooltipLine(Mod, "DecoyLife", "Creates a " + DecoyName + " decoy with " + DefaultHP + " life" +
                    "\nOnly one decoy can be active at once"));
            }
            else
            {
                index = tooltips.FindIndex(x => x.Name == "CritChance");
                if (index != -1)
                {
                    tooltips.Insert(index + 1, new TooltipLine(Mod, "DecoyLife", "Creates a " + DecoyName + " decoy with " + DefaultHP + " life" +
                        "\nOnly one decoy can be active at once"));
                }
            }

            tooltips.RemoveAll(x => x.Name == "Knockback");
            tooltips.RemoveAll(x => x.Name == "Speed");
            if (Item.damage == 1)
            {
                tooltips.RemoveAll(x => x.Name == "Damage");
                tooltips.RemoveAll(x => x.Name == "CritChance");
            }

            TooltipLine rightClickLine = new TooltipLine(Mod, "decoyRightClick", "Right click to kill all decoys insantly");
            rightClickLine.IsModifier = true;
            tooltips.Add(rightClickLine);
        }
    }
    abstract class DecoyNPC : ModNPC
    {
        /// <summary>
        /// The standard SetDefaults, DON'T USE THIS FOR DECOYS. Use SetStats instead.
        /// </summary>
        public override void SetDefaults()
        {
            NPC.GetGlobalNPC<InfightingNPC>().decoy = true;
            NPC.friendly = true;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            SetStats();
        }

        public virtual void SetStats()
        {

        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return null;
        }

        public virtual bool DoContactDamage => true;

        public override bool CheckActive()
        {
            return false;
        }

        /// <summary>
        /// Don't override this for DecoyNPCs.
        /// </summary>
        public override void PostAI()
        {
            if (DoContactDamage)
            {
                foreach (NPC target in Main.npc)
                {
                    if (target.active && target.whoAmI != NPC.whoAmI && target.Hitbox.Intersects(NPC.Hitbox) && target.GetGlobalNPC<InfightingNPC>().infightIframes <= 0 && !target.friendly)
                    {
                        int damage = Main.DamageVar(NPC.damage, Main.player[(int)NPC.ai[0]].luck);
                        int struckDamage = (int)target.StrikeNPC(damage, 0f, Math.Sign(target.Center.X - NPC.Center.X), Main.rand.NextBool((int)NPC.ai[1], 100));
                        Main.player[(int)NPC.ai[0]].addDPS(struckDamage);
                        target.GetGlobalNPC<InfightingNPC>().infightIframes = 10;
                    }
                }
            }

            if (NPC.Distance(Main.player[(int)NPC.ai[0]].Center) > 2500f)
            {
                NPC.active = false;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool SpecialOnKill()
        {
            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.RemoveWhere(x => true, true);
        }
    }
}