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
        private IScoreCounter _scoreCounter;
        [Inject(Id = "TurnsView")]
        private ITextView _turnsView;
        [Inject(Id = "ScoreView")]
        private ITextView _scoreView;
        [Inject]
        private IScoreSaver _scoreSaver;
        [Inject]
        private IGameOverMenu _gameOverMenu;
        [Inject]
        private ILeaderboardController _leaderboardController;
        
        private bool _isFirstMatch = false;
        private bool _isColumnsMoving = false;
        private bool _isMatchedElementsExist = true;
        private bool _isGameOver = false;

        

         private void Start()
        {
            _fieldModel.CellChanged += OnCellChanged;
            _fieldModel.ElementsReplaced += OnElementsReplaced;
            _fieldModel.ElementsMovedDown += _fieldView.MoveDownElements;
            _turnsView.UpdateText(_turnsCounter.Amount.ToString());
            _turnsCounter.AmountChanged += (amount) => _turnsView.UpdateText(amount.ToString());
            _scoreCounter.ScoreChanged += (amount) => _scoreView.UpdateText(amount.ToString());
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
                 _scoreCounter.AddScore(matchedElements);
                 _isColumnsMoving = true;
                 _fieldModel.ShiftElements();
             }
             else if(_turnsCounter.IsTurnsOver)
             {
                 StartGameOver();
             }
         }

         private void OnElementsReplaced(ElementsReplacedArgs args)
        {
            _fieldView.ReplaceVisualElements(args.elementA.x,args.elementA.y,args.elementB.x,args.elementB.y);
        }

         private void Update()
         {
             if (!_isGameOver)
             {
                 if (!_isColumnsMoving)
                 {
                     var action = _inputController.DetectAction();
                     if (action.isActionHappened)
                     {
                         var secondElementCoordinates = action.elementToMoveCoordinates + action.moveDirection;
                         if (_fieldModel.IsElementInField(secondElementCoordinates))
                         {
                             var matchedElements = _fieldModel.FindMatchedElements(action.elementToMoveCoordinates,
                                 secondElementCoordinates);
                             _isMatchedElementsExist = matchedElements.Count > 0;
                             if (_isMatchedElementsExist)
                             {
                                 _isFirstMatch = true;
                                 _fieldModel.ReplaceElements(action.elementToMoveCoordinates, secondElementCoordinates);
                                 _fieldModel.DeleteElements(matchedElements);
                                 _scoreCounter.AddScore(matchedElements);
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
         }
        private void StartGameOver()
        {
            _isGameOver = true;
            _gameOverMenu.ShowMenu();
             ScoreAmount recordData = _scoreSaver.Load();
             ScoreAmount scoreData = _scoreCounter.SaveData;
             if (CurrentAuth.auth != AuthType.Anonymous)
             {
                 _leaderboardController.SendScore(scoreData.score);
             }

             if(scoreData.score > recordData.score)
            {
                _scoreSaver.Save(scoreData);
                _gameOverMenu.ChangeScore(scoreData.score,scoreData.score,true);

            }
            else
            {
                _gameOverMenu.ChangeScore( scoreData.score,recordData.score,false);
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