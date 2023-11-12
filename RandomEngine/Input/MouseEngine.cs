namespace RandomEngine.Input;

public class MouseEngine
{
    private readonly Engine _engine;

    private MouseState _previousState;

    private MouseState _currentState;

    public MouseEngine(Engine engine)
    {
        _engine = engine;
    }

    public bool IsMouseVisible { get => _engine.IsMouseVisible; set => _engine.IsMouseVisible = value; }

    public Point PositionScreen => _currentState.Position;

    public Vector2 PositionWorld { get; private set; }

    public bool WheelUp => _currentState.ScrollWheelValue > _previousState.ScrollWheelValue;

    public bool WheelDown => _currentState.ScrollWheelValue < _previousState.ScrollWheelValue;

    public bool LeftUp => _currentState.LeftButton == ButtonState.Released;

    public bool RightUp => _currentState.RightButton == ButtonState.Released;

    public bool MiddleUp => _currentState.MiddleButton == ButtonState.Released;

    public bool LeftDown => _currentState.LeftButton == ButtonState.Pressed;

    public bool RightDown => _currentState.RightButton == ButtonState.Pressed;

    public bool MiddleDown => _currentState.MiddleButton == ButtonState.Pressed;

    public bool LeftClicked => _currentState.LeftButton == ButtonState.Released && _previousState.LeftButton == ButtonState.Pressed;

    public bool RightClicked => _currentState.RightButton == ButtonState.Released && _previousState.RightButton == ButtonState.Pressed;

    public bool MiddleClicked => _currentState.MiddleButton == ButtonState.Released && _previousState.MiddleButton == ButtonState.Pressed;

    public void SetCursor(Texture2D texture, int originX = 0, int originY = 0)
    {
        MouseCursor.FromTexture2D(texture, originX, originY);
    }

    public bool HitTestWorld(Rectangle rectangle)
    {
        return rectangle.Contains(PositionWorld);
    }

    internal void Update(GameTime gameTime)
    {
        _previousState = _currentState;
        _currentState = Mouse.GetState(); ;

        PositionWorld = _engine.Camera.ScreenToWorld(new Vector2(PositionScreen.X, PositionScreen.Y));
    }
}