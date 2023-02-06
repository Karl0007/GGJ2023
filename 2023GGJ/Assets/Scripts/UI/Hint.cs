using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GGJ
{
    public class Hint : Singleton<Hint>
    {
        public List<UICircle> circle;
		private Tween fade;

		public void SelectType(BallType type)
		{
			
			circle.ForEach(x => 
			{
				x.SetState(x.type == type ? 1 : 0.9f, BallManager.Instance.Root.Type.IsSafe(x.type) ? Color.white : Color.red);
			});
			fade?.Complete();
			fade = CameraManager.Instance.GetComponent<Camera>().DOColor(type.GetBGColor(), 0.2f).OnComplete(() => fade = null);
			//GGJ.CameraManager.Instance.GetComponent<Camera>().backgroundColor = type.GetBGColor();
		}
    }
}