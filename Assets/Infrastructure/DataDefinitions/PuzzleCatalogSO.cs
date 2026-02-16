using Navi.Core.Domain;
using System;
using UnityEngine;

namespace Navi.Infrastructure.DataDefinitions

{
    [CreateAssetMenu(menuName = "Navi/Puzzle Catalog", fileName = "PuzzleCatalog")]
    public sealed class PuzzleCatalogSO : ScriptableObject
    {
        [SerializeField] private PuzzleDefinitionSO[] puzzles;

        public PuzzleDefinitionSO[] Puzzles => puzzles;

        public PuzzleDefinitionSO Find(PuzzleId id)
        {
            foreach (var p in puzzles)
            {
                if (p != null && p.Id == id)
                    return p;
            }

            throw new InvalidOperationException($"Puzzle not found: {id.Value}");
        }
    }
}
