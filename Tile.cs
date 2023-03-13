using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    public enum TileTypes { Path, Buildable, Closed }

    private TileTypes tileType = TileTypes.Closed;
    private Tile connectedTile;

    public static UnityEvent finalize = new UnityEvent();

    [SerializeField] private Tower tower;

    private int tileIndexX;
    private int tileIndexY;

    private void Awake()
    {
        finalize.AddListener(FinalizeTile);
        Sprite newTex = GetComponent<SpriteRenderer>().sprite;
    }

    private void FinalizeTile()
    {
        //Assigns buildable tiles on the basis of adjacency to path tiles.
        if(tileType != TileTypes.Path)
        {
            Tile[,] tileArray = transform.parent.GetComponent<Level>().GetTileArray();
            if (tileIndexX > 0 && tileArray[tileIndexX - 1, tileIndexY].tileType == TileTypes.Path)
                tileType = TileTypes.Buildable;
            if (tileIndexX < tileArray.GetLength(0) - 1 && tileArray[tileIndexX + 1, tileIndexY].tileType == TileTypes.Path)
                tileType = TileTypes.Buildable;
            if (tileIndexY > 0 && tileArray[tileIndexX, tileIndexY - 1].tileType == TileTypes.Path)
                tileType = TileTypes.Buildable;
            if (tileIndexY < tileArray.GetLength(1) - 1 && tileArray[tileIndexX, tileIndexY + 1].tileType == TileTypes.Path)
                tileType = TileTypes.Buildable;
        }

        //Assigns sprites based on the type of the tile.
        switch (tileType)
        {
            case TileTypes.Path:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/path-tile");
                break;
            case TileTypes.Buildable:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/open-tile");
                break;
            case TileTypes.Closed:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/blank-tile");
                break;
        }
    }

    public object GetTower()
    {
        return tower;
    }

    public void SetTower(Tower newTower)
    {
        tower = newTower;
    }

    public TileTypes GetTileType()
    {
        return tileType;
    }

    public void SetTileType(TileTypes newType)
    {
        tileType = newType;
    }

    public void SetConnectedTile(Tile newTile)
    {
        connectedTile = newTile;
    }

    public Tile GetConnectedTile()
    {
        return connectedTile;
    }

    public void SetTileCoords(int _x, int _y)
    {
        tileIndexX = _x;
        tileIndexY = _y;
    }

    public Vector2 GetTileCoords()
    {
        return new Vector2(tileIndexX, tileIndexY);
    }
}
