using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GGJ
{
	public class BallRender : MonoBehaviour
	{
		public SpriteRenderer mainCircle;
		public SpriteRenderer outCircle;
		private Tween alpha;
		private Tween color;
		private Tween size;
		private Tween flash;

		public void SetAsEnemy(bool danger,bool same)
		{
			outCircle.gameObject.SetActive(danger || same);
			outCircle.color = same ? Color.white : Color.red;
			flash?.Kill();
			outCircle.transform.localScale = Vector3.one * 1.1f;
			if (danger) flash = outCircle.transform.DOScale(1.3f, 0.15f).SetLoops(-1, LoopType.Yoyo);
		}

		public void SetActivation(bool activation)
		{
			alpha?.Complete();
			alpha = mainCircle.DOFade(activation ? 1f : 0.3f, 0.4f).OnComplete(() => alpha = null);
		}

		public void SetIsRoot(bool root)
		{
			flash?.Kill();
			outCircle.color = Color.green;
			outCircle.gameObject.SetActive(root);
		}

		public void SetDeepth(int deepth)
		{
			size?.Complete();
			size = transform.DOScale(BallManager.Instance.DeepthToSize(deepth), 0.4f).OnComplete(() => size = null);
		}

		public void SetType(BallType type)
		{
			color?.Complete();
			color = mainCircle.DOColor(type.GetColor(), 0.4f).OnComplete(() => color = null);
			return;
		}

		private void OnDestroy()
		{
			alpha?.Kill();
			color?.Kill();
			size?.Kill();
			flash?.Kill();
		}

	}

}
