using System;
using System.Collections.Generic;

namespace Navi.Core.Domain
{
    public sealed class PuzzleFactory
    {
        private readonly Random _rng = new Random();

        public PuzzleGame CreateTutorial3x3()
        {
            // Start SOLVED then shuffle by valid moves (always solvable)
            var game = new PuzzleGame(3, CreateSolvedTiles(3));

            ShuffleByValidMoves(game, shuffleMoves: 30);

            // Very rare: can end up solved again. If so, shuffle a bit more.
            if (game.IsSolved())
                ShuffleByValidMoves(game, shuffleMoves: 10);

            return game;
        }

        private static int[] CreateSolvedTiles(int size)
        {
            int n = size * size;
            var tiles = new int[n];
            for (int i = 0; i < n - 1; i++) tiles[i] = i + 1;
            tiles[n - 1] = 0;
            return tiles;
        }

        private void ShuffleByValidMoves(PuzzleGame game, int shuffleMoves)
        {
            int size = game.Size;
            int lastEmpty = -1;

            for (int i = 0; i < shuffleMoves; i++)
            {
                int empty = game.EmptyIndex;
                var neighbors = GetNeighborIndices(empty, size);

                // avoid immediate undo (optional, but makes shuffle better)
                if (lastEmpty != -1 && neighbors.Count > 1)
                    neighbors.Remove(lastEmpty);

                int pick = neighbors[_rng.Next(neighbors.Count)];
                game.TryMoveIndex(pick);

                lastEmpty = empty;
            }
        }

        private static List<int> GetNeighborIndices(int index, int size)
        {
            int x = index % size;
            int y = index / size;

            var result = new List<int>(4);
            if (x > 0) result.Add(index - 1);
            if (x < size - 1) result.Add(index + 1);
            if (y > 0) result.Add(index - size);
            if (y < size - 1) result.Add(index + size);

            return result;
        }
    }
}
