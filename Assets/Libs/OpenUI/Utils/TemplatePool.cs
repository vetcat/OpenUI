using System.Collections.Generic;
using UnityEngine;

namespace Libs.OpenUI.Utils
{
    public class TemplatePool<T> where T : MonoBehaviour
    {
        private List<T> _instantiatedTemplates;
        private T _template;
        private Transform _parent;

        public TemplatePool(T template, Transform parent, int capacity = 6)
        {
            template.gameObject.SetActive(false);
            _template = template;
            _parent = parent;
            _instantiatedTemplates = new List<T>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                var instance = GameObject.Instantiate(template, parent, false);
                instance.gameObject.SetActive(false);
                _instantiatedTemplates.Add(instance);
            }
        }

        public T GetFirstAvailable()
        {
            foreach (var template in _instantiatedTemplates)
            {
                if (!template.gameObject.activeSelf)
                {
                    template.gameObject.SetActive(true);
                    return template;
                }
            }

            var instance = GameObject.Instantiate(_template, _parent, false);
            instance.gameObject.SetActive(true);
            _instantiatedTemplates.Add(instance);
            return instance;
        }

        public void ReleaseAll()
        {
            foreach (var template in _instantiatedTemplates)
            {
                template.gameObject.SetActive(false);
            }
        }

        public void Release(T template)
        {
            if (_instantiatedTemplates.Contains(template))
            {
                template.gameObject.SetActive(false);
            }
        }

        public void SetAllToScaleOne()
        {
            foreach (var instantiatedTemplate in _instantiatedTemplates)
            {
                instantiatedTemplate.transform.localScale = Vector3.one;
            }
        }

        public IEnumerable<T> GetActiveTemplates()
        {
            foreach (var instantiatedTemplate in _instantiatedTemplates)
            {
                if (instantiatedTemplate.gameObject.activeSelf)
                {
                    yield return instantiatedTemplate;
                }
            }
        }
    }
}
