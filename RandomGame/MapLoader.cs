using RandomEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomGame;

public class MapLoader
{
    private Tileset<TileType> _tileset;

    public MapLoader()
    {
        _tileset = new Tileset<TileType>();

        _tileset.AddTile(TileType.Undefined, Assets.Textures.White);
        _tileset.AddTile(TileType.Grass, Assets.Textures.Grass);
        _tileset.AddTile(TileType.Tree, Assets.Textures.Tree);
    }

    public TileMapEngine<TileType> Load(CameraEngine cameraEngine)
    {
        var map = new TileMapEngine<TileType>(_tileset, 16, 16, 96, cameraEngine);

        for (var x = 0; x < map.Width; x++)
            for (var y = 0; y < map.Height; y++)
                map[x, y] = TileType.Grass;

        for (var x = 0; x < map.Width; x++)
        {
            map[x, 0] = TileType.Tree;
            map[x, map.Height - 1] = TileType.Tree;
        }

        for (var y = 0; y < map.Width; y++)
        {
            map[0, y] = TileType.Tree;
            map[map.Width - 1, y] = TileType.Tree;
        }

        map[2, 3] = TileType.Tree;
        map[3, 3] = TileType.Tree;
        map[4, 3] = TileType.Tree;
        map[5, 3] = TileType.Tree;
        map[6, 3] = TileType.Tree;
        map[7, 3] = TileType.Tree;
        map[8, 3] = TileType.Tree;

        map[6, 6] = TileType.Tree;
        map[7, 6] = TileType.Tree;
        map[8, 6] = TileType.Tree;
        map[9, 6] = TileType.Tree;
        map[10, 6] = TileType.Tree;

        map[9, 10] = TileType.Tree;
        map[9, 11] = TileType.Tree;
        map[9, 12] = TileType.Tree;
        map[10, 10] = TileType.Tree;
        map[10, 11] = TileType.Tree;
        map[10, 12] = TileType.Tree;
        map[11, 10] = TileType.Tree;
        map[11, 11] = TileType.Tree;
        map[11, 12] = TileType.Tree;

        return map;
    }
}