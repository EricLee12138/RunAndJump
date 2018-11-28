using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RunAndJump.LevelCreator {
    
    [CustomEditor(typeof(Level))]
    public class LevelInspector : Editor {
        private Level _myTarget;
        private GameObject obj;

        private int _newTotalColumns;
        private int _newTotalRows;

        private void OnEnable() {
            Debug.Log("OnEnable was called...");
            _myTarget = (Level)target;
            InitLevel();
            ResetResizeValues();
        }
        
        private void OnDisable() {
            Debug.Log("OnDisable was called...");
        }
        
        private void OnDestroy() {
            Debug.Log("OnDestroy was called...");
        }
        
        public override void OnInspectorGUI() {
            DrawLevelDataGUI();
            DrawLevelSizeGUI();

            if (GUI.changed) {
                EditorUtility.SetDirty(_myTarget);
            }
        }

        private void DrawLevelDataGUI() {
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
            // _myTarget.TotalTime = EditorGUILayout.IntField("Total Time", Mathf.Max(0, _myTarget.TotalTime));
            _myTarget.TotalTime = EditorGUI.IntField(new Rect(0, 500, EditorGUIUtility.currentViewWidth, 20), "Total Time", Mathf.Max(0, _myTarget.TotalTime));
            _myTarget.Gravity = EditorGUILayout.FloatField("Gravity", _myTarget.Gravity);
            _myTarget.Bgm = (AudioClip)EditorGUILayout.ObjectField("Bgm", _myTarget.Bgm, typeof(AudioClip), false);
            _myTarget.Background = (Sprite)EditorGUILayout.ObjectField("Background", _myTarget.Background, typeof(Sprite), false);
        }
        private void DrawLevelSizeGUI() {
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);

            _newTotalColumns = EditorGUILayout.IntField("Columns", Mathf.Max(1, _newTotalColumns));
            _newTotalRows = EditorGUILayout.IntField("Rows", Mathf.Max(1, _newTotalRows));
            
            // with this variable we can enable or disable GUI 
            bool oldEnabled = GUI.enabled;
            GUI.enabled = (_newTotalColumns != _myTarget.TotalColumns || _newTotalRows != _myTarget.TotalRows);
            bool buttonResize = GUILayout.Button("Resize", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight));
            if (buttonResize) {
                if (EditorUtility.DisplayDialog(
                    "Level Creator",
                    "Are you sure you want to resize the level?\nThis action cannot be undone.",
                    "Yes", "No")) {
                    ResizeLevel();
                }
            }
            bool buttonReset = GUILayout.Button("Reset");
            if (buttonReset) {
                ResetResizeValues();
            }
            GUI.enabled = oldEnabled;
        }

        private void InitLevel()
        {
            if (_myTarget.Pieces == null || _myTarget.Pieces.Length == 0)
            {
                Debug.Log("Initializing the Pieces array...");
                _myTarget.Pieces = new LevelPiece[_myTarget.TotalColumns * _myTarget.TotalRows];
            }
        }

        private void ResetResizeValues() {
            _newTotalColumns = _myTarget.TotalColumns;
            _newTotalRows = _myTarget.TotalRows;
        }

        private void ResizeLevel() {
            LevelPiece[] newPieces = new LevelPiece[_newTotalColumns * _newTotalRows];
            for (int col = 0; col < _myTarget.TotalColumns; ++col) {
                for (int row = 0; row < _myTarget.TotalRows; ++row) {
                    if (col < _newTotalColumns && row < _newTotalRows) {
                        newPieces[col + row * _newTotalColumns] = _myTarget.Pieces[col + row * _myTarget.TotalColumns];
                    } else {
                        LevelPiece piece = _myTarget.Pieces[col + row * _myTarget.TotalColumns];
                        if (piece != null) {                    
                            // we must to use DestroyImmediate in a Editor context
                            Object.DestroyImmediate(piece.gameObject);
                        }
                    }
                }
            }
            _myTarget.Pieces = newPieces;
            _myTarget.TotalColumns = _newTotalColumns;
            _myTarget.TotalRows = _newTotalRows;
        }
    }
}