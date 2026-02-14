using Navi.Core.Domain;

namespace Navi.Core.Interfaces
{
    public interface IPuzzleCatalog
    {
        PuzzleDefinition Get(PuzzleId id);
    }
}
