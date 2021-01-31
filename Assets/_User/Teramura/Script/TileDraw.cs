using Game;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace User.Teramura
{
    public class TileDraw : MonoBehaviour
    {
        [SerializeField] Tile[] numTiles;
        [SerializeField] Tile[] fixNumTiles;
        [SerializeField] Tile[] plusTiles;
        [SerializeField] Tile equalTile;
        [SerializeField] Tile emptyTile;
        [SerializeField] Tile wallTile;
        [SerializeField] GameObject kariUI;
        Block block;
        LevelData levelData;
        Tilemap tileMap;

        // Start is called before the first frame update
        IEnumerator Start()
        {
            tileMap = GetComponent<Tilemap>();

            levelData = new LevelData(7, 3);
            levelData.SetAt(0, 0, Block.Num3);
            levelData.SetAt(1, 0, Block.Empty);
            levelData.SetAt(2, 0, Block.Num5);
            levelData.SetAt(3, 0, Block.Wall);
            levelData.SetAt(4, 0, Block.Wall);
            levelData.SetAt(5, 0, Block.Wall);
            levelData.SetAt(6, 0, Block.Wall);
            levelData.SetAt(0, 1, Block.Empty);
            levelData.SetAt(1, 1, Block.Empty);
            levelData.SetAt(2, 1, Block.Empty);
            levelData.SetAt(3, 1, Block.Fixed8);
            levelData.SetAt(4, 1, Block.FixedEqual);
            levelData.SetAt(5, 1, Block.Fixed4);
            levelData.SetAt(6, 1, Block.Fixed5);
            levelData.SetAt(0, 2, Block.Num4);
            levelData.SetAt(1, 2, Block.Plus);
            levelData.SetAt(2, 2, Block.Num7);
            levelData.SetAt(3, 2, Block.Wall);
            levelData.SetAt(4, 2, Block.Wall);
            levelData.SetAt(5, 2, Block.Wall);
            levelData.SetAt(6, 2, Block.Wall);
            UpdateTile();

            while (!Input.GetKeyDown("s")) yield return null;
            levelData.TryMoveDown(1, 2);
            UpdateTile();
            yield return null;

            while (!Input.GetKeyDown("s")) yield return null;
            levelData.TryMoveRight(1, 1);
            UpdateTile();
            yield return null;

            while (!Input.GetKeyDown("s")) yield return null;
            levelData.TryMoveLeft(2, 2);
            UpdateTile();
            yield return null;

            while (!Input.GetKeyDown("s")) yield return null;
            levelData.TryMoveDown(1, 2);
            UpdateTile();
            yield return null;

            while (!Input.GetKeyDown("s")) yield return null;
            levelData.TryMoveUp(0, 0);
            UpdateTile();
            yield return null;

            while (!Input.GetKeyDown("s")) yield return null;
            kariUI.SetActive(true);
            yield return null;

            while (!Input.GetKeyDown("s")) yield return null;
            kariUI.SetActive(false);
        }

        void UpdateTile()
        {
            for (int i = 0; i < levelData.Height; i++)
            {
                for (int k = 0; k < levelData.Width; k++)
                {
                    block = levelData.GetAt(k, i);

                    if (block.IsDigit)
                    {
                        var number = block.Value - '0';

                        if (block.IsFixed)
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), fixNumTiles[number]);
                        }
                        else
                        {
                            tileMap.SetTile(new Vector3Int(k, i, 0), numTiles[number]);
                        }
                    }
                    else if (block == Block.Plus)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), plusTiles[0]);
                    }
                    else if (block == Block.FixedPlus)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), plusTiles[1]);
                    }
                    else if (block == Block.FixedEqual)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), equalTile);
                    }
                    else if (block == Block.Wall)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), wallTile);
                    }
                    else
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), emptyTile);
                    }
                }
            }
        }
    }
}
