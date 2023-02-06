using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class EffectManager : Singleton<EffectManager>
    {
        public AudioClip[] bgmAudios;

        void Start()
        {
            StartCoroutine(Audio());
        }
        // Update is called once per frame

        public void PlayAudio(AudioClip clip)
        {
            this.GetComponent<AudioSource>().PlayOneShot(clip, 1f);
        }

        IEnumerator Audio()
        {
            while (true)
            {
                for (int i = 0; i < bgmAudios.Length; i++)
                {
                    this.GetComponent<AudioSource>().clip = bgmAudios[i];
                    this.GetComponent<AudioSource>().Play();
                    yield return new WaitForSeconds(bgmAudios[i].length);
                }
            }
        }

        public void PlayEffect(GameObject gameObject,Vector3 pos,float time = 1)
		{
            Destroy(Instantiate(gameObject, pos, Quaternion.identity),time);
		}
    }
}
