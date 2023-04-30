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
    public class MusicManager : SingletonMonoBehaviour<MusicManager>
    {
        [SerializeField]
        protected AudioClip[] clips;
        [SerializeField]
        protected AudioSource source;
        [SerializeField]
        protected float volume = 1f;

        protected int nextIndex;

        public void PlayClip(int index)
        {
            if (clips.Length <= index || index < 0) return;
            if (index == nextIndex) return;
            nextIndex = index;
        }

        protected override void Initialise()
        {
            base.Initialise();

            source.volume = volume;
            source.loop = true;
            source.Stop();
        }

        private void Update()
        {
            if (source.clip != clips[nextIndex])
            {
                if (source.volume > 0f)
                {
                    source.volume -= Time.unscaledDeltaTime;
                    if (source.volume < 0) source.volume = 0;
                } else
                {
                    source.Stop();
                    source.clip = clips[nextIndex];
                    source.Play();
                }
            } else
            {
                if (source.volume < volume)
                {
                    source.volume += Time.unscaledDeltaTime;
                    if (source.volume > volume) source.volume = volume;
                }
            }
        }
    }
}