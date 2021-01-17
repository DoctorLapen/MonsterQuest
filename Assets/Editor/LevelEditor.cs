using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MonsterQuest.Editor
{
    public class LevelEditor : EditorWindow
    {
        private const int OFFSET_STEP = 35;
        private IntegerField _heightOfField;
        private IntegerField _widthOfField;
        private VisualElement _levelGrid;
        private CellData[,] _levelCells ;

        [MenuItem("Window/MonsterQuest/LevelEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelEditor>();
            window.titleContent = new GUIContent("MonsterQuestLevelEditor");
            window.Show();
        }

        private void OnEnable()
        {
            VisualElement root = rootVisualElement;
                
            VisualTreeAsset levelEditorUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/LevelEditorWindow.uxml");
            
            
            levelEditorUXML.CloneTree(root);
            Button createLevelButton = root.Query<Button>("CreateButton").First();
            createLevelButton.clickable.clicked += CreateLevelField;
            _widthOfField = root.Query<IntegerField>("WidthOfField").First();
            _heightOfField = root.Query<IntegerField>("HeightOfField").First();
            _levelGrid = root.Query<VisualElement>("LevelGrid").First();
            Button saveButton = root.Query<Button>("SaveButton").First();
            saveButton.clickable.clicked += SaveLevel;
           
        }
        

        private void CreateLevelField()
        {

            int heightOffset = 0;
            int widthOffset = 0;
            _levelCells = new CellData[_widthOfField.value, _heightOfField.value];
            for (int row = 0; row < _heightOfField.value; row++)
            {
                for (int column = 0; column < _widthOfField.value; column++)
                {
                    Button cell = new Button();
                    
                    cell.style.height = 25;
                    cell.style.width = 25;
                    cell.style.position = Position.Absolute;
                    cell.style.left = widthOffset;
                    widthOffset += OFFSET_STEP;
                    cell.style.top = heightOffset;
                    
                    
                    CellData cellData = new CellData();
                    cellData.isSelected = true;
                    cell.userData = cellData;
                    _levelCells[column,row]= cellData ;
                    cell.style.backgroundColor = new StyleColor(Color.white);
                    cell.clickable.clickedWithEventInfo += ChangeLevelForm;
                    
                  _levelGrid .Add(cell);
                }

                widthOffset = 0;
                heightOffset += OFFSET_STEP;

            }

            _levelGrid.style.height = heightOffset;
        }

        private void ChangeLevelForm(EventBase eventData)
        {
            var element = (VisualElement) eventData.target;
            var cellData = (CellData) element.userData;
            if (cellData.isSelected)
            {
                
                cellData.isSelected = false;
                element.style.backgroundColor = new StyleColor(new Color(1f, 1f, 1f, 0.38f));
            }
            else
            {
                
                cellData.isSelected = true;
                element.style.backgroundColor = new StyleColor(Color.white);
            }

        }
        private void SaveLevel()
        {
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            FillSerializableField(levelData);
            SaveLevelAsset(levelData);

        }

        private void FillSerializableField(LevelData levelData)
        {
            for (int column = 0; column < _widthOfField.value; column++)
            {
                levelData.serializableField.Add(new CellList());
                for (int row = 0; row < _heightOfField.value; row++)
                {
                    CellData cellData = _levelCells[column,row];
                    Cell cell = new Cell();
                    if (cellData.isSelected)
                    {
                        cell = new Cell(false);
                    }
                    else
                    {
                        cell = new Cell(true);
                    }

                    levelData.serializableField[column].list.Add(cell);
                }
            }
        }
        private static void SaveLevelAsset(LevelData levelData)
        {
            string path = EditorUtility.SaveFilePanel("Save Level", "Assets", "Level", "asset");

            if (path.Length != 0)
            {
                int index = path.IndexOf("/Assets/") + 1;
                path = path.Substring(index);
                AssetDatabase.CreateAsset(levelData, path);
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();

                Selection.activeObject = levelData;
            }
        }

    }
}