using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WorldDataGenerator creates large-scale data to be contained in WorldDataContainers.

public static class WorldDataGenerator
{
    public static WorldDataContainer GenerateEmptyWorld(int worldWidth, int worldLength, int worldHeight)
    {
        WorldDataContainer worldData = new WorldDataContainer(worldWidth, worldLength, worldHeight);

        // Fill bottom layer with ground
        for (int x = 0; x < worldData.GetWidth(); x++)
        {
            for (int z = 0; z < worldData.GetLength(); z++)
            {
                worldData.SetData(x, z, 0, Tile.ID.BlockGrass);
            }
        }

        // Fill layers above with empty
        for (int y = 1; y < worldData.GetHeight(); y++)
        {
            for (int x = 0; x < worldData.GetWidth(); x++)
            {
                for (int z = 0; z < worldData.GetLength(); z++)
                {
                    worldData.SetData(x, z, y, Tile.ID.Empty);
                }
            }
        }


        // Block Walls
        for (int x = 5; x < worldData.GetWidth() - 5; x++)
        {
            worldData.SetData(x, 5, 1, Tile.ID.BlockBrick);
            worldData.SetData(x, worldData.GetLength() - 6, 1, Tile.ID.BlockBrick);
        }

        for (int z = 5; z < worldData.GetLength() - 5; z++)
        {
            worldData.SetData(5, z, 1, Tile.ID.BlockBrick);
            worldData.SetData(worldData.GetWidth() - 6, z, 1, Tile.ID.BlockBrick);
        }

        return worldData;
    }
}
