using RandomEngine.Debug;
using RandomEngine.Entities;
using RandomEngine.Extensions;
using System.Text;

namespace RandomEngine.UI;

public class DebugInterface
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly Engine _engine;
    private readonly SpriteFontBase _font;
    private readonly FrameCounter _frameCounter = new();

    public DebugInterface(GraphicsDeviceManager graphics, Engine engine, SpriteFontBase font)
    {
        _graphics = graphics;
        _engine = engine;
        _font = font;
    }

    public bool ShowDebug { get; set; } = true;
    public bool ShowInspector { get; set; } = true;
    public Entity? InspectedEntity { get; set; }
    public bool Locked { get; set; }
    public Color ForeColor { get; set; } = Color.Lime;
    public Color BackColor { get; set; } = Color.Black.With(a: 128);

    public void Draw(Engine engine, GameTime gameTime)
    {
        _frameCounter.Update(gameTime);
        var debugInfos = DefineDebugInfos();

        if (ShowDebug)
            DrawDebugInfos(engine, debugInfos);

        if (ShowInspector)
            DrawEntityInspector(engine);
    }

    private void DrawEntityInspector(Engine engine)
    {
        var width = 300;
        var height = 512;
        var boxMargin = 16;
        var rectangle = new Rectangle(_engine.Camera.Viewport.Width - width - boxMargin, boxMargin, width, height + boxMargin);
        engine.Render.Rectangle(rectangle, BackColor);

        var sb = new StringBuilder();
        sb.AppendLine("Entity Inspector");
        sb.AppendLine("Press F2 to toggle");

        if (InspectedEntity == null)
        {
            sb.AppendLine(string.Empty);
            sb.AppendLine($"No entity selected");
        }
        else
        {
            sb.AppendLine("Press F3 to lock on");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"ID: {InspectedEntity.Id}");
            sb.AppendLine(String.Empty);
            sb.AppendLine($"Position: X{InspectedEntity.Position.X:0} Y{InspectedEntity.Position.Y:0}");
            sb.AppendLine(String.Empty);
            sb.AppendLine($"Color");
            sb.AppendLine($"  R{InspectedEntity.Color.R:0} G{InspectedEntity.Color.G:0} B{InspectedEntity.Color.B:0} A{InspectedEntity.Color.A:0}");
            sb.AppendLine(String.Empty);
            sb.AppendLine($"Animation");
            sb.AppendLine($"  Name: {InspectedEntity?.CurrentAnimation?.Name}");
            sb.AppendLine($"  Time per frame: {InspectedEntity?.CurrentAnimation?.TimePerFrame}");
            sb.AppendLine($"  Current Frame: {InspectedEntity?.CurrentAnimation?.CurrentFrame}");
            sb.AppendLine(String.Empty);
            sb.AppendLine($"Direction");
            sb.AppendLine($"  Name: {InspectedEntity.Directions}");
            sb.AppendLine(String.Empty);
            sb.AppendLine($"Behaviours");
            sb.AppendLine($"{string.Join("\n\r", InspectedEntity.Behaviours.Select(x => x.GetType().Name.Replace("EntityBehaviour", string.Empty, StringComparison.OrdinalIgnoreCase)))}");
        }

        engine.Render.Text(_font, sb.ToString(), new Vector2(rectangle.X + boxMargin, rectangle.Y + boxMargin), ForeColor);
    }

    private void DrawDebugInfos(Engine engine, StringBuilder debugInfos)
    {
        var width = 300;
        var height = 512;
        var boxMargin = 16;
        engine.Render.Rectangle(new Rectangle(boxMargin, boxMargin, width, height + boxMargin), BackColor);
        engine.Render.Text(_font, debugInfos.ToString(), new Vector2(24, 24), ForeColor);
    }

    private StringBuilder DefineDebugInfos()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Debug");
        sb.AppendLine("Press F1 to toggle");
        sb.AppendLine(string.Empty);
        sb.AppendLine($"{_graphics.PreferredBackBufferWidth:0} x {_graphics.PreferredBackBufferHeight:0}");
        sb.AppendLine($"VSync {(_graphics.SynchronizeWithVerticalRetrace ? "On" : "Off")}");
        sb.AppendLine(string.Empty);
        sb.AppendLine($"{_frameCounter.AverageFramesPerSecond:0} fps");
        sb.AppendLine(string.Empty);
        sb.AppendLine($"Mouse");
        sb.AppendLine($"  Screen: x {_engine.Mouse.PositionScreen.X:0} y {_engine.Mouse.PositionScreen.Y:0}");
        sb.AppendLine($"  World: x {_engine.Mouse.PositionWorld.X:0} y {_engine.Mouse.PositionScreen.Y:0}");
        sb.AppendLine(string.Empty);
        sb.AppendLine($"Camera");
        sb.AppendLine($"  Zoom: {_engine.Camera.Zoom:0.00}");
        sb.AppendLine($"  Pos: x{_engine.Camera.X:0} y{_engine.Camera.Y:0} ");
        sb.AppendLine($"  Visible area");
        sb.AppendLine($"    Left {_engine.Camera.VisibleArea.Left} Right {_engine.Camera.VisibleArea.Right}");
        sb.AppendLine($"    Top {_engine.Camera.VisibleArea.Top} Bottom {_engine.Camera.VisibleArea.Bottom}");
        sb.AppendLine(string.Empty);
        sb.AppendLine($"Media Player");
        sb.AppendLine($"  Song: {MediaPlayer.Queue.ActiveSong.Name}");
        sb.AppendLine(string.Empty);
        sb.AppendLine($"Keys");

        foreach (var key in _engine.Keyboard.PressedKeys)
            sb.AppendLine($"  {key}");

        sb.AppendLine(string.Empty);

        return sb;
    }
}