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
        private string _tagToCompare;
        [SerializeField]
        private GraphicRaycaster _raycaster;
       
        [SerializeField]
        private EventSystem _eventSystem;
        
        private PointerEventData _pointerEventData;
        private Vector2 _pointA;
        private Vector2 _pointB;
        private bool _isFirstMove;
        private Vector2Int _selectedElementCoordinates;
       

       

        public ActionData DetectAction()
        {
            
            ActionData actionData = new ActionData();
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                     _pointA = touch.position;
                     _pointB = touch.position;
                     _isFirstMove = true;

                    _pointerEventData = new PointerEventData(_eventSystem);
                    
                    _pointerEventData.position = touch.position;
                    
                    List<RaycastResult> results = new List<RaycastResult>();
                    
                    _raycaster.Raycast(_pointerEventData, results);
                    if (results.Count > 0)
                    {

                        RaycastResult result = results[0];
                        if (result.gameObject.CompareTag(_tagToCompare))
                        {
                            CellCoordinate cellCoordinate = result.gameObject.GetComponent<CellCoordinate>();
                            _selectedElementCoordinates = cellCoordinate.Coordinate;
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _pointB = touch.position;
                    if (_isFirstMove)
                    {
                        if (IsSwipe())
                        {
                            actionData.isActionHappened = true;
                            actionData.elementToMoveCoordinates = _selectedElementCoordinates;
                            if (!IsVerticalSwipe())
                            {
                                

                                float swipeDirection = _pointB.x - _pointA.x;
                                //Right
                                if (swipeDirection > 0)
                                {
                                    actionData.moveDirection = new Vector2Int(1, 0);
                                }
                                //Left
                                else
                                {
                                    actionData.moveDirection = new Vector2Int(-1, 0);
                                }

                            }
                            else
                            {
                                float swipeDirection = _pointB.x - _pointA.x;
                                //Bottom
                                if (swipeDirection > 0)
                                {
                                    actionData.moveDirection = new Vector2Int(0, -1);
                                }
                                //Top
                                else
                                {
                                    actionData.moveDirection = new Vector2Int(0, 1);
                                }
                            }
                            _isFirstMove = false;
                        }
                    }
                }
                
            }

            return actionData;
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

    

    public class ActionData
    {
        public bool isActionHappened;
        public Vector2Int moveDirection;
        public Vector2Int elementToMoveCoordinates;
    }
}