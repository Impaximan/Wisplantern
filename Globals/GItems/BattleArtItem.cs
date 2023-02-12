using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Wisplantern.BattleArts;
using Wisplantern.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.DataStructures;
using Terraria.Audio;
using System.IO;
using Terraria.IO;
using Terraria.Localization;

namespace Wisplantern.Globals.GItems
{
    class BattleArtItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public BattleArt battleArt;
        public bool isBattleArtItem = false;
        public BattleArt battleArtItemBattleArt;

        public override void NetSend(Item item, BinaryWriter writer)
        {
            if (battleArt != null && battleArt is not None)
            {
                writer.Write(battleArt.ID);
                writer.Write(multiplayerShouldApplyBattleArt ? 1 : 0);
            }
            else
            {
                writer.Write(0);
                writer.Write(0);
            }
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            int battleArtID = reader.ReadInt32();
            if (battleArtID != 0)
            {
                battleArt = BattleArtID.GetBattleArtFromID(battleArtID);
            }
            multiplayerShouldApplyBattleArt = reader.ReadInt32() == 1;
        }

        public override void OnCreate(Item item, ItemCreationContext context)
        {

        }

        public override GlobalItem NewInstance(Item target)
        {
            battleArt ??= new None();
            return base.NewInstance(target);
        }

        const int syncInterval = 10;
        int syncTimer = syncInterval;
        public override void HoldItem(Item item, Player player)
        {
            syncTimer++;
            if (syncTimer >= syncInterval)
            {
                syncTimer = 0;
                if (battleArt != null && battleArt is not None && player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < player.inventory.Length; i++)
                    {
                        if (player.inventory[i] == item)
                        {
                            NetMessage.SendData(MessageID.SyncEquipment, ignoreClient: player.whoAmI, number: player.whoAmI, number2: i);  // syncs the i slot in the player's inventory
                            break;
                        }
                    }
                }
            }

            if ((!player.ItemAnimationActive || player.altFunctionUse != 2) && wasUsingBattleArt)
            {
                if (battleArt != null)
                {
                    battleArt.PostUseBattleArt(item, player);
                }
                item.damage = ogDamage;
                item.knockBack = ogKnockback;
                item.shootSpeed = ogShootSpeed;
                item.useTime = ogUseTime;
                item.useStyle = ogUseStyle;
                item.useAnimation = ogUseAnimation;
                item.mana = ogMana;
                item.shoot = ogShoot;
                item.noMelee = ogNoMelee;
                item.UseSound = ogSoundStyle;
                if (player.altFunctionUse != 2 && player.ItemTimeIsZero)
                {
                    wasUsingBattleArt = false;
                }
                player.GetModPlayer<BattleArtPlayer>().usingBattleArt = false;
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if ((!player.ItemAnimationActive || player.altFunctionUse != 2) && wasUsingBattleArt)
            {
                if (battleArt != null)
                {
                    battleArt.PostUseBattleArt(item, player);
                }
                item.damage = ogDamage;
                item.knockBack = ogKnockback;
                item.shootSpeed = ogShootSpeed;
                item.useTime = ogUseTime;
                item.useStyle = ogUseStyle;
                item.useAnimation = ogUseAnimation;
                item.mana = ogMana;
                item.shoot = ogShoot;
                item.noMelee = ogNoMelee;
                item.UseSound = ogSoundStyle;
                if (player.altFunctionUse != 2 && player.ItemTimeIsZero)
                {
                    wasUsingBattleArt = false;
                }
                player.GetModPlayer<BattleArtPlayer>().usingBattleArt = false;
            }
        }

        public override void SaveData(Item item, TagCompound tag)
        {
            tag.Add("BattleArt", battleArt == null ? BattleArtID.None : battleArt.ID);
        }

        public override void LoadData(Item item, TagCompound tag)
        {
            battleArt = BattleArtID.GetBattleArtFromID(tag.GetInt("BattleArt"));
        }

        List<int> blacklist = new()
        {
            ItemID.BookStaff,
            ItemID.MonkStaffT3,
            ItemID.DD2SquireDemonSword,
            ItemID.BouncingShield
        };

        public bool CanGetBattleArt(Item item, Player player)
        {
            if (item.GetGlobalItem<BattleArtItem>().battleArt != null && item.GetGlobalItem<BattleArtItem>().battleArt is not None)
            {
                return true;
            }
            if (item.ModItem != null)
            {
                if (item.ModItem.AltFunctionUse(player))
                {
                    return false;
                }
            }
            if (ItemLoader.AltFunctionUse(item, player))
            {
                return false;
            }
            if (blacklist.Contains(item.type))
            {
                return false;
            }
            if (item.channel)
            {
                return false;
            }
            return true;
        }

        bool multiplayerShouldApplyBattleArt = false;
        bool ShouldApplyBattleArt(Player player)
        {
            if (player.whoAmI != Main.myPlayer && multiplayerShouldApplyBattleArt)
            {
                return true;
            }
            multiplayerShouldApplyBattleArt = battleArt is not None && battleArt != null && player.altFunctionUse == 2;
            return multiplayerShouldApplyBattleArt;
        }

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (ShouldApplyBattleArt(player))
            {
                battleArt.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
            }
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (ShouldApplyBattleArt(player))
            {
                return battleArt.Shoot(item, player, source, position, velocity, type, damage, knockback);
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        //Saved stats
        int ogDamage;
        float ogShootSpeed;
        int ogUseAnimation;
        int ogUseTime;
        int ogUseStyle;
        int ogMana;
        float ogKnockback;
        int ogShoot;
        bool ogNoMelee;
        SoundStyle? ogSoundStyle;

        bool wasJustUsed = false;
        bool wasUsingBattleArt = false;

        public override bool CanUseItem(Item item, Player player)
        {
            if (ShouldApplyBattleArt(player) && player.statMana >= item.mana)
            {
                Item dummy = new Item(item.type);
                ogDamage = dummy.damage;
                ogShootSpeed = dummy.shootSpeed;
                ogUseAnimation = dummy.useAnimation;
                ogUseTime = dummy.useTime;
                ogUseStyle = dummy.useStyle;
                ogMana = dummy.mana;
                ogKnockback = dummy.knockBack;
                ogShoot = dummy.shoot;
                ogNoMelee = dummy.noMelee;
                ogSoundStyle = dummy.UseSound;
                wasJustUsed = true;

                if (player.HasBuff<Buffs.BattleArtCooldown>())
                {
                    return false;
                }

                wasUsingBattleArt = true;
                battleArt.PreUseBattleArt(ref item, player);

                player.GetModPlayer<BattleArtPlayer>().usingBattleArt = true;
            }

            return base.CanUseItem(item, player);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (ShouldApplyBattleArt(player))
            {
                battleArt.OnHitNPC(item, player, target, damage, knockBack, crit);
            }
        }

        public override bool? CanHitNPC(Item item, Player player, NPC target)
        {
            if (ShouldApplyBattleArt(player))
            {
                return battleArt.CanHitNPC(item, player, target);
            }
            return base.CanHitNPC(item, player, target);
        }

        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if (ShouldApplyBattleArt(player))
            {
                battleArt.UseItemHitbox(item, player, ref hitbox, ref noHitbox);
            }
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            if (ShouldApplyBattleArt(player))
            {
                battleArt.UseStyle(item, player, heldItemFrame);
                NetMessage.SendData(MessageID.SyncPlayer, player.whoAmI);
            }
        }

        public override bool? UseItem(Item item, Player player)
        {
            //if (battleArt != null && battleArt is not None && player.whoAmI == Main.myPlayer)
            //{
            //    NetMessage.SendData(MessageID.SyncPlayer, player.whoAmI);
            //    NetMessage.SendData(MessageID.PlayerControls, player.whoAmI);
            //}
            if (ShouldApplyBattleArt(player) && player.whoAmI == Main.myPlayer)
            {
                NetMessage.SendData(MessageID.SyncPlayer, player.whoAmI);
                battleArt.UseBattleArt(item, player, wasJustUsed);
                wasUsingBattleArt = true;
                if (wasJustUsed) wasJustUsed = false;
            }
            return base.UseItem(item, player);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            if (multiplayerShouldApplyBattleArt)
            {
                return true;
            }
            if (battleArt != null && battleArt is not None)
            {
                return true;
            }
            return base.AltFunctionUse(item, player);
        }

        public override bool CanRightClick(Item item)
        {
            if (isBattleArtItem)
            {
                Player player = Main.player[Main.myPlayer];
                return battleArtItemBattleArt.CanBeAppliedToItem(player.HeldItem) && CanGetBattleArt(player.HeldItem, player);
            }
            return base.CanRightClick(item);
        }

        public override void RightClick(Item item, Player player)
        {
            if (isBattleArtItem)
            {
                if (battleArtItemBattleArt.CanBeAppliedToItem(player.HeldItem))
                {
                    if (Main.mouseItem != null && Main.mouseItem.type != ItemID.None && !Main.mouseItem.IsAir)
                    {
                        if (Main.mouseItem.GetGlobalItem<BattleArtItem>().battleArt != null && Main.mouseItem.GetGlobalItem<BattleArtItem>().battleArt is not None)
                        {
                            player.QuickSpawnItem(new EntitySource_Misc("BattleArtReplacement"), Main.mouseItem.GetGlobalItem<BattleArtItem>().battleArt.ItemType);
                        }
                        SoundStyle sound = new SoundStyle("Wisplantern/Sounds/Effects/Enchant" + Main.rand.Next(1, 4));
                        sound.Volume = 2f;
                        SoundEngine.PlaySound(sound);
                        Main.mouseItem.GetGlobalItem<BattleArtItem>().battleArt = battleArtItemBattleArt;
                    }
                    else
                    {
                        if (player.HeldItem.GetGlobalItem<BattleArtItem>().battleArt != null && player.HeldItem.GetGlobalItem<BattleArtItem>().battleArt is not None)
                        {
                            player.QuickSpawnItem(new EntitySource_Misc("BattleArtReplacement"), player.HeldItem.GetGlobalItem<BattleArtItem>().battleArt.ItemType);
                        }
                        SoundStyle sound = new SoundStyle("Wisplantern/Sounds/Effects/Enchant" + Main.rand.Next(1, 4));
                        sound.Volume = 2f;
                        SoundEngine.PlaySound(sound);
                        player.HeldItem.GetGlobalItem<BattleArtItem>().battleArt = battleArtItemBattleArt;
                    }
                    NetMessage.SendData(MessageID.SyncPlayer, number: player.whoAmI);
                    //NetMessage.SendData(MessageID.SyncItem, number: item.netID);
                }
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (battleArt != null && battleArt is not None)
            {
                TooltipLine nameLine = new TooltipLine(Mod, "BattleArtName", battleArt.BattleArtName)
                {
                    OverrideColor = battleArt.Color
                };
                tooltips.Insert(1, nameLine);

                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    TooltipLine descLine = new TooltipLine(Mod, "BattleArtDesc", battleArt.BattleArtDescription);
                    descLine.OverrideColor = new Color(255, 255, 200);
                    tooltips.Add(descLine);
                }
                else
                {
                    TooltipLine holdLAlt = new TooltipLine(Mod, "HoldLAlt", "Hold left alt to see battle art description");
                    holdLAlt.IsModifier = true;
                    tooltips.Add(holdLAlt);
                }
            }

            if (isBattleArtItem)
            {
                tooltips.Find(x => x.Name == "ItemName" && x.Mod == "Terraria").OverrideColor = battleArtItemBattleArt.Color;

                TooltipLine line1 = new(Mod, "BattleArt1", "-Battle Art-")
                {
                    OverrideColor = new Color(255, 255, 200)
                };

                TooltipLine line2 = new (Mod, "BattleArt2", "Can be used on " + battleArtItemBattleArt.BattleArtApplicabilityDescription());

                TooltipLine line3 = new (Mod, "BattleArt3", "Gives a weapon the following effect: ");

                TooltipLine line4 = new(Mod, "BattleArt4", battleArtItemBattleArt.BattleArtDescription)
                {
                    OverrideColor = Color.Lerp(battleArtItemBattleArt.Color, Color.White, 0.5f)
                };

                TooltipLine line5 = new (Mod, "BattleArt5", "Right click on this in your inventory while holding an applicable weapon to apply");

                tooltips.Add(line1);
                tooltips.Add(line2);
                tooltips.Add(line3);
                tooltips.Add(line4);
                tooltips.Add(line5);
            }

            if (!CanGetBattleArt(item, Main.player[Main.myPlayer]))
            {
                TooltipLine noBattleArtLine = new TooltipLine(Mod, "NoBattleArt", "Cannot be given a battle art");
                noBattleArtLine.IsModifier = true;
                noBattleArtLine.IsModifierBad = true;
                tooltips.Add(noBattleArtLine);
            }
        }
    }

    class BattleArtPlayer : ModPlayer
    {
        public bool usingBattleArt = false;

        public override void ResetEffects()
        {
            usingBattleArt = false;
        }

        public override void PostUpdate()
        {
            if (Player.ItemAnimationActive && Player.HeldItem.GetGlobalItem<BattleArtItem>().battleArt != null && Player.HeldItem.GetGlobalItem<BattleArtItem>().battleArt is not None && Player.altFunctionUse == 2)
            {
                Player.HeldItem.GetGlobalItem<BattleArtItem>().battleArt.PostUpdatePlayer(Player);
            }
        }
    }
}
