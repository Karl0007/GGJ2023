using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace GGJ
{
	public class GameManager : Singleton<GameManager>
	{
		public float timeSpend;
		public float gametime;
		private Wave[] 波次数量组;
		public Text 倒计时;
		public GameObject 预警时间预制体;
		public GameObject canvas;
		public GameObject FinishCountDown;
		public GameObject Account;
		public GameObject BonusCountDown;
		public GameObject Legendary;
		public GameObject Unbelievable;
		public GameObject Nice;
		public Text HighScore;
		public Text HighScoreAccount;
		public Text ScoreAccount;

		public bool 奖励时间;
		public float 已持续奖励时间;
		public int 奖励时间次数;
		public float 上一次奖励时间已有;

		public AudioClip[] BGM组;

		private IEnumerator Start()
		{
			Time.timeScale = 0;
			CreatInitBall();
			波次数量组 = WaveManager.Instance.GetWaveGoup();
			HighScore.text = "High Score : " + (PlayerPrefs.HasKey(ScoreManager.HighScoreKey) ? PlayerPrefs.GetInt(ScoreManager.HighScoreKey) : 0);
			yield return new WaitForSecondsRealtime(4);
			Hint.Instance.SelectType(BallManager.Instance.Root.Type);
			Time.timeScale = 1;
			yield return new WaitForSecondsRealtime(gametime - 6);
			FinishCountDown.SetActive(true);
			yield return new WaitForSecondsRealtime(4.7f);
			BallManager.Instance.Root.Remove(1, 0);
			yield return new WaitForSecondsRealtime(1.3f);
			ScoreManager.Instance.EndGame();
			Account.SetActive(true);
			HighScoreAccount.text = "High Score : " + (PlayerPrefs.HasKey(ScoreManager.HighScoreKey) ? PlayerPrefs.GetInt(ScoreManager.HighScoreKey) : 0);
			ScoreAccount.text = "Your Score : " + ScoreManager.Instance.Score;
			Time.timeScale = 0;
		}

		private void Update()
		{
			gametime -= Time.deltaTime;
			倒计时.text = "Time : " + gametime.ToString("f2");
			timeSpend += Time.deltaTime;//记录累计时间
			for (int i = 0; i < 波次数量组.Length; i++)
			{
				if (timeSpend > 波次数量组[i].出现时间)
				{
					波次数量组[i].累计间隔时间 += Time.deltaTime;
					if (波次数量组[i].是否可以发射)//第一次发射
					{
						波次数量组[i].是否可以发射 = false;
						BallManager.Instance.GenerateEnemyWave(波次数量组[i]);//创建初始一波

					}
					else if (波次数量组[i].累计间隔时间 >= 波次数量组[i].间隔时间)
					{

						BallManager.Instance.GenerateEnemyWave(波次数量组[i]);//创建一波
						波次数量组[i].累计间隔时间 = 0;

					}
					else if (波次数量组[i].间隔时间 - 波次数量组[i].累计间隔时间 <= 波次数量组[i].预警时间 && 波次数量组[i].处于预警 == false)//当 间隔创建剩余时间小于等于 预警时间时，创建预警预制体
					{
						CreatWarningTime(i);
					}
				}
				else if ((波次数量组[i].出现时间 - timeSpend) <= 波次数量组[i].预警时间 && 波次数量组[i].处于预警 == false)//当第一次创建的剩余时间小于等于 预警时间时，创建预警预制体
				{
					CreatWarningTime(i);
				}
			}


			上一次奖励时间已有 += Time.deltaTime;
			if (timeSpend>=30 && 奖励时间==false && 奖励时间次数>0)//判断可以出现奖励时间了
            {

                    if (上一次奖励时间已有>30 && Random.Range(0,26)>24)
                    {
						奖励时间次数--;
						上一次奖励时间已有 = 0;
						奖励时间 = true;
						已持续奖励时间 = 0;
						this.GetComponent<AudioSource>().Stop();
						this.GetComponent<AudioSource>().PlayOneShot(BGM组[1], 1f);
					Instantiate(BonusCountDown, BonusCountDown.transform.parent).SetActive(true);
					Debug.Log("进入奖励时间");
					}
            }
            else if (gametime<=30 && 奖励时间次数>0)
            {
				奖励时间次数--;
				上一次奖励时间已有 = 0;
				奖励时间 = true;
				已持续奖励时间 = 0;
				this.GetComponent<AudioSource>().Stop();
				this.GetComponent<AudioSource>().PlayOneShot(BGM组[1], 1f);
				Instantiate(BonusCountDown, BonusCountDown.transform.parent).SetActive(true);
				Debug.Log("进入奖励时间");
			}
            else
            {
                if (奖励时间==true)
                {
					已持续奖励时间 += Time.deltaTime;
                    if (已持续奖励时间 < 10)
                    {
						List<float> bgcolor = new List<float>();
						for (int i = 0; i < 3; i++)
                        {
							bgcolor.Add(Random.Range(0, 30) / 255f);

						}
						int x = Random.Range(0, 2);//随机为0的数量
                        if (x==1)
                        {
							int y1 = Random.Range(0, 3); //随机为0的位数
							bgcolor[y1] = 0;
						}
                        else
                        {
							while (true)
							{
								int y1 = Random.Range(0, 3); //随机为0的位数
								int y2 = Random.Range(0, 3); //随机为0的位数
								if (y1 != y2)
								{
									bgcolor[y1] = 0;
									bgcolor[y2] = 0;
									break;
								}
							}
						}
                            
						CameraManager.Instance.gameObject.GetComponent<Camera>().backgroundColor = new Color(bgcolor[0], bgcolor[1], bgcolor[2], 255/255f);
					}
                    else
                    {
						奖励时间 = false;
						this.GetComponent<AudioSource>().Stop();
						this.GetComponent<AudioSource>().clip=BGM组[0];
						this.GetComponent<AudioSource>().Play();
						Debug.Log("奖励时间结束");



						Hint.Instance.SelectType(BallManager.Instance.Root.Type);
						//CameraManager.Instance.gameObject.GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 255 / 255f);
					}
					
				}
				
			}
		}
		private void CreatWarningTime(int i)
		{
			波次数量组[i].当前创建数量 = UnityEngine.Random.Range(波次数量组[i].最小数量, 波次数量组[i].最大数量);
			波次数量组[i].属性 = (BallType)Random.Range(1, 4);//随机一波属性-到时候会放在预警块
			var pos = BallManager.Instance.RandomPositionDirection();//获取初始随机位置
			波次数量组[i].处于预警 = true;
			波次数量组[i].波次出现位置 = pos;
			//设置位置限制在屏幕边缘
			var 预制体位置 = Camera.main.WorldToScreenPoint(pos);
			if (预制体位置.x > 0)
			{
				if (预制体位置.x > Screen.width - 50)
				{
					预制体位置.x = Screen.width - 100;
				}
			}
			else
			{
				if (预制体位置.x < 50)
				{
					预制体位置.x = 100;
				}
			}
			if (预制体位置.y > 0)
			{
				if (预制体位置.y > Screen.height - 50)
				{
					预制体位置.y = Screen.height - 100;
				}
			}
			else
			{
				if (预制体位置.y < 50)
				{
					预制体位置.y = 100;
				}
			}



			var WarningTtime = Instantiate(预警时间预制体, Camera.main.ScreenToWorldPoint(预制体位置), Quaternion.identity).GetComponentInChildren<WarningTtime>();

			//设置朝向
			Vector3 rotateVector = Vector3.zero - WarningTtime.gameObject.transform.position;
			rotateVector.z = 0;
			float anglie = Vector3.SignedAngle(Vector3.up, rotateVector, Vector3.forward);
			Quaternion rotaion = new Quaternion();
			if (预制体位置.x < Screen.width / 2)
			{
				rotaion = Quaternion.Euler(0, 0, anglie);
				WarningTtime.左边();
			}
			else
			{
				rotaion = Quaternion.Euler(0, 0, anglie + 180);
				WarningTtime.右边();
			}
			WarningTtime.gameObject.transform.rotation = rotaion;



			//设置具体值
			WarningTtime.剩余时间值 = 波次数量组[i].预警时间;
			WarningTtime.变化(波次数量组[i]);
		}
		public void CreatInitBall()
		{
			var root = BallManager.Instance.CreatNewBall(Vector2.zero);
			BallManager.Instance.SetAsRoot(root);
			var left = BallManager.Instance.CreatNewBall(Vector2.left);
			var right = BallManager.Instance.CreatNewBall(Vector2.right);

			//var left1 = BallManager.Instance.CreatNewBall(Vector2.left*2);
			//var right1 = BallManager.Instance.CreatNewBall(Vector2.right*2);
			//var left2 = BallManager.Instance.CreatNewBall(Vector2.left*3);
			//var right2 = BallManager.Instance.CreatNewBall(Vector2.right*3);

			root.AddChildren(left);
			root.AddChildren(right);
			//left.AddChildren(left1);
			//left.AddChildren(left2);
			//right.AddChildren(right1);
			//right.AddChildren(right2);
			BallManager.Instance.UpdateRender();
			BallManager.Instance.Root.UpdateDeepth();
		}

		public void RestartGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
