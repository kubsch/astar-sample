using RandomEngine.Entities;

namespace RandomEngine;

public class DrawEngine
{
    private readonly Texture2D _pixel;
    private readonly SpriteBatch _spriteBatch;
    private readonly GraphicsDevice _graphicsDevice;

    public DrawEngine(GraphicsDevice graphicsDevice)
    {
        _spriteBatch = new SpriteBatch(graphicsDevice);
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _graphicsDevice = graphicsDevice;
    }

    public RenderTarget2D? RenderTarget { get; private set; }

    public void Text(SpriteFontBase font, string text, Vector2 position, Color color, Vector2? scale = null)
    {
        _spriteBatch.DrawString(font, text, position, color, scale);
    }

    public void Rectangle(Rectangle rectangle, Color color, bool fill = true)
    {
        if (fill)
            _spriteBatch.Draw(_pixel, rectangle, color);
        else
            Rectangle(rectangle, color, 1);
    }

    public void Rectangle(Rectangle rectangle, Color color, int lineWidth)
    {
        Line(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Right, rectangle.Y), color, lineWidth);
        Line(new Vector2(rectangle.X + 1f, rectangle.Y), new Vector2(rectangle.X + 1f, rectangle.Bottom + lineWidth), color, lineWidth);
        Line(new Vector2(rectangle.X, rectangle.Bottom), new Vector2(rectangle.Right, rectangle.Bottom), color, lineWidth);
        Line(new Vector2(rectangle.Right + 1f, rectangle.Y), new Vector2(rectangle.Right + 1f, rectangle.Bottom + lineWidth), color, lineWidth);
    }

    public void Line(Vector2 point1, Vector2 point2, Color color, float lineWidth)
    {
        float distance = Vector2.Distance(point1, point2);
        float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

        Line(point1, distance, angle, color, lineWidth);
    }

    public void Line(Vector2 point, float length, float angle, Color color, float lineWidth)
    {
        _spriteBatch.Draw(_pixel,
                         point,
                         null,
                         color,
                         angle,
                         Vector2.Zero,
                         new Vector2(length, lineWidth),
                         SpriteEffects.None,
                         0);
    }

    public void Point(float x, float y, Color color)
    {
        Point(new Vector2(x, y), color);
    }

    public void Point(Vector2 position, Color color)
    {
        _spriteBatch.Draw(_pixel, position, color);
    }

    public void DrawCircle(Vector2 center, float radius, int sides, Color color)
    {
        Points(center, CreateCircle(radius, sides), color, 1.0f);
    }

    public void Texture(Texture2D texture, Vector2 position)
    {
        _spriteBatch.Draw(texture, position, Color.White);
    }

    public void Texture(Texture2D texture, Vector2 position, Color color)
    {
        _spriteBatch.Draw(texture, position, color);
    }

    public void Texture(Texture2D texture, Rectangle destination, Color color)
    {
        _spriteBatch.Draw(texture, destination, new Rectangle(0, 0, texture.Width, texture.Height), color);
    }

    public void Entity(Entity entity)
    {
        if (entity.CurrentAnimation == null)
            return;

        _spriteBatch.Draw(
            entity.CurrentAnimation.Texture,
            entity.Position,
            entity.CurrentAnimation.SourceRectangle,
            entity.Color,
            entity.Rotation, new Vector2(entity.CurrentAnimation.FrameWidth / 2, entity.CurrentAnimation.FrameHeight / 2),
            entity.Scale,
            entity.Directions.HasFlag(Directions.Left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
    }

    public void Begin(
        SpriteSortMode sortMode = SpriteSortMode.Deferred,
        BlendState blendState = null!,
        SamplerState samplerState = null!,
        DepthStencilState depthStencilState = null!,
        RasterizerState rasterizerState = null!,
        Effect effect = null!,
        Matrix? transformMatrix = null)
    {
        _spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
    }

    public void End()
    {
        _spriteBatch.End();
    }

    public RenderTarget2D SetRenderTargetTexture()
    {
        RenderTarget = new RenderTarget2D(
            _graphicsDevice,
            _graphicsDevice.PresentationParameters.BackBufferWidth,
            _graphicsDevice.PresentationParameters.BackBufferHeight,
            false,
            _graphicsDevice.PresentationParameters.BackBufferFormat,
            DepthFormat.Depth24);

        _graphicsDevice.SetRenderTarget(RenderTarget);

        return RenderTarget;
    }

    public void SetRenderTargetScreen()
    {
        RenderTarget = null!;
        _graphicsDevice.SetRenderTarget(RenderTarget);
    }

    private void Points(Vector2 position, List<Vector2> points, Color color, float thickness)
    {
        if (points.Count < 2)
            return;

        for (int i = 1; i < points.Count; i++)
            Line(points[i - 1] + position, points[i] + position, color, thickness);
    }

    private List<Vector2> CreateCircle(double radius, int sides)
    {
        List<Vector2> vectors = new List<Vector2>();

        const double max = 2.0 * Math.PI;
        double step = max / sides;

        for (double theta = 0.0; theta < max; theta += step)
        {
            vectors.Add(new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta))));
        }

        vectors.Add(new Vector2((float)(radius * Math.Cos(0)), (float)(radius * Math.Sin(0))));

        return vectors;
    }
}