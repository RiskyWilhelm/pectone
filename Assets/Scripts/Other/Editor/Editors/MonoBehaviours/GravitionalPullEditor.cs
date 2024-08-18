using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GravitionalPull))]
public class GravitionalPullEditor : Editor
{
	private GravitionalPull Target => (target as GravitionalPull);


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
			DrawInteractivePullDirection();
	}

	private void DrawInteractivePullDirection()
	{
		Handles.color = Color.magenta;

		// Draw interactable
		EditorGUI.BeginChangeCheck();
		Quaternion newRotation = Handles.FreeRotateHandle(Quaternion.Euler(Target.pullDirectionWorldEuler), Target.transform.position, HandleUtility.GetHandleSize(Target.transform.position) * 0.25f);
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(Target, "Changed Pull Direction");
			Target.pullDirectionWorldEuler = newRotation.eulerAngles;
		}

		// Draw the direction
		if (Target.pullDirectionWorldEuler == Vector3.zero)
			return;

		Handles.ArrowHandleCap(
			0,
			Target.transform.position,
			Target.isPullDirectionWorldAxis ? Quaternion.Euler(Target.pullDirectionWorldEuler) : (Target.transform.rotation * Quaternion.Euler(Target.pullDirectionWorldEuler)),
			HandleUtility.GetHandleSize(Target.transform.position) * 1.25f,
			EventType.Repaint
		);
	}
}
