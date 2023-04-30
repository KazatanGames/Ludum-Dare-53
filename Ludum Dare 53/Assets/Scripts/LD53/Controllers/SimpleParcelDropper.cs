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
    public class SimpleParcelDropper : MonoBehaviour
    {
        [SerializeField]
        protected IntroParcelController parcelPrefab;
        [SerializeField]
        protected float minTime = 1f;
        [SerializeField]
        protected float maxTime = 2f;

        protected float nextDrop;

        private void Start()
        {
            nextDrop = Random.Range(minTime, maxTime);
        }

        private void Update()
        {
            nextDrop -= Time.deltaTime;
            if (nextDrop < 0)
            {
                nextDrop = Random.Range(minTime, maxTime);
                IntroParcelController pc = Instantiate(parcelPrefab, transform).GetComponent<IntroParcelController>();
                pc.body.AddTorque(
                    Random.Range(-LD53AppManager.INSTANCE.AppConfig.parcelTorqueMax, LD53AppManager.INSTANCE.AppConfig.parcelTorqueMax),
                    Random.Range(-LD53AppManager.INSTANCE.AppConfig.parcelTorqueMax, LD53AppManager.INSTANCE.AppConfig.parcelTorqueMax),
                    Random.Range(-LD53AppManager.INSTANCE.AppConfig.parcelTorqueMax, LD53AppManager.INSTANCE.AppConfig.parcelTorqueMax)
                );
            }
        }
    }
}