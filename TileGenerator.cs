using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{

    void Start()
    {
        GetTileMap(16 * (1 + (int)GameManager.GetInstance().GetlevelType() / 2), 10 * (1 + (int)GameManager.GetInstance().GetlevelType() / 2));
    }


    public static GameObject GetTileMap(int width, int height)
    {

        Level level = CreateZigZagLevel(width, height);
        GameManager.GetInstance().SetCurrentLevel(level);

        //Set camera size based on level width and height.
        float minOrthSize = Mathf.Max(height / 2, width / 3);
        //Add a small additional zoom-out for UI room.
        Camera.main.orthographicSize = minOrthSize + minOrthSize * .2f;

        Tile.finalize.Invoke();



        LinkPathNodes(level.GetPathHead(), level.GetTileArray());
        return level.gameObject;
    }

    private static void LinkPathNodes(Tile PathHead, Tile[,] tileArray)
    {
        Tile currentTile = PathHead;
        do
        {
            Vector2 currentCoords = currentTile.GetTileCoords();
            if (currentCoords.x > 0)
            {
                Tile potentialTile = tileArray[(int)currentCoords.x - 1, (int)currentCoords.y];
                if (potentialTile.GetTileType() == Tile.TileTypes.Path && potentialTile.GetConnectedTile() == null)
                    currentTile.SetConnectedTile(potentialTile);
            }

            if(currentCoords.x < tileArray.GetLength(1))
            {
                Tile potentialTile = tileArray[(int)currentCoords.x + 1, (int)currentCoords.y];
                if (potentialTile.GetTileType() == Tile.TileTypes.Path && potentialTile.GetConnectedTile() == null)
                    currentTile.SetConnectedTile(potentialTile);
            }

            if (currentCoords.y > 0)
            {
                Tile potentialTile = tileArray[(int)currentCoords.x, (int)currentCoords.y - 1];
                if (potentialTile.GetTileType() == Tile.TileTypes.Path && potentialTile.GetConnectedTile() == null)
                    currentTile.SetConnectedTile(potentialTile);
            }

            if (currentCoords.y < tileArray.GetLength(0))
            {
                Tile potentialTile = tileArray[(int)currentCoords.x, (int)currentCoords.y + 1];
                if (potentialTile.GetTileType() == Tile.TileTypes.Path && potentialTile.GetConnectedTile() == null)
                    currentTile.SetConnectedTile(potentialTile);
            }

            currentTile = currentTile.GetConnectedTile();

        } while (currentTile != null);
    }


    private static Level CreateStraightMap(int width, int height)
    {
        Level level = new GameObject("Level").AddComponent<Level>();
        Tile[,] tileArray = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Tile tile = Instantiate(Resources.Load<GameObject>("Prefabs/Tile").GetComponent<Tile>());
                tile.transform.position = new Vector2(i - width / 2 + .5f, j - height / 2 + .5f);
                tile.transform.parent = level.transform;

                tileArray[i, j] = tile;

                tile.SetTileCoords(i, j);

                if (j == (height / 2))
                    tileArray[i, j].SetTileType(Tile.TileTypes.Path);

                if (i == width - 1 && tileArray[i, j].GetTileType() == Tile.TileTypes.Path)
                    level.GetComponent<Level>().SetPathHead(tileArray[i, j]);
            }
        }

        level.SetTileArray(tileArray);
        return level;
    }

    private static Level CreateZigZagLevel(int width, int height)
    {
        Level level = new GameObject("Level").AddComponent<Level>();
        Tile[,] tileArray = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Tile tile = Instantiate(Resources.Load<GameObject>("Prefabs/Tile").GetComponent<Tile>());
                tile.transform.position = new Vector2(i - width / 2 + .5f, j - height / 2 + .5f);
                tile.transform.parent = level.transform;

                tileArray[i, j] = tile;

                tile.SetTileCoords(i, j);

            }
        }

        level.GetComponent<Level>().SetPathHead(tileArray[width - 1, height / 2]);
        int lastWidth = width - 1;
        int lastHeight = height / 2;
        for (int i = lastWidth; i >= width * 0.75f; i--)
        {
            lastWidth = i;
            tileArray[lastWidth, lastHeight].SetTileType(Tile.TileTypes.Path);
        }
        for(int j = lastHeight; j < height * .75f; j++)
        {
            lastHeight = j;
            tileArray[lastWidth, lastHeight].SetTileType(Tile.TileTypes.Path);
        }
        for (int i = lastWidth; i >= width * 0.5f; i--)
        {
            lastWidth = i;
            tileArray[lastWidth, lastHeight].SetTileType(Tile.TileTypes.Path);
        }
        for (int j = lastHeight; j > height * .25f; j--)
        {
            lastHeight = j;
            tileArray[lastWidth, lastHeight].SetTileType(Tile.TileTypes.Path);
        }
        for (int i = lastWidth; i >= width * 0.25f; i--)
        {
            lastWidth = i;
            tileArray[lastWidth, lastHeight].SetTileType(Tile.TileTypes.Path);
        }
        for (int j = lastHeight; j <= height * .5f; j++)
        {
            lastHeight = j;
            tileArray[lastWidth, lastHeight].SetTileType(Tile.TileTypes.Path);
        }
        for (int i = lastWidth; i >= 0; i--)
        {
            lastWidth = i;
            tileArray[lastWidth, lastHeight].SetTileType(Tile.TileTypes.Path);
        }

        level.SetTileArray(tileArray);
        return level;
    }

}
