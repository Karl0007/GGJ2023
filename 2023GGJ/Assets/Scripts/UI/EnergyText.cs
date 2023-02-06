using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ {

	public class EnergyText : MonoBehaviour
	{
		public Text text;
		public float value;

		// Update is called once per frame
		void Update()
		{
			if (ScoreManager.Instance.VelocityAdd >= value)
			{
				text.transform.localScale = Vector3.one * 1.5f;
				text.color = Color.yellow;
			}
			else
			{
				text.transform.localScale = Vector3.one;
				text.color = Color.white;
			}
		}

	}
}