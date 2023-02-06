using HedgehogTeam.EasyTouch;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{

	public static class EnumUtils
	{
		public static int GetIdx(this BallType self)
		{
			switch (self)
			{
				case BallType.Fire:
					return 0;
				case BallType.Water:
					return 1;
				case BallType.Grass:
					return 2;
				default:
					return -1;
			}
		}

		//顺序传达
		public static BallType GetNext(this BallType self)
		{
			switch (self)
			{
				case BallType.Fire:
					return BallType.Grass;
				case BallType.Water:
					return BallType.Fire;
				case BallType.Grass:
					return BallType.Water;
				default:
					return BallType.None;
			}
		}
		//颜色优化
		public static Color GetColor(this BallType self)
		{
			switch (self)
			{
				case BallType.Fire:
					return DataManager.inst.FireColor;
				case BallType.Water:
					return DataManager.inst.WaterColor;
				case BallType.Grass:
					return DataManager.inst.GrassColor;
				default:
					return Color.white;
			}
		}
		public static Color GetBGColor(this BallType self)
		{
			switch (self)
			{
				case BallType.Fire:
					return DataManager.inst.FireBG;//= new Color(65/255f,0,0,1);
				case BallType.Water:
					return DataManager.inst.WaterBG;//= new Color(0, 35 / 255f, 65 / 255f, 1);
				case BallType.Grass:
					return DataManager.inst.GrassBG;// = new Color(0, 65 / 255f, 21 / 255f, 1);
				default:
					return Color.black;
			}
		}

		public static bool IsSafe(this BallType self,BallType other)
		{
			switch (self)
			{
				case BallType.Fire:
					return other == BallType.Fire || other == BallType.Grass;
				case BallType.Water:
					return other == BallType.Water || other == BallType.Fire;
				case BallType.Grass:
					return other == BallType.Grass || other == BallType.Water;
				default:
					return false;
			}
		}

		public static void PlayKill(this BallType type,Vector3 pos)
		{
			switch (type)
			{
				case BallType.Fire:
					EffectManager.Instance.PlayEffect(ResourcesManager.inst.FireKill, pos);
					break;
				case BallType.Water:
					EffectManager.Instance.PlayEffect(ResourcesManager.inst.WaterKill, pos);
					break;
				case BallType.Grass:
					EffectManager.Instance.PlayEffect(ResourcesManager.inst.GrassKill, pos);
					break;
				default:
					break;
			}
		}
	}

	public static class RandomUtils
	{
		public static T RandomSelect<T>(this List<T> list)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
	}

	public static class RectUtils
	{
		public static Vector2 Clamp(this Rect area, Vector2 pos)
		{
			return new Vector2(Mathf.Clamp(pos.x, area.xMin, area.xMax), Mathf.Clamp(pos.y, area.yMin, area.yMax));
		}

		public static Vector2 FixedTouchPos(this Gesture gesture)
		{
			return InputManager.Instance.TouchArea.Clamp(gesture.GetTouchToWorldPoint(0));
		}
	}
	public enum BallType
	{
		None,
		Fire,
		Water,
		Grass,
		End
	}

	public class BallManager : Singleton<BallManager>
	{
		[ShowInInspector]
		public Ball Root { get; private set; }
		[ShowInInspector]
		public HashSet<Ball> AllBalls { get; private set; } = new HashSet<Ball>();
		public HashSet<Ball> NewBalls { get; private set; } = new HashSet<Ball>();//新建的数值组
		protected Vector3 RootTarget;
		public float MaxDist { get; private set; }
		public float AvgVelocity { get; private set; }
		public int BallCnt { get; private set; }
		public float DeepthToSize(int x) => Mathf.Lerp(1.3f, 0.3f, Mathf.Clamp01(x / 10f));


		private void Start()
		{
			StartCoroutine(GenerateEnemy());
		}

		private IEnumerator GenerateEnemy()
		{
			while (true)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.4f, 1f));
				for (int i = 0; i < UnityEngine.Random.Range(1, 2); i++)
				{
					NewEnemy();
				}
			}

		}
        public void GenerateEnemyWave(Wave 波次数量)
		{
			NewBalls.Clear();
			波次数量.处于预警 = false;
			
			var pos =波次数量.波次出现位置;//获取初始随机位置方向
			for (int i = 0; i < 波次数量.当前创建数量; i++)
            {
				Ball 球= CreatNewBall(RandomBallPositionDirection(pos, 波次数量.当前创建数量));
				球.InitEnemy(pos,波次数量.属性);
			}
		}


		public Vector3 RandomPositionDirection()
        {
			var signX = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
			var signY = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
			var posX = UnityEngine.Random.Range(0, 20);
			var posY = UnityEngine.Random.Range(18, 20);
			var pos = UnityEngine.Random.Range(0, 2) == 0 ? new Vector2(posX * signX, posY * signY) : new Vector2(posY * signX, posX * signY);
			return pos;
		}
		private Vector3 RandomBallPositionDirection(Vector3 v3,int 数量)
        {
			var 修正值 = 数量 /10;
			var posX = UnityEngine.Random.Range(v3.x-修正值,v3.x+ 修正值);
			var posY = UnityEngine.Random.Range(v3.y- 修正值, v3.y + 修正值);
			var pos = new Vector2(posX, posY);
			return pos;
		}
		public void NewEnemy()
		{
			var pos = RandomPositionDirection();
			CreatNewBall(pos).InitEnemy(pos, (BallType)UnityEngine.Random.Range((int)BallType.None + 1, (int)BallType.End));
		}

		private void FixedUpdate()
		{
			RefreshDrag();
			MoveToTarget();
			CheckDist();
		}

		public void CheckDist()
		{
			MaxDist = 0;
			AvgVelocity = 0;
			BallCnt = 0;
			AllBalls.RemoveWhere((x) => {
				if (x.IsEnemy && x.transform.position.magnitude > 50f)
				{
					Destroy(x.gameObject);
					return true;
				}
				else
				{
					if (!x.IsEnemy)
					{
						MaxDist = Mathf.Max(MaxDist, Mathf.Max(Mathf.Abs(x.transform.position.x), Mathf.Abs(x.transform.position.y)));
						AvgVelocity += x.rigidbody.velocity.magnitude;
						BallCnt += 1;
					}
				}
				return false;
			});
			AvgVelocity /= BallCnt;
		}

		public void ChangeRootElement(Vector2 pos)
		{
			List<Ball> ballList = new List<Ball>() { null, null, null };
			Root.FindNearest(pos, ballList);
			if (ballList[Root.Type.GetNext().GetIdx()] != null)
			{
				SetAsRoot(ballList[Root.Type.GetNext().GetIdx()]);
				EffectManager.Instance.PlayAudio(ResourcesManager.inst.SwithAudio.RandomSelect());
			}
			else if (ballList[Root.Type.GetNext().GetNext().GetIdx()] != null)
			{
				SetAsRoot(ballList[Root.Type.GetNext().GetNext().GetIdx()]);
				EffectManager.Instance.PlayAudio(ResourcesManager.inst.SwithAudio.RandomSelect());
			}
		}
		public void RefreshSize()
		{
			AllBalls.ForEach(ball => ball.render.transform.localScale = 
				Mathf.Lerp(1f, 0.5f, Mathf.Clamp01(ball.Deep / 7f)) * Vector3.one);
		}

		public void RefreshDrag()
		{
			AllBalls.ForEach(ball => ball.rigidbody.drag = 
			Mathf.Lerp(
				0,
				Mathf.Lerp(
					DataManager.inst.MaxDrag,
					1,
					Mathf.Clamp01(ball.Deep / 7f)
				),
				Mathf.Clamp01(ball.rigidbody.velocity.magnitude / DataManager.inst.MaxVelocity))
			);
		}

		public void MoveToTarget()
		{
			Root.transform.position += (RootTarget - Root.transform.position) * 0.15f;
			Root.rigidbody.velocity *= 0.95f;
		}

		public void UpdateRender()
		{
			AllBalls.ForEach(ball => { if (!ball.isDestroy) ball.UpdateState(); });
		}

		[Button("Creat")]
		public Ball CreatNewBall(Vector2 pos)
		{
			var ball = Instantiate(ResourcesManager.inst.BallPrefab, pos, Quaternion.identity).GetComponent<Ball>();
			AllBalls.Add(ball);
			return ball;
		}

		[Button("Link")]
		public void LinkBall(Ball parent,Ball children)
		{
			parent.AddChildren(children);
		}

		[Button("Root")]
		public void SetAsRoot(Ball root)
		{
			if (root == Root) return;
			Root?.SetRoot(false);
			root.SetRoot(true);
			Root = root;
			Root.UpdateDeepth();
			UpdateRender();
			Hint.Instance.SelectType(Root.Type);
		}

		[Button("Remove")]
		public void RemoveBall(Ball ball)
		{
			ball.Remove();
			UpdateRender();
		}

		public void SetRootTarget(Vector2 pos)
		{
			RootTarget = pos;
			RootTarget.z = Root.transform.position.z;
		}

		public bool CheckClickRoot(Vector2 pos)
		{
			foreach (var item in AllBalls)
			{
				if (!item.isDestroy && !item.IsEnemy && item.rigidbody.OverlapPoint(pos))
				{
					SetAsRoot(item);
					return true;
				}
			}
			return false;
		}
    }
}