using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace User.Aguro
{
    public class TitleSceneManager : MonoBehaviour
    {
        [SerializeField] private int clearedStageNumber;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Text newGameText;
        //[SerializeField] private Button continueButton;
        [SerializeField] private Button levelSelectButton;

        // Start is called before the first frame update
        void Start()
        {
            if (clearedStageNumber>0)
            {
                newGameText.text = "Continue";
            }
        }
    }
}
