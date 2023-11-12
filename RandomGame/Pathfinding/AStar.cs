namespace RandomGame;

public class AStar
{
    private readonly int _maxRows;
    private readonly int _maxCols;

    public AStar(Node[,] nodes)
    {
        Nodes = nodes;
        _maxCols = Nodes.GetUpperBound(0);
        _maxRows = Nodes.GetUpperBound(1);
    }

    public Node[,] Nodes { get; private set; }
    public PriorityQueue<Node, float> Open { get; private set; } = new();
    public HashSet<Node> Closed { get; private set; } = new();
    public List<Node> ProcessOrder { get; private set; } = new();

    public Stack<Node>? Search(Node startNode, Node targetNode)
    {
        ProcessOrder = new(); // only for visualizing
        Open = new PriorityQueue<Node, float>();
        Closed = new HashSet<Node>();

        Open.Enqueue(startNode, startNode.TotalCost);

        var current = null as Node;

        while (Open.Count != 0 && !Closed.Any(x => x.Position == targetNode.Position))
        {
            current = Open.Dequeue();
            Closed.Add(current);
            ProcessOrder.Add(current); // only for visualizing

            var adjacentNodes = GetAdjacentNodes(current);

            foreach (var adjacentNode in adjacentNodes)
            {
                if (!Closed.Contains(adjacentNode) && adjacentNode.Walkable)
                {
                    var isFound = false;

                    foreach (var openListNode in Open.UnorderedItems)
                        if (openListNode.Element == adjacentNode)
                            isFound = true;

                    if (!isFound)
                    {
                        adjacentNode.Parent = current;
                        adjacentNode.Heuristic = Heuristic(adjacentNode, targetNode);
                        adjacentNode.CummulatedCost = adjacentNode.IndividualCost + adjacentNode.Parent.CummulatedCost;

                        Open.Enqueue(adjacentNode, adjacentNode.TotalCost);
                    }
                }
            }
        }

        if (!Closed.Any(x => x.Position == targetNode.Position) || current is null)
            return null;

        var path = new Stack<Node>();

        do
        {
            path.Push(current);
            current = current.Parent;
        } while (current != startNode && current is not null);

        return path;
    }

    private static float Heuristic(Node node, Node targetNode)
    {
        return Math.Abs(node.Position.X - targetNode.Position.X) + Math.Abs(node.Position.Y - targetNode.Position.Y);
    }

    private List<Node> GetAdjacentNodes(Node node)
    {
        List<Node> adjacentNodes = new List<Node>();

        int row = (int)node.Position.Y;
        int col = (int)node.Position.X;

        if (row + 1 < _maxRows)
            adjacentNodes.Add(Nodes[col, row + 1]);

        if (row - 1 >= 0)
            adjacentNodes.Add(Nodes[col, row - 1]);

        if (col - 1 >= 0)
            adjacentNodes.Add(Nodes[col - 1, row]);

        if (col + 1 < _maxCols)
            adjacentNodes.Add(Nodes[col + 1, row]);

        return adjacentNodes;
    }

    public class Node
    {
        public Node(Vector2 position, bool walkable, float individualCost = 1)
        {
            Position = position;
            Walkable = walkable;
            IndividualCost = individualCost;
        }

        public Vector2 Position { get; }
        public float IndividualCost { get; }
        public bool Walkable { get; }

        public Node? Parent { get; set; }
        public float CummulatedCost { get; set; }  // g
        public float Heuristic { get; set; }    // h

        // f = g + h
        public float TotalCost
        {
            get
            {
                if (Heuristic != -1 && CummulatedCost != -1)
                    return Heuristic + CummulatedCost;
                else
                    return -1;
            }
        }
    }
}