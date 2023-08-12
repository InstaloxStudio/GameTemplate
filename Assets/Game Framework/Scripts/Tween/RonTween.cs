using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ron.Tween
{
    public static class RonTween
    {
        private static Dictionary<Transform, TweenHandle> activeTweens = new Dictionary<Transform, TweenHandle>();

        public static TweenHandle TweenPosition(this MonoBehaviour owner, Transform target, Vector3 destination, float speed, bool isRelative = false)
        {
            StopTween(target); // Stop any existing tween on the target

            TweenHandle handle = new TweenHandle(owner, target);
            handle.SetActiveTween(owner.StartCoroutine(TweenPositionRoutine(owner, handle, target, destination, speed, isRelative)));

            activeTweens[target] = handle;
            return handle;
        }

        public static void StopTween(Transform target)
        {
            if (activeTweens.ContainsKey(target))
            {
                activeTweens[target].StopTween();
                activeTweens.Remove(target);
            }
        }

        private static IEnumerator TweenPositionRoutine(MonoBehaviour owner, TweenHandle handle, Transform target, Vector3 destination, float speed, bool isRelative)
        {
            if (isRelative)
            {
                destination += target.position;
            }

            float distance = Vector3.Distance(target.position, destination);
            float duration = distance / speed;

            Coroutine coroutine = owner.StartCoroutine(ActualTweenRoutine(handle, target, destination, duration));
            handle.SetActiveTween(coroutine);
            yield return coroutine;
        }

        private static IEnumerator ActualTweenRoutine(TweenHandle handle, Transform target, Vector3 destination, float duration)
        {
            float elapsed = 0f;
            Vector3 startPosition = target.position;

            while (elapsed < duration && !handle.IsStopped() && target != null)
            {
                if (!handle.IsPaused())
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / duration;
                    target.position = Vector3.Lerp(startPosition, destination, t);

                    handle.OnUpdateCallback?.Invoke(t);
                }
                yield return null;
            }

            if (!handle.IsStopped() && target != null)
            {
                target.position = destination;
                handle.OnCompleteCallback?.Invoke();

            }

        }

        public static TweenHandle TweenScale(this MonoBehaviour owner, Transform target, Vector3 targetScale, float speed)
        {
            StopTween(target); // Stop any existing tween on the target

            TweenHandle handle = new TweenHandle(owner, target);
            handle.SetActiveTween(owner.StartCoroutine(TweenScaleRoutine(owner, handle, target, targetScale, speed)));

            activeTweens[target] = handle;
            return handle;
        }

        private static IEnumerator TweenScaleRoutine(MonoBehaviour owner, TweenHandle handle, Transform target, Vector3 targetScale, float speed)
        {
            float distance = Vector3.Distance(target.localScale, targetScale);
            float duration = distance / speed;

            yield return owner.StartCoroutine(ActualTweenScaleRoutine(handle, target, targetScale, duration));
        }

        private static IEnumerator ActualTweenScaleRoutine(TweenHandle handle, Transform target, Vector3 targetScale, float duration)
        {
            Vector3 startScale = target.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration && target != null)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                target.localScale = Vector3.Lerp(startScale, targetScale, t);

                handle.OnUpdateCallback?.Invoke(t);

                yield return null;
            }
            if (target != null)
            {
                target.localScale = targetScale;

                handle.OnCompleteCallback?.Invoke();
            }

        }


        public class TweenHandle
        {
            private MonoBehaviour owner;
            private Transform target;
            private Coroutine activeTween;
            private bool isPaused;
            private bool isStopped;

            public System.Action<float> OnUpdateCallback;
            public System.Action OnCompleteCallback;
            public TweenHandle(MonoBehaviour owner, Transform target)
            {
                this.owner = owner;
                this.target = target;
            }

            public TweenHandle OnStart(System.Action callback)
            {
                callback?.Invoke();
                return this;
            }

            public TweenHandle OnUpdate(System.Action<float> callback)
            {
                OnUpdateCallback = callback;
                return this;
            }

            public TweenHandle OnComplete(System.Action callback)
            {
                OnCompleteCallback = callback;
                return this;
            }

            public void PauseTween()
            {
                isPaused = true;
            }

            public void ResumeTween()
            {
                isPaused = false;
            }

            public void StopTween()
            {
                isStopped = true;
                owner.StopCoroutine(activeTween);
                activeTween = null;
            }

            private IEnumerator WaitForTweenUpdate(System.Action<float> callback)
            {
                while (activeTween != null)
                {
                    callback?.Invoke(Time.deltaTime);
                    yield return null;
                }
            }

            private IEnumerator WaitForTweenCompletion(System.Action callback)
            {
                if (activeTween != null)
                {
                    yield return activeTween;
                }
                callback?.Invoke();
            }

            internal void SetActiveTween(Coroutine coroutine)
            {
                activeTween = coroutine;
            }

            internal bool IsPaused()
            {
                return isPaused;
            }

            internal bool IsStopped()
            {
                return isStopped;
            }
        }
    }


    public static class EasingFunctions
    {
        // Linear
        public static float Linear(float t)
        {
            return t;
        }

        // Quadratic
        public static float EaseInQuad(float t)
        {
            return t * t;
        }

        public static float EaseOutQuad(float t)
        {
            return t * (2 - t);
        }

        public static float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

        // Cubic
        public static float EaseInCubic(float t)
        {
            return t * t * t;
        }

        public static float EaseOutCubic(float t)
        {
            return (--t) * t * t + 1;
        }

        public static float EaseInOutCubic(float t)
        {
            return t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
        }

        // Quartic
        public static float EaseInQuart(float t)
        {
            return t * t * t * t;
        }

        public static float EaseOutQuart(float t)
        {
            return 1 - (--t) * t * t * t;
        }

        public static float EaseInOutQuart(float t)
        {
            return t < 0.5f ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
        }

        // Quintic
        public static float EaseInQuint(float t)
        {
            return t * t * t * t * t;
        }

        public static float EaseOutQuint(float t)
        {
            return 1 + (--t) * t * t * t * t;
        }

        public static float EaseInOutQuint(float t)
        {
            return t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
        }

        // Sine
        public static float EaseInSine(float t)
        {
            return (float)(1 - Math.Cos(t * Math.PI / 2));
        }

        public static float EaseOutSine(float t)
        {
            return (float)Math.Sin(t * Math.PI / 2);
        }

        public static float EaseInOutSine(float t)
        {
            return (float)(-1 / 2 * (Math.Cos(Math.PI * t) - 1));
        }

        // Circular
        public static float EaseInCirc(float t)
        {
            return (float)(1 - Math.Sqrt(1 - t * t));
        }

        public static float EaseOutCirc(float t)
        {
            return (float)Math.Sqrt(1 - (--t) * t);
        }

        public static float EaseInOutCirc(float t)
        {
            return t < 0.5f ? (float)((1 - Math.Sqrt(1 - 4 * t * t)) / 2) : (float)((Math.Sqrt(1 - (2 * t - 2) * (2 * t - 2)) + 1) / 2);
        }

        // Exponential
        public static float EaseInExpo(float t)
        {
            return (float)(t == 0 ? 0 : Math.Pow(2, 10 * (t - 1)));
        }

        public static float EaseOutExpo(float t)
        {
            return (float)(t == 1 ? 1 : 1 - Math.Pow(2, -10 * t));
        }

        public static float EaseInOutExpo(float t)
        {
            if (t == 0) return 0;
            if (t == 1) return 1;

            if (t < 0.5f)
            {
                return (float)(0.5 * Math.Pow(2, 20 * t - 10));
            }
            else
            {
                return (float)(0.5 * (2 - Math.Pow(2, -20 * t + 10)));
            }
        }

        // Elastic
        public static float EaseInElastic(float t)
        {
            return (float)(-1 * Math.Pow(2, 10 * (t - 1)) * Math.Sin((t - 1.075) * (2 * Math.PI) / 0.3));
        }

        public static float EaseOutElastic(float t)
        {
            return (float)(Math.Pow(2, -10 * t) * Math.Sin((t - 0.075) * (2 * Math.PI) / 0.3) + 1);
        }

        public static float EaseInOutElastic(float t)
        {
            if (t < 0.5f)
            {
                return (float)(-0.5 * Math.Pow(2, 20 * t - 10) * Math.Sin((40 * t - 22.5) * (2 * Math.PI) / 0.3));
            }
            else
            {
                return (float)(0.5 * Math.Pow(2, -20 * t + 10) * Math.Sin((40 * t - 22.5) * (2 * Math.PI) / 0.3) + 1);
            }
        }

        // Back
        public static float EaseInBack(float t)
        {
            const float s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        }

        public static float EaseOutBack(float t)
        {
            const float s = 1.70158f;
            return --t * t * ((s + 1) * t + s) + 1;
        }

        public static float EaseInOutBack(float t)
        {
            float s = 1.70158f;
            t *= 2;

            if (t < 1)
            {
                s *= 1.525f;
                return 0.5f * (t * t * ((s + 1) * t - s));
            }
            else
            {
                t -= 2;
                s *= 1.525f;
                return 0.5f * (t * t * ((s + 1) * t + s) + 2);
            }
        }


    }


    public class TweenStep
    {
        public MonoBehaviour Owner { get; set; }
        public System.Action Action { get; set; }
        public float Duration { get; set; }
    }

    public class TweenSequence
    {
        private List<TweenStep> steps = new List<TweenStep>();
        private MonoBehaviour owner;

        // Add this delegate
        public System.Action OnSequenceComplete { get; set; }

        public TweenSequence(MonoBehaviour owner)
        {
            this.owner = owner;
        }

        public TweenSequence Append(TweenStep step)
        {
            steps.Add(step);
            return this;
        }

        public Coroutine Play()
        {
            return owner.StartCoroutine(PlayRoutine());
        }

        private IEnumerator PlayRoutine()
        {
            foreach (TweenStep step in steps)
            {
                float elapsed = 0f;

                while (elapsed < step.Duration)
                {
                    elapsed += Time.deltaTime;
                    step.Action();
                    yield return null;
                }
            }

            // Invoke the OnSequenceComplete delegate here
            OnSequenceComplete?.Invoke();
        }
    }


}
