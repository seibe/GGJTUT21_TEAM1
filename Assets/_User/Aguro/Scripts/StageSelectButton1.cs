using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace User.Aguro
{
    public class ContinueButton : MonoBehaviour
    {
        [SerializeField] private int continueStageNumber;
        [SerializeField] private string sceneName;
        private Button button;
        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClickButton);
        }

        public void OnClickButton()
        {
            //TODO: continueStageNumberを代入する
            Debug.Log(continueStageNumber);
            SceneManager.LoadScene(sceneName);
        }
    }
}
