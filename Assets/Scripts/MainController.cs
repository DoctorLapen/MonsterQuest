using System;
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

        private void Start()
        {
            _fieldModel.CellChanged += OnCellChanged;
            _fieldModel.ElementsReplaced += OnElementsReplaced;
            _fieldModel.InitializeField();
        }

        private void OnElementsReplaced(ElementsReplacedArgs args)
        {
            Debug.Log(args.elementA);
            Debug.Log(args.elementB);
            _fieldView.ReplaceVisualElements(args.elementA.x,args.elementA.y,args.elementB.x,args.elementB.y);
        }

        private void Update()
        {
            ActionData action = _inputController.DetectAction();
            if (action.isActionHappened)
            {
                Debug.Log(action.elementToMoveCoordinates);
             //   Debug.Log(action.moveDirection);
                Vector2Int secondElementCoordinates = action.elementToMoveCoordinates + action.moveDirection;
                Debug.Log(secondElementCoordinates);
                if (_fieldModel.IsElementInField(secondElementCoordinates))
                {
                    _fieldModel.ReplaceElements(action.elementToMoveCoordinates,secondElementCoordinates);
                }
            }
        }

        private void OnCellChanged(CellChangedArgs eventArgs)
        {
            if (eventArgs.changeType == ChangeType.Initialize)
            {
                _fieldView.SpawnBackgroundCell(eventArgs.column,eventArgs.row);
            }
            _fieldView.SpawnElement(eventArgs.column,eventArgs.row, eventArgs.element);
            
        }
    }
}