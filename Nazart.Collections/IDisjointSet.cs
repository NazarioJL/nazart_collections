using System.Collections.Generic;

namespace Nazart.Collections {
    /// <summary>
    ///     Interface of a Disjoint-Set data structure.
    ///     <see href="https://en.wikipedia.org/wiki/Disjoint-set_data_structure" />
    /// </summary>
    /// <typeparam name="T">The generic type of the Disjoint-Set.</typeparam>
    public interface IDisjointSet<T> {
        /// <summary>
        ///     Finds the root element of the set that the argument belongs to.
        /// </summary>
        /// <param name="item">The item to find the root parent.</param>
        /// <returns>The representative member to which the item belongs to, may be the item itself.</returns>
        T Find(T item);

        /// <summary>
        ///     Merges the two items to the same set.
        /// </summary>
        /// <param name="a">One of the items to merge into the set.</param>
        /// <param name="b">One of the items to merge into the set.</param>
        /// <returns><see langword="true" /> if items were not in the same set, <see langword="false" /> otherwise.</returns>
        bool Union(T a, T b);

        /// <summary>
        ///     Tests if two items are in same set.
        /// </summary>
        /// <param name="a">One of the items to test if is same set.</param>
        /// <param name="b">One of the items to test if is same set.</param>
        /// <returns><see langword="true" /> if items are in same set, <see langword="false" /> otherwise.</returns>
        bool AreInSameSet(T a, T b);

        /// <summary>
        ///     Enumerates each of the sets in the data structure.
        /// </summary>
        /// <returns>An enumerable of each of the sets.</returns>
        IEnumerable<IEnumerable<T>> GetSets();
    }
}
