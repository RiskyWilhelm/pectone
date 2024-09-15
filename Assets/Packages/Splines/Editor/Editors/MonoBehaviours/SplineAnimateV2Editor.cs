using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Splines;

[CustomEditor(typeof(SplineAnimateV2))]
[CanEditMultipleObjects]
public class SplineAnimateV2Editor : Editor
{
	private readonly List<VisualElement> m_Roots = new();
	private readonly List<Slider> m_ProgressSliders = new();
	private readonly List<FloatField> m_ElapsedTimeFields = new();
	private readonly List<EnumField> m_ObjectForwardFields = new();
	private readonly List<EnumField> m_ObjectUpFields = new();

	private SerializedProperty _targetComponentMode;
	private SerializedProperty _animationMethodMode;
	private SerializedProperty _objectForwardAxis;
	private SerializedProperty _objectUpAxis;
	private SerializedProperty _startOffset;
	private SerializedObject m_TargetTransformSO;

	private SplineAnimateV2 m_SplineAnimateV2;

	private const string k_UxmlPath = "Assets/Packages/Splines/Editor/Resources/splineanimatev2-inspector.uxml";
	private static VisualTreeAsset s_TreeAsset;
	private static StyleSheet s_ThemeStyleSheet;

	private SplineAnimateV2[] m_Components;

	private void OnEnable()
	{
		m_SplineAnimateV2 = target as SplineAnimateV2;
		if (m_SplineAnimateV2 == null)
			return;

		m_SplineAnimateV2.Updated += OnSplineAnimateUpdated;

		try
		{
			_animationMethodMode = serializedObject.FindProperty(nameof(_animationMethodMode));
			_targetComponentMode = serializedObject.FindProperty(nameof(_targetComponentMode));
			_objectForwardAxis = serializedObject.FindProperty(nameof(_objectForwardAxis));
			_objectUpAxis = serializedObject.FindProperty(nameof(_objectUpAxis));
			_startOffset = serializedObject.FindProperty(nameof(_startOffset));
		}
		catch (Exception)
		{
			return;
		}

		InitializeTargetForEditor();
		m_Components = targets.Select(x => x as SplineAnimateV2).Where(y => y != null).ToArray();

		foreach (var animate in m_Components)
		{
			if (animate.SplineContainer != null)
				animate.RecalculateAnimationParameters();
		}

		m_Roots.Clear();
		m_ObjectForwardFields.Clear();
		m_ObjectUpFields.Clear();
		m_ProgressSliders.Clear();
		m_ElapsedTimeFields.Clear();

		EditorApplication.update += OnEditorUpdate;
		Spline.Changed += OnSplineChange;
		SplineContainer.SplineAdded += OnContainerSplineSetModified;
		SplineContainer.SplineRemoved += OnContainerSplineSetModified;
	}

	private void OnDisable()
	{
		if (m_SplineAnimateV2 != null)
			m_SplineAnimateV2.Updated -= OnSplineAnimateUpdated;

		if (!EditorApplication.isPlaying)
		{
			foreach (var animate in m_Components)
			{
				if (animate.SplineContainer != null)
				{
					animate.RecalculateAnimationParameters();
					animate.Restart(false);
				}
			}
		}

		EditorApplication.update -= OnEditorUpdate;
		Spline.Changed -= OnSplineChange;
		SplineContainer.SplineAdded -= OnContainerSplineSetModified;
		SplineContainer.SplineRemoved -= OnContainerSplineSetModified;
	}

	private void OnEditorUpdate()
	{
		if (!EditorApplication.isPlaying)
		{
			if (m_SplineAnimateV2.SplineContainer != null && m_SplineAnimateV2.IsPlaying)
			{
				m_SplineAnimateV2.Update();
				RefreshProgressFields();
			}
		}
		else if (m_SplineAnimateV2.IsPlaying)
			RefreshProgressFields();
	}

	private void OnSplineChange(Spline spline, int knotIndex, SplineModification modificationType)
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode)
			return;

		foreach (var animate in m_Components)
		{
			if (animate.SplineContainer != null && animate.SplineContainer.Splines.Contains(spline))
				animate.RecalculateAnimationParameters();
		}
	}

	private void OnContainerSplineSetModified(SplineContainer container, int spline)
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode)
			return;

		foreach (var animate in m_Components)
		{
			if (animate.SplineContainer == container)
				animate.RecalculateAnimationParameters();
		}
	}

	public override VisualElement CreateInspectorGUI()
	{
		var root = new VisualElement();

		if (s_TreeAsset == null)
			s_TreeAsset = (VisualTreeAsset)AssetDatabase.LoadAssetAtPath(k_UxmlPath, typeof(VisualTreeAsset));
		s_TreeAsset.CloneTree(root);

		if (s_ThemeStyleSheet == null)
			s_ThemeStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"Packages/com.unity.splines/Editor/Stylesheets/SplineAnimateInspector{(EditorGUIUtility.isProSkin ? "Dark" : "Light")}.uss");

		root.styleSheets.Add(s_ThemeStyleSheet);

		var targetComponentModeField = root.Q<EnumField>("target-component-mode");
		targetComponentModeField.RegisterValueChangedCallback((evt) => OnTargetComponentModeChanged(evt));
		RefreshPositionControllerParamFields((SplineAnimateV2.TargetMode)_targetComponentMode.enumValueIndex);

		var targetTransformField = root.Q<PropertyField>("target-transform");
		targetTransformField.RegisterValueChangeCallback((_) => InitializeTargetForEditor());

		var targetRigidbodyField = root.Q<PropertyField>("target-rigidbody");
		targetRigidbodyField.RegisterValueChangeCallback((_) => InitializeTargetForEditor());

		var methodField = root.Q<PropertyField>("method");
		methodField.RegisterValueChangeCallback((_) => { RefreshMethodParamFields((SplineAnimate.Method)_animationMethodMode.enumValueIndex); });
		RefreshMethodParamFields((SplineAnimate.Method)_animationMethodMode.enumValueIndex);

		var objectForwardField = root.Q<EnumField>("object-forward");
		objectForwardField.RegisterValueChangedCallback((evt) => OnObjectAxisFieldChange(evt, _objectForwardAxis, _objectUpAxis));

		var objectUpField = root.Q<EnumField>("object-up");
		objectUpField.RegisterValueChangedCallback((evt) => OnObjectAxisFieldChange(evt, _objectUpAxis, _objectForwardAxis));

		var playButton = root.Q<Button>("play");
		playButton.SetEnabled(!EditorApplication.isPlaying);
		playButton.clicked += OnPlayClicked;

		var pauseButton = root.Q<Button>("pause");
		pauseButton.SetEnabled(!EditorApplication.isPlaying);
		pauseButton.clicked += OnPauseClicked;

		var resetButton = root.Q<Button>("reset");
		resetButton.SetEnabled(!EditorApplication.isPlaying);
		resetButton.clicked += OnResetClicked;

		var progressSlider = root.Q<Slider>("normalized-progress");
		progressSlider.SetEnabled(!EditorApplication.isPlaying);
		progressSlider.RegisterValueChangedCallback((evt) => OnProgressSliderChange(evt.newValue));

		var elapsedTimeField = root.Q<FloatField>("elapsed-time");
		elapsedTimeField.SetEnabled(!EditorApplication.isPlaying);
		elapsedTimeField.RegisterValueChangedCallback((evt) => OnElapsedTimeFieldChange(evt.newValue));

		var startOffsetField = root.Q<PropertyField>("start-offset");
		startOffsetField.RegisterValueChangeCallback((evt) =>
		{
			m_SplineAnimateV2.StartOffset = _startOffset.floatValue;
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
				m_SplineAnimateV2.Restart(false);
				OnElapsedTimeFieldChange(elapsedTimeField.value);
			}
		});

		m_Roots.Add(root);
		m_ProgressSliders.Add(progressSlider);
		m_ElapsedTimeFields.Add(elapsedTimeField);

		m_ObjectForwardFields.Add(objectForwardField);
		m_ObjectUpFields.Add(objectUpField);

		return root;
	}

	private void OnTargetComponentModeChanged(ChangeEvent<Enum> evt)
	{
		RefreshPositionControllerParamFields((SplineAnimateV2.TargetMode)evt.newValue);
		InitializeTargetForEditor();
	}

	private void InitializeTargetForEditor()
	{
		switch ((SplineAnimateV2.TargetMode)_targetComponentMode.enumValueIndex)
		{
			case SplineAnimateV2.TargetMode.Transform:
			{
				if (m_SplineAnimateV2.TargetTransform)
					m_TargetTransformSO = new SerializedObject(m_SplineAnimateV2.TargetTransform);
				else
					m_TargetTransformSO = new SerializedObject(m_SplineAnimateV2.transform);
			}
			break;

			case SplineAnimateV2.TargetMode.Rigidbody:
			{
				if (m_SplineAnimateV2.TargetRigidbody)
					m_TargetTransformSO = new SerializedObject(m_SplineAnimateV2.TargetRigidbody.transform);
				else
					m_TargetTransformSO = new SerializedObject(m_SplineAnimateV2.transform);
			}
			break;
		}
	}

	private void RefreshPositionControllerParamFields(SplineAnimateV2.TargetMode newValue)
	{
		foreach (var root in m_Roots)
		{

			var targetTransformField = root.Q<PropertyField>("target-transform");
			var targetRigidbodyField = root.Q<PropertyField>("target-rigidbody");

			if (newValue == (int)SplineAnimateV2.TargetMode.Transform)
			{
				targetTransformField.style.display = DisplayStyle.Flex;
				targetRigidbodyField.style.display = DisplayStyle.None;
			}
			else
			{
				targetTransformField.style.display = DisplayStyle.None;
				targetRigidbodyField.style.display = DisplayStyle.Flex;
			}
		}
	}

	private void RefreshMethodParamFields(SplineAnimate.Method method)
	{
		foreach (var root in m_Roots)
		{

			var durationField = root.Q<PropertyField>("duration");
			var maxSpeedField = root.Q<PropertyField>("max-speed");

			if (method == (int)SplineAnimate.Method.Time)
			{
				durationField.style.display = DisplayStyle.Flex;
				maxSpeedField.style.display = DisplayStyle.None;
			}
			else
			{
				durationField.style.display = DisplayStyle.None;
				maxSpeedField.style.display = DisplayStyle.Flex;
			}
		}
	}

	private void RefreshProgressFields()
	{
		for (int i = 0; i < m_ProgressSliders.Count && i < m_ElapsedTimeFields.Count; ++i)
		{
			var progressSlider = m_ProgressSliders[i];
			var elapsedTimeField = m_ElapsedTimeFields[i];
			if (progressSlider == null || elapsedTimeField == null)
				continue;

			progressSlider.SetValueWithoutNotify(m_SplineAnimateV2.GetLoopInterpolation(false));
			elapsedTimeField.SetValueWithoutNotify(m_SplineAnimateV2.ElapsedTime);
		}
	}

	private void OnProgressSliderChange(float progress)
	{
		m_SplineAnimateV2.Pause();
		m_SplineAnimateV2.NormalizedTime = progress;

		RefreshProgressFields();
	}

	private void OnElapsedTimeFieldChange(float elapsedTime)
	{
		m_SplineAnimateV2.Pause();
		m_SplineAnimateV2.ElapsedTime = elapsedTime;

		RefreshProgressFields();
	}

	private void OnObjectAxisFieldChange(ChangeEvent<Enum> changeEvent, SerializedProperty axisProp, SerializedProperty otherAxisProp)
	{
		if (changeEvent.newValue == null)
			return;


		var newValue = (SplineAnimate.AlignAxis)changeEvent.newValue;
		var previousValue = (SplineAnimate.AlignAxis)changeEvent.previousValue;

		// Swap axes if the picked value matches that of the other axis field
		if (newValue == (SplineAnimate.AlignAxis)otherAxisProp.enumValueIndex)
		{
			otherAxisProp.enumValueIndex = (int)previousValue;
			serializedObject.ApplyModifiedProperties();
		}
		// Prevent the user from configuring object's forward and up as opposite axes
		if (((int)newValue) % 3 == otherAxisProp.enumValueIndex % 3)
		{
			axisProp.enumValueIndex = (int)previousValue;
			serializedObject.ApplyModifiedPropertiesWithoutUndo();
		}

		foreach (var objectForwardField in m_ObjectForwardFields)
			objectForwardField.SetValueWithoutNotify((SplineComponent.AlignAxis)_objectForwardAxis.enumValueIndex);
		foreach (var objectUpField in m_ObjectUpFields)
			objectUpField.SetValueWithoutNotify((SplineComponent.AlignAxis)_objectUpAxis.enumValueIndex);
	}

	private void OnPlayClicked()
	{
		if (!m_SplineAnimateV2.IsPlaying)
		{
			m_SplineAnimateV2.RecalculateAnimationParameters();
			if (m_SplineAnimateV2.NormalizedTime == 1f)
				m_SplineAnimateV2.Restart(true);
			else
				m_SplineAnimateV2.Play();
		}
	}

	private void OnPauseClicked()
	{
		m_SplineAnimateV2.Pause();
	}

	private void OnResetClicked()
	{
		m_SplineAnimateV2.RecalculateAnimationParameters();
		m_SplineAnimateV2.Restart(false);
		RefreshProgressFields();
	}

	private void OnSplineAnimateUpdated(Vector3 position, Quaternion rotation)
	{
		if (m_SplineAnimateV2 == null)
			return;

		if (!EditorApplication.isPlaying)
		{
			m_TargetTransformSO.Update();

			var localPosition = position;
			var localRotation = rotation;
			if (m_SplineAnimateV2.transform.parent != null)
			{
				localPosition = m_SplineAnimateV2.transform.parent.worldToLocalMatrix.MultiplyPoint3x4(position);
				localRotation = Quaternion.Inverse(m_SplineAnimateV2.transform.parent.rotation) * localRotation;
			}

			m_TargetTransformSO.FindProperty("m_LocalPosition").vector3Value = localPosition;
			m_TargetTransformSO.FindProperty("m_LocalRotation").quaternionValue = localRotation;

			m_TargetTransformSO.ApplyModifiedProperties();
		}
	}

	[DrawGizmo(GizmoType.Selected | GizmoType.Active)]
	private static void DrawSplineAnimateGizmos(SplineAnimateV2 splineAnimate, GizmoType gizmoType)
	{
		if (splineAnimate.SplineContainer == null)
			return;

		const float k_OffsetGizmoSize = 0.15f;
		splineAnimate.SplineContainer.Evaluate(splineAnimate.StartOffsetT, out var offsetPos, out var forward, out var up);

#if UNITY_2022_2_OR_NEWER
		using (new Handles.DrawingScope(Handles.elementColor))
#else
            using (new Handles.DrawingScope(SplineHandleUtility.knotColor))
#endif
			if (Vector3.Magnitude(forward) <= Mathf.Epsilon)
			{
				if (splineAnimate.StartOffsetT < 1f)
					forward = splineAnimate.SplineContainer.EvaluateTangent(Mathf.Min(1f, splineAnimate.StartOffsetT + 0.01f));
				else
					forward = splineAnimate.SplineContainer.EvaluateTangent(splineAnimate.StartOffsetT - 0.01f);

			}
		Handles.ConeHandleCap(-1, offsetPos, Quaternion.LookRotation(Vector3.Normalize(forward), up), k_OffsetGizmoSize * HandleUtility.GetHandleSize(offsetPos), EventType.Repaint);
	}
}
