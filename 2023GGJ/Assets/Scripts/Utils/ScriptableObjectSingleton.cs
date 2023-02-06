using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GGJ
{

    public abstract class ScriptableObjectSingleton<T> : SerializedScriptableObject where T : ScriptableObjectSingleton<T>
    {
		#region Inst
		/// <summary>
		/// 配置路径。
		/// </summary>
		protected static string DataPath => "Data";
		protected static string ObjectPath => typeof(T).Name;
		protected static string FinalPath => $"{DataPath}/{ObjectPath}";

		private static T m_Instance;
		/// <summary>
		/// 实例。
		/// </summary>
		public static T inst
		{
			get
			{
				if (m_Instance == null)
				{
#if UNITY_EDITOR
					Editor_RefreshInstance();
#endif
					m_Instance = Resources.Load<T>(FinalPath);
				}
				return m_Instance;
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// 刷新实例。
		/// </summary>
		public static void Editor_RefreshInstance()
		{
			m_Instance = Resources.Load<T>(FinalPath);
			if (m_Instance == null)
			{
				m_Instance = CreateInstance<T>();
				UnityEditor.AssetDatabase.CreateAsset(m_Instance, $"Assets/Resources/{FinalPath}.asset");
			}
		}
#endif
		#endregion
	}
}
