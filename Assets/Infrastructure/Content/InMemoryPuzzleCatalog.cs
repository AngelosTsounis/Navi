using System;
using System.Collections.Generic;
using Navi.Core.Domain;
using Navi.Core.Interfaces;

namespace Navi.Infrastructure.Content
{
    public sealed class InMemoryPuzzleCatalog : IPuzzleCatalog
    {
        private readonly Dictionary<PuzzleId, PuzzleDefinition> _defs;

        public InMemoryPuzzleCatalog()
        {
            // Start with just the tutorial puzzle. Add more later.
            var tutorial = new PuzzleDefinition(
                id: new PuzzleId("tutorial_3x3"),
                size: 3,
                rewardRupees: 10,
                shuffleMoves: 30,
                spriteSetKey: "tutorial_3x3" // must match whatever you use in UI to pick sprites
            );

            _defs = new Dictionary<PuzzleId, PuzzleDefinition>
            {
                { tutorial.Id, tutorial }
            };
        }

        public PuzzleDefinition Get(PuzzleId id)
        {
            if (_defs.TryGetValue(id, out var def)) return def;
            throw new InvalidOperationException($"PuzzleDefinition not found: {id.Value}");
        }
    }
}
