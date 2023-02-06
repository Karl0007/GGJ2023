using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GGJ
{

    public class DataManager : ScriptableObjectSingleton<DataManager>
    {
        [FoldoutGroup("���Ӳ���")]
        public float JointDistance;
        [FoldoutGroup("���Ӳ���")]
        public float JointRatio;
        [FoldoutGroup("���Ӳ���")]
        public float JointFrequency;

        public void InitJoint(SpringJoint2D joint)
		{
            joint.autoConfigureDistance = false;
            joint.distance = JointDistance;
            joint.dampingRatio = JointDistance;
            joint.frequency = JointFrequency;
        }

        [FoldoutGroup("���Ӳ���"),Button]
        public void ResetAllJoint()
		{
			foreach (var item in GameObject.FindObjectsOfType<SpringJoint2D>())
			{
                InitJoint(item);
			}
		}

        [FoldoutGroup("���ٲ���")]
        public float MaxVelocity;
        [FoldoutGroup("���ٲ���")]
        public float MaxDrag;

        [FoldoutGroup("���ٲ���")]
        public Vector2 EnemyVelocity;
        [FoldoutGroup("���ٲ���")]
        public float TargetRange;

        [FoldoutGroup("��ɫ��Ϣ")]
        public Color FireColor;
        [FoldoutGroup("��ɫ��Ϣ")]
        public Color WaterColor;
        [FoldoutGroup("��ɫ��Ϣ")]
        public Color GrassColor;

        [FoldoutGroup("��ɫ��Ϣ")]
        public Color FireBG;
        [FoldoutGroup("��ɫ��Ϣ")]
        public Color WaterBG;
        [FoldoutGroup("��ɫ��Ϣ")]
        public Color GrassBG;


        [FoldoutGroup("Combo�ȼ�")]
        public int Merge1;
        [FoldoutGroup("Combo�ȼ�")]
        public int Merge2;
        [FoldoutGroup("Combo�ȼ�")]
        public int Merge3;
        [FoldoutGroup("Combo�ȼ�")]
        public int Combo1;
        [FoldoutGroup("Combo�ȼ�")]
        public int Combo2;
        [FoldoutGroup("Combo�ȼ�")]
        public int Combo3;

    }
}