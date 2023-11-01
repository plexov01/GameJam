namespace GameJob.Features.UI
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    /// <summary>
    /// Пул панелей
    /// </summary>
    public class PanelPool : MonoBehaviour
    {
        private List<GameObject> _prebasPanelForPool = new List<GameObject>();
        private List<GameObject> _pooledPanels = new List<GameObject>();

        private void Awake()
        {
            _prebasPanelForPool = Resources.LoadAll<GameObject>("PrefabsPanel").ToList();
            foreach (Transform child in transform)
            {
                _pooledPanels.Add(child.gameObject);
            }
        }
        /// <summary>
        /// Получить панель из пула
        /// </summary>
        /// <param name="panelName"></param>
        public GameObject GetPanelFromPool(string panelName)
        {
            GameObject instance = _pooledPanels.FirstOrDefault(x => x.name == panelName);

            if (instance!=null)
            {
                instance.SetActive(true);
                return instance;
            }

            GameObject prefab = _prebasPanelForPool.FirstOrDefault(x => x.name == panelName);
            if (prefab != null)
            {
                // Create a new instance
                GameObject newInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
         
                // Make sure you set it's name (so you remove the Clone that Unity ads)
                newInstance.name = panelName;

                // Set it's position to zero
                newInstance.transform.localPosition = Vector3.zero;
         
                _pooledPanels.Add(newInstance);
         
                return newInstance;
            }
      
            Debug.LogWarning("Object pool doesn't have a prefab for the object with name " + panelName);
            return null;
        }
    }
}

