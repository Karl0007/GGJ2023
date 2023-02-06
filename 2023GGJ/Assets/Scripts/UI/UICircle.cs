using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GGJ
{
	public class UICircle : MonoBehaviour
	{
		public Image OutCircle;
		public Image InCircle;
		public BallType type;
		private Tween outTween;
		private Tween inTween;

		public  Text 提示文本;
		public Text 当前状态;
		private void Awake()
		{
			InCircle.color = type.GetColor();
		}

		public void SetState(float ina, Color outc)
		{
			inTween?.Complete();
			outTween?.Complete();
			inTween = InCircle.DOFade(ina, 0.3F).OnComplete(() => inTween = null);
			outTween = OutCircle.DOColor(outc, 0.3F).OnComplete(() => outTween = null);
            if (outc == Color.white)
            {
				提示文本.text = "粘合";

			}
            else
            {
				提示文本.text = "抵消";
			}
			if (ina == 1)
			{
				当前状态.text = "当前";

			}
			else
			{
				当前状态.text = "";
			}
		}

		private void OnDestroy()
		{
			inTween?.Kill();
			outTween?.Kill();
		}
	}
}