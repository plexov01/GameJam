namespace GameJam.Features.UI
{ 
	using UnityEngine;
	using System;
	using UnityEngine.Serialization;

	/// <summary>
	/// Экземпляр панели
	/// </summary>
	[Serializable]
	public class PanelInstanceModel
	{
		/// <summary>
		/// Имя панели
		/// </summary>
		public string panelName = default;
		/// <summary>
		/// Экзмепляр панели
		/// </summary>
		public GameObject Panel = default;
	}
}