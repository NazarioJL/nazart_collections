using System;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Nazart.Collections.Test {
    public class FastDisjointSetTests {

        public class TheConstructor {
            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            public void ThrowsForInvalidArguments(int input) {
                Assert.Throws<ArgumentException>(() => new FastDisjointSet(input));
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            public void CorrectlyCreatesObject(int input) {
                var fastDisjointSet = new FastDisjointSet(input);
                Assert.Equal(input, fastDisjointSet.N);
            }
        }

        public class TheFindMethod {
            [Fact]
            public void InitiallyReturnsTheParentForEachSet() {
                const int n = 10;
                var fastDisjointSet = new FastDisjointSet(n);

                foreach (var i in Enumerable.Range(0, n)) {
                    var actual = fastDisjointSet.Find(i);
                    var expected = i;

                    Assert.Equal(expected, actual);
                }
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(10)]
            [InlineData(11)]
            public void ThrowsIfItemDoesNotExist(int input) {
                const int n = 10;
                var fastDisjointSet = new FastDisjointSet(n);
                Assert.Throws<ArgumentException>(() => fastDisjointSet.Find(input));
            }

            [Fact]
            public void ReturnsCorrectParentAfterUnion() {
                // Create a Disjoint-Set with 10 elements
                // Union 2 groups of which one is bigger
                const int n = 10;
                var fastDisjointSet = new FastDisjointSet(n);

                fastDisjointSet.Union(0, 1);
                var parent0 = fastDisjointSet.Find(0);
                var parent1 = fastDisjointSet.Find(1);

                Assert.Equal(parent1, parent0);
            }

            [Fact]
            public void ReturnsCorrectParentAfterUnionOfDifferentSetSizes() {
                // Create a Disjoint-Set with 10 elements
                // Make a group of 2 elements, and a group of 3, 
                // after merging, both groups should have the parent of the group of 3
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                fastDisjointSet.Union(0, 1);
                fastDisjointSet.Union(0, 2);
                fastDisjointSet.Union(3, 4);

                // Structure is now like following:
                // [0,1,2], [3,4]]
                var expected = fastDisjointSet.Find(0);

                fastDisjointSet.Union(0, 3);

                var actual = fastDisjointSet.Find(4);
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsCorrectParentAfterMergingAllItems() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                foreach (var item in Enumerable.Range(1, n - 1)) {
                    fastDisjointSet.Union(0, item);
                }

                var parent = fastDisjointSet.Find(0);
                // There should be only one set with all parents the same
                foreach (var item in Enumerable.Range(1, n - 1)) {
                    var actual = fastDisjointSet.Find(item);
                    Assert.Equal(parent, actual);
                }
            }
        }

        public class TheUnionMethod {
            [Fact]
            public void CorrectlyMerges2Items() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                fastDisjointSet.Union(0, 1);
                Assert.Equal(fastDisjointSet.Find(0), fastDisjointSet.Find(1));

                // Assert other elements are all distinct
                const int othersCount = n - 2;
                var othersRange = Enumerable.Range(2, othersCount);

                var othersUniqueParentCount = othersRange
                    .Select(e => fastDisjointSet.Find(e))
                    .Distinct()
                    .Count();

                Assert.Equal(othersCount, othersUniqueParentCount);
            }

            [Fact]
            public void CorrectlyMerges3Items() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                fastDisjointSet.Union(0, 1);
                fastDisjointSet.Union(1, 2);
                Assert.Equal(fastDisjointSet.Find(0), fastDisjointSet.Find(1));
                Assert.Equal(fastDisjointSet.Find(1), fastDisjointSet.Find(2));

                // Assert other elements are all distinct
                const int othersCount = n - 3;
                var othersRange = Enumerable.Range(3, othersCount);

                var othersUniqueParentCount = othersRange
                    .Select(e => fastDisjointSet.Find(e))
                    .Distinct()
                    .Count();

                Assert.Equal(othersCount, othersUniqueParentCount);
            }

            [Fact]
            public void CorrectlyMergesAllItems() {
                // Same as
                // TheFindMethod.ReturnsCorrectParentAfterMergingAllItems
            }

            [Fact]
            public void ReturnsCorrectValueWhenMergingItemsInSameSet() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                Assert.True(fastDisjointSet.Union(0, 1));
                Assert.False(fastDisjointSet.Union(0, 1));
            }

            [Fact]
            public void ReturnsCorrectValueWhenIndirectlyMergingItemsInSameSet() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                fastDisjointSet.Union(0, 1);
                fastDisjointSet.Union(1, 2);

                Assert.False(fastDisjointSet.Union(0, 2));
            }
        }

        public class TheAreInSameMethod {
            [Fact]
            public void ReturnsCorrectValueForItemsNotInSameSet() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                Assert.False(fastDisjointSet.AreInSameSet(0, 1));
            }

            [Fact]
            public void ReturnsCorrectValueForItemsInSameSet() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                fastDisjointSet.Union(0, 1);

                Assert.True(fastDisjointSet.AreInSameSet(0, 1));
            }
        }

        // TODO: Investigate better set comparison mechanisms, order is not in the contract
        public class TheGetSetsMethod {
            [Fact]
            public void ReturnsCorrectValueForInitializedSets() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                var expected = Enumerable
                    .Range(0, 5)
                    .Select(e => (new[] {e}));

                var actual = fastDisjointSet.GetSets();

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsCorrectValueForPartiallyMergedSets() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                // Make merges so structure looks like:
                // [[0,1],[2],[3,4]]
                fastDisjointSet.Union(0, 1);
                fastDisjointSet.Union(3, 4);

                var expected = new[] {
                    new[] {0, 1},
                    new[] {2},
                    new[] {3, 4},
                };

                var actual = fastDisjointSet.GetSets();

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ReturnsCorrectValueForFullyMergedSets() {
                const int n = 5;
                var fastDisjointSet = new FastDisjointSet(n);

                // Merge all items
                foreach (var item in Enumerable.Range(1, n - 1)) {
                    fastDisjointSet.Union(0, item);
                }

                var expected = new[] {
                    new[] {0, 1, 2, 3, 4},
                };

                var actual = fastDisjointSet.GetSets();

                Assert.Equal(expected, actual);
            }
        }
    }
}
