using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldLoader
{
    public static void InstantiateWorld(World world)
    {
        for (int level = 0; level < world.GetLevels(); level++)
        {
            for (int layer = 0; layer < 2; layer++)
            {
                for (int x = 0; x < world.GetWidth(); x++)
                {
                    for (int y = 0; y < world.GetWidth(); y++)
                    {
                        World.Tile tile = world.GetTile(x, y, level, layer);

                        // TODO: How to find prefabs in folders without having to explicitly name them, so that a majority
                        // of prefabs can be ran through one simple code instead of cases for every single prefab?
                        // How to avoid just having to move everything out into one massive folder?

                        if (tile == World.Tile.Pfb_Floor_Small)
                        {
                            Instantiate("Small Grid Test/" + tile.ToString(), x, level, y, 0);
                        }

                        else if (tile == World.Tile.Pfb_Water)
                        {
                            Instantiate("Terrain/" + tile.ToString(), x, level, y, 0);
                        }

                        else if (tile == World.Tile.Pfb_Entity_Slime)
                        {
                            Instantiate("Entities/" + tile.ToString(), x, level, y, 0);
                        }

                        else if (tile == World.Tile.Pfb_Flower)
                        {
                            // TEMP: Trickery to only place flowers in a checkerboard grid to avoid adjacent flowers
                            // This leaves ghost flowers in the data array and this code should be changed or removed
                            if (Mathf.Abs(x - y) % 2 == 0)
                            {
                                Instantiate("Foliage/" + tile.ToString(), x, level, y, 0);
                            }
                        }

                        else if (tile == World.Tile.Cliff)
                        {
                            SpawnCliffs(x, y, level, layer, world);
                        }
                    }
                }
            }
        }
    }

    private static void SpawnCliffs(int x, int y, int level, int layer, World world)
    {
        // Ignore edges of map to avoid out of index error
        if (x == 0 || y == 0 || x == world.GetWidth() - 1 || y == world.GetWidth() - 1)
        {
            return;
        }

        // Spawn water bottom level
        if (level == 0)
        {
            Instantiate("Terrain/Pfb_Water", x, level, y, 0);
        }

        bool side = false;

        World.Tile leftTile = world.GetTile(x - 1, y, level, layer);
        World.Tile rightTile = world.GetTile(x + 1, y, level, layer);
        World.Tile topTile = world.GetTile(x, y - 1, level, layer);
        World.Tile bottomTile = world.GetTile(x, y + 1, level, layer);
        World.Tile topleftTile = world.GetTile(x - 1, y - 1, level, layer);
        World.Tile toprightTile = world.GetTile(x + 1, y - 1, level, layer);
        World.Tile bottomleftTile = world.GetTile(x - 1, y + 1, level, layer);
        World.Tile bottomrightTile = world.GetTile(x + 1, y + 1, level, layer);

        // Left Cliff
        if (leftTile == World.Tile.Underground)
        {
            Instantiate("Terrain/Pfb_Cliff_Short", x, level, y, 180);
            side = true;
        }

        // Right Cliff
        if (rightTile == World.Tile.Underground)
        {
            Instantiate("Terrain/Pfb_Cliff_Short", x, level, y, 0);
            side = true;
        }

        // Top Cliff
        if (topTile == World.Tile.Underground)
        {
            Instantiate("Terrain/Pfb_Cliff_Short", x, level, y, 90);
            side = true;
        }

        // Bottom Cliff
        if (bottomTile == World.Tile.Underground)
        {
            Instantiate("Terrain/Pfb_Cliff_Short", x, level, y, 270);
            side = true;
        }

        // Place floor if no side, meaning only corner
        if (side == false)
        {
            Instantiate("Small Grid Test/Pfb_Floor_Small", x, level, y, 0);
        }

        //Top Right Corner
        if (toprightTile == World.Tile.Underground &&
            topTile == World.Tile.Cliff &&
            rightTile == World.Tile.Cliff)
        {
            Instantiate("Terrain/Pfb_Cliff_Corner_Short", x, level, y, 90);
        }

        // Top Left Corner
        if (topleftTile == World.Tile.Underground &&
            topTile == World.Tile.Cliff &&
            leftTile == World.Tile.Cliff)
        {
            Instantiate("Terrain/Pfb_Cliff_Corner_Short", x, level, y, 180);
        }

        // Bottom Right Corner
        if (bottomrightTile == World.Tile.Underground &&
            bottomTile == World.Tile.Cliff &&
            rightTile == World.Tile.Cliff)
        {
            Instantiate("Terrain/Pfb_Cliff_Corner_Short", x, level, y, 0);
        }

        // Bottom Left Corner
        if (bottomleftTile == World.Tile.Underground &&
            bottomTile == World.Tile.Cliff &&
            leftTile == World.Tile.Cliff)
        {
            Instantiate("Terrain/Pfb_Cliff_Corner_Short", x, level, y, 270);
        }
    }

    private static GameObject Instantiate(System.String prefabPath, float x, float y, float z, float r)
    {
        GameObject obj =
            GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/" + prefabPath),
            new Vector3(x / 2f, y * 0.75f, z / 2f),
            Quaternion.Euler(0, r, 0))
            as GameObject;
        return obj;
    }
}
