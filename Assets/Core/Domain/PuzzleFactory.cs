namespace Navi.Core.Domain
{
    public sealed class PuzzleFactory
    {
        public PuzzleGame CreateTutorial3x3()
        {
            // One move away from solved (simple for first run)
            // 1 2 3
            // 4 5 6
            // 7 0 8  -> move 8 left to solve
            return new PuzzleGame(3, new[] { 1, 2, 3, 4, 5, 6, 7, 0, 8 });
        }
    }
}
