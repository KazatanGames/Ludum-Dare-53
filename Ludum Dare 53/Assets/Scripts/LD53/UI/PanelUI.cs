using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Ludum Dare 53
 *
 * A game made in 2 days.
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.LD53
{
    public class PanelUI : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform[] thingsToDisable;
        [SerializeField]
        protected float openHeight;
        [SerializeField]
        protected float closeHeight;

        [SerializeField]
        protected bool open;

        protected RectTransform thisRTransform;

        private void Awake()
        {
            thisRTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (open)
            {
                Enable();
            } else
            {
                Disable();
            }
        }

        public void ToggleOpen()
        {
            LD53AppManager.INSTANCE.PlayClick();
            open = !open;
            if (open)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        protected void Enable()
        {
            foreach(RectTransform rt in thingsToDisable)
            {
                rt.gameObject.SetActive(true);
            }
            thisRTransform.sizeDelta = new(thisRTransform.sizeDelta.x, openHeight);
        }
        protected void Disable()
        {
            foreach (RectTransform rt in thingsToDisable)
            {
                rt.gameObject.SetActive(false);
            }
            thisRTransform.sizeDelta = new(thisRTransform.sizeDelta.x, closeHeight);
        }

    }
}