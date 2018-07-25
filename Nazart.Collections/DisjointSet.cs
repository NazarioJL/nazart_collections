using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Nazart.Collections {
    /// <summary>
    ///     A generic implementation of a Disjoint-Set.
    /// </summary>
    /// <typeparam name="T">The generic type of the Disjoint-Set.</typeparam>
    public sealed class DisjointSet<T> : IDisjointSet<T> {
        private readonly FastDisjointSet _fastDisjointSet;
        private readonly T[] _items;
        private readonly IDictionary<T, int> _lookup;

        private DisjointSet() { }

        /// <summary>
        ///     Creates an instance of a Disjoint-Set with the given data. Each item in the
        ///     collection will be a unique set.
        /// </summary>
        /// <param name="data">The data to create the Disjoint-Set from. Each item must be unique.</param>
        public DisjointSet(IEnumerable<T> data) {
            if (data == null) throw new ArgumentNullException(nameof(data));

            _items = data.ToArray();
            _lookup = _items.Select((e, i) => (e, i)).ToImmutableDictionary(k => k.e, v => v.i);
            _fastDisjointSet = new FastDisjointSet(_items.Length);
        }

        /// <inheritdoc />
        public T Find(T item) {
            if (!_lookup.TryGetValue(item, out var index))
                throw new ArgumentException("Item is not in the set.", nameof(item));

            var parent = _fastDisjointSet.Find(index);

            return _items[parent];
        }

        /// <inheritdoc />
        public bool Union(T a, T b) {
            var (indexOfA, indexOfB) = GetIndexes(a, b);

            return _fastDisjointSet.Union(indexOfA, indexOfB);
        }

        /// <inheritdoc />
        public bool AreInSameSet(T a, T b) {
            var (indexOfA, indexOfB) = GetIndexes(a, b);

            return _fastDisjointSet.AreInSameSet(indexOfA, indexOfB);
        }


        /// <inheritdoc />
        public IEnumerable<IEnumerable<T>> GetSets() {
            // Get sets of indices from underlying disjoint set implementation
            var groupedIndices = _fastDisjointSet.GetSets();

            foreach (var group in groupedIndices) yield return group.Select(e => _items[e]);
        }

        /// Utility method to retrieve a tuple of values by indices from the lookup.
        private (int indexOfA, int indexOfB) GetIndexes(T a, T b) {
            if (!_lookup.TryGetValue(a, out var indexOfA))
                throw new ArgumentException($"Item '{b.ToString()}' is not in the set.", nameof(a));

            if (!_lookup.TryGetValue(b, out var indexOfB))
                throw new ArgumentException($"Item '{b.ToString()}' is not in the set.", nameof(b));

            return (indexOfA, indexOfB);
        }
    }
}
