namespace GameJam.Features.UI
{ 
	using UnityEngine;
	using System;

	/// <summary>
	/// Экземпляр панели
	/// </summary>
	[Serializable]
	public class PanelInstanceModel
	{
		/// <summary>
		/// Id панели
		/// </summary>
		public string PanelId = default;
		/// <summary>
		/// Экзмепляр панели
		/// </summary>
		public GameObject Panel = default;
	}
}