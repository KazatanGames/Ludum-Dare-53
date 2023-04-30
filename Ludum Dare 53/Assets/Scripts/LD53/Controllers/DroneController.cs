using KazatanGames.Framework;
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
        protected AudioSource audioSource; // rotors
        [SerializeField]
        protected AudioSource fireAudioSource; // parcels
        [SerializeField]
        protected float targetVolume = 0.75f;
        [SerializeField]
        protected float targetPitch = 1.5f;
        [SerializeField]
        protected AudioClip[] fireClips;

        private void Awake()
        {
            UpdatePosition();
            GameModel.Current.OnReset += OnReset;
            GameModel.Current.OnFire += OnFire;
        }

        private void OnDestroy()
        {
            GameModel.Current.OnReset -= OnReset;
            GameModel.Current.OnFire -= OnFire;
        }

        private void Update()
        {
            UpdatePosition();
            UpdateRotation();
            audioSource.volume = GameModel.Current.introRatio * 0.75f;
            audioSource.pitch = GameModel.Current.introRatio * 1.5f;
        }

        protected void OnReset()
        {
            UpdatePosition();
            UpdateRotation();
        }

        protected void OnFire(Vector3 where, Vector3 towards)
        {
            fireAudioSource.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)]);
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
    }
}