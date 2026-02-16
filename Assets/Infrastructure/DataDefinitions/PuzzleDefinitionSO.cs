using Navi.Core.Domain;
using UnityEngine;

namespace Navi.Infrastructure.DataDefinitions

{
    [CreateAssetMenu(menuName = "Navi/Puzzle Definition", fileName = "PuzzleDefinition")]
    public sealed class PuzzleDefinitionSO : ScriptableObject
    {
        [Header("Id")]
        [SerializeField] private string id = "tutorial_3x3";

        [Header("Rules")]
        [SerializeField] private int size = 3;
        [SerializeField] private int shuffleMoves = 30;

        [Header("Rewards")]
        [SerializeField] private int rewardRupees = 10;

        [Header("Sprites (SOLVED order, length = size*size)")]
        [SerializeField] private Sprite[] pieceSprites;

        public PuzzleId Id => new PuzzleId(id);
        public int Size => size;
        public int ShuffleMoves => shuffleMoves;
        public int RewardRupees => rewardRupees;
        public Sprite[] PieceSprites => pieceSprites;
    }
}
