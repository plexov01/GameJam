namespace GameJam.Features.UI
{
    using GameJob.Features.UI;
    using System.Collections.Generic;
    using UnityEngine;
    /// <summary>
    /// Менеджер управления панелями
    /// </summary>
    public class PanelsManager : MonoBehaviour
    {
        private List<PanelInstanceModel> _panelInstanceModels = new List<PanelInstanceModel>();

        private PanelPool _panelPool = default;

        private void Start()
        {
            _panelPool = FindObjectOfType<PanelPool>();
        }
        /// <summary>
        /// Показать панель
        /// </summary>
        /// <param name="panelName"></param>
        public void ShowPanel(string panelName)
        {
            GameObject panelInstance = _panelPool.GetPanelFromPool(panelName);

            if (panelInstance!=null)
            {
                if (_panelInstanceModels.Count>0)
                {
                    PanelInstanceModel lastPanelInstance = _panelInstanceModels[_panelInstanceModels.Count - 1];
                    
                    lastPanelInstance?.Panel.SetActive(false);
                }
                _panelInstanceModels.Clear();
                _panelInstanceModels.Add(new PanelInstanceModel
                {
                    panelName = panelName,
                    Panel = panelInstance
                });
            }
            else
            {
                Debug.LogWarning($"Trying to use panelId = {panelName}, but this is not found in the ObjectPool");
            }
        }
        
    }

}
