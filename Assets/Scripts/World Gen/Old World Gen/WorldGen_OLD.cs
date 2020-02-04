using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    private int worldDepth = System.Enum.GetNames(typeof(Tile.Layer)).Length;
    private int worldWidth = 64;
    private int worldHeight = 64;

    private Tile.Type[,,] worldData;

    void Start()
    {
        worldData = new Tile.Type[worldDepth, worldWidth, worldHeight];

        FillLayer(Tile.Layer.FLR_1, Tile.Type.Ground);

        GenerateForestClump(Tile.Layer.OBJ_1, 32, 32);

        //GenerateRandom(Tile.Layer.OBJ_1, Tile.Type.Tree, 25, true);
        //GenerateRandom(Tile.Layer.OBJ_1, Tile.Type.Slime, 15, false);
        //GenerateRandom(Tile.Layer.OBJ_1, Tile.Type.Shrub, 25, false);

        InstantiateWorld();
    }


    // Basic Functions

    private void FillLayer(Tile.Layer layer, Tile.Type obj)
    {
        for (int x = 0; x < worldWidth; x += 2) { for (int z = 0; z < worldHeight; z += 2)
        {
            worldData[(int)layer, x, z] = obj;
        }}
    }

    private void GenerateRandom(Tile.Layer layer, Tile.Type obj, int percent, bool blockAdjacent)
    {
        for (int x = 0; x < worldWidth; x++) { for (int z = 0; z < worldHeight; z++)
        {
            if (Random.Range(0, 100) < percent)
            {
                if (worldData[(int)layer, x, z] == Tile.Type.Open)
                {
                    worldData[(int)layer, x, z] = obj;

                    if (blockAdjacent)
                    {
                        BlockAdjacent(layer, x, z);
                    }
                }
            }
        }}
    }

    private void BlockAdjacent(Tile.Layer layer, int x, int z)
    {
        for (int xx = -1; xx <= 1; xx++) { for (int zz = -1; zz <= 1; zz++)
        {
            if ((xx != 0 || zz != 0)
               && x + xx >= 0
               && z + zz >= 0
               && x + xx < worldWidth
               && z + zz < worldHeight)
            {
                if (worldData[(int)layer, x + xx, z + zz] == Tile.Type.Open)
                {
                    worldData[(int)layer, x + xx, z + zz] = Tile.Type.Blocked;
                }
            }
        }}
    }


    // Forest Clump Generation

    private void GenerateForestClump(Tile.Layer layer, int originX, int originY)
    {
        // Init lists
        List<Vector2Int> activeSpots = new List<Vector2Int>();

        List<Vector2Int> relativeSpotsToBlock = new List<Vector2Int>();
        {
            relativeSpotsToBlock.Add(new Vector2Int(1, 1));
            relativeSpotsToBlock.Add(new Vector2Int(1, 0));
            relativeSpotsToBlock.Add(new Vector2Int(1, -1));
            relativeSpotsToBlock.Add(new Vector2Int(0, 1));
            relativeSpotsToBlock.Add(new Vector2Int(0, -1));
            relativeSpotsToBlock.Add(new Vector2Int(-1, -1));
            relativeSpotsToBlock.Add(new Vector2Int(-1, 0));
            relativeSpotsToBlock.Add(new Vector2Int(-1, 1));

            relativeSpotsToBlock.Add(new Vector2Int(-2, 0));
            relativeSpotsToBlock.Add(new Vector2Int(2, 0));
            relativeSpotsToBlock.Add(new Vector2Int(0, 2));
            relativeSpotsToBlock.Add(new Vector2Int(0, -2));

            //relativeSpotsToBlock.Add(new Vector2Int(1, -2));
            //relativeSpotsToBlock.Add(new Vector2Int(-1, -2));
            //relativeSpotsToBlock.Add(new Vector2Int(1, 2));
            //relativeSpotsToBlock.Add(new Vector2Int(-1, 2));
            //relativeSpotsToBlock.Add(new Vector2Int(2, 1));
            //relativeSpotsToBlock.Add(new Vector2Int(2, -1));
            //relativeSpotsToBlock.Add(new Vector2Int(-2, 1));
            //relativeSpotsToBlock.Add(new Vector2Int(-2, -1));
        }

        List<Vector2Int> relativeSpotsToCheck = new List<Vector2Int>();
        {
            relativeSpotsToCheck.Add(new Vector2Int(1, -2));
            relativeSpotsToCheck.Add(new Vector2Int(-1, -2));
            relativeSpotsToCheck.Add(new Vector2Int(1, 2));
            relativeSpotsToCheck.Add(new Vector2Int(-1, 2));
            relativeSpotsToCheck.Add(new Vector2Int(2, 1));
            relativeSpotsToCheck.Add(new Vector2Int(2, -1));
            relativeSpotsToCheck.Add(new Vector2Int(-2, 1));
            relativeSpotsToCheck.Add(new Vector2Int(-2, -1));

            relativeSpotsToCheck.Add(new Vector2Int(2, 2));
            relativeSpotsToCheck.Add(new Vector2Int(-2, 2));
            relativeSpotsToCheck.Add(new Vector2Int(2, -2));
            relativeSpotsToCheck.Add(new Vector2Int(-2, -2));

            relativeSpotsToCheck.Add(new Vector2Int(3, 3));
            relativeSpotsToCheck.Add(new Vector2Int(-3, 3));
            relativeSpotsToCheck.Add(new Vector2Int(3, -3));
            relativeSpotsToCheck.Add(new Vector2Int(-3, -3));

            relativeSpotsToCheck.Add(new Vector2Int(3, 2));
            relativeSpotsToCheck.Add(new Vector2Int(3, 1));
            relativeSpotsToCheck.Add(new Vector2Int(3, 0));
            relativeSpotsToCheck.Add(new Vector2Int(3, -1));
            relativeSpotsToCheck.Add(new Vector2Int(3, -2));

            relativeSpotsToCheck.Add(new Vector2Int(-3, 2));
            relativeSpotsToCheck.Add(new Vector2Int(-3, 1));
            relativeSpotsToCheck.Add(new Vector2Int(-3, 0));
            relativeSpotsToCheck.Add(new Vector2Int(-3, -1));
            relativeSpotsToCheck.Add(new Vector2Int(-3, -2));

            relativeSpotsToCheck.Add(new Vector2Int(2, -3));
            relativeSpotsToCheck.Add(new Vector2Int(1, -3));
            relativeSpotsToCheck.Add(new Vector2Int(0, -3));
            relativeSpotsToCheck.Add(new Vector2Int(-1, -3));
            relativeSpotsToCheck.Add(new Vector2Int(-2, -3));

            relativeSpotsToCheck.Add(new Vector2Int(2, 3));
            relativeSpotsToCheck.Add(new Vector2Int(1, 3));
            relativeSpotsToCheck.Add(new Vector2Int(0, 3));
            relativeSpotsToCheck.Add(new Vector2Int(-1, 3));
            relativeSpotsToCheck.Add(new Vector2Int(-2, 3));

            relativeSpotsToCheck.Add(new Vector2Int(-4, 1));
            relativeSpotsToCheck.Add(new Vector2Int(-4, 0));
            relativeSpotsToCheck.Add(new Vector2Int(-4, -1));

            relativeSpotsToCheck.Add(new Vector2Int(4, 1));
            relativeSpotsToCheck.Add(new Vector2Int(4, 0));
            relativeSpotsToCheck.Add(new Vector2Int(4, -1));

            relativeSpotsToCheck.Add(new Vector2Int(1, 4));
            relativeSpotsToCheck.Add(new Vector2Int(0, 4));
            relativeSpotsToCheck.Add(new Vector2Int(-1, 4));

            relativeSpotsToCheck.Add(new Vector2Int(1, -4));
            relativeSpotsToCheck.Add(new Vector2Int(0, -4));
            relativeSpotsToCheck.Add(new Vector2Int(-1, -4));
        }

        // Add original spot and block nearby
        // WARNING: This is repeated code! Create a method
        activeSpots.Add(new Vector2Int(originX, originY));
        worldData[(int)layer, originX, originY] = Tile.Type.Tree;
        foreach (Vector2Int spotsToBlock in relativeSpotsToBlock)
        {
            if (worldData[(int)layer, originX + spotsToBlock.x, originY + spotsToBlock.y] == Tile.Type.Open)
            {
                worldData[(int)layer, originX + spotsToBlock.x, originY + spotsToBlock.y] = Tile.Type.Blocked;
            }
        }

        // Clump creation
        while (activeSpots.Count > 0)
        {
            // Choose random active spot
            Vector2Int spot = activeSpots[Random.Range(0, activeSpots.Count)];
            
            // Shuffle spotsToCheck
            for (int i = 0; i < relativeSpotsToCheck.Count; i++)
            {
                Vector2Int temp = relativeSpotsToCheck[i];
                int randomIndex = Random.Range(i, relativeSpotsToCheck.Count);
                relativeSpotsToCheck[i] = relativeSpotsToCheck[randomIndex];
                relativeSpotsToCheck[randomIndex] = temp;
            }

            // Loop through spotsToCheck
            for (int i = 0; i < relativeSpotsToCheck.Count; i++)
            {
                // If open spot found, create new active spot, end loop
                if (worldData[(int)layer, spot.x + relativeSpotsToCheck[i].x, spot.y + relativeSpotsToCheck[i].y] == Tile.Type.Open)
                {
                    // WARNING: This is repeated code! Create a method

                    // !! Location filtering for testing only, normally activeSpots.Add() would always happen
                    if (spot.x + relativeSpotsToCheck[i].x < 50 &&
                        spot.x + relativeSpotsToCheck[i].x > 14 &&
                        spot.y + relativeSpotsToCheck[i].y < 50 &&
                        spot.y + relativeSpotsToCheck[i].y > 14)
                    {
                        activeSpots.Add(new Vector2Int(spot.x + relativeSpotsToCheck[i].x, spot.y + relativeSpotsToCheck[i].y));
                    }
                    worldData[(int)layer, spot.x + relativeSpotsToCheck[i].x, spot.y + relativeSpotsToCheck[i].y] = Tile.Type.Tree;
                    foreach (Vector2Int spotsToBlock in relativeSpotsToBlock)
                    {
                        if (worldData[(int)layer,
                            spot.x + relativeSpotsToCheck[i].x + spotsToBlock.x,
                            spot.y + relativeSpotsToCheck[i].y + spotsToBlock.y] == Tile.Type.Open)
                        {
                            worldData[(int)layer,
                                spot.x + relativeSpotsToCheck[i].x + spotsToBlock.x,
                                spot.y + relativeSpotsToCheck[i].y + spotsToBlock.y] = Tile.Type.Blocked;
                        }
                    }
                    // Terminate loop, go to choosing a new active spot
                    break;
                }

                // If no open spots found by the end of loop, remove this from active spots
                activeSpots.Remove(spot);
            }
        }
    }


    // Instantiation

    private void InstantiateWorld()
    {
        for (int x = 0; x < worldWidth; x++) { for (int z = 0; z < worldHeight; z++)
        {
            // LAYER FLOOR 1
            if ((int)worldData[(int)Tile.Layer.FLR_1, x, z] >= Tile.EMPTY_TILE_TYPES)
            {
                Instantiate(
                    Resources.Load<GameObject>("Prefabs/" +
                    worldData[(int)Tile.Layer.FLR_1, x, z].ToString()),
                    x, -0.5f, z);
            }

            // LAYER OBJECT 1
            if ((int)worldData[(int)Tile.Layer.OBJ_1, x, z] >= Tile.EMPTY_TILE_TYPES)
            {
                Instantiate(
                    Resources.Load<GameObject>("Prefabs/" +
                    worldData[(int)Tile.Layer.OBJ_1, x, z].ToString()),
                    x, 0, z);
            }
        }}
    }

    private void Instantiate(GameObject prefab, float x, float y, float z)
    {
        GameObject obj = Instantiate(prefab, new Vector3(x / 2, y, z / 2), Quaternion.identity, transform) as GameObject;
    }
}
