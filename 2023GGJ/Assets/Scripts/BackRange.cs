using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	public class BackRange : Singleton<BackRange>
	{

		public SpriteRenderer Squre;
		public SpriteMask Mask;
		private const float width = 0.5f;

		protected override void Awake()
		{
			base.Awake();
			UpdateSize();
			Squre.color = new Color(1, 1, 1, 0.1f);
			Squre.DOFade(0.2f, 0.7f).SetLoops(-1, LoopType.Yoyo);
		}

		public void UpdateSize()
		{
			Squre.transform.localScale = 2 * InputManager.TouchSize * Vector3.one;
			Mask.transform.localScale = (2 *  InputManager.TouchSize - width) * Vector3.one;
		}
	}
}