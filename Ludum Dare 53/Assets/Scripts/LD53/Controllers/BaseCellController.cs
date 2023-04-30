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
    public class BaseCellController : MonoBehaviour
    {
        [SerializeField]
        protected Transform distanceDisableTransform;

        protected bool isEnabled = true;

        private void Awake()
        {
            Initialise();

            GameSceneManager.INSTANCE.OnDroneMovedDistance += OnDroneMovedDistance;
        }

        private void Start()
        {
            OnDroneMovedDistance();
        }

        private void OnDestroy()
        {
            GameSceneManager.INSTANCE.OnDroneMovedDistance -= OnDroneMovedDistance;
        }

        protected virtual void Initialise()
        {
            transform.localScale = new Vector3(
                transform.localScale.x * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize,
                transform.localScale.y,
                transform.localScale.z * LD53AppManager.INSTANCE.AppConfig.playAreaGridSize
            );
        }

        protected void OnDroneMovedDistance()
        {
            if (distanceDisableTransform == null) return;
            isEnabled = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(GameModel.Current.dronePosition.x, GameModel.Current.dronePosition.z)) <= LD53AppManager.INSTANCE.AppConfig.drawDistance;
            distanceDisableTransform.gameObject.SetActive(isEnabled);
        }
    }
}