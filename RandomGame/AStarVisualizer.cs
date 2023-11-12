using RandomEngine.Entities;

namespace RandomGame;

public class AStarVisualizer
{
    private const int RenderDelayInMs = 60;

    private readonly Engine _engine;
    private readonly Entity _player;
    private Vector2? _hoveredTile;
    private AStar? _aStar;
    private List<AStar.Node>? _path;
    private TimeSpan _aStarInfoTimer;
    private TileMapEngine<TileType> _map = null!;

    public AStarVisualizer(Engine engine, TileMapEngine<TileType> map, Entity player)
    {
        _engine = engine;
        _map = map;
        _player = player;
    }

    public bool Enabled { get; set; } = true;
    public bool Visible { get; set; } = true;

    public void Update(GameTime gameTime)
    {
        if (!Enabled)
            return;

        var mapBounds = new Rectangle(0, 0, _map.TileSize * _map.Width, _map.TileSize * _map.Height);
        _hoveredTile = null;
        if (mapBounds.Contains(_engine.Mouse.PositionWorld))
        {
            _hoveredTile = new Vector2((int)(_engine.Mouse.PositionWorld.X / _map.TileSize), (int)(_engine.Mouse.PositionWorld.Y / _map.TileSize));

            if (_engine.Mouse.LeftClicked)
            {
                AStar.Node[,] nodes = new AStar.Node[_map.Width, _map.Height];
                for (var x = 0; x < _map.Width; x++)
                    for (var y = 0; y < _map.Height; y++)
                        nodes[x, y] = new AStar.Node(new Vector2(x, y), _map[x, y] == TileType.Grass);
                _aStar = new AStar(nodes);

                var startNode = _aStar.Nodes[(int)(_player.Position.X / _map.TileSize), (int)(_player.Position.Y / _map.TileSize)];
                var endNode = _aStar.Nodes[(int)_hoveredTile.Value.X, (int)_hoveredTile.Value.Y];

                _path = _aStar?.Search(startNode, endNode)?.ToList();
                _aStarInfoTimer = TimeSpan.FromMilliseconds(RenderDelayInMs);
            }
        }
    }

    public void Draw(GameTime gameTime)
    {
        if (!Enabled || !Visible)
            return;

        Rectangle NodeToRectangle(AStar.Node node)
        {
            return new Rectangle((int)node.Position.X * _map.TileSize, (int)node.Position.Y * _map.TileSize, _map.TileSize, _map.TileSize);
        }

        if (_hoveredTile is Vector2 tile && _map[(int)tile.X, (int)tile.Y] == TileType.Grass)
            _engine.Render.Rectangle(new Rectangle((int)tile.X * _map.TileSize, (int)tile.Y * _map.TileSize, _map.TileSize, _map.TileSize), Color.FromNonPremultiplied(0, 255, 0, 32), true);

        if (_aStar is not null)
        {
            _aStarInfoTimer += gameTime.ElapsedGameTime;
            var step = (int)(_aStarInfoTimer.TotalMilliseconds / RenderDelayInMs);

            if (step < _aStar.ProcessOrder.Count)
            {
                for (int i = 0; i < Math.Min(_aStar.ProcessOrder.Count, step); i++)
                {
                    var node = _aStar.ProcessOrder[i];
                    _engine.Render.Rectangle(NodeToRectangle(node), Color.FromNonPremultiplied(255, 255, 255, 64), true);
                }
                _engine.Render.Rectangle(NodeToRectangle(_aStar.ProcessOrder[Math.Min(_aStar.ProcessOrder.Count, step)]), Color.FromNonPremultiplied(0, 0, 0, 64), true);
            }
            else
            {
                foreach (var node in _aStar.Nodes)
                    if (_aStar.ProcessOrder.Contains(node))
                    {
                        var text = $"{_aStar.ProcessOrder.IndexOf(node) + 1}\r\nf {node.TotalCost}\r\ng {node.IndividualCost}\r\nh {node.Heuristic}";
                        var textSize = Assets.Fonts.Neuro12.MeasureString(text);
                        _engine.Render.Text(Assets.Fonts.Neuro12, text, new Vector2((int)node.Position.X * _map.TileSize + _map.TileSize / 2 - textSize.X / 2, (int)node.Position.Y * _map.TileSize + _map.TileSize / 2 - textSize.Y / 2), Color.Black);
                    }
                    else if (node.Heuristic > 0)
                    {
                        var text = $"OPEN\r\nf {node.TotalCost}\r\ng {node.IndividualCost}\r\nh {node.Heuristic}";
                        var textSize = Assets.Fonts.Neuro12.MeasureString(text);
                        _engine.Render.Text(Assets.Fonts.Neuro12, text, new Vector2((int)node.Position.X * _map.TileSize + _map.TileSize / 2 - textSize.X / 2, (int)node.Position.Y * _map.TileSize + _map.TileSize / 2 - textSize.Y / 2), Color.Black);
                    }

                foreach (var node in _aStar.Open.UnorderedItems)
                    _engine.Render.Rectangle(NodeToRectangle(node.Element), Color.FromNonPremultiplied(255, 0, 0, 32), true);

                foreach (var node in _aStar.Closed)
                    _engine.Render.Rectangle(NodeToRectangle(node), Color.FromNonPremultiplied(0, 0, 255, 32), true);

                if (_path != null)
                    foreach (var node in _path)
                        _engine.Render.Rectangle(NodeToRectangle(node), Color.FromNonPremultiplied(0, 0, 255, 32), true);
            }
        }
    }
}