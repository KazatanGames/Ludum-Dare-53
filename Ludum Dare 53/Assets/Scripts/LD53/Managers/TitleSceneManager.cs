﻿using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
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
    public class TitleSceneManager : SingletonMonoBehaviour<TitleSceneManager>
    {
        public void PlayClicked()
        {
            SceneManager.LoadScene(LD53AppManager.INSTANCE.AppConfig.gameScene, LoadSceneMode.Single);
        }
    }
}