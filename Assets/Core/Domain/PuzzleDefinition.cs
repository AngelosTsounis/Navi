namespace Navi.Core.Domain
{
    public sealed class PuzzleDefinition
    {
        public PuzzleId Id { get; }
        public int Size { get; }
        public int RewardRupees { get; }
        public int ShuffleMoves { get; }
        public string SpriteSetKey { get; } 

        public PuzzleDefinition(PuzzleId id, int size, int rewardRupees, int shuffleMoves, string spriteSetKey)
        {
            Id = id;
            Size = size;
            RewardRupees = rewardRupees;
            ShuffleMoves = shuffleMoves;
            SpriteSetKey = spriteSetKey;
        }
    }
}
