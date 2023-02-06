using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ {
	public class ScoreManager : Singleton<ScoreManager> 
	{
		public const string HighScoreKey = "HighScore";
		public int Score { get; private set; }
		public int CurScore { get; private set; }
		public int MergeCombo { get; private set; }
		public int KillCombo { get; private set; }
		public float VelocityAdd { get; private set; }


		private const float ComboCd = 2f;
		private float LastMergeTime;
		private float LastKillTime;
		private bool IsMerging;
		private bool IsKilling;
		private bool InMergeTime => LastMergeTime + ComboCd > Time.time;
		private bool InKillTime => LastKillTime + ComboCd > Time.time;
		private RectTransform CanvasRect => GameManager.Instance.canvas.transform as RectTransform;
		
		
		[SerializeField] private Transform ComboTransform;


		[SerializeField] private Text ScoreNum;
		[SerializeField] private Slider energy;

		private Tween textTw;
		private Tween scaleTw;
		private Tween colorTw;

		public void EndGame()
		{
			MergeEnd();
			KillEnd();
			ScoreNum.text = Score.ToString();
			if (PlayerPrefs.HasKey(HighScoreKey))
			{
				PlayerPrefs.SetInt(HighScoreKey, Mathf.Max(PlayerPrefs.GetInt(HighScoreKey), Score));
			}
			else
			{
				PlayerPrefs.SetInt(HighScoreKey, Score);
			}
		}

		public static Vector2 WorldToUGUIPosition(RectTransform canvasRectTransform, Camera camera, Vector3 worldPosition)
		{

			//世界坐标-》ViewPort坐标? ?

			Vector2 viewPos = (Vector2)camera.WorldToViewportPoint(worldPosition) - canvasRectTransform.pivot;

			//ViewPort坐标-〉UGUI坐标

			return new Vector2(canvasRectTransform.rect.width * viewPos.x, canvasRectTransform.rect.height * viewPos.y);

		}

		public void AddScore() {
			textTw?.Complete();
			scaleTw.Complete();
			colorTw?.Complete();
			textTw = DOTween.To(() => CurScore, x => { CurScore = x; ScoreNum.text = CurScore.ToString(); }, Score, 0.4F).OnComplete(() => textTw = null);
			scaleTw = ScoreNum.transform.parent.DOScale(1.5F, 0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() => scaleTw = null);
			colorTw = ScoreNum.DOColor(Color.red,0.2f).SetLoops(2, LoopType.Yoyo).OnComplete(() => colorTw = null);
		}

		private void FixedUpdate()
		{
			if (IsMerging && !InMergeTime) MergeEnd();
			if (IsKilling && !InKillTime) KillEnd();
			var targetEnergy = Mathf.Clamp(BallManager.Instance.AvgVelocity / 20F + Mathf.Clamp01(BallManager.Instance.BallCnt / 45f), 1, 4);
			if (targetEnergy > VelocityAdd) VelocityAdd = targetEnergy; else VelocityAdd *= 0.997f;
			VelocityAdd = Mathf.Clamp(VelocityAdd, 1, 4);
			energy.value = (VelocityAdd - 1) / 2F;
		}


		public void KillEnd()
		{
			var delta = Mathf.CeilToInt(KillCombo * 10);
			Score += delta;
			AddScore();
			KillCombo = 0;
			LastKillTime = 0;
			IsKilling = false;
		}

		public void MergeEnd()
		{
			var delta = Mathf.CeilToInt(MergeCombo * MergeCombo);
			Score += delta;
			AddScore();
			MergeCombo = 0;
			LastMergeTime = 0;
			IsMerging = false;
		}

		[Button("Merge")]
		public void OnMerge(Vector2 pos)
		{
			//击杀combo优先级高于合成
			//改一下 加一个总球数系数 调一下优先级
			if (IsKilling)
			{
				return;
			}
			var add = (int)VelocityAdd * (GameManager.Instance.奖励时间 ? 2 : 1);
			var number = ResourcesManager.inst.MergeNumber.Spawn(Vector3.zero, add);
			
			//if (!IsMerging)
			//{

			//	number.SetAnchoredPosition(CanvasRect, ComboTransform.localPosition);
			//}
			//else
			//{
				var uiPos = WorldToUGUIPosition(CanvasRect, Camera.main, pos);
				number.SetAnchoredPosition(CanvasRect, uiPos);
			//}

			if (MergeCombo < DataManager.inst.Merge1 && MergeCombo + add >= DataManager.inst.Merge1)
				StartCoroutine(PlayDelay(0.6f,GameManager.Instance.Nice));
			if (MergeCombo < DataManager.inst.Merge2 && MergeCombo + add >= DataManager.inst.Merge2)
				StartCoroutine(PlayDelay(0.6f, GameManager.Instance.Unbelievable));
			if (MergeCombo < DataManager.inst.Merge3 && MergeCombo + add >= DataManager.inst.Merge3)
				StartCoroutine(PlayDelay(0.6f, GameManager.Instance.Legendary));

			IsMerging = true;
			MergeCombo += add;
			LastMergeTime = Time.time;
		}

		public void OnKill(Vector2 pos,int layer)
		{
			if (IsMerging)
			{
				MergeEnd();
			}
			var add = layer * (int)VelocityAdd * (GameManager.Instance.奖励时间 ? 2 : 1);

			var number = ResourcesManager.inst.KillNumber.Spawn(Vector3.zero, add);
			number.SetAnchoredPosition(CanvasRect, ComboTransform.localPosition);

			if (KillCombo < DataManager.inst.Combo1 && KillCombo + add >= DataManager.inst.Combo1)
				StartCoroutine(PlayDelay(0,GameManager.Instance.Nice));
			if (KillCombo < DataManager.inst.Combo2 && KillCombo + add >= DataManager.inst.Combo2)
				StartCoroutine(PlayDelay(0, GameManager.Instance.Unbelievable));
			if (KillCombo < DataManager.inst.Combo3 && KillCombo + add >= DataManager.inst.Combo3)
				StartCoroutine(PlayDelay(0, GameManager.Instance.Legendary));


			IsKilling = true;
			KillCombo += add;
			LastKillTime = Time.time;
		}

		public IEnumerator PlayDelay(float time,GameObject go)
		{
			yield return new WaitForSecondsRealtime(time);
			Instantiate(go, go.transform.parent).SetActive(true);
		}
	}

}