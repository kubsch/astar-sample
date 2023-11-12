using RandomEngine.Entities;
using RandomEngine.UI;

namespace RandomGame.GameStates;

public class GameStateWorld : IGameState
{
    private readonly Engine _engine;
    private EntityEngine _entities;
    private TileMapEngine<TileType> _map = null!;
    private DebugInterface? _debugInterface;
    private bool _transitionIn;
    private TimeSpan _transitionTime;
    private Entity _player;
    private PathfindingBehaviourAStar _playerBehaviour;
    private Vector2? _hoveredTile;
    private AStarVisualizer _aStarVisualizer;

    public GameStateWorld(Engine engine)
    {
        _engine = engine;

        // ATT: see Initialize()
        _entities = null!;
        _player = null!;
        _playerBehaviour = null!;
        _aStarVisualizer = null!;
    }

    public string Name => "world";

    public void Initialize()
    {
        InitCamera(_engine);
        InitWorld();
        InitPlayer();
        InitUi();
    }

    public void Update(GameTime gameTime)
    {
        UpdateGameState(gameTime);
        UpdateDebugInterface();
        UpdatePathfinding(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        DrawWorld(gameTime);
        DrawUI(gameTime);
        DrawTransition(gameTime);
    }

    public void WakeUp(GameTime gameTime, IGameState? oldGameState)
    {
        _transitionIn = true;
        _transitionTime = TimeSpan.Zero;

        _engine.Music.Loop(Assets.Songs.HeroOfThe80s);
        _engine.IsMouseVisible = true;
    }

    private void UpdateGameState(GameTime gameTime)
    {
        _entities.Update(gameTime);

        if (_engine.Keyboard.IsKeyReleased(Keys.Escape))
            _engine.ToGameState(gameTime, new FadeTransition(_engine, this, _engine.GetGameState("menu")));
    }

    private void UpdateDebugInterface()
    {
        if (_debugInterface == null)
            return;

        if (_engine.Keyboard.IsKeyReleased(Keys.F1))
            _debugInterface.ShowDebug = !_debugInterface.ShowDebug;

        if (_engine.Keyboard.IsKeyReleased(Keys.F2))
            _debugInterface.ShowInspector = !_debugInterface.ShowInspector;

        if (_engine.Keyboard.IsKeyReleased(Keys.F3) && _debugInterface.InspectedEntity is not null)
            _debugInterface.Locked = !_debugInterface.Locked;

        if (_debugInterface.ShowInspector && !_debugInterface.Locked)
            foreach (var entity in _entities)
            {
                if (entity.CurrentAnimation is null)
                    continue;

                var bounds = new Rectangle((int)entity.Position.X - entity.CurrentAnimation.FrameWidth / 2, (int)entity.Position.Y - entity.CurrentAnimation.FrameHeight / 2, entity.CurrentAnimation.FrameWidth, entity.CurrentAnimation.FrameHeight);
                _debugInterface.InspectedEntity = null;
                if (bounds.Contains(_engine.Mouse.PositionWorld))
                {
                    _debugInterface.InspectedEntity = entity;
                    break;
                }
            }
    }

    private void UpdatePathfinding(GameTime gameTime)
    {
        var mapBounds = new Rectangle(0, 0, _map.TileSize * _map.Width, _map.TileSize * _map.Height);
        _hoveredTile = null;
        if (mapBounds.Contains(_engine.Mouse.PositionWorld))
        {
            _hoveredTile = new Vector2((int)(_engine.Mouse.PositionWorld.X / _map.TileSize), (int)(_engine.Mouse.PositionWorld.Y / _map.TileSize));

            if (_engine.Mouse.LeftClicked)
                _playerBehaviour.MoveToTarget(new Vector2(_hoveredTile.Value.X * _map.TileSize, _hoveredTile.Value.Y * _map.TileSize));
        }

        _aStarVisualizer?.Update(gameTime);
    }

    private void DrawWorld(GameTime gameTime)
    {
        _engine.Render.Begin(transformMatrix: _engine.Camera.Transform);

        _map.Draw(_engine, gameTime);

        _aStarVisualizer.Draw(gameTime);
        DrawEntityInspector();

        _entities.Draw(_engine, gameTime);

        _engine.Render.End();
    }

    private void DrawUI(GameTime gameTime)
    {
        _engine.Render.Begin();

        if (_debugInterface is not null)
            _debugInterface.Draw(_engine, gameTime);

        _engine.Render.End();
    }

    private void DrawEntityInspector()
    {
        if (_debugInterface?.InspectedEntity is not null)
        {
            var entity = _debugInterface.InspectedEntity;

            if (entity.CurrentAnimation is null)
                return;

            var color = _debugInterface.Locked ? Color.FromNonPremultiplied(255, 0, 0, 12) : Color.FromNonPremultiplied(0, 255, 0, 12);
            var rect = new Rectangle((int)entity.Position.X - entity.CurrentAnimation.FrameWidth / 2, (int)entity.Position.Y - entity.CurrentAnimation.FrameHeight / 2, entity.CurrentAnimation.FrameWidth, entity.CurrentAnimation.FrameHeight);

            _engine.Render.Rectangle(rect, color);
            color = _debugInterface.Locked ? Color.FromNonPremultiplied(255, 0, 0, 128) : Color.FromNonPremultiplied(0, 255, 0, 128);
            _engine.Render.Rectangle(rect, color, false);
        }
    }

    private void DrawTransition(GameTime gameTime)
    {
        if (!_transitionIn)
            return;

        var transitionLength = TimeSpan.FromMilliseconds(byte.MaxValue);

        if (_transitionTime < transitionLength)
        {
            _engine.Render.Begin();

            var alpha = Math.Clamp(byte.MaxValue - _transitionTime.Milliseconds, 0, byte.MaxValue);

            _engine.Render.Rectangle(_engine.GraphicsDevice.Viewport.Bounds, Color.Black.With(a: alpha));

            _engine.Render.End();
        }
        else
            _transitionIn = false;

        _transitionTime += gameTime.ElapsedGameTime;
    }

    private void InitWorld()
    {
        _entities = new EntityEngine(_engine);
        _map = new MapLoader().Load(_engine.Camera);
    }

    private void InitUi()
    {
        _debugInterface = new DebugInterface(_engine.Graphics, _engine, Assets.Fonts.Neuro18)
        {
            ShowDebug = false,
            ShowInspector = false
        };

        _aStarVisualizer = new(_engine, _map, _player);
    }

    private void InitPlayer()
    {
        _player = _entities.Create();

        var anim = new Animation("idle", Assets.Textures.CyborgIdle, 125, 48);
        _player.Animations.Add(anim.Name, anim);

        anim = new Animation("run", Assets.Textures.CyborgRun, 75, 48);
        _player.Animations.Add(anim.Name, anim);

        _player.Position = new Vector2(4 * _map.TileSize + _map.TileSize / 2, 4 * _map.TileSize + _map.TileSize / 2);
        _playerBehaviour = new PathfindingBehaviourAStar(_player, _engine, _map);
        _player.Behaviours.Add(_playerBehaviour);
        _player.SetAnimation("idle");
    }

    private void InitCamera(Game game)
    {
        _engine.Camera.Behaviours.Clear();
        _engine.Camera.Behaviours.Add(new MouseMovedCameraBehaviour(_engine.Mouse));
        _engine.Camera.X = 500;
        _engine.Camera.Y = 500;
    }
}