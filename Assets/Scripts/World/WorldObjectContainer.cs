using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WorldDataContainer:
// - Holds a multidimensional array of references to live game objects,
// - Is a live mirror image of a WorldDataContainer,
// - Holds a reference to the parent game object which contains all live game objects as children.

public class WorldObjectContainer
{
    private GameObject[,,] worldObjects;
    private Transform parent;
    private WorldDataContainer worldData;

    public WorldObjectContainer(int worldWidth, int worldLength, int worldHeight, Transform parent, WorldDataContainer worldData)
    {
        worldObjects = new GameObject[worldWidth, worldLength, worldHeight];
        this.worldData = worldData;
        this.parent = parent;
    }

    public void SetObject(int x, int z, int y, GameObject obj)
    {
        if (worldObjects[x, z, y] != null)
        {
            WorldObjectManager.DestroyObject(this, x, z, y);
        }
        worldObjects[x, z, y] = obj;
    }

    public GameObject GetObject(int x, int z, int y)
    {
        return worldObjects[x, z, y];
    }

    public GameObject[,] GetNeighbors(int x, int z, int y)
    {
        GameObject[,] neighbors = new GameObject[3, 3];

        for (int ix = 0; ix <= 2; ix++)
        {
            for (int iz = 0; iz <= 2; iz++)
            {
                if (ix + x - 1 >= 0 && iz + z - 1 >= 0 && ix + x - 1 < GetWidth() && iz + z - 1 < GetLength())
                {
                    neighbors[ix, iz] = worldObjects[ix + x - 1, iz + z - 1, y];
                }
            }
        }

        return neighbors;
    }

    public Transform GetParent()
    {
        return parent;
    }

    public WorldDataContainer GetWorldData()
    {
        return worldData;
    }

    public int GetWidth()
    {
        return worldObjects.GetLength(0);
    }

    public int GetLength()
    {
        return worldObjects.GetLength(1);
    }

    public int GetHeight()
    {
        return worldObjects.GetLength(2);
    }
}
