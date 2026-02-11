using System;
using System.Collections.Generic;

namespace Navi.Core.Domain
{
    // Sliding puzzle state + rules (pure C#)
    public sealed class PuzzleGame
    {
        private readonly int[] _tiles; // 0 = empty

        public int Size { get; }
        public IReadOnlyList<int> Tiles => _tiles; // read-only view
        public event Action Changed;

        public PuzzleGame(int size, int[] tiles)
        {
            if (size < 2) throw new ArgumentOutOfRangeException(nameof(size));
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            if (tiles.Length != size * size) throw new ArgumentException("Invalid tile count.");

            Size = size;
            _tiles = (int[])tiles.Clone();
        }

        public bool TryMoveIndex(int index)
        {
            if (index < 0 || index >= _tiles.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            int empty = Array.IndexOf(_tiles, 0);
            if (empty < 0) throw new InvalidOperationException("No empty tile (0) found.");

            if (!IsAdjacent(index, empty)) return false;

            // swap
            (_tiles[index], _tiles[empty]) = (_tiles[empty], _tiles[index]);
            Changed?.Invoke();
            return true;
        }

        public bool IsSolved()
        {
            // 1..N-1 then 0 at end
            for (int i = 0; i < _tiles.Length - 1; i++)
                if (_tiles[i] != i + 1) return false;

            return _tiles[^1] == 0;
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
