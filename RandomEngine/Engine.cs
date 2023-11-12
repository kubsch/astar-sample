using RandomEngine.Camera;
using RandomEngine.Input;
using RandomEngine.Media;

namespace RandomEngine;

public class Engine : Game
{
    private IGameState _currentGameState;

    private Dictionary<string, IGameState> _gameStates = new();

    public Engine()
    {
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = true;

        Graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
            PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,
            SynchronizeWithVerticalRetrace = false,
            IsFullScreen = true,
            HardwareModeSwitch = false
        };

        Mouse = new(this);
    }

    public GraphicsDeviceManager Graphics { get; }
    public DrawEngine Render { get; private set; }
    public KeyboardEngine Keyboard { get; } = new();
    public MouseEngine Mouse { get; }
    public CameraEngine Camera { get; private set; } = null!;
    public MusicEngine Music { get; } = new();
    public Utils Utils { get; } = new();

    public void AddGameState(IGameState gameState)
    {
        _gameStates.Add(gameState.Name, gameState);
    }

    public IGameState GetGameState(string key)
    {
        return _gameStates[key];
    }

    public void ToGameState(GameTime gameTime, IGameState gameState)
    {
        var oldGameState = _currentGameState;

        _currentGameState = gameState;
        _currentGameState.WakeUp(gameTime, oldGameState);
    }

    public void Run(string gameStateKey)
    {
        _currentGameState = _gameStates[gameStateKey];
        Run();
    }

    protected override void Initialize()
    {
        Camera = new CameraEngine(Graphics.GraphicsDevice.Viewport);
        Render = new DrawEngine(Graphics.GraphicsDevice);

        foreach (var gameState in _gameStates.Values)
            gameState.Initialize();

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        Camera.Update(gameTime, Graphics.GraphicsDevice.Viewport);
        Keyboard.Update(gameTime);
        Mouse.Update(gameTime);

        _currentGameState?.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _currentGameState?.Draw(gameTime);
        base.Draw(gameTime);
    }
}