namespace AoC.Common;
public class Dijkstra<T> where T : class
{
    private readonly HashSet<T> _points = new();
    private readonly Dictionary<T, HashSet<T>> _edges = new();

    private readonly Dictionary<(T from, T to), List<T>> _routeCache = new();

    public Dijkstra()
    {
    }

    public Dijkstra(List<T> points, List<Edge<T>> edges)
    {
        foreach (var point in points)
        {
            AddNode(point);
        }

        foreach (var edge in edges)
        {
            AddEdge(edge.From, edge.To, edge.Unidirectional);
        }
    }

    public Dijkstra<T> AddNode(T node)
    {
        _points.Add(node);

        return this;
    }

    public Dijkstra<T> AddEdge(T from, T to, bool unidirectional = false)
    {
        if (!_edges.ContainsKey(from))
        {
            _edges[from] = new();
        }

        _edges[from].Add(to);

        if (unidirectional)
        {
            return this;
        }

        if (!_edges.ContainsKey(to))
        {
            _edges[to] = new();
        }

        _edges[to].Add(from);

        return this;
    }

    public List<T> Solve(T start, T end)
    {
        if (_routeCache.TryGetValue((start, end), out var cachedRoute))
        {
            return cachedRoute;
        }

        // Shortest distance between the node and the start, initially set to Infinity (or thereabouts)
        var dist = _edges.Keys.ToDictionary(k => k, _ => int.MaxValue);

        // Given a node, tracks the best node to move to reach the end as quickly as possible,
        // initially set to Unknown
        var prev = _edges.Keys.ToDictionary(k => k, _ => (T?)null);

        // List of all points to process
        var q = _edges.Keys.ToList();

        // Helper HashSet for better performance in lookup
        var inQ = new HashSet<T>(q);

        dist[start] = 0;

        // Ensure that the start point is the first item in the queue
        q.Remove(start);
        q.Insert(0, start);
        var insertionIndex = 0; // Tracked index to ensure that we insert nodes at the appropriate places in the queue

        // While we have nodes to process
        while (q.Count > 0)
        {
            var next = q[0]; // Grab the first node on the list

            // Already at the end, exit loop
            if (next.Equals(end))
            {
                break;
            }

            q.RemoveAt(0); // Remove 'next' from the queue
            inQ.Remove(next);

            // Fetch all nearby nodes that are still in the queue
            var neighbors = _edges[next].Where(inQ.Contains).ToList();

            // Calculate distance travelled to reach the node
            var distance = dist[next] + 1;

            foreach (var n in neighbors)
            {
                // If we've travelled a shorter distance than before
                if (distance < dist[n])
                {
                    // Move the node as close to the front as insertionIndex allows us
                    // This ensures that it is processed before other, further away nodes,
                    // but not before closer nodes
                    q.Remove(n);
                    q.Insert(insertionIndex++, n);

                    // Record the new, shorter, distance
                    dist[n] = distance;

                    // Mark next -> as the desirable step to take
                    prev[n] = next;
                }
            }

            // Ensure that the index doesn't grow uncontrollably
            insertionIndex = Math.Max(0, insertionIndex - 1);
        }

        // Time to back-track and solve generate the path
        var path = new List<T>();
        var current = (T?)end;

        while (current != null)
        {
            path.Insert(0, current);
            current = prev[current];
        }

        // If we managed to find our way back to the start,
        // return the list of nodes, otherwise return an empty list
        return _routeCache[(start, end)] = path[0] == start ? path : new();
    }
}

