using System;

namespace Navi.Core.Domain
{
    // Sliding puzzle state + rules (pure C#)
    public sealed class PuzzleGame
    {
        public int Size { get; }
        public int[] Tiles { get; } // 0 = empty
        public event Action Changed;

        public PuzzleGame(int size, int[] tiles)
        {
            if (size < 2) throw new ArgumentOutOfRangeException(nameof(size));
            if (tiles.Length != size * size) throw new ArgumentException("Invalid tile count.");

            Size = size;
            Tiles = (int[])tiles.Clone();
        }

        public bool TryMoveIndex(int index)
        {
            int empty = Array.IndexOf(Tiles, 0);
            if (empty < 0) throw new InvalidOperationException("No empty tile (0) found.");

            if (!IsAdjacent(index, empty)) return false;

            // swap
            (Tiles[index], Tiles[empty]) = (Tiles[empty], Tiles[index]);
            Changed?.Invoke();
            return true;
        }

        public bool IsSolved()
        {
            // 1..N-1 then 0 at end
            for (int i = 0; i < Tiles.Length - 1; i++)
                if (Tiles[i] != i + 1) return false;

            return Tiles[^1] == 0;
        }

        private bool IsAdjacent(int a, int b)
        {
            int ax = a % Size; int ay = a / Size;
            int bx = b % Size; int by = b / Size;
            int dx = Math.Abs(ax - bx);
            int dy = Math.Abs(ay - by);
            return (dx + dy) == 1;
        }
    }
}
