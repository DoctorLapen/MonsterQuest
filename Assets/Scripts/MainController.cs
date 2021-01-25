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
            _fieldModel.InitializeField();
        }

        private void Update()
        {
            _inputController.DetectAction();
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