using UnityEngine;

namespace Libs.OpenUI
{
    public class FlipRoot : MonoBehaviour
    {
        public RectTransform RootFront;
        public RectTransform RootBack;
        [HideInInspector] public bool IsInFlipAnimationProcess;
        [HideInInspector] public bool IsFlipBackAnimationFlag;
    }
}