using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WorldDataContainer holds a multidimensional array of Tile IDs.

public class WorldDataContainer
{
    private Tile.ID[,,] worldData;

    public WorldDataContainer(int worldWidth, int worldLength, int worldHeight)
    {
        worldData = new Tile.ID[worldWidth, worldLength, worldHeight];
    }

    public Tile.ID[,] GetNeighbors(int x, int z, int y)
    {
        Tile.ID[,] neighbors = new Tile.ID[3, 3];

        for (int ix = 0; ix <= 2; ix++)
        {
            for (int iz = 0; iz <= 2; iz++)
            {
                if (ix + x - 1 >= 0 && iz + z - 1 >= 0 && ix + x - 1 < GetWidth() && iz + z - 1 < GetLength())
                {
                    neighbors[ix, iz] = worldData[ix + x - 1, iz + z - 1, y];
                }
            }
        }

        return neighbors;
    }

    public void SetData(int x, int z, int y, Tile.ID id)
    {
        worldData[x, z, y] = id;
    }

    public Tile.ID GetData(int x, int z, int y)
    {
        if (x < 0 || y < 0 || z < 0 || x >= GetWidth() || y >= GetHeight() || z >= GetLength())
        {
            return Tile.ID.Empty;
        }
        return worldData[x, z, y];
    }

    public int GetWidth()
    {
        return worldData.GetLength(0);
    }

    public int GetLength()
    {
        return worldData.GetLength(1);
    }

    public int GetHeight()
    {
        return worldData.GetLength(2);
    }
}
