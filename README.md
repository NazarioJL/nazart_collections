# Nazart Collections

Repository for specialized C# Data Structures. 

## List of Data Structures

### Disjoint-Set or Union-Find

These data structures are used to track a set of elements partitioned into a number of disjoint subsets. They are useful for finding minimum spanning trees of a graph or testing for cycles.

- DisjointSet\<T\>
- FastDisjointSet

#### Usages

Detecting cycles in undirected-graphs. Consider the following graph with a cycle, modeled as a list of edge tuples: `(0,1), (0,2), (1,3), (2,3), (1,4)`

``` mono
 0-1-4
 | |
 2-3
```

A cycle detection algorithm can be developed by using the `FastDisjointSet` like so:

``` cs
bool HasCycle(IEnumerable<(int,int)> edges, int nodeCount) {
    var disjointSet = new FastDisjointSet(nodeCount);
    
    foreach(var (a,b) in edges) {
        if (!disjointSet.Union(a, b)) {
            return true;
        }
    }

    return false;
}
```
