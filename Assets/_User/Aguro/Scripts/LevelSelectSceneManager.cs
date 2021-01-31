using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace User.Aguro
{
    public class LevelSelectSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject stageSelectButtonPrefab;

        [SerializeField] private GameObject canvas;

        // Start is called before the first frame update
        void Start()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 1; x < 7; x++)
                {
                    GameObject stageSelectButtonGameObject = Instantiate(stageSelectButtonPrefab,
                        new Vector3(x * 200.0f - 700.0f + Screen.width / 2.0f,
                            300.0f - y * 200.0f + Screen.height / 2.0f, 0.0f), Quaternion.identity);
                    stageSelectButtonGameObject.transform.parent = canvas.transform;
                    stageSelectButtonGameObject.GetComponentInChildren<Text> ().text = (y*6+x).ToString();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
