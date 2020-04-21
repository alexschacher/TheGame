using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_OLD
{
    public enum Layer { FLR_1, OBJ_1, FLR_2, OBJ_2, FLR_3, OBJ_3 }

    public const int EMPTY_TILE_TYPES = 2;

    public enum Type
    {
        Open,
        Blocked,
        Ground,
        Tree,
        Slime,
        Shrub
    }
}
