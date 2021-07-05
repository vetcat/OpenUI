using EasyButtons;
using UnityEngine;
using UnityEngine.UI;

namespace Libs.OpenUI.Utils
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleState : MonoBehaviour
    {
        public Toggle Toggle;
        public GameObject StateOn;
        public GameObject StateOff;

        private void Awake()
        {
            Toggle.onValueChanged.AddListener(SetState);
            SetState(false);
        }
        
        public void SetState(bool value)
        {
            StateOn.SetActive(Toggle.isOn);
            StateOff.SetActive(!Toggle.isOn);
        }

        [Button]
        public void TestChangeState()
        {
            Toggle.isOn = !Toggle.isOn;
        }

        private void OnDestroy()
        {
            Toggle.onValueChanged.RemoveAllListeners();
        }
    }
}
