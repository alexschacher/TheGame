using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    private int width;
    private int levels;
    private Tile[,,,] tiles;

    public enum Layer
    {
        Floor,
        Object
    }
    public enum Tile : byte
    {
        Empty,
        Underground,
        Pfb_Floor_Small,
        Pfb_Water,
        Pfb_Flower,
        Pfb_Entity_Slime,
        Cliff
    }

    public World(int width)
    {
        this.width = width;
        this.levels = 4;
        tiles = new Tile[width, width, levels, 2];
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetLevels()
    {
        return levels;
    }

    public Tile GetTile(int x, int y, int level, Layer layer)
    {
        return tiles[x, y, level, (int)layer];
    }

    public Tile GetTile(int x, int y, int level, int layer)
    {
        return tiles[x, y, level, layer];
    }

    public void SetTile(int x, int y, int level, Layer layer, Tile tile)
    {
        tiles[x, y, level, (int)layer] = tile;
    }
}