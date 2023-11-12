using RandomEngine.Entities;

namespace RandomGame;

internal class PathfindingBehaviourTeleport : IEntityBehaviour
{
    private readonly TileMapEngine<TileType> _map;
    private Entity _player;
    private Engine _engine;

    public PathfindingBehaviourTeleport(Entity player, Engine engine, TileMapEngine<TileType> _map)
    {
        _player = player;
        _engine = engine;
        this._map = _map;
    }

    public Point? TargetTile { get; set; }

    public void Update(GameTime gameTime)
    {
        if (TargetTile is Point target)
        {
            _player.Position = new(target.X * _map.TileSize + _map.TileSize / 2, target.Y * _map.TileSize + _map.TileSize / 2);
            TargetTile = null;
        }
    }
}