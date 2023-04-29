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
        protected DroneRotorDescriptor[] rotors;

        protected List<float> rotorRotations;

        private void Awake()
        {
            rotorRotations = new();
            for(int i = 0; i < rotors.Length; i++)
            {
                rotorRotations.Add(0f);
            }

            UpdatePosition();
        }

        private void Update()
        {
            if (GameSceneManager.INSTANCE.IsPaused) return;

            UpdatePosition();
            UpdateRotors();
        }

        protected void UpdatePosition()
        {
            transform.position = GameModel.Current.dronePosition;
        }

        protected void UpdateRotors()
        {
            for(int i = 0; i < rotors.Length; i++)
            {
                DroneRotorDescriptor drd = rotors[i];
                float rotorSpeed = GameModel.Current.droneAcceleration.x * drd.rotorWeights.x + GameModel.Current.droneAcceleration.y * drd.rotorWeights.y + GameModel.Current.droneAcceleration.z * drd.rotorWeights.z;
                float rRot = rotorRotations[i];
                rRot += rotorSpeed * LD53AppManager.INSTANCE.AppConfig.maxRotorRotationSpeed * Time.deltaTime;
                rRot %= 360f;
                rotorRotations[i] = rRot;
                drd.rotorTransform.localRotation = Quaternion.Euler(90f, rRot, 0f);
            }
        }
    }
}