namespace RandomEngine.Input;

public class KeyboardEngine
{
    private KeyboardState _previousKeyState;

    private KeyboardState _currentKeyState;

    public bool NumLock => _currentKeyState.NumLock;

    public bool CapsLock => _currentKeyState.CapsLock;

    public Keys[] PressedKeys { get; private set; } = { };

    public bool IsKeyDown(Keys key) => _currentKeyState.IsKeyDown(key);

    public bool IsKeyUp(Keys key) => _currentKeyState.IsKeyUp(key);

    public bool IsKeyReleased(Keys key) => _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);

    internal void Update(GameTime gameTime)
    {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();
        PressedKeys = _currentKeyState.GetPressedKeys();
    }
}