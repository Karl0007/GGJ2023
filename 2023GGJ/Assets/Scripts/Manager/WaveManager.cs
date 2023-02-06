using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GGJ
{
    public class WaveManager : Singleton<WaveManager>
    {
        public Wave[] 波次数量组;
        public Wave[] GetWaveGoup()
        {
            foreach (var item in 波次数量组)
            {
                item.属性= (BallType)UnityEngine.Random.Range(1, 4);
            }
            return 波次数量组;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
