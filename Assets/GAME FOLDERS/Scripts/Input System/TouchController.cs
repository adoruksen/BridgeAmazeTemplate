using UnityEngine;
using Managers;

namespace AmazeSystem
{
    public class TouchController : MonoBehaviour
    {
        public const float MAX_SWIPE_TIME = 0.5f;

        // Factor of the screen width that we consider a swipe
        // 0.17 works well for portrait mode 16:9 phone
        public const float MIN_SWIPE_DISTANCE = 0.17f;
        [HideInInspector]
        Vector2 startPos;
        float startTime;
        private bool ballMove;

        public bool BallMove
        {
            get => ballMove;
            set => ballMove = value;
        }

        private Vector2 direction;

        public Vector2 Direction
        {
            get => direction;
            set => direction = value;
        }

        private void Update()
        {
            if (!GameManager.Instance.isAmazeState) return;
            if (!ballMove)
            {
#if UNITY_EDITOR

                if (Input.GetKeyDown(KeyCode.UpArrow))
                    direction = Vector2.up;
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    direction = Vector2.down;
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    direction = Vector2.right;
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    direction = Vector2.left;
#endif
                if (Input.touches.Length > 0)
                {
                    Touch t = Input.GetTouch(0);
                    if (t.phase == TouchPhase.Began)
                    {
                        startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                        startTime = Time.time;
                    }
                    if (t.phase == TouchPhase.Ended)
                    {
                        if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
                            return;

                        Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                        Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                        if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                            return;

                        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                        { // Horizontal swipe
                            if (swipe.x > 0)
                            {
                                direction = Vector2.right;
                            }
                            else
                            {
                                direction = Vector2.left;
                            }
                        }
                        else
                        { // Vertical swipe
                            if (swipe.y > 0)
                            {
                                direction = Vector2.up;
                            }
                            else
                            {
                                direction = Vector2.down;
                            }
                        }
                    }
                }
            }
        }
    }



    #region useless
    //    [HideInInspector]
    //    public bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    //    [HideInInspector]
    //    private Vector2 _swipeDelta, _startTouch;
    //    private const float _deadZone = 100;
    //    public Vector2 SwipeDelta => _swipeDelta;

    //    private void Update()
    //    {
    //        if (!GameManager.Instance.isAmazeState) return;
    //        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            tap = true;
    //            _startTouch = Input.mousePosition;
    //        }
    //        else if (Input.GetMouseButtonUp(0))
    //        {
    //            _startTouch = _swipeDelta = Vector2.zero;
    //        }
    //        _swipeDelta = Vector2.zero;
    //        if (_startTouch != Vector2.zero)
    //        {
    //            if (Input.GetMouseButton(0))
    //            {
    //                _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
    //            }
    //        }
    //        SwipeDirection();

    //    }

    //    private void SwipeDirection()
    //    {
    //        if (SwipeDelta.magnitude > _deadZone)
    //        {
    //            var x = _swipeDelta.x;
    //            var y = _swipeDelta.y;

    //            if (Mathf.Abs(x) > Mathf.Abs(y))
    //            {
    //                if (x < 0) swipeLeft = true;
    //                else swipeRight = true;
    //            }
    //            else
    //            {
    //                if (y < 0) swipeDown = true;
    //                else swipeUp = true;
    //            }
    //            _startTouch = _swipeDelta = Vector2.zero;
    //        }
    //    }
    //}
    #endregion
}