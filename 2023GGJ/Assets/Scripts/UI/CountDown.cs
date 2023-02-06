using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GGJ;

public class CountDown : MonoBehaviour
{
	public Text text;
	public List<string> Text;
	public List<AudioClip> 音效组;
	public float TargetAlpha;


	private IEnumerator Start()
	{
		var i = 0;
		foreach (var item in Text)
		{
			this.GetComponent<AudioSource>().PlayOneShot(音效组[i], 1f);
			text.text = Text[i];
			text.transform.localScale = Vector3.one * 10;
			text.color = text.color - new Color(0,0,0,1);
			text.DOFade(TargetAlpha, 0.3f).SetUpdate(true);
			text.transform.DOScale(1, 0.3f).SetUpdate(true);
			if (i == Text.Count - 1)
			{
				yield return new WaitForSecondsRealtime(0.25F);
				CameraManager.Instance.Shake();
				yield return new WaitForSecondsRealtime(0.75F);
			}
			else
			{
				yield return new WaitForSecondsRealtime(1);
			}

			i++;
		}
		Destroy(gameObject);
	}

}
