using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Nazart.Collections {
    /// <summary>
    /// Implementation of a Disjoint Set data structure that uses the relative
    /// ordinal values of a set of a specific size. 
    /// </summary>
    public sealed class FastDisjointSet : IDisjointSet<int> {
        private readonly int[] _parents;
        private readonly int[] _setSize;

        /// <summary>
        /// The cardinality of the set.
        /// </summary>
        public int N { get; private set; }

        private FastDisjointSet() { }

        /// <summary>
        /// Creates a new Disjoint-Set with a specific cardinality.
        /// </summary>
        /// <param name="n">The size of the set.</param>
        public FastDisjointSet(int n) {
            if (n < 1) {
                throw new ArgumentException("The set must be created with a positive integer.", nameof(n));
            }

            this.N = n;
            _parents = new int[this.N];
            _setSize = new int[this.N];

            for (var i = 0; i < N; i++) {
                _parents[i] = i;
                _setSize[i] = 1;
            }
        }

        /// <inheritdoc />
        public int Find(int item) {
            if (item < 0 || item >= this.N) {
                throw new ArgumentException("The ordinal value of the item is not within the set.", nameof(item));
            }

            var parent = item;
            while (parent != _parents[parent]) {
                var current = parent;
                parent = _parents[parent];
                // Path splitting, make 
                _parents[current] = _parents[parent];
            }

            // Make the item's parent the top level parent, this makes flatter trees
            _parents[item] = parent;

            return parent;
        }

        /// <inheritdoc />
        public bool Union(int a, int b) {
            // Find parents of both items
            var parentOfA = Find(a);
            var parentOfB = Find(b);

            if (parentOfA == parentOfB) {
                // Items are in the same set
                // TODO: Consider throwing if method should not be idempotent, or return
                return false;
            }

            var setSizeA = _setSize[parentOfA];
            var setSizeB = _setSize[parentOfB];

            int child, parent;

            // Make the smaller set a child of the parent of the bigger set
            if (setSizeA > setSizeB) {
                child = parentOfB;
                parent = parentOfA;
            }
            else {
                child = parentOfA;
                parent = parentOfB;
            }

            // Set child to point to parent and adjust respective sizes
            _parents[child] = parent;
            _setSize[parent] += _setSize[child];
            _setSize[child] = 0;

            // We can also make original items point to the new parent of both
            _parents[a] = parent;
            _parents[b] = parent;

            return true;
        }

        /// <inheritdoc />
        public bool AreInSameSet(int a, int b) {
            return Find(a) == Find(b);
        }

        /// <inheritdoc />
        public IEnumerable<IEnumerable<int>> GetSets() {
            var sets = Enumerable.Range(0, _parents.Length).GroupBy(Find, e => e);
            return sets;
        }
    }
}
