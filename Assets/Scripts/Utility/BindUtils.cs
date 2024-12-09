using System;
using UnityEngine.UI;

namespace BindUtils
{
    public class ActionEx
    {
        public static void Binding(ref Action _target, Action _event)
        {
            _target += _event;
        }

        public static void Binding<T>(ref Action<T> _target, Action<T> _event)
        {
            _target += _event;
        }

        public static void Binding(ref Action _target, Action[] _events)
        {
            foreach (Action e in _events)
            {
                _target += e;
            }
        }

        public static void Binding<T>(ref Action<T> _target, Action<T>[] _events)
        {
            foreach (Action<T> e in _events)
            {
                _target += e;
            }
        }
        
        public static void Unbinding(ref Action _target, Action _event)
        {
            _target -= _event;
        }

        public static void Unbinding<T>(ref Action<T> _target, Action<T> _event)
        {
            _target -= _event;
        }

        public static void Unbinding(ref Action _target)
        {
            _target = null;
        }

        public static void Unbinding<T>(ref Action<T> _target)
        {
            _target = null;
        }
    }

    namespace UI
    {
        /// <summary>버튼 이벤트</summary>
        public static class ButtonEx
        {
            public static void AddEvent(Button _button, Action _action)
            {
                _button?.onClick.AddListener(() => _action?.Invoke());
            }

            public static void RemoveEvent(Button _button)
            {
                _button?.onClick.RemoveAllListeners();
            }
        }

        /// <summary>토글 이벤트</summary>
        public static class ToggleEx
        {
            public static void AddEvent(Toggle _toggle, Action<bool> _action)
            {
                _toggle?.onValueChanged.AddListener(x => _action?.Invoke(x));
            }

            public static void RemoveEvent(Toggle _toggle)
            {
                _toggle?.onValueChanged.RemoveAllListeners();
            }
        }

        /// <summary>슬라이더 이벤트</summary>
        public static class SliderEx
        {
            public static void AddEvent(Slider _slider, Action<float> _action)
            {
                _slider?.onValueChanged.AddListener(value => _action?.Invoke(value));
            }

            public static void RemoveEvent(Slider _toggle)
            {
                _toggle?.onValueChanged.RemoveAllListeners();
            }
        }
    }

}
