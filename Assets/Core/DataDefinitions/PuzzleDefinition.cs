using UnityEngine;

namespace Navi.Core.Domain
{
    public sealed class PuzzleDefinition
    {
        public PuzzleId Id { get; }
        public int Size { get; }
        public int RewardRupees { get; }
        public int ShuffleMoves { get; }
        public Sprite[] PieceSprites { get; }

        public PuzzleDefinition(PuzzleId id, int size, int rewardRupees, int shuffleMoves, Sprite[] pieceSprites)
        {
            Id = id;
            Size = size;
            RewardRupees = rewardRupees;
            ShuffleMoves = shuffleMoves;
            PieceSprites = pieceSprites;
        }
    }
}
