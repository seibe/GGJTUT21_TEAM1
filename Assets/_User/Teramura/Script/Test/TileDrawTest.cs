using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace User.Teramura
{
    public class TileDrawTest : MonoBehaviour
    {
        [SerializeField] Tile tile1;
        [SerializeField] Tile tile2;
        [SerializeField] Tile[] numTiles;
        

        // Start is called before the first frame update
        void Start()
        {
            int x = 3;
            int y = 3;
            int[] position = { 0, 1, 2, 1, 1, 0, 1, 2, 1};
            
            
            var tileMap = GetComponent<Tilemap>();
            for(int i = 0;i < y;i++)
            {
                for(int k = 0;k < x;k++)
                {
                    if (position[k + y * i] == 1)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), tile1);
                    }
                    else if (position[k + y * i] == 2)
                    {
                        tileMap.SetTile(new Vector3Int(k, i, 0), tile2);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
