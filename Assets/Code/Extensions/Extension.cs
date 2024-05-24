using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using Color = UnityEngine.Color;
using Object = UnityEngine.Object;

namespace Extensions
{
    public static class Extension
    {
        /// <summary>
        /// Moves particles to a given destination and plays it
        /// </summary>
        /// <param name="particles"></param>
        /// <param name="position"></param>
        public static void Play(this ParticleSystem particles, Vector3 position)
        {
            particles.transform.position = position;
            particles.Play();
        }

        /// <summary>
        /// Scale diactivated transform to it's localScale with activation
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="duration"></param>
        /// <param name="update"></param>
        public static Tween ScaleIn(this Transform transform, Ease? ease = null, float duration = 0.5f, bool update = false)
        {
            Vector3 savedScale = transform.localScale != Vector3.zero ? transform.localScale : Vector3.one;

            transform.localScale = Vector3.zero;

            transform.gameObject.SetActive(true);
            if (ease == null)
            {
                return transform.DOScale(savedScale, duration).SetUpdate(update);
            }
            else
            {
                return transform.DOScale(savedScale, duration).SetEase(ease.Value).SetUpdate(update);
            }
        }

        public static Tween ScaleIn(this Transform transform, float scale, Ease? ease = null, float duration = 0.5f, bool update = false)
        {
            transform.localScale = Vector3.zero;

            transform.gameObject.SetActive(true);
            if (ease == null)
            {
                return transform.DOScale(scale, duration).SetUpdate(update);
            }
            else
            {
                return transform.DOScale(scale, duration).SetEase(ease.Value).SetUpdate(update);
            }
        }

        public static Tween ScaleIn(this Transform transform, Vector3 scale, Ease? ease = null, float duration = 0.5f, bool update = false)
        {
            transform.localScale = Vector3.zero;

            transform.gameObject.SetActive(true);
            if (ease == null)
            {
                return transform.DOScale(scale, duration).SetUpdate(update);
            }
            else
            {
                return transform.DOScale(scale, duration).SetEase(ease.Value).SetUpdate(update);
            }
        }

        public static void ScaleIn(this Transform transform, Tweener tween, float duration = 0.5f)
        {
            Vector3 savedScale = transform.localScale;
            transform.localScale = Vector3.zero;

            transform.gameObject.SetActive(true);
            //transform.DOScale(savedScale, duration);
            tween.Play();
        }


        public static Tween FadeIn(this Image image, Ease? ease = null, float duration = 0.5f, bool update = false)
        {
            float alpha = image.color.a != 0f ? image.color.a : 1f;

            image.color.SetAlpha(0f);
            image.gameObject.SetActive(true);

            if (ease == null)
            {
                return image.DOAlpha(alpha, duration).SetUpdate(update);
            }
            else
            {
                return image.DOAlpha(alpha, duration).SetEase(ease.Value).SetUpdate(update);
            }
        }


        public static Tween ScaleOutWithDiactivation(this Transform transform, float duration = 0.5f, Ease ease = Ease.Unset, bool update = false)
        {
            return transform.DOScale(0, duration).SetEase(ease).SetUpdate(update).OnComplete(() => transform.gameObject.SetActive(false));
        }

        /// <summary>
        /// Changes image alpha
        /// </summary>
        /// <param name="image"></param>
        /// <param name="alpha"></param>
        /// <param name="duration"></param>
        public static Tween DOAlpha(this Image image, float alpha, float duration = 0.5f)
        {
            return image.DOColor(GetColorWithAlpha(image.color, alpha), duration);
        }

        public static Tween DOPulse(this Transform transform, float minScale = 1, float maxScale = 1.2f, float scaleInDuration = 0.25f, float scaleOutDuration = 0.25f, int loops = -1, bool update = false)
        {
            var sequence = DOTween.Sequence();

            return sequence
                    .Append(transform.DOScale(maxScale, scaleInDuration))
                    .Append(transform.DOScale(minScale, scaleOutDuration))
                    .SetLoops(loops, LoopType.Restart)
                    .SetUpdate(update);
        }

        public static Tween DOPulse(this Transform transform, Vector3 minScale, Vector3 maxScale, float scaleInDuration = 0.25f, float scaleOutDuration = 0.25f, int loops = -1, bool update = false)
        {
            var sequence = DOTween.Sequence();

            return sequence
                    .Append(transform.DOScale(maxScale, scaleInDuration))
                    .Append(transform.DOScale(minScale, scaleOutDuration))
                    .SetLoops(loops, LoopType.Restart)
                    .SetUpdate(update);
        }

        public static Tween DOParabola(this Transform transform, Vector3 destination, float height, float duration, Ease ease = Ease.Linear, bool update = false)
        {
            return DOTween.To(() => 0f, x => transform.position = Extension.Parabola(transform.position, destination, height, x), 1f, duration).SetEase(ease).SetUpdate(update);
        }

        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Color GetColorWithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public static Texture2D ChangeFormat(this Texture2D oldTexture, TextureFormat newFormat)
        {
            //Create new empty Texture
            Texture2D newTex = new Texture2D(2, 2, newFormat, false);
            //Copy old texture pixels into new one
            newTex.SetPixels(oldTexture.GetPixels());
            //Apply
            newTex.Apply();

            return newTex;
        }

        public static IEnumerable<T> FindInterfacesOfType<T>(bool includeInactive = false)
        {
            return SceneManager.GetActiveScene().GetRootGameObjects()
                    .SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
        }

        public static T GetRandom<T>(this IEnumerable<T> source, Func<T, bool> selector)
        {
            var result = source.Where(item => selector(item)).ToList();
            return result.Count > 0 ? result[UnityEngine.Random.Range(0, result.Count())] : default;
        }

        public static T GetRandom<T>(this IEnumerable<T> items)
        {
            return items.ElementAt(UnityEngine.Random.Range(0, items.Count()));
        }

        public static T GetRandom<T>(this IEnumerable<T> items, System.Random random)
        {
            return items.ElementAt(random.Next(0, items.Count()));
        }

        public static T GetRandom<T>(this IEnumerable<T> items, Vector2Int range)
        {
            return items.ElementAt(UnityEngine.Random.Range(range.x, range.y));
        }

        public static Vector3 GetRandom(Vector3 first, Vector3 second)
        {
            var minMaxX = GetMinMax(first.x, second.x);
            var minMaxY = GetMinMax(first.y, second.y);
            var minMaxZ = GetMinMax(first.z, second.z);

            return new Vector3(UnityEngine.Random.Range(minMaxX.Key, minMaxX.Value), UnityEngine.Random.Range(minMaxY.Key, minMaxY.Value), UnityEngine.Random.Range(minMaxZ.Key, minMaxZ.Value));
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, Func<T, bool> selector, int amount)
        {
            var result = source.Where(item => selector(item)).ToList();
            amount = Mathf.Clamp(amount, 0, result.Count());

            List<int> takenIndexes = new List<int>();
            int randomIndex = UnityEngine.Random.Range(0, amount);
            while (takenIndexes.Count < amount)
            {
                while (takenIndexes.Contains(randomIndex))
                {
                    randomIndex = UnityEngine.Random.Range(0, amount);
                }
                yield return result.ElementAt(randomIndex);
            }
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int amount)
        {
            amount = Mathf.Clamp(amount, 0, source.Count());

            List<int> takenIndexes = new List<int>();
            int randomIndex = UnityEngine.Random.Range(0, amount);
            while (takenIndexes.Count < amount)
            {
                while (takenIndexes.Contains(randomIndex))
                {
                    randomIndex = UnityEngine.Random.Range(0, amount);
                }
                takenIndexes.Add(randomIndex);
                yield return source.ElementAt(randomIndex);
            }
        }

        public static Vector3 GetVectorFromAngle(float angle)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;

            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + 90;
            return angle < 0 ? angle + 360 : angle;
        }

        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
        {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector2.Lerp(start, end, t);

            return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
        }

        public static void SetLocalPositionAndRotation(this Transform transform, Vector3 position, Quaternion rotation)
        {
            transform.localPosition = position;
            transform.localRotation = rotation;
        }

        public static void Clear(this LineRenderer lineRenderer)
        {
            lineRenderer.positionCount = 0;
        }

        public static bool IsInLayerMask(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        public static void SetLayer(this GameObject gameObject, int layer, bool includeChildren)
        {
            if (!includeChildren)
            {
                gameObject.layer = layer;
                return;
            }

            gameObject.GetComponentsInChildren<Transform>().ToList().ForEach(child => child.gameObject.layer = layer);
        }

        public static void SetColor(this ColorOverLifetimeModule colorOverLifetimeModule, Color color)
        {
            var gradient = new Gradient();
            var colorKeys = colorOverLifetimeModule.color.gradient.colorKeys;
            var alphaKeys = colorOverLifetimeModule.color.gradient.alphaKeys;
            List<GradientColorKey> keys = new List<GradientColorKey>();

            for (int i = 0; i < colorKeys.Length; i++)
            {
                keys.Add(new GradientColorKey(color, colorKeys[i].time));
            }

            gradient.SetKeys(keys.ToArray(), alphaKeys);
            colorOverLifetimeModule.color = gradient;
        }

        public static Vector3 GetRandomVector3(float min, float max)
        {
            return new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
        }

        public static T GetClosest<T>(this IEnumerable<T> values, Vector3 center) where T : Component
        {
            return values.OrderBy(v => Vector3.Distance(v.transform.position, center)).First();
        }

        public static bool IsInternet()
        {
            return !(Application.internetReachability == NetworkReachability.NotReachable);
        }

        public static Texture2D GetTextureCopy(Texture2D reference)
        {
            Texture2D maskCopy = new Texture2D(reference.width, reference.height, TextureFormat.RGBA32, false);
            maskCopy.SetPixels32(reference.GetPixels32());
            maskCopy.Apply();
            return maskCopy;
        }

        public static void Show<T>(this T component) where T : Component
        {
            component.gameObject.SetActive(true);
        }

        public static void Hide<T>(this T component) where T : Component
        {
            component.gameObject.SetActive(false);
        }

        public static IEnumerable<TResult> SelectWhere<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return selector(item);
                }
            }
        }

        public static T InstantiateIdentical<T>(this Object from, T original) where T : Component
        {
            var copy = Object.Instantiate(original, original.transform.position, original.transform.rotation);
            copy.transform.localScale = original.transform.lossyScale;
            return copy;
        }

        public static GameObject InstantiateIdentical(this Object from, GameObject original)
        {
            var copy = Object.Instantiate(original, original.transform.position, original.transform.rotation);
            copy.transform.localScale = original.transform.lossyScale;
            return copy;
        }

        public static T MakeCopy<T>(this Object from, T original, bool setParent = false) where T : Component
        {
            var copy = InstantiateIdentical<T>(from, original);
            if (setParent)
            {
                copy.transform.parent = original.transform;
            }
            return copy;
        }

        public static void DoIfTrue(this Object from, bool pred, Action action)
        {
            if (pred) action?.Invoke();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return list.Count() == 0;
        }

        public static T GetAndRemoveFirstOrDefault<T>(this List<T> list)
        {
            T result = list.FirstOrDefault();
            list.Remove(result);
            return result;
        }

        public static void Enable(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        public static void Disable(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        public static T GetMax<T>(IEnumerable<T> values) where T : IComparable<T>
        {
            int lenght = values.Count();
            if (lenght > 0)
            {
                T max = values.First();

                for (int i = 1; i < lenght; i++)
                {
                    if (values.ElementAt(i).CompareTo(max) > 0)
                    {
                        max = values.ElementAt(i);
                    }
                }
                return max;
            }
            else
            {
                Debug.LogWarning("list is empty. Returning default!");
                return default;
            }
        }

        public static T GetMax<T>(params T[] values) where T : IComparable<T>
        {
            T max = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i].CompareTo(max) > 0)
                {
                    max = values[i];
                }
            }
            return max;
        }

        public static T GetMin<T>(params T[] values) where T : IComparable<T>
        {
            T min = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i].CompareTo(min) < 0)
                {
                    min = values[i];
                }
            }
            return min;
        }

        public static void GetMinMax<T>(T first, T second, out T min, out T max) where T : IComparable<T>
        {
            if (first.CompareTo(second) > 0)
            {
                max = first;
                min = second;
            }
            else
            {
                max = second;
                min = first;
            }
        }

        public static KeyValuePair<T, T> GetMinMax<T>(T first, T second) where T : IComparable<T>
        {
            KeyValuePair<T, T> minMax;
            if (first.CompareTo(second) > 0)
            {
                minMax = new KeyValuePair<T, T>(second, first);
            }
            else
            {
                minMax = new KeyValuePair<T, T>(first, second);
            }

            return minMax;
        }

        public static bool ContainsListener(this Button button, Action listener)
        {
            int listenerCount = button.onClick.GetPersistentEventCount();

            for (int i = 0; i < listenerCount; i++)
            {
                var eventBase = button.onClick.GetPersistentTarget(i);
                if (eventBase.Equals(listener))
                {
                    return true;
                }
            }
            return false;
        }

        public static void PlayWithDestroy(this ParticleSystem particleSystem, bool unparent = false)
        {
            PlayWithDestroy(particleSystem, particleSystem.main.duration, unparent);
        }

        public static void PlayWithDestroy(this ParticleSystem particleSystem, float duration, bool unparent = false)
        {
            if (unparent) particleSystem.transform.parent = null;
            particleSystem.Play();
            Object.Destroy(particleSystem.gameObject, duration);
        }

        public static T GetWithChance<T>(IEnumerable<KeyValuePair<T, float>> pairs, Vector2 randomRange)
        {
            pairs = pairs.OrderByDescending(pair => pair.Value);
            float randomChance = UnityEngine.Random.Range(randomRange.x, randomRange.y);
            float currentChance = 0f;
            foreach (var pair in pairs)
            {
                currentChance += pair.Value;
                //if (currentChance == randomRange.y) return pair.Key;
                if (randomChance <= currentChance) return pair.Key;
            }
            Debug.LogError("Error with calculating");
            return pairs.Last().Key;
        }

        public static void Print(this object obj, object message)
        {
            Debug.Log(message);
        }

        public static void PrintWarning(this object obj, object message)
        {
            Debug.LogWarning(message);
        }

        public static void PrintError(this object obj, object message)
        {
            Debug.LogError(message);
        }

        public static void SetText<T>(this TMP_Text text, T value)
        {
            text.text = value.ToString();
        }

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);
            return value;
        }

        public static float GetMaxFromVector(Vector3 vector3)
        {
            return Mathf.Max(vector3.x, Mathf.Max(vector3.y, vector3.z));
        }

        public static bool IsDefault<T>(this T value) where T : struct
        {
            bool isDefault = value.Equals(default(T));

            return isDefault;
        }

        public static float GetTByLerpedValue(float value, float min, float max)
        {
            return 1 / ((max - min) / (-min + value));
        }

        public static bool IsNull<T>(T obj) where T : class => obj == null || obj.Equals(null);

        public static bool IsUnityNull(this object obj)
        {
            // Checks whether an object is null or Unity pseudo-null
            // without having to cast to UnityEngine.Object manually

            return obj == null || ((obj is UnityEngine.Object) && ((UnityEngine.Object)obj) == null);
        }


        public static Vector3 GetCameraSpaceUIToWorldSpacePosition(Camera camera, RectTransform rectTransform, Canvas canvas, Vector3 position)
        {
            var rectWorldPos = rectTransform.TransformPoint(position);
            var viewport = camera.WorldToViewportPoint(rectWorldPos);
            var ray = canvas.worldCamera.ViewportPointToRay(viewport);
            var pos = ray.GetPoint(canvas.planeDistance);
            return pos;
        }

        public static Vector3 GetCameraSpaceUIPosition(Camera camera, Vector3 position, RectTransform rectTransform)
        {

            // convert screen coords
            Vector2 adjustedPosition = camera.WorldToScreenPoint(position);

            adjustedPosition.x *= rectTransform.rect.width / (float)camera.pixelWidth;
            adjustedPosition.y *= rectTransform.rect.height / (float)camera.pixelHeight;

            // set it
            return adjustedPosition - rectTransform.sizeDelta / 2f;
        }
    }
}