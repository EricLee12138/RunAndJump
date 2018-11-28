using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RunAndJump.LevelCreator {
	public static class MenuItems {
		[MenuItem ("Tools/Level Creator/New Empty Level", false, 1)]
		private static void NewEmptyLevel() {
			// do something
		}

        [MenuItem("Tools/Level Creator/Log Selected Transform Name", false, 70)]
        static void LogSelectedTransformName()
        {
            Debug.Log("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
        }

        [MenuItem("Tools/Level Creator/Log Selected Transform Name", true)]
        static bool ValidateLogSelectedTransformName()
        {
            return false;
        }

        [MenuItem("Tools/Level Creator/Custom Game Object", false, 5)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("Custom Game Object");
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
	}
}
