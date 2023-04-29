using KazatanGames.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 * Ludum Dare 53
 *
 * A game made in 2 days.
 *
 * © Kazatan Games Ltd, 2023
 */
namespace KazatanGames.LD53
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        protected CinemachineVirtualCamera virtualCamBase;

        protected float offsetHeight;
        protected float targetOffsetHeight;
        protected float heightVelocity;

        private void Awake()
        {
            offsetHeight = targetOffsetHeight = LD53AppManager.INSTANCE.AppConfig.cameraMinHeight;
            heightVelocity = 0f;
        }

        private void Update()
        {
            targetOffsetHeight = Mathf.Lerp(LD53AppManager.INSTANCE.AppConfig.cameraMinHeight, LD53AppManager.INSTANCE.AppConfig.cameraMaxHeight, Mathf.Clamp(GameModel.Current.droneSpeed / LD53AppManager.INSTANCE.AppConfig.maxDroneSpeed, 0f, 1f));
        }

        private void LateUpdate()
        {
            offsetHeight = Mathf.SmoothDamp(offsetHeight, targetOffsetHeight, ref heightVelocity, LD53AppManager.INSTANCE.AppConfig.cameraHeightSmoothTime);
            virtualCamBase.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new(0f, offsetHeight, 0f);
        }
    }
}