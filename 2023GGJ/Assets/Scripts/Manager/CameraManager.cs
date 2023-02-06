using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ
{
	public class CameraManager : Singleton<CameraManager>
	{
		public float MinSize => Mathf.Lerp(10, 22, Mathf.Clamp01(BallManager.Instance.AllBalls.Count(x => !x.IsEnemy) / 90F));

		private Tween shake;

		private void Start()
		{
			Camera.main.orthographicSize = MinSize;
		}

		private void Update()
		{
			var target = Mathf.Max(BallManager.Instance.MaxDist, MinSize);
			Camera.main.orthographicSize += (target - Camera.main.orthographicSize) * (target > Camera.main.orthographicSize ? 0.05f : 0.001f);
		}

		public void Shake()
		{
			shake?.Complete();
			shake = Camera.main.DOShakePosition(0.2f, 3, 100).SetUpdate(true).OnComplete(() => shake = null);
			Camera.main.DOColor(Color.white, 0.01f).SetUpdate(true).SetLoops(2, LoopType.Yoyo);
		}
	}
}