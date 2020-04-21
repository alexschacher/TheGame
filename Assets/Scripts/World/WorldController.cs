using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WorldController manages all world containers and functions.

public class WorldController : MonoBehaviour
{
    private WorldDataContainer worldData;
    private WorldObjectContainer worldObjects;

    [SerializeField] private int worldWidth = 16;
    [SerializeField] private int worldLength = 16;
    [SerializeField] private int worldHeight = 4;

    public void Init()
    {
        worldData = WorldDataGenerator.GenerateEmptyWorld(worldWidth, worldLength, worldHeight);
        worldObjects = new WorldObjectContainer(worldWidth, worldLength, worldHeight, this.transform, worldData);

        WorldObjectManager.InstantiateAllObjects(worldData, worldObjects);
    }

    public void ModifyWorldData(int x, int z, int y, Tile.ID id)
    {
        if (x < worldData.GetWidth() && z < worldData.GetLength() && y < worldData.GetHeight() && x >= 0 && z >= 0 && y >= 0)
        {
            worldData.SetData(x, z, y, id);
            WorldObjectManager.ReplaceObject(worldObjects, x, z, y, id);
        }
    }

    public WorldDataContainer GetWorldDataContainer()
    {
        return worldData;
    }

    public WorldObjectContainer GetWorldObjectContainer()
    {
        return worldObjects;
    }
}
