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
    public class ParcelController : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource audioSource;
        [SerializeField]
        protected AudioClip[] audioClips;

        public Rigidbody body;
        protected bool colliding;

        private void OnCollisionEnter(Collision collision)
        {
            if (colliding) return;
            colliding = true;

            float vol = Mathf.Clamp(Mathf.InverseLerp(20f, 100f, collision.impulse.magnitude), 0.4f, 1f);

            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], vol);
        }

        private void OnCollisionExit(Collision collision)
        {
            colliding = false;
        }
    }
}