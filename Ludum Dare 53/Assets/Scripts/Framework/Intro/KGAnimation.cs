namespace KazatanGames.Framework
{
    using UnityEngine;
    using System.Collections;

    public class KGAnimation : BaseIntroElement
    {
        [SerializeField]
        protected GameObject movingLight;
        [SerializeField]
        protected float lightDistance = 5f;
        [SerializeField]
        protected float startDelay = 1f;
        [SerializeField]
        protected float animTime = 3f;

        protected float halfAnimTime;

        public override bool IsComplete => animTime <= 0f;
        public override bool DestroyImmediatelyOnComplete => false;

        protected override void Initialise()
        {
            halfAnimTime = animTime / 2f;
            movingLight.transform.position = new Vector3(-lightDistance, movingLight.transform.position.y, movingLight.transform.position.z);
        }

        protected override void DoUpdate()
        {
            if (startDelay > 0f)
            {
                startDelay -= Time.deltaTime;
                return;
            }
            if (animTime > 0f)
            {
                if (animTime >= halfAnimTime)
                {
                    // first half
                    float ratio = (animTime - halfAnimTime) / halfAnimTime;
                    movingLight.transform.position = new Vector3(
                        Easing.Circular.InOut(ratio) * -lightDistance,
                        movingLight.transform.position.y,
                        movingLight.transform.position.z
                    );
                }
                else
                {
                    // second half
                    float ratio = 1f - (animTime / halfAnimTime);
                    movingLight.transform.position = new Vector3(
                        Easing.Circular.InOut(ratio) * lightDistance,
                        movingLight.transform.position.y,
                        movingLight.transform.position.z
                    );
                }
            }

            animTime -= Time.deltaTime;
        }
    }
}