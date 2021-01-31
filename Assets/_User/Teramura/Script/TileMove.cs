using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace User.Teramura
{
    public class TileMove : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            var tileMap = GetComponent<Tilemap>();
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;

            if (tileMap)
            {
                
            }
        }
    }
}
