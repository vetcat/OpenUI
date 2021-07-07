﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Libs.OpenUI.Utils
{
    public static class ExtensionsUi
    {
        
        public static Canvas GetCanvas(string tag)
        {
            var canvasGo = GameObject.FindWithTag(tag);
            if (canvasGo == null)
            {
                Debug.LogError($"Canvas {tag} not found");
                return null;
            }

            return canvasGo.GetComponent<Canvas>();
        }
        
        public static void SetAlpha(this Image image, float alpha)
        {
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }

        public static void SetAlpha(this Text text, float alpha)
        {
            var color = text.color;
            color.a = alpha;
            text.color = color;
        }

        public static void SetColor(this SpriteRenderer spriteRenderer, Color color)
        {
            spriteRenderer.color = color;
        }

        public static void SetPositionAndSizeToRect(this RectTransform transform, Rect position)
        {
            transform.anchoredPosition = new Vector2(position.x, position.y);
            transform.sizeDelta = new Vector2(position.width, position.height);
        }

        // Shared array used to receive result of RectTransform.GetWorldCorners
        static Vector3[] corners = new Vector3[4];

        /// <summary>
        /// Transform the bounds of the current rect transform to the space of another transform.
        /// </summary>
        /// <param name="source">The rect to transform</param>
        /// <param name="target">The target space to transform to</param>
        /// <returns>The transformed bounds</returns>
        public static Bounds TransformBoundsTo(this RectTransform source, Transform target)
        {
            // Based on code in ScrollRect's internal GetBounds and InternalGetBounds methods
            var bounds = new Bounds();
            if (source != null)
            {
                source.GetWorldCorners(corners);

                var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                var matrix = target.worldToLocalMatrix;
                for (int j = 0; j < 4; j++)
                {
                    Vector3 v = matrix.MultiplyPoint3x4(corners[j]);
                    vMin = Vector3.Min(v, vMin);
                    vMax = Vector3.Max(v, vMax);
                }

                bounds = new Bounds(vMin, Vector3.zero);
                bounds.Encapsulate(vMax);
            }

            return bounds;
        }

        /// <summary>
        /// Normalize a distance to be used in verticalNormalizedPosition or horizontalNormalizedPosition.
        /// </summary>
        /// <param name="axis">Scroll axis, 0 = horizontal, 1 = vertical</param>
        /// <param name="distance">The distance in the scroll rect's view's coordiante space</param>
        /// <returns>The normalized scoll distance</returns>
        public static float NormalizeScrollDistance(this ScrollRect scrollRect, int axis, float distance)
        {
            // Based on code in ScrollRect's internal SetNormalizedPosition method
            var viewport = scrollRect.viewport;
            var viewRect = viewport != null ? viewport : scrollRect.GetComponent<RectTransform>();
            var viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);

            var content = scrollRect.content;
            var contentBounds = content != null ? content.TransformBoundsTo(viewRect) : new Bounds();

            var hiddenLength = contentBounds.size[axis] - viewBounds.size[axis];
            return distance / hiddenLength;
        }

        /// <summary>
        /// Scroll the target element to the vertical center of the scroll rect's viewport.
        /// Assumes the target element is part of the scroll rect's contents.
        /// </summary>
        /// <param name="scrollRect">Scroll rect to scroll</param>
        /// <param name="target">Element of the scroll rect's content to center vertically</param>
        /// <param name="duration"></param>
        public static void ScrollToCenter(this ScrollRect scrollRect, RectTransform target, float duration = 0.5f)
        {
            // The scroll rect's view's space is used to calculate scroll position
            var view = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

            // Calcualte the scroll offset in the view's space
            var viewRect = view.rect;
            var elementBounds = target.TransformBoundsTo(view);
            var offset = viewRect.center.y - elementBounds.center.y;

            // Normalize and apply the calculated offset
            var scrollPos = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(1, offset);
            scrollPos = Mathf.Clamp(scrollPos, 0f, 1f);
            if (duration > 0f)
            {
                DOTween.To(() => scrollRect.verticalNormalizedPosition, x => scrollRect.verticalNormalizedPosition = x,
                    scrollPos, duration);
            }
            else
            {
                scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollPos, 0f, 1f);
            }
        }
    }
}