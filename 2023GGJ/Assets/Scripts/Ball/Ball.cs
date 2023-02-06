using HedgehogTeam.EasyTouch;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ
{
	public class Ball : MonoBehaviour
	{
		public new Rigidbody2D rigidbody;
		public new Collider2D collider;
		public Collider2D trigger;
		public BallRender render;

		public HashSet<Ball> Children = new HashSet<Ball>();
		public Ball Parent = null;


		private int deep;
		public int Deep {
			get => deep; 
			private set
			{
				render.SetDeepth(value);
				GetComponents<SpringJoint2D>().ForEach((x) => x.distance = BallManager.Instance.DeepthToSize(value) * 1.1F);
				//rigidbody.mass = BallManager.Instance.DeepthToSize(value);
				deep = value;
			}
		}

		public bool isDestroy { get; private set; }

		private bool isActive;
		public bool IsActive
		{
			get => isActive;
			set 
			{
				render.SetActivation(value);
				isActive = value; 
			}
		}

		private bool isRoot;
		public bool IsRoot
		{
			get => isRoot;
			private set
			{
				render.SetIsRoot(value);
				isRoot = value;
			}
		}

		private BallType type;
		public BallType Type { 
			get => type;
			set
			{
				render.SetType(value);
				type = value;
			} 
		}

		private bool isEnemy;
		public bool IsEnemy
		{
			get => isEnemy;
			set 
			{
				isEnemy = value; 
			}
		} 

		private void Start()
		{
            if (Type == BallType.None)
            {
				Type = (BallType)Random.Range((int)BallType.None + 1, (int)BallType.End);
			}
			
		}

		public void UpdateState()
		{

			if (IsEnemy)
			{
				//IsRoot = false;
				//IsActive = true;
				render.SetAsEnemy(!BallManager.Instance.Root.Type.IsSafe(Type), BallManager.Instance.Root.Type == Type);
				trigger.gameObject.layer = collider.gameObject.layer = LayerMask.NameToLayer("Enemy");
			}
			else
			{
				IsRoot = BallManager.Instance.Root == this;
				IsActive = BallManager.Instance.Root.Type == Type;
				trigger.gameObject.layer = collider.gameObject.layer = LayerMask.NameToLayer(IsActive ? "Player" : "NoActive");
			}
		}

		public void SetRoot(bool root)
		{
			IsRoot = root;
			if (IsRoot)
			{
				rigidbody.bodyType = RigidbodyType2D.Kinematic;
				SetParentAsChildren();
			}
			else
			{                                                                                                    
				rigidbody.bodyType = RigidbodyType2D.Dynamic;
			}
		}

		public void UpdateDeepth(int deep = 0)
		{
			if (isDestroy) return;
			Deep = deep;
			foreach (var ball in Children)
			{
				ball.UpdateDeepth(deep + 1);
			}
		}

		public void SetParentAsChildren()
		{
			if (Parent == null) return;
			Parent.SetParentAsChildren();
			var joints = Parent.GetComponents<SpringJoint2D>().Where(joint => joint.connectedBody == rigidbody);
			Debug.Assert(joints.Count() == 1);
			Destroy(joints.FirstOrDefault());
			//Debug.Assert(Parent.Children.Contains(this) && Edges.Contains(Parent) && Parent.Edges.Contains(this) && !Children.Contains(Parent));
			Parent.Children.Remove(this);
			m_AddChildren(Parent);
			Parent.SetRoot(false);
			Parent = null;
		}

		public void AddChildren(Ball child)
		{

			if (child == null) return;
			//Debug.Assert(!Edges.Contains(child) && !Children.Contains(child) && child.Parent == null);
			m_AddChildren(child);
		}

		public void Remove(int layer = 0,float time = 0.15f)
		{


			if (isDestroy) return;
			foreach (var ball in Children)
			{
				ball.Remove(layer + 1);
			}
			Children.Clear();
			if (!IsRoot) 
			{
				isDestroy = true;
				BallManager.Instance.AllBalls.Remove(this);
				trigger.enabled = collider.enabled = false;
				deep = layer;
				Invoke(nameof(Kill), layer * time);
			}
			else
			{
				GetComponents<SpringJoint2D>().ForEach((x) => Destroy(x));
			}
		}

		private void Kill()
		{
			if (!IsEnemy) ScoreManager.Instance.OnKill(rigidbody.position, deep + 1);
			Type.PlayKill(transform.position);
			EffectManager.Instance.PlayAudio(ResourcesManager.inst.KillAudio.RandomSelect());
			Destroy(gameObject);
		}

		private void m_AddChildren(Ball child)
		{
			var joint = gameObject.AddComponent<SpringJoint2D>();
			joint.connectedBody = child.rigidbody;
			DataManager.inst.InitJoint(joint);
			Children.Add(child);
			child.Parent = this;
			child.Deep = Deep + 1;
		}

		public void InitEnemy(Vector2 pos,BallType type)
		{
			Type = type;
			IsEnemy = true;
			rigidbody.MovePosition(pos);
			rigidbody.bodyType = RigidbodyType2D.Kinematic;
			rigidbody.velocity = 
				((Vector2)BallManager.Instance.Root.transform.position + Random.insideUnitCircle * DataManager.inst.TargetRange - pos).normalized * 
				Random.Range(DataManager.inst.EnemyVelocity.x, DataManager.inst.EnemyVelocity.y);
			UpdateState();
		}

		public void FindNearest(Vector2 pos,List<Ball> balls)
		{
			if (isDestroy) return;
			foreach (var ball in Children)
			{
				if (balls[ball.Type.GetIdx()] == null ||
					Vector2.Distance(ball.transform.position, pos) < Vector2.Distance(balls[ball.Type.GetIdx()].transform.position, pos))
				{
					balls[ball.Type.GetIdx()] = ball;
				}
				ball.FindNearest(pos, balls);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (isDestroy || !IsActive || IsEnemy) return;
			var other = collision.gameObject.GetComponentInParent<Ball>();
			if (other != null && other.IsEnemy)
			{
				if (Type.IsSafe(other.Type))
				{
					other.IsEnemy = false;
					AddChildren(other);
					other.SetRoot(false);
					EffectManager.Instance.PlayAudio(ResourcesManager.inst.MergeAudio.RandomSelect());
					EffectManager.Instance.PlayEffect(ResourcesManager.inst.Merge, (transform.position + other.transform.position) / 2);
					ScoreManager.Instance.OnMerge(rigidbody.position);
				}
				else
				{
					Parent?.Children.Remove(this);
					Remove();
					other.Remove();
				}
			}
			other.UpdateState();
		}

	}
}