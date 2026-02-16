using Navi.Core.Domain;
using Navi.Core.Interfaces;

namespace Navi.Infrastructure.DataDefinitions
{
    public sealed class PuzzleCatalog : IPuzzleCatalog
    {
        private readonly PuzzleCatalogSO _catalog;

        public PuzzleCatalog(PuzzleCatalogSO catalog)
        {
            _catalog = catalog;
        }

        public PuzzleDefinition Get(PuzzleId id)
        {
            var so = _catalog.Find(id);

            // Convert to your runtime definition type
            return new PuzzleDefinition(
                id: so.Id,
                size: so.Size,
                rewardRupees: so.RewardRupees,
                shuffleMoves: so.ShuffleMoves,
                pieceSprites: so.PieceSprites
            );
        }
    }
}
