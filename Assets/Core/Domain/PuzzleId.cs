using System;

namespace Navi.Core.Domain
{
    public readonly struct PuzzleId : IEquatable<PuzzleId>
    {
        public string Value { get; }

        public PuzzleId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("PuzzleId cannot be null/empty.", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;

        public bool Equals(PuzzleId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object obj) => obj is PuzzleId other && Equals(other);

        public override int GetHashCode() => Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;

        public static bool operator ==(PuzzleId left, PuzzleId right) => left.Equals(right);
        public static bool operator !=(PuzzleId left, PuzzleId right) => !left.Equals(right);
    }
}