namespace RandomGame.GameStates;

public class GameStateIntro : IGameState
{
    private readonly Engine _engine;
    private TimeSpan _introTime = TimeSpan.Zero;
    private Vector2 _textPosition;
    private byte _fade = 0;

    public GameStateIntro(Engine engine)
    {
        _engine = engine;
    }

    public string Name => "intro";

    public void Draw(GameTime gameTime)
    {
        DrawIntro(gameTime);
    }

    public void Initialize()
    {
        Assets.Load(_engine.Content);

        var width = Assets.Fonts.Neuro64.MeasureString("SKU PRESENTS").X;
        _textPosition = new Vector2(_engine.Camera.Viewport.Width / 2 - width / 2, _engine.Camera.Viewport.Height / 2);
        _engine.Music.Play(Assets.Songs.Scary);
    }

    public void WakeUp(GameTime gameTime, IGameState? oldTarget)
    {
        _engine.IsMouseVisible = false;
    }

    public void Update(GameTime gameTime)
    {
        _introTime += gameTime.ElapsedGameTime;

        if (_introTime >= TimeSpan.FromSeconds(5) || _engine.Keyboard.IsKeyReleased(Keys.Escape) || _engine.Mouse.LeftClicked)
            _engine.ToGameState(gameTime, _engine.GetGameState("menu"));
    }

    private void DrawIntro(GameTime gameTime)
    {
        _engine.Render.Begin();

        if (_introTime > TimeSpan.FromMilliseconds(250) && _introTime < TimeSpan.FromMilliseconds(1500))
        {
            _fade += (byte)((_introTime.TotalMilliseconds - 500) / 2);

            var color = Color.FromNonPremultiplied(255, 255, 255, _fade);

            _engine.Render.Text(Assets.Fonts.Neuro64, "SKU", _textPosition, color);
        }
        else if (_introTime >= TimeSpan.FromMilliseconds(1500) && _introTime < TimeSpan.FromMilliseconds(3000))
        {
            _engine.Render.Text(Assets.Fonts.Neuro64, "SKU PRESENTS", _textPosition, Color.White);
        }
        else if (_introTime > TimeSpan.FromMilliseconds(3000) && _introTime < TimeSpan.FromMilliseconds(4000))
        {
            _fade += (byte)((_introTime.TotalMilliseconds - 500) / 2);

            var color = Color.FromNonPremultiplied(255, 255, 255, _fade);

            _engine.Render.Text(Assets.Fonts.Neuro64, "SKU PRESENTS", _textPosition, color);
        }

        _engine.Render.End();
    }
}