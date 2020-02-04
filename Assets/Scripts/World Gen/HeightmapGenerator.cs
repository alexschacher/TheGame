using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeightmapGenerator
{
    public static float[,] GeneratePerlinNoise(int mapWidth, float scale, bool random)
    {
        /* TODO: Introduce Lacunarity and Frequency If Needed */

        /* Initialize Variables */
        float[,] heightmap = new float[mapWidth, mapWidth];
        float offsetX = 0;
        float offsetY = 0;

        /* Normalize Scale Across Width */
        scale = mapWidth / scale;

        /* Randomize Starting Point */
        if (random)
        {
            offsetX = (float)Random.Range(0, 999999);
            offsetY = (float)Random.Range(0, 999999);
        }

        /* Fill Array With Perlin Noise */
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                float xCoord = (float)x / mapWidth * scale + offsetX;
                float yCoord = (float)y / mapWidth * scale + offsetY;
                heightmap[x, y] = Mathf.PerlinNoise(xCoord, yCoord);
            }
        }

        /* Return Heightmap */
        return heightmap;
    }

    public static float[,] GenerateIslandMask(int mapWidth, float edgeBias)
    {
        /* Initialize Array */
        float[,] heightmap = new float[mapWidth, mapWidth];

        /* Fill Array With Island Mask */
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                /* Determine Distance From Center, from 0(Middle) to 1(Edge) */
                float distanceToCenter = Mathf.Sqrt(
                    ((mapWidth / 2) - x) * ((mapWidth / 2) - x) +
                    ((mapWidth / 2) - y) * ((mapWidth / 2) - y)) / (mapWidth / 2);

                /* Push Mask Closer To The Edge Using EdgeBias, Then Normalize Back Down To 1 On The Edge */
                heightmap[x, y] = (distanceToCenter * edgeBias) - (edgeBias - 1f);

                /* Clamp Negative Values (In The Middle) Back Up To 0 */
                if (heightmap[x, y] < 0)
                {
                    heightmap[x, y] = 0;
                }
            }
        }

        /* Return Heightmap */
        return heightmap;
    }

    public static float[,] ScaleHeightmapRange(float[,] heightmap, float oldMin, float oldMax, float newMin, float newMax)
    {
        /* Scales range of values from old range to new range */
        /* Example: value 0.5 in range of 0-1 is scaled to a range of 10-20, outputting a value of 15 */

        float oldRange = oldMax - oldMin;
        float newRange = newMax - newMin;

        for (int x = 0; x < heightmap.GetLength(0); x++)
        {
            for (int y = 0; y < heightmap.GetLength(1); y++)
            {
                heightmap[x, y] = (((heightmap[x, y] - oldMin) * newRange) / oldRange) + newMin;
            }
        }

        return heightmap;
    }
}