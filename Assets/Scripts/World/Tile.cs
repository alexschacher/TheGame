using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tile class holds an enum list of IDs representing game objects that can be generated, loaded, saved, and instantiated.
// Also includes helper methods that check if an ID is of a certain qualifier.

public static class Tile
{
    public enum ID
    {
        // Empty
        Empty,
        EmptyButBlocked,

        // Blocks
        BlockGrass,
        BlockDirt,
        BlockBrick,

        // Foliage
        Tree,
        Grass
    }

    public static bool IsBlock(Tile.ID id)
    {
        switch (id)
        {
            case ID.BlockGrass:
            case ID.BlockDirt:
            case ID.BlockBrick:

            return true;
            default: return false;
        }
    }
}
