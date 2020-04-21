using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    private WorldObjectContainer worldObjectContainer;
    private Tile.ID id;
    private int x, z, y;

    public void Init(WorldObjectContainer worldObjectContainer, Tile.ID id, int x, int z, int y)
    {
        this.worldObjectContainer = worldObjectContainer;
        this.id = id;
        this.x = x;
        this.z = z;
        this.y = y;
    }

    public Tile.ID GetID()
    {
        return id;
    }

    public WorldObjectContainer GetWorldObjectContainer()
    {
        return worldObjectContainer;
    }

    public int GetX()
    {
        return x;
    }

    public int GetZ()
    {
        return z;
    }

    public int GetY()
    {
        return y;
    }
}
