using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class MainController : MonoBehaviour
    {
        [Inject]
        private IFieldModel _fieldModel;

        [Inject]
        private IFieldView _fieldView;

        [Inject]
        private IInputController _inputController;

        [Inject]
        private ITurnsCounter _turnsCounter;
        [Inject]
        private ITextView _turnsView;
        
        private bool _isFirstMatch = false;
        private bool _isColumnsMoving = false;
        private bool _isMatchedElementsExist = true;

        

         private void Start()
        {
            _fieldModel.CellChanged += OnCellChanged;
            _fieldModel.ElementsReplaced += OnElementsReplaced;
            _fieldModel.ElementsMovedDown += _fieldView.MoveDownElements;
            _turnsView.UpdateText(_turnsCounter.Amount.ToString());
            _turnsCounter.AmountChanged += (amount) => _turnsView.UpdateText(amount.ToString());
            _fieldView.ColumnsMoved += OnColumnsMoved;
            _fieldModel.InitializeField();
        }

         private void OnColumnsMoved()
         {
             _isColumnsMoving = false;
             HashSet<Vector2Int> matchedElements =
                 _fieldModel.FindMatchedElements();
             _isMatchedElementsExist = matchedElements.Count > 0;
             if (_isMatchedElementsExist)
             {
                 _fieldModel.DeleteElements(matchedElements);
                 _isColumnsMoving = true;
                 _fieldModel.ShiftElements();
             }
         }

         private void OnElementsReplaced(ElementsReplacedArgs args)
        {
            _fieldView.ReplaceVisualElements(args.elementA.x,args.elementA.y,args.elementB.x,args.elementB.y);
        }

        private void Update()
        {
            if (!_turnsCounter.IsTurnsOver)
            {
                
                if (!_isColumnsMoving)
                {
                    ActionData action = _inputController.DetectAction();
                    if (action.isActionHappened)
                    {

                        Vector2Int secondElementCoordinates = action.elementToMoveCoordinates + action.moveDirection;
                        if (_fieldModel.IsElementInField(secondElementCoordinates))
                        {
                            HashSet<Vector2Int> matchedElements =
                                _fieldModel.FindMatchedElements(action.elementToMoveCoordinates,
                                    secondElementCoordinates);
                            _isMatchedElementsExist = matchedElements.Count > 0;
                            if (_isMatchedElementsExist)
                            {
                                _isFirstMatch = true;
                                _fieldModel.ReplaceElements(action.elementToMoveCoordinates, secondElementCoordinates);
                                _fieldModel.DeleteElements(matchedElements);
                                _isColumnsMoving = true;
                                _fieldModel.ShiftElements();
                            }

                            if (_isFirstMatch)
                            {
                                _turnsCounter.СountTurn();
                                _isFirstMatch = false;
                            }
                        }



                    }
                }

            }
            else
            {
                Debug.Log("GameOver");
            }
        }

    


        private void OnCellChanged(CellChangedArgs eventArgs)
        {
            if (eventArgs.changeType == ChangeType.Initialize)
            {
                _fieldView.SpawnBackgroundCell(eventArgs.column,eventArgs.row);
                _fieldView.SpawnElement(eventArgs.column,eventArgs.row, eventArgs.element);
            }
            else  if(eventArgs.changeType == ChangeType.Delete)
            {
                _fieldView.DeleteElement(eventArgs.column,eventArgs.row);
            }
            else if (eventArgs.changeType == ChangeType.CreateNew)
            {
                _fieldView.SpawnElement(eventArgs.column,eventArgs.row, eventArgs.element);
            }

        }
    }
}