using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WorldObjectFactory is a tool for WorldObjectManager that:
// - Creates game objects based off of a list of creation instructions and returns that object.

public static class WorldObjectFactory
{
    private static GameObject InstantiateGameObject(GameObject prefab, int x, int z, int y, float r, Tile.ID id)
    {
        return GameObject.Instantiate(prefab, new Vector3(x, y * 0.5f, z), Quaternion.Euler(0, r, 0)) as GameObject;
    }

    public static GameObject Instantiate(int x, int z, int y, Tile.ID id)
    {
        GameObject obj;

        switch (id)
        {
            case (Tile.ID.Empty): return null;
            case (Tile.ID.EmptyButBlocked): return null;

            case (Tile.ID.BlockGrass):
                obj = InstantiateGameObject(Resource.pfb["Block"], x, z, y, 0, id);
                obj.GetComponent<Block>().Init(Resource.mat["Grass"], Resource.mat["Dirt"], true);
                return obj;

            case (Tile.ID.BlockDirt):
                obj = InstantiateGameObject(Resource.pfb["Block"], x, z, y, 0, id);
                obj.GetComponent<Block>().Init(Resource.mat["Dirt"], Resource.mat["Dirt"], true);
                return obj;

            case (Tile.ID.BlockBrick):
                obj = InstantiateGameObject(Resource.pfb["Block"], x, z, y, 0, id);
                obj.GetComponent<Block>().Init(Resource.mat["Brick"], Resource.mat["Brick"], false);
                return obj;

            case (Tile.ID.Tree):
                obj = InstantiateGameObject(Resource.pfb["Tree"], x, z, y, 0, id);
                return obj;

            case (Tile.ID.Grass):
                obj = InstantiateGameObject(Resource.pfb["Grass"], x, z, y, 0, id);
                return obj;

            default: return null;
        }
    }
}
