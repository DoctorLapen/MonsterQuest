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

        private bool _isMoveCorrutineOn = false;
        private bool _isFirstMatch = false;

        

         private void Start()
        {
            _fieldModel.CellChanged += OnCellChanged;
            _fieldModel.ElementsReplaced += OnElementsReplaced;
            _turnsView.UpdateText(_turnsCounter.Amount.ToString());
            _turnsCounter.AmountChanged += (amount) => _turnsView.UpdateText(amount.ToString());
            _fieldModel.InitializeField();
        }

        private void OnElementsReplaced(ElementsReplacedArgs args)
        {
            _fieldView.ReplaceVisualElements(args.elementA.x,args.elementA.y,args.elementB.x,args.elementB.y);
        }

        private void Update()
        {
            if (!_turnsCounter.IsTurnsOver)
            {
                if (!_isMoveCorrutineOn)
                {
                   
                    ActionData action = _inputController.DetectAction();
                    if (action.isActionHappened)
                    {

                        Vector2Int secondElementCoordinates = action.elementToMoveCoordinates + action.moveDirection;
                        if (_fieldModel.IsElementInField(secondElementCoordinates))
                        {
                            StartCoroutine(MakeMoveElements(action, secondElementCoordinates));
                        }

                    }
                }
            }
            else
            {
                Debug.Log("GameOver");
            }
        }

        private IEnumerator MakeMoveElements(ActionData action, Vector2Int secondElementCoordinates)
        {
            bool isMatchedElementsExist = true;
            while (isMatchedElementsExist)
            {
                HashSet<Vector2Int> matchedElements =
                    _fieldModel.FindMatchedElements(action.elementToMoveCoordinates, secondElementCoordinates);
                isMatchedElementsExist = matchedElements.Count > 0;
                if (isMatchedElementsExist)
                {
                    _isFirstMatch = true;
                    _isMoveCorrutineOn = true;
                    _fieldModel.ReplaceElements(action.elementToMoveCoordinates, secondElementCoordinates);
                    _fieldModel.DeleteElements(matchedElements);
                    yield return new WaitForSeconds(1.5f);
                    _fieldModel.FillEmptyCells();
                    _fieldModel.AddNewElements();
                    yield return new WaitForSeconds(1.5f);
                }
            }
            _isMoveCorrutineOn = false;
            if (_isFirstMatch)
            {
                _turnsCounter.СountTurn();
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
            else if (eventArgs.changeType == ChangeType.MoveDown)
            {
                _fieldView.MoveDownElement(eventArgs.column,eventArgs.row);
            }
            else if (eventArgs.changeType == ChangeType.CreateNew)
            {
                _fieldView.SpawnElement(eventArgs.column,eventArgs.row, eventArgs.element);
            }

        }
    }
}