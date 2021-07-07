using UnityEngine;

namespace Game.Providers
{
    public interface ICanvasProvider
    {
        Canvas Canvas { get; }
        float GetAspectFactor();
    }
}