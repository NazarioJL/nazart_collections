using System;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Nazart.Collections.Test
{
    // Most of the funcionality of the DisjointSet class is implemented by the 
    // underlying FastDisjointSet. These tests will focus on the non overlapping
    // functionality.
    public class GenericDisjointSetTests {

        public class TheConstructor {
            [Fact]
            public void CorrectlyThrowsForNonUniqueElements() {
                Assert.Throws<ArgumentException>(() => new DisjointSet<string>(new[] { "A", "B", "A" }));                
            }

            [Fact]
            public void CorrectlyCreatesObject() {
                var data = new[] { "A", "B", "C" };
                var disjointSet = new DisjointSet<string>(data);

                var expected = data.Select(e => new[] {e});
                var actual = disjointSet.GetSets();

                Assert.Equal(expected, actual);
            }
        }

        public class TheUnionMethod {
            [Fact]
            public void CorrectlyMerges2Items() {
                var data = new[] { "A", "B", "C", };
                var disjointSet = new DisjointSet<string>(data);

                disjointSet.Union("A", "B");
                var parentOfA = disjointSet.Find("A");
                var parentOfB = disjointSet.Find("B");
                var parentOfC = disjointSet.Find("C");

                Assert.Equal(parentOfA, parentOfB);
                Assert.NotEqual(parentOfA, parentOfC);
            }

            [Fact]
            public void ReturnsCorrectValueForAlreadyMergedItems() {
                var data = new[] { "A", "B", "C", };
                var disjointSet = new DisjointSet<string>(data);

                Assert.True(disjointSet.Union("A", "B"));
                Assert.False(disjointSet.Union("A", "B"));
            }
        }

        [Fact]
        public void AlreadyMergedItems() {
            var data = new[] { "A", "B", "C", };
            var disjointSet = new DisjointSet<string>(data);

            disjointSet.Union("A", "B");
            
            Assert.True(disjointSet.AreInSameSet("A", "B"));
            Assert.False(disjointSet.AreInSameSet("A", "C"));
        }
    }
}

