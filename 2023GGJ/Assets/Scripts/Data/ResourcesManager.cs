using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DamageNumbersPro;

namespace GGJ
{

    public class ResourcesManager : ScriptableObjectSingleton<ResourcesManager>
    {

        [FoldoutGroup("Prefabs")]
        public GameObject BallPrefab;

        [FoldoutGroup("Effects")]
        public GameObject Click;
        [FoldoutGroup("Effects")]
        public GameObject Merge;
        [FoldoutGroup("Effects")]
        public GameObject FireKill;
        [FoldoutGroup("Effects")]
        public GameObject WaterKill;
        [FoldoutGroup("Effects")]
        public GameObject GrassKill;

        [FoldoutGroup("Audio")]
        public List<AudioClip> MergeAudio;
        [FoldoutGroup("Audio")]
        public List<AudioClip> KillAudio;
        [FoldoutGroup("Audio")]
        public List<AudioClip> SwithAudio;

        [FoldoutGroup("Number")]
        public DamageNumberGUI MergeNumber;
        [FoldoutGroup("Number")]
        public DamageNumberGUI KillNumber;
    }
}