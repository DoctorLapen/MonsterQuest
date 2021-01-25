using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class InputController :MonoBehaviour, IInputController
    {
        [SerializeField]
        private float _minDistanceForSwipe;

        [SerializeField]
        private int _actionsPerSwipe;

        [SerializeField]
        private string _tagToCompare;
        [SerializeField]
        private GraphicRaycaster m_Raycaster;
       
        [SerializeField]
        private EventSystem m_EventSystem;
        private PointerEventData m_PointerEventData;

        private int _actionCount;
        private Vector2 _pointA;
        private Vector2 _pointB;
        private bool _isFirstMove;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            
        }

        public ActionData DetectAction()
        {
            
            ActionData actionData = new ActionData();
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    // _pointA = touch.position;
                    // _pointB = touch.position;
                    // _isFirstMove = true;
                    // _actionCount = 0;
                    

                    Vector2 pos = touch.position;
                    //Set up the new Pointer Event
                    m_PointerEventData = new PointerEventData(m_EventSystem);
                    //Set the Pointer Event Position to that of the mouse position
                    m_PointerEventData.position = touch.position;

                    //Create a list of Raycast Results
                    List<RaycastResult> results = new List<RaycastResult>();

                    //Raycast using the Graphics Raycaster and mouse click position
                    m_Raycaster.Raycast(m_PointerEventData, results);
                    Debug.Log("touch");
                    if (results.Count > 0)
                    {

                        RaycastResult result = results[0];
                        Debug.Log("Hit " + result.gameObject.name);
                        if (result.gameObject.CompareTag(_tagToCompare))
                        {
                            CellCoordinate cellCoordinate = result.gameObject.GetComponent<CellCoordinate>();
                            Debug.Log(cellCoordinate.Coordinate);
                        }
                    }






                }
                /*else if (touch.phase == TouchPhase.Moved)
                {
                    _pointB = touch.position;
                    if(IsHorizontalSwipe())
                    {
                        
                        if (_isFirstMove || _actionCount > _actionsPerSwipe)
                        {
                            actionData.isActionHappened = true;

                            float swipeDirection = _pointB.x - _pointA.x;
                            if (swipeDirection > 0)
                            {
                                actionData.action = MoveAction.Right;
                            }
                            else
                            {
                                actionData.action = MoveAction.Left;
                            }

                            _isFirstMove = false;
                        }

                        _actionCount++;
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (IsToBottomSwipe())
                    {
                        actionData.isActionHappened = true;
                        actionData.action = MoveAction.ToBottomEnd;
                    }
                    else if (_pointB == _pointA)
                    {
                        actionData.isActionHappened = true;
                        actionData.action = MoveAction.Rotate;
                    }


                }*/
            }

            return actionData;
        }

        private bool IsHorizontalSwipe()
        {
            if (IsSwipe())
            {
                
                if (!IsVerticalSwipe())
                {

                    return true;
                }
                
            }

            return false;
        }

        private bool IsToBottomSwipe()
        {
            if (IsSwipe())
            {
                if (IsVerticalSwipe())
                {
                    float swipeDirection = _pointB.y - _pointA.y;
                    if (swipeDirection < 0)
                    {
                        return true;
                    }
                    
                }
            }

            return false;
        }

        private bool IsSwipe()
        {
            return HorizontalMovementDistance() > _minDistanceForSwipe || VerticalMovementDistance() > _minDistanceForSwipe;
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(_pointB.x - _pointA.x);
        }
        private float VerticalMovementDistance() 
        {
            return Mathf.Abs(_pointB.y - _pointA.y);
        }

       


    }

    public enum MoveAction
    {
        Right,
        Left,
        ToBottomEnd,
        Rotate
    }

    public class ActionData
    {
        public bool isActionHappened;
        public MoveAction action;
    }
}