using Wisplantern.BattleArts;

namespace Wisplantern.ID
{
    public static class BattleArtID
    {
        public static BattleArt GetBattleArtFromID(int ID)
        {
            return ID switch
            {
                None => new None(),
                Parry => new Parry(),
                Uppercut => new Uppercut(),
                AerialRetreat => new AerialRetreat(),
                TriCast => new TriCast(),
                _ => new None()
            };
        }

        public const int None = 0;
        public const int Parry = 1;
        public const int Uppercut = 2;
        public const int AerialRetreat = 3;
        public const int TriCast = 4;
    }
}
