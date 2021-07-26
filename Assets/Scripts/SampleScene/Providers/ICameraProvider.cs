using UnityEngine;

namespace SampleScene.Providers
{
    public interface ICameraProvider
    {
        Camera MainCamera { get; }
        Vector2 GetScreenPosition(Vector3 worldPosition);
        Vector3 GetWorldPosition(Vector2 screenPosition);
    }
}