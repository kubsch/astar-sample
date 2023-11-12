using System.Windows.Forms;

namespace RandomEngine.Camera;

public class CameraEngine
{
    private float _zoom = 2f;

    public CameraEngine(Viewport viewport)
    {
        Viewport = viewport;
    }

    public float Zoom
    {
        get
        {
            return _zoom;
        }
        set
        {
            _zoom = Math.Clamp(value, MinZoom, MaxZoom); ;
        }
    }

    public int X { get; set; }
    public int Y { get; set; }
    public Rectangle VisibleArea { get; private set; }
    public Matrix Transform { get; private set; }
    public float ZoomStep { get; set; } = .05f;
    public float MinZoom { get; set; } = 1f;
    public float MaxZoom { get; set; } = 2.5f;

    public List<ICameraBehaviour> Behaviours { get; } = new();
    public Viewport Viewport { get; private set; }

    public float ZoomIn()
    {
        return Zoom += ZoomStep;
    }

    public float ZoomOut()
    {
        return Zoom -= ZoomStep;
    }

    public Vector2 ScreenToWorld(Vector2 screenCoordinates)
    {
        var invertedTransformMatrix = Matrix.Invert(Transform);
        var positionWorldVector = Vector2.Transform(screenCoordinates, invertedTransformMatrix);

        return positionWorldVector;
    }

    public Vector2 WorldToScreen(Vector2 worldCoordinates)
    {
        var positionScreenVector = Vector2.Transform(worldCoordinates, Transform);

        return positionScreenVector;
    }

    internal void Update(GameTime gameTime, Viewport viewport)
    {
        Viewport = viewport;
        UpdateTransform(viewport);

        foreach (var behaviour in Behaviours)
            behaviour.Update(this);
    }

    private void UpdateTransform(Viewport viewport)
    {
        Transform =
            Matrix.CreateTranslation(new Vector3(-X, -Y, 0)) *
            Matrix.CreateScale(Zoom) *
            Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));

        UpdateVisibleArea(viewport);
    }

    private void UpdateVisibleArea(Viewport viewport)
    {
        var inverseViewMatrix = Matrix.Invert(Transform);

        var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
        var tr = Vector2.Transform(new Vector2(viewport.X, 0), inverseViewMatrix);
        var bl = Vector2.Transform(new Vector2(0, viewport.Y), inverseViewMatrix);
        var br = Vector2.Transform(new Vector2(viewport.Width, viewport.Height), inverseViewMatrix);

        var min = new Vector2(
            MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
            MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
        var max = new Vector2(
            MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
            MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));

        VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
    }
}