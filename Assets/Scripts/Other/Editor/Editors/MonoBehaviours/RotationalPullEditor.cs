using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(RotationalPull))]
public class RotationalPullEditor : Editor
{
	private RotationalPull Target => (target as RotationalPull);


	// Initialize
	// WARNING: Custom properties wont get drawn due to Unity. They are using IMGUI for DrawDefaultInspector() internally
	public override VisualElement CreateInspectorGUI()
	{
		var rootElement = new VisualElement();
		InspectorElement.FillDefaultInspector(rootElement, serializedObject, this);
		return rootElement;
	}


	// Update
	private void OnSceneGUI()
	{
		if (Target.e_IsActivatedInteractiveEditing)
			DrawInteractiveUpDirection();
	}

	private void DrawInteractiveUpDirection()
	{
		Handles.color = Color.magenta;

		// Draw interactable
		EditorGUI.BeginChangeCheck();
		Quaternion newRotation = Handles.FreeRotateHandle(Quaternion.Euler(Target.upDirectionWorldEuler), Target.transform.position, HandleUtility.GetHandleSize(Target.transform.position) * 0.25f);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(Target, "Changed Up Direction");
			Target.upDirectionWorldEuler = newRotation.eulerAngles;
		}

		// Draw the direction
		if (Target.upDirectionWorldEuler == Vector3.zero)
			return;

		Handles.ArrowHandleCap(
			0,
			Target.transform.position,
			Target.isUpDirectionWorldAxis ? Quaternion.Euler(Target.upDirectionWorldEuler) : (Target.transform.rotation * Quaternion.Euler(Target.upDirectionWorldEuler)),
			HandleUtility.GetHandleSize(Target.transform.position) * 1.25f,
			EventType.Repaint
		);
	}
}
