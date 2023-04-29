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
    public class DroneController : MonoBehaviour
    {
        [SerializeField]
        protected Transform aimArrowStretcher; // stretch this (z)
        [SerializeField]
        protected Transform aimArrow; // rotate this

        private void Awake()
        {
            UpdatePosition();
        }

        private void Update()
        {
            if (GameSceneManager.INSTANCE.IsPaused) return;

            UpdatePosition();
            UpdateRotation();
            UpdateArrow();
        }

        protected void UpdatePosition()
        {
            transform.position = GameModel.Current.dronePosition;
        }

        protected void UpdateRotation()
        {
            float leanX = (GameModel.Current.dronePlayerAccel.normalized.x / 2f) + 0.5f;
            float leanZ = (GameModel.Current.dronePlayerAccel.normalized.z / 2f) + 0.5f;

            float leanAngleX = Mathf.Lerp(-LD53AppManager.INSTANCE.AppConfig.droneMaxLeanAngle, LD53AppManager.INSTANCE.AppConfig.droneMaxLeanAngle, leanZ);
            float leanAngleZ = -Mathf.Lerp(-LD53AppManager.INSTANCE.AppConfig.droneMaxLeanAngle, LD53AppManager.INSTANCE.AppConfig.droneMaxLeanAngle, leanX);

            transform.localRotation = Quaternion.Euler(leanAngleX, 0f, leanAngleZ);
        }

        protected void UpdateArrow()
        {
            if (GameModel.Current.firePower > 0)
            {
                if (!aimArrow.gameObject.activeSelf) aimArrow.gameObject.SetActive(true);

                aimArrowStretcher.localScale = new Vector3(Mathf.Lerp(LD53AppManager.INSTANCE.AppConfig.aimArrowMinScaleX, LD53AppManager.INSTANCE.AppConfig.aimArrowMaxScaleX, GameModel.Current.firePowerRatio), 1f, Mathf.Lerp(LD53AppManager.INSTANCE.AppConfig.aimArrowMinScaleZ, LD53AppManager.INSTANCE.AppConfig.aimArrowMaxScaleZ, GameModel.Current.firePowerRatio));

                aimArrow.LookAt(new Vector3(GameModel.Current.aimInput.x, transform.position.y, GameModel.Current.aimInput.y));
            } else
            {
                if (aimArrow.gameObject.activeSelf) aimArrow.gameObject.SetActive(false);
            }
        }
    }
}