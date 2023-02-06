using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GGJ
{

    public class DataManager : ScriptableObjectSingleton<DataManager>
    {
        [FoldoutGroup("链接参数")]
        public float JointDistance;
        [FoldoutGroup("链接参数")]
        public float JointRatio;
        [FoldoutGroup("链接参数")]
        public float JointFrequency;

        public void InitJoint(SpringJoint2D joint)
		{
            joint.autoConfigureDistance = false;
            joint.distance = JointDistance;
            joint.dampingRatio = JointDistance;
            joint.frequency = JointFrequency;
        }

        [FoldoutGroup("链接参数"),Button]
        public void ResetAllJoint()
		{
			foreach (var item in GameObject.FindObjectsOfType<SpringJoint2D>())
			{
                InitJoint(item);
			}
		}

        [FoldoutGroup("球速参数")]
        public float MaxVelocity;
        [FoldoutGroup("球速参数")]
        public float MaxDrag;

        [FoldoutGroup("球速参数")]
        public Vector2 EnemyVelocity;
        [FoldoutGroup("球速参数")]
        public float TargetRange;

        [FoldoutGroup("颜色信息")]
        public Color FireColor;
        [FoldoutGroup("颜色信息")]
        public Color WaterColor;
        [FoldoutGroup("颜色信息")]
        public Color GrassColor;

        [FoldoutGroup("颜色信息")]
        public Color FireBG;
        [FoldoutGroup("颜色信息")]
        public Color WaterBG;
        [FoldoutGroup("颜色信息")]
        public Color GrassBG;


        [FoldoutGroup("Combo等级")]
        public int Merge1;
        [FoldoutGroup("Combo等级")]
        public int Merge2;
        [FoldoutGroup("Combo等级")]
        public int Merge3;
        [FoldoutGroup("Combo等级")]
        public int Combo1;
        [FoldoutGroup("Combo等级")]
        public int Combo2;
        [FoldoutGroup("Combo等级")]
        public int Combo3;

    }
}