using RandomEngine.Extensions;

namespace RandomEngine;

public class FadeTransition : Transition
{
    private Texture2D? _texture = null;
    private TimeSpan _time;

    public FadeTransition(Engine engine, IGameState gameStateOld, IGameState gameStateNew)
        : base(engine, gameStateOld, gameStateNew)
    {
    }

    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(byte.MaxValue);

    public override string Name => "fade-transition";

    public override void Draw(GameTime gameTime)
    {
        if (Finished)
            return;

        if (_texture == null)
        {
            _texture = Engine.Render.SetRenderTargetTexture();
            GameStateOld.Draw(gameTime);
            Engine.Render.SetRenderTargetScreen();
        }

        Engine.Render.Begin();

        var alpha = Math.Clamp(byte.MaxValue - _time.Milliseconds, 0, byte.MaxValue);
        Engine.Render.Texture(_texture, new Vector2(0, 0), Color.White.With(a: alpha));

        Engine.Render.End();
    }

    public override void Update(GameTime gameTime)
    {
        if (Finished)
            return;

        _time += gameTime.ElapsedGameTime;
        if (_time >= Duration)
            Finish(gameTime);
    }
}