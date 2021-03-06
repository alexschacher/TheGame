﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private WorldDataContainer worldData;
    private ObjectData objectData;
    private bool isRounded;
    private MeshFilter meshFilter;

    public void Init(Material topMat, Material sideMat, bool isRounded)
    {
        this.isRounded = isRounded;

        Material[] materials = transform.GetChild(1).GetComponent<Renderer>().materials;
        materials[0] = topMat;
        materials[1] = sideMat;
        transform.GetChild(1).GetComponent<Renderer>().materials = materials;

        meshFilter = transform.GetChild(1).GetComponent<MeshFilter>();
    }

    private void Start()
    {
        objectData = GetComponent<ObjectData>();
        worldData = objectData.GetWorldObjectContainer().GetWorldData();

        UpdateModel();
        UpdateNeighbors();
    }

    public void UpdateModel()
    {
        if (objectData == null) { return; }

        Tile.ID[,] neighbors = worldData.GetNeighbors(objectData.GetX(), objectData.GetZ(), objectData.GetY());

        int bitMask = 0;
        if (Tile.IsBlock(neighbors[1, 0])) bitMask += 1;
        if (Tile.IsBlock(neighbors[0, 1])) bitMask += 2;
        if (Tile.IsBlock(neighbors[2, 1])) bitMask += 4;
        if (Tile.IsBlock(neighbors[1, 2])) bitMask += 8;

        switch(bitMask)
        {
            case  0: meshFilter.mesh = Resource.mesh["Block_RoundLone"];   transform.rotation = Quaternion.Euler(0, 0, 0);     break;
            case  1: meshFilter.mesh = Resource.mesh["Block_RoundEnd"];    transform.rotation = Quaternion.Euler(0, -90, 0);   break;
            case  2: meshFilter.mesh = Resource.mesh["Block_RoundEnd"];    transform.rotation = Quaternion.Euler(0, 0, 0);     break;
            case  3: meshFilter.mesh = Resource.mesh["Block_RoundCorner"]; transform.rotation = Quaternion.Euler(0, 0, 0);     break;
            case  4: meshFilter.mesh = Resource.mesh["Block_RoundEnd"];    transform.rotation = Quaternion.Euler(0, 180, 0);   break;
            case  5: meshFilter.mesh = Resource.mesh["Block_RoundCorner"]; transform.rotation = Quaternion.Euler(0, -90, 0);   break;
            case  6: meshFilter.mesh = Resource.mesh["Block_Lane"];        transform.rotation = Quaternion.Euler(0, 90, 0);    break;
            case  7: meshFilter.mesh = Resource.mesh["Block_Edge"];        transform.rotation = Quaternion.Euler(0, -90, 0);   break;
            case  8: meshFilter.mesh = Resource.mesh["Block_RoundEnd"];    transform.rotation = Quaternion.Euler(0, 90, 0);    break;
            case  9: meshFilter.mesh = Resource.mesh["Block_Lane"];        transform.rotation = Quaternion.Euler(0, 0, 0);     break;
            case 10: meshFilter.mesh = Resource.mesh["Block_RoundCorner"]; transform.rotation = Quaternion.Euler(0, 90, 0);    break;
            case 11: meshFilter.mesh = Resource.mesh["Block_Edge"];        transform.rotation = Quaternion.Euler(0, 0, 0);     break;
            case 12: meshFilter.mesh = Resource.mesh["Block_RoundCorner"]; transform.rotation = Quaternion.Euler(0, 180, 0);   break;
            case 13: meshFilter.mesh = Resource.mesh["Block_Edge"];        transform.rotation = Quaternion.Euler(0, 180, 0);   break;
            case 14: meshFilter.mesh = Resource.mesh["Block_Edge"];        transform.rotation = Quaternion.Euler(0, 90, 0);    break;
            case 15: meshFilter.mesh = Resource.mesh["Block_Mid"];         transform.rotation = Quaternion.Euler(0, 0, 0);     break;
            default: return;
        }
    }

    private void UpdateNeighbors()
    {
        GameObject[,] neighbors = objectData.GetWorldObjectContainer().GetNeighbors(objectData.GetX(), objectData.GetZ(), objectData.GetY());

        for (int ix = 0; ix <3; ix++)
        {
            for (int iz = 0; iz < 3; iz++)
            {
                if (neighbors[ix, iz] != null)
                {
                    if (neighbors[ix, iz].GetComponent<Block>() != null)
                    {
                        neighbors[ix, iz].GetComponent<Block>().UpdateModel();
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        UpdateNeighbors();
    }
}