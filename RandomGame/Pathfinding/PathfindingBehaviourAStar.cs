using RandomEngine.Entities;

namespace RandomGame;

internal class PathfindingBehaviourAStar : IEntityBehaviour
{
    private readonly TileMapEngine<TileType> _map;
    private Entity _player;
    private Engine _engine;
    private AStar _astar;
    private Stack<AStar.Node>? _path;

    private float _timer;

    private float _speed = 2;

    private Vector2 _startPosition;

    private Vector2 _endPosition;

    public PathfindingBehaviourAStar(Entity player, Engine engine, TileMapEngine<TileType> map)
    {
        _player = player;
        _engine = engine;
        _map = map;

        AStar.Node[,] nodes = new AStar.Node[_map.Width, _map.Height];
        for (var x = 0; x < _map.Width; x++)
            for (var y = 0; y < _map.Height; y++)
                nodes[x, y] = new AStar.Node(new Vector2(x, y), _map[x, y] == TileType.Grass);

        _astar = new AStar(nodes);
    }

    public void MoveToTarget(Vector2 target)
    {
        var startNode = _astar.Nodes[(int)(_player.Position.X / _map.TileSize), (int)(_player.Position.Y / _map.TileSize)];
        var endNode = _astar.Nodes[(int)(target.X / _map.TileSize), (int)(target.Y / _map.TileSize)];

        _path = _astar.Search(startNode, endNode);
        _startPosition = _player.Position;
        _endPosition = _player.Position;
        _player.SetAnimation("run");
    }

    public void Update(GameTime gameTime)
    {
        if (_path is Stack<AStar.Node> path && path.Any() && _endPosition == _startPosition)
        {
            var nextNode = _path.Pop();
            _endPosition = new Vector2(nextNode.Position.X * _map.TileSize + _map.TileSize / 2, nextNode.Position.Y * _map.TileSize + _map.TileSize / 2);
            if (_endPosition.X < _startPosition.X)
                _player.Directions = Directions.Left;
            else if (_endPosition.X > _startPosition.X)
                _player.Directions = Directions.Right;

            _timer = 0;
        }

        if (_startPosition != _endPosition)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _timer += deltaTime * _speed;
            _timer = MathHelper.Min(_timer, 1);
            _player.Position = Vector2.Lerp(_startPosition, _endPosition, _timer);

            if (_player.Position == _endPosition)
            {
                _startPosition = _endPosition;
                if (_path == null || !_path.Any())
                    _player.SetAnimation("idle");
            }
        }
    }
}