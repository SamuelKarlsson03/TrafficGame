using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityPeople
{
    public class CityPeople : MonoBehaviour
    {
        private AnimationClip[] myClips;
        private Animator animator;

        [SerializeField] bool isCongoMove = false;

        void Start()
        {
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                if (isCongoMove)
                {
                    animator.Play("Exercise_warmingUp_170f");
                }

                if (!isCongoMove)
                {
                myClips = animator.runtimeAnimatorController.animationClips;
                PlayAnyClip();
                StartCoroutine(ShuffleClips());

                }
            }

        }

        void PlayAnyClip()
        {
            var cl = myClips[Random.Range(0, myClips.Length)];
            animator.CrossFadeInFixedTime(cl.name, 1.0f, -1, Random.value * cl.length);
        }

        IEnumerator ShuffleClips()
        {
            while (true)
            {
                yield return new WaitForSeconds(15.0f + Random.value * 5.0f);
                PlayAnyClip();
            }
        }

    }
}
