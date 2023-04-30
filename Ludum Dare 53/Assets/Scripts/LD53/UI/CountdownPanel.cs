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
    public class CountdownPanel : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource audioPlayer;
        [SerializeField]
        protected AudioClip sfxCountdown;
        [SerializeField]
        protected AudioClip sfxGo;

        public event Action OnGo;

        public void PlayCountdown()
        {
            audioPlayer.PlayOneShot(sfxCountdown);
        }

        public void PlayGo()
        {
            audioPlayer.PlayOneShot(sfxGo);
            OnGo?.Invoke();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

    }
}