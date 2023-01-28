using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Wisplantern.ID;
using Wisplantern.BattleArts;
using Microsoft.Xna.Framework;

namespace Wisplantern
{
    public abstract class BattleArt
    {
        public virtual int ItemType => ItemID.None;

        /// <summary>
        /// Whether or not this battle art can be applied to x item.
        /// Note that overriding this method makes whatever you put for battleArtType irrelevant as it overrides that logic.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool CanBeAppliedToItem(Item item)
        {
            if (item.damage == 0 || item.accessory || item.maxStack > 1)
            {
                return false;
            }
            return BattleArtType switch
            {
                BattleArtType.Sword => item.useStyle == ItemUseStyleID.Swing && item.pick == 0 && item.axe == 0 && item.hammer == 0 && item.DamageType == DamageClass.Melee,
                BattleArtType.Axe => item.useStyle == ItemUseStyleID.Swing && item.axe != 0,
                BattleArtType.Pick => item.useStyle == ItemUseStyleID.Swing && item.pick != 0,
                BattleArtType.Hammer => item.useStyle == ItemUseStyleID.Swing && item.pick != 0,
                BattleArtType.BowAndRepeater => item.DamageType == DamageClass.Ranged && item.useAmmo == AmmoID.Arrow,
                _ => true,
            };
        }

        /// <summary>
        /// The tooltip for what the battle art can be applied to. For example, "swords".
        /// Don't override this if not also overriding CanBeAppliedToItem(Item item).
        /// </summary>
        /// <returns></returns>
        public virtual string BattleArtApplicabilityDescription()
        {
            return BattleArtType switch
            {
                BattleArtType.Sword => "swords",
                BattleArtType.Axe => "axes",
                BattleArtType.Pick => "pickaxes",
                BattleArtType.Hammer => "hammers",
                BattleArtType.BowAndRepeater => "bows and repeaters",
                _ => "anything",
            };
        }

        /// <summary>
        /// Determines the logic for what weapons this can be applied to.
        /// Does nothing if you override CanBeAppliedToItem(Item item).
        /// </summary>
        public virtual BattleArtType BattleArtType => BattleArtType.Any;

        /// <summary>
        /// The name of the battle art.
        /// </summary>
        public virtual string BattleArtName => "Unnamed";

        /// <summary>
        /// What the battle art actually does when used.
        /// </summary>
        public virtual string BattleArtDescription => "No description entered.";

        /// <summary>
        /// Called before using the battle art. Use this to change item use styles or stats temporarily. Changes to stats made here will be automatically reverted after the battle art is done being used.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual void PreUseBattleArt(ref Item item, Player player)
        {

        }

        /// <summary>
        /// Called every frame that the battle art is being used.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual void UseBattleArt(Item item, Player player, bool firstFrame)
        {

        }

        /// <summary>
        /// Does the same thing as ModItem.UseStyle when the battle art is being used.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        public virtual void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {

        }

        /// <summary>
        /// Called the frame after the battle art's use ends. Use this to reset anything that was not automatically reset from PreUseBattleArt.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <param name="firstFrame"></param>
        public virtual void PostUseBattleArt(Item item, Player player)
        {

        }

        public virtual void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

        }

        /// <summary>
        /// Called in a ModPlayer's PostUpdate. Useful for changing things that have to be changed late in a frame.
        /// </summary>
        /// <param name="player"></param>
        public virtual void PostUpdatePlayer(Player player)
        {

        }

        /// <summary>
        /// Called in an item's OnHitNPC when the battle art is active.
        /// </summary>
        /// <param name="player"></param>
        public virtual void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {

        }

        /// <summary>
        /// Allows you to modify melee hitboxes while the battle art is being used.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <param name="hitbox"></param>
        /// <param name="noHitbox"></param>
        public virtual void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool? CanHitNPC(Item item, Player player, NPC target)
        {
            return null;
        }

        /// <summary>
        /// The BattleArtID of the battle art. Unfortunately, this is needed. Extremely tragic.
        /// </summary>
        public virtual int ID => BattleArtID.None;

        /// <summary>
        /// The displayed color of the battle art.
        /// </summary>
        public virtual Color Color => Color.Red;
    }

    public enum BattleArtType : byte
    {
        Any = 0,
        Sword = 1,
        Axe = 2,
        Pick = 3,
        Hammer = 4,
        BowAndRepeater = 5,
    }
}
