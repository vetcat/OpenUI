using System;
using System.Collections;
using UnityEngine;

namespace Libs.OpenUI
{
    /// <summary>
    /// Коротины для зеинджекта - здесь место исполнения - сами коротины и вызов из классов
    /// </summary>
    public class AsyncProcessor : MonoBehaviour
    {
        public void Delay(float time, Action success)
        {
            //Debug.LogError("Delay = " + time);
            StartCoroutine(DelayCoroutine(time, success));
        }

        private IEnumerator DelayCoroutine(float time, Action success)
        {
            yield return new WaitForSeconds(time);
            success?.Invoke();
        }

        public void WaitForEndOfFrame(Action success)
        {
            StartCoroutine(WaitForEndOfFrameCoroutine(success));
        }

        private IEnumerator WaitForEndOfFrameCoroutine(Action success)
        {
            yield return new WaitForEndOfFrame();
            success?.Invoke();
        }

        public void DestroyGameObject(GameObject go)
        {
            Destroy(go);
        }

        public void DestroyImmediateGameObject(GameObject go)
        {
            DestroyImmediate(go);
        }

        public void DestroyGameObject(GameObject go, float time)
        {
            Destroy(go, time);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        void OnApplicationQuit()
        {
            Debug.Log("[AsyncProcessor] OnApplicationQuit");
        }
    }
}
