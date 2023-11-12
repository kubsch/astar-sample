namespace RandomEngine;

public class Tileset<T>
    where T : Enum
{
    private readonly Dictionary<T, Texture2D> _textures = new();

    public Texture2D this[T tile]
    {
        get
        {
            return _textures[tile];
        }
    }

    public void AddTile(T tile, Texture2D texture)
    {
        _textures.Add(tile, texture);
    }
}