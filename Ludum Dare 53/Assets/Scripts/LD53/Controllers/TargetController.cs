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
    public class TargetController : BaseCellController
    {
        [SerializeField]
        protected Transform bigTarget;
        [SerializeField]
        protected Transform medTarget;
        [SerializeField]
        protected Transform smlTarget;

        [SerializeField]
        protected float bigTime = 3f;
        [SerializeField]
        protected float medTime = 2f;
        [SerializeField]
        protected float smlTime = 1.5f;

        [SerializeField]
        protected float rangeBig = 2f;
        [SerializeField]
        protected float rangeMed = 3f;
        [SerializeField]
        protected float rangeSml = 4f;

        protected float bigAccum = 0f;
        protected float medAccum = 0f;
        protected float smlAccum = 0f;

        public GridPos pos;

        protected bool bigDirection = true;
        protected bool medDirection = true;
        protected bool smlDirection = true;

        private void Update()
        {
            bigAccum += Time.deltaTime * (bigDirection ? 1 : -1);
            medAccum += Time.deltaTime * (medDirection ? 1 : -1);
            smlAccum += Time.deltaTime * (smlDirection ? 1 : -1);

            if (bigAccum > bigTime)
            {
                bigDirection = false;
                bigAccum = bigTime - (bigAccum - bigTime);
            } else if (bigAccum < 0)
            {
                bigDirection = true;
                bigAccum = -bigAccum;
            }
            if (medAccum > medTime)
            {
                medDirection = false;
                medAccum = medTime - (medAccum - medTime);
            }
            else if (medAccum < 0)
            {
                medDirection = true;
                medAccum = -medAccum;
            }
            if (smlAccum > smlTime)
            {
                smlDirection = false;
                smlAccum = smlTime - (smlAccum - smlTime);
            }
            else if (smlAccum < 0)
            {
                smlDirection = true;
                smlAccum = -smlAccum;
            }

            float bigRatio = Easing.Quadratic.InOut(bigAccum / bigTime);
            float medRatio = Easing.Quartic.InOut(medAccum / medTime);
            float smlRatio = Easing.Quadratic.InOut(smlAccum / smlTime);

            bigTarget.localPosition = new(0, bigRatio * rangeBig, 0);
            medTarget.localPosition = new(0, medRatio * rangeMed, 0);
            smlTarget.localPosition = new(0, smlRatio * rangeSml, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "parcel")
            {
                GameModel.Current.TargetHit(pos);

                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}