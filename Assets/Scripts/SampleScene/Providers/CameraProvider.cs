using UnityEngine;

namespace SampleScene.Providers
{
    public class CameraProvider : ICameraProvider
    {
        public Camera MainCamera { get; }

        public CameraProvider()
        {
            MainCamera = Camera.main;
        }

        public Vector2 GetScreenPosition(Vector3 worldPosition)
        {
            return MainCamera.WorldToScreenPoint(worldPosition);
        }
        
        public Vector3 GetWorldPosition(Vector2 screenPosition)
        {
            return MainCamera.ScreenToWorldPoint(screenPosition);
        }
    }
}