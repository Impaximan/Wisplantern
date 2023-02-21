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
                BloodySlash => new BloodySlash(),
                Siphon => new Siphon(),
                RadialCast => new RadialCast(),
                ExtendedSmokeBomb => new ExtendedSmokeBomb(),
                FinishOff => new FinishOff(),
                _ => new None()
            };
        }

        public const int None = 0;
        public const int Parry = 1;
        public const int Uppercut = 2;
        public const int AerialRetreat = 3;
        public const int TriCast = 4;
        public const int BloodySlash = 5;
        public const int Siphon = 6;
        public const int RadialCast = 7;
        public const int ExtendedSmokeBomb = 8;
        public const int FinishOff = 9;
    }
}
