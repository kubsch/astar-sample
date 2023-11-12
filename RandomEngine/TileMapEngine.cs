using RandomEngine.Camera;

namespace RandomEngine;

public class TileMapEngine<TTileEnum>
    where TTileEnum : Enum
{
    private readonly CameraEngine _camera;
    private TTileEnum[,] _tiles;
    private Tileset<TTileEnum> _tileSet;

    public TileMapEngine(Tileset<TTileEnum> tileSet, int width, int height, int tileSize, CameraEngine camera)
    {
        Width = width;
        Height = height;
        TileSize = tileSize;
        _camera = camera;
        _tiles = new TTileEnum[width, height];
        _tileSet = tileSet;
    }

    public TTileEnum this[int x, int y]
    {
        get
        {
            return _tiles[x, y];
        }
        set
        {
            _tiles[x, y] = value;
        }
    }

    public int Width { get; }
    public int Height { get; }
    public int TileSize { get; }

    public void Draw(Engine engine, GameTime gameTime)
    {
        var culled = CullMap(_camera.Viewport.Width, _camera.Viewport.Height, TileSize);

        for (var x = culled.Left; x < culled.Right; x++)
            for (var y = culled.Top; y < culled.Bottom; y++)
            {
                var tileRect = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                engine.Render.Texture(_tileSet[this[x, y]], tileRect, Color.White);
            }
    }

    public void Update(GameTime gameTime)
    {
    }

    private Rectangle CullMap(float viewportWidth, float viewportHeight, int tileSize)
    {
        // Ausrechnen, welche Tiles überhaupt im Viewport sind

        const int offset = 2;

        var vpWidth = viewportWidth / _camera.Zoom;
        var vpHeight = viewportHeight / _camera.Zoom;

        var left = (int)((_camera.X - vpWidth / 2) / tileSize);
        var top = (int)((_camera.Y - vpHeight / 2) / tileSize);
        var right = (int)(left + vpWidth / tileSize);
        var bottom = (int)(top + vpHeight / tileSize);

        left = Math.Clamp(left - offset, 0, Width);
        top = Math.Clamp(top - offset, 0, Height);
        right = Math.Clamp(right + offset, 0, Width);
        bottom = Math.Clamp(bottom + offset, 0, Width);

        return new Rectangle(left, top, right - left, bottom - top);
    }
}