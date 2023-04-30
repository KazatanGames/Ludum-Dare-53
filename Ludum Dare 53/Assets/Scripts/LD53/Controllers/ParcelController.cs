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
        [SerializeField]
        protected float minVol = 0.25f;
        [SerializeField]
        protected float maxVol = 1f;
        [SerializeField]
        protected float volLowThreshold = 10f;
        [SerializeField]
        protected float volHighThreshold = 125f;
        [SerializeField]
        protected float originalLifetime = 5f;

        public Rigidbody body;
        protected bool colliding;
        protected float lifetime;

        private void Start()
        {
            lifetime = originalLifetime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (colliding) return;
            colliding = true;

            float vol = Mathf.Clamp(Mathf.InverseLerp(volLowThreshold, volHighThreshold, collision.impulse.magnitude), minVol, maxVol);

            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], vol);
        }

        private void OnCollisionExit(Collision collision)
        {
            colliding = false;
        }

        private void Update()
        {
            if (body.IsSleeping())
            {
                lifetime -= Time.deltaTime;
                if (lifetime < 0)
                {
                    Instantiate(LD53AppManager.INSTANCE.AppConfig.prefabRegister.parcelDestroyPrefab, transform.localPosition, Quaternion.identity, transform.parent);
                    Destroy(gameObject);
                }
            } else
            {
                lifetime = originalLifetime;
            }
        }
    }
}