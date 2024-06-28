using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Component
{
    /// <summary>
    /// 自定义按钮组件
    /// <para>单击、长按、可拖动</para>
    /// </summary>
    public class CommonButton : Selectable, IPointerClickHandler, ISubmitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public event Action Clicked;
        public event Action LongPress;
        public event Action LongPressCancel;
        public event Action BeginDrag;
        public event Action EndDrag;

        public bool IsLongPressing => isLongPressing;
        public bool IsDragging => isDragging;

        public bool EnableDrag { get; set; }

        public float LongPressThresholdTime
        {
            get => longPressThresholdTime;
            set => longPressThresholdTime = value > 0 ? value : 0;
        }

        [SerializeField]
        private float longPressThresholdTime = 0.3f;

        [SerializeField]
        private bool enableDrag; // 是否可拖动

        [SerializeField]
        private RectTransform bound; // 拖动区域限制

        private float pointerDownTime; // 按下的时间累加
        private bool isDown; // 是否按下
        private bool isLongPressing; // 是否正在长按
        private bool isLongPressTriggered; // 是否触发了长按
        private bool isDragging; // 是否正在拖动
        private bool isDraggingTriggered; // 是否触发了拖动

        private RectTransform rectTransform;
        private Vector3 offset;
        private Vector2 offset2D;
        private float minX, minY, maxX, maxY;
        private Vector3[] boundWorldCorners = new Vector3[4];
        private Vector3[] worldCorners = new Vector3[4];

        protected CommonButton()
        {
        }

        protected override void Start()
        {
            base.Start();
            rectTransform = GetComponent<RectTransform>();
        }

        protected void Update()
        {
            // 长按检测
            if (!isDragging && isDown && !isLongPressing)
            {
                pointerDownTime += Time.unscaledDeltaTime;
                if (pointerDownTime > longPressThresholdTime)
                {
                    isLongPressing = true;
                    isLongPressTriggered = true;
                    LongPress?.Invoke();
                }
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            isLongPressing = false;
            isLongPressTriggered = false;
            pointerDownTime = 0;
            isDragging = false;
            isDraggingTriggered = false;
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
            pointerDownTime = 0;
            if (isLongPressing)
            {
                isLongPressing = false;
                LongPressCancel?.Invoke();
            }

            base.OnPointerUp(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (!isLongPressTriggered && !isDraggingTriggered)
            {
                Press();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (!isLongPressTriggered && !isDraggingTriggered)
            {
                Press();
            }

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (enableDrag)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out Vector3 worldPoint);
                offset = rectTransform.position - worldPoint;
                if (bound != null)
                {
                    bound.GetWorldCorners(boundWorldCorners);
                    rectTransform.GetWorldCorners(worldCorners);
                    float halfXDistance = (worldCorners[2].x - worldCorners[0].x) / 2;
                    float halfYDistance = (worldCorners[2].y - worldCorners[0].y) / 2;
                    minX = boundWorldCorners[0].x + halfXDistance;
                    minY = boundWorldCorners[0].y + halfYDistance;
                    maxX = boundWorldCorners[2].x - halfXDistance;
                    maxY = boundWorldCorners[2].y - halfYDistance;
                }

                isDragging = true;
                isDraggingTriggered = true;
                BeginDrag?.Invoke();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (enableDrag)
            {
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out Vector3 worldPoint);
                if (bound == null)
                {
                    rectTransform.position = worldPoint + offset;
                }
                else
                {
                    var position = worldPoint + offset;
                    position.x = Mathf.Clamp(position.x, minX, maxX);
                    position.y = Mathf.Clamp(position.y, minY, maxY);
                    rectTransform.position = position;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (enableDrag)
            {
                isDragging = false;
                EndDrag?.Invoke();
            }
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("CommonButton.Clicked", this);
            Clicked?.Invoke();
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}