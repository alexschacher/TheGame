using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WorldObjectManager:
// - Reads data from WorldDataContainer
// - Instantiates game objects using WorldObjectFactory
// - Stores references to game objects in WorldObjectContainer
// - Can replace and destroy objects in WorldObjectContainer

public static class WorldObjectManager
{
    public static void InstantiateAllObjects(WorldDataContainer worldData, WorldObjectContainer worldObjects)
    {
        // Instantiate all objects based on IDs contained in worldData into worldObjects container
        for (int y = 0; y < worldData.GetHeight(); y++)
        {
            for (int x = 0; x < worldData.GetWidth(); x++)
            {
                for (int z = 0; z < worldData.GetLength(); z++)
                {
                    InstantiateObject(x, z, y, worldData.GetData(x, z, y), worldObjects);
                }
            }
        }
    }

    public static void InstantiateObject(int x, int z, int y, Tile.ID id, WorldObjectContainer worldObjects)
    {
        GameObject obj = WorldObjectFactory.Instantiate(x, z, y, id);

        if (obj == null) { return; }

        ObjectData objData = obj.AddComponent<ObjectData>();
        objData.Init(worldObjects, id, x, z, y);
        worldObjects.SetObject(x, z, y, obj);
        obj.transform.SetParent(worldObjects.GetParent());
    }

    public static void DestroyAllObjects(WorldObjectContainer worldObjects)
    {
        for (int y = 0; y < worldObjects.GetHeight(); y++)
        {
            for (int x = 0; x < worldObjects.GetWidth(); x++)
            {
                for (int z = 0; z < worldObjects.GetLength(); z++)
                {
                    DestroyObject(worldObjects, x, z, y);
                }
            }
        }
    }

    public static void DestroyObject(WorldObjectContainer worldObjects, int x, int z, int y)
    {
        GameObject.Destroy(worldObjects.GetObject(x, z, y));
    }

    public static void ReplaceObject(WorldObjectContainer worldObjects, int x, int z, int y, Tile.ID id)
    {
        // TODO: Check ID of object to see if it is the same, if so, do nothing.

        DestroyObject(worldObjects, x, z, y);
        InstantiateObject(x, z, y, id, worldObjects);
    }
}
