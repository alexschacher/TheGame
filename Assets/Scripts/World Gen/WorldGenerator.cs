using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private int worldWidth = 24;
    private World world;

    public Material heightmapMaterial;
    private Texture2D texture2d;

    private float islandMaskStrength = 1f;
    private float level1PerlinMin = 0.32f;
    private float level1Threshold = 0.05f;
    private float level1PerlinScale = 7;

    private float level2Threshold = 0.50f;
    private float level2PerlinScale = 10;

    private float level3Threshold = 0.40f;
    private float level3PerlinScale = 6;

    void Start()
    {
        world = new World(worldWidth);

        texture2d = new Texture2D(worldWidth, worldWidth, TextureFormat.RGBA32, false, true);
        texture2d.filterMode = FilterMode.Point;

        GenerateEmptyWorld();
        GenerateHeightmap();

        texture2d.Apply();
        //heightmapMaterial.SetTexture("_MainTex", texture2d);

        PlaceCliffs(0);
        PlaceCliffs(1);
        PlaceCliffs(2);

        TrimCliffs(0);
        TrimCliffs(1);
        TrimCliffs(2);

        for (int i = 0; i < world.GetLevels(); i++)
        {
            FillLayerRandomOnFloor(i, World.Tile.Pfb_Flower, 0.1f, true);
            //FillLayerRandomOnFloor(i, World.Tile.Pfb_Entity_Slime, 0.02f, true);
        }

        WorldLoader.InstantiateWorld(world);
    }
     
    private void GenerateEmptyWorld()
    {
        FillLayer(0, World.Layer.Floor, World.Tile.Pfb_Floor_Small);
        FillLayer(1, World.Layer.Floor, World.Tile.Empty);
        FillLayer(2, World.Layer.Floor, World.Tile.Empty);
        FillLayer(3, World.Layer.Floor, World.Tile.Empty);

        FillLayer(0, World.Layer.Object, World.Tile.Empty);
        FillLayer(1, World.Layer.Object, World.Tile.Empty);
        FillLayer(2, World.Layer.Object, World.Tile.Empty);
        FillLayer(3, World.Layer.Floor, World.Tile.Empty);
    }

    private void GenerateHeightmap()
    {
        float[,] heightmap;

        /* Generate Perlin Map and Normalize to 0.2 to 1.0 Range */
        heightmap = HeightmapGenerator.GeneratePerlinNoise(worldWidth, level1PerlinScale, true);
        heightmap = HeightmapGenerator.ScaleHeightmapRange(heightmap, 0, 1, level1PerlinMin, 1);

        /* Add Island Mask To Perlin Map */
        float[,] islandMaskHeightmap = HeightmapGenerator.GenerateIslandMask(worldWidth, islandMaskStrength);

        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldWidth; y++)
            {
                heightmap[x, y] -= islandMaskHeightmap[x, y];

                /* For Debug Purposes, Apply To a Texture */
                texture2d.SetPixel(x, y, new Color(heightmap[x, y], heightmap[x, y], heightmap[x, y]));
            }
        }

        /* Set World Data to Heightmaps */
        RaiseTerrain(heightmap, level1Threshold, 1);

        heightmap = HeightmapGenerator.GeneratePerlinNoise(worldWidth, level2PerlinScale, true);
        RaiseTerrain(heightmap, level2Threshold, 2);

        heightmap = HeightmapGenerator.GeneratePerlinNoise(worldWidth, level3PerlinScale, true);
        RaiseTerrain(heightmap, level3Threshold, 3);

        /* Replace Bottom Level Floor with Water */
        ReplaceLayer(0, World.Layer.Floor, World.Tile.Pfb_Floor_Small, World.Tile.Pfb_Water);
    }

    private void RaiseTerrain(float[,] heightmap, float heightThreshold, int levelRaisedTo)
    {
        for (int x = 0; x < heightmap.GetLength(0); x++)
        {
            for (int y = 0; y < heightmap.GetLength(1); y++)
            {
                // Raise floor in world data based on heightmap
                if (heightmap[x, y] > heightThreshold)
                {
                    // Only raise if 9 adjacent tiles below are not empty, in order to only allow "pyramid" terrain
                    if (world.GetTile(x, y, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x - 1, y, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x + 1, y, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x, y - 1, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x, y + 1, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x - 1, y + 1, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x - 1, y - 1, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x + 1, y + 1, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty ||
                        world.GetTile(x + 1, y - 1, levelRaisedTo - 1, World.Layer.Floor) == World.Tile.Empty)
                    {
                        goto NextIteration;
                    }

                    // Raise Level
                    world.SetTile(x, y, levelRaisedTo, World.Layer.Floor, World.Tile.Pfb_Floor_Small);
                    world.SetTile(x, y, levelRaisedTo - 1, World.Layer.Floor, World.Tile.Underground);
                }
            NextIteration: continue;
            }
        }
    }

    private void PlaceCliffs(int level)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldWidth; y++)
            {
                World.Tile tile = world.GetTile(x, y, level, World.Layer.Floor);
                if (tile == World.Tile.Pfb_Floor_Small ||
                    tile == World.Tile.Pfb_Water)
                {
                    for (int xx = -1; xx < 2; xx++)
                    {
                        for (int yy = -1; yy < 2; yy++)
                        {
                            if (x + xx >= 0 && x + xx < worldWidth && y + yy >= 0 && y + yy < worldWidth)
                            {
                                if (world.GetTile(x + xx, y + yy, level, World.Layer.Floor) == World.Tile.Underground)
                                {
                                    world.SetTile(x, y, level, World.Layer.Floor, World.Tile.Cliff);
                                    goto NextIteration;
                                }
                            }
                        }
                    }
                }
                NextIteration: continue;
            }
        }
    }

    private void TrimCliffs(int level)
    {
        // create grid mockup: remember this stage cliffs are just one unit
        
        // check if adjacent on any 4 side to floor
        // if not, check if adjacent on any 4 corner to floor
        // if not, this cliff is illegal
        // push this cliff up a level
    }

    private void ReplaceLayer(int level, World.Layer layer, World.Tile oldTile, World.Tile newTile)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldWidth; y++)
            {
                if (world.GetTile(x, y, level, layer) == oldTile)
                {
                    world.SetTile(x, y, level, layer, newTile);
                }
            }
        }
    }

    private void FillLayer(int level, World.Layer layer, World.Tile tile)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldWidth; y++)
            {
                world.SetTile(x, y, level, layer, tile);
            }
        }
    }

    private void FillLayerRandomOnFloor(int level, World.Tile tile, float percentage, bool onlyPlaceOnEmptySpot)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldWidth; y++)
            {
                // If there is floor below
                if (world.GetTile(x, y, level, World.Layer.Floor) == World.Tile.Pfb_Floor_Small)
                {
                    // Check if spot is open or not if applicable
                    if (world.GetTile(x, y, level, World.Layer.Object) != World.Tile.Empty && onlyPlaceOnEmptySpot)
                    {
                        // Do nothing if not empty
                    }
                    else
                    {
                        // Chance to place object
                        float randomFloat = Random.Range(0f, 1f);
                        if (randomFloat < percentage)
                        {
                            // Place object
                            world.SetTile(x, y, level, World.Layer.Object, tile);
                        }
                    }
                }
            }
        }
    }

    private void FillLayerRandom(int level, World.Layer layer, World.Tile tile, float percentage)
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldWidth; y++)
            {
                // Chance to place object
                float randomFloat = Random.Range(0f, 1f);
                if (randomFloat < percentage)
                {
                    // Place object
                    world.SetTile(x, y, level, layer, tile);
                }
            }
        }
    }
}