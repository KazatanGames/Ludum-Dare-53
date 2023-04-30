using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/**
 * Ludum Dare 53
 *
 * A game made in 2 days.
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.LD53
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI timerTxt;

        private void Update()
        {
            if (LD53AppManager.INSTANCE.Common.chosenGameMode == GameMode.TargetHunt)
            {
                int mins = Mathf.FloorToInt(GameModel.Current.gameTime / 60);
                int remSecs = Mathf.FloorToInt(GameModel.Current.gameTime - (mins * 60));
                int remMsecs = Mathf.FloorToInt(GameModel.Current.gameTime * 1000) % 1000;

                timerTxt.SetText($"{mins:D2}:{remSecs:D2}:{remMsecs:D3}");
            }
            else if (LD53AppManager.INSTANCE.Common.chosenGameMode == GameMode.TargetHunt)
            {
                timerTxt.SetText($"{GameModel.Current.gameScore:D7}");
            }
            else
            {
                timerTxt.SetText("-------");
            }
        }

        public void RestartClicked()
        {
            GameSceneManager.INSTANCE.RestartGame();
        }

        public void MenuClicked()
        {
            SceneManager.LoadScene(LD53AppManager.INSTANCE.AppConfig.titleScene, LoadSceneMode.Single);
        }
    }
}