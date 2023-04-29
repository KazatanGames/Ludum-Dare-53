﻿using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Ludum Dare 53
 *
 * A game made in 2 days.
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.LD53
{
    public class MinimapTexturer : MonoBehaviour
    {
        [SerializeField]
        protected RawImage rawImage;
        [SerializeField]
        protected int fps = 2;
        [SerializeField]
        protected Color officeColor = Color.white;
        [SerializeField]
        protected Color concreteColor = Color.gray;
        [SerializeField]
        protected Color grassColor = Color.green;
        [SerializeField]
        protected Color roadColor = Color.black;
        [SerializeField]
        protected Color droneColor = Color.magenta;

        protected float timePerUpdate;
        protected float timeBank;

        protected Texture2D tex;

        private void Start()
        {
            timeBank = 0f;
            timePerUpdate = 0f;

            tex = new(LD53AppManager.INSTANCE.AppConfig.playAreaSize.x, LD53AppManager.INSTANCE.AppConfig.playAreaSize.y);
            tex.filterMode = FilterMode.Point;
            rawImage.texture = tex;

            Render();
        }

        private void Update()
        {
            timeBank += Time.deltaTime;
            if (timeBank > timePerUpdate)
            {
                timeBank = 0f;
                Render();
            }
        }

        protected void Render()
        {
            for (int x = 0; x < LD53AppManager.INSTANCE.AppConfig.playAreaSize.x; x++)
            {
                for (int y = 0; y < LD53AppManager.INSTANCE.AppConfig.playAreaSize.y; y++)
                {
                    Vector2Int here = new(x, y);
                    Color c = Color.magenta;

                    if (Equals(here, PositionHelpers.WorldToGridPos(GameModel.Current.dronePosition)))
                    {
                        c = droneColor;
                    } else
                    {
                        switch (GameModel.Current.cells[x, y].cellType)
                        {
                            case CellTypeEnum.Road:
                                c = roadColor;
                                break;
                            case CellTypeEnum.Concrete:
                                c = concreteColor;
                                break;
                            case CellTypeEnum.Grass:
                                c = grassColor;
                                break;
                            case CellTypeEnum.Office:
                                c = officeColor;
                                break;
                        }
                    }

                    tex.SetPixel(x, y, c);
                }
            }

            tex.Apply();
        }
    }
}