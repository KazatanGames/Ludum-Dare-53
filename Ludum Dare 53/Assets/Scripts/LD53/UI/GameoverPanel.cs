using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * Ludum Dare 53
 *
 * A game made in 2 days.
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.LD53
{
    public class GameoverPanel : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI titleTxt;
        [SerializeField]
        protected TextMeshProUGUI scoreHeadingTxt;
        [SerializeField]
        protected TextMeshProUGUI scoreTxt;

        private void Start()
        {
            int mins = Mathf.FloorToInt(GameModel.Current.gameTime / 60);
            int remSecs = Mathf.FloorToInt(GameModel.Current.gameTime - (mins * 60));
            int remMsecs = Mathf.FloorToInt(GameModel.Current.gameTime * 1000) % 1000;

            string timeText = $"{mins:D2}:{remSecs:D2}:{remMsecs:D3}";

            switch (LD53AppManager.INSTANCE.Common.chosenGameMode)
            {
                case GameMode.Eternal:
                    titleTxt.SetText("Out of Time!");
                    titleTxt.color = Color.red;
                    scoreHeadingTxt.SetText("You lasted");
                    scoreTxt.SetText(timeText);
                    break;
                case GameMode.TargetHunt:
                    titleTxt.SetText("Parcel Deliveries COMPLETE!");
                    titleTxt.color = Color.green;
                    scoreHeadingTxt.SetText("You took");
                    scoreTxt.SetText(timeText);
                    break;
            }

            GameModel.Current.OnReset += OnReset;
        }

        protected void OnReset()
        {
            GameModel.Current.OnReset -= OnReset;
            Destroy(gameObject);
        }
    }
}