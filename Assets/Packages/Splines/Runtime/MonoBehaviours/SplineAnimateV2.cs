using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

/// <summary> Upgraded version of <see cref="SplineAnimate"/> </summary>
[AddComponentMenu("Splines/Spline Animate V2")]
[ExecuteInEditMode]
public partial class SplineAnimateV2 : SplineComponent
{
	/// <summary> Describes the component type which controls the position. </summary>
	public enum TargetMode
	{
		Transform,

		Rigidbody
	}

	[SerializeField]
	private SplineContainer _splineContainer;

	[SerializeField]
	[Tooltip("The component mode to override position/rotation and such")]
	private TargetMode _targetComponentMode;

	[SerializeField]
	private Transform _targetTransform;

	[SerializeField]
	private Rigidbody _targetRigidbody;

	[SerializeField]
	private bool _playOnAwake = true;

	[SerializeField]
	[Tooltip("The loop mode that the animation uses. Loop modes cause the animation to repeat after it finishes. The following loop modes are available:.\n" +
			"Once - Traverse the spline once and stop at the end.\n" +
			"Loop Continuous - Traverse the spline continuously without stopping.\n" +
			"Ease In Then Continuous - Traverse the spline repeatedly without stopping. If Ease In easing is enabled, apply easing to the first loop only.\n" +
			"Ping Pong - Traverse the spline continuously without stopping and reverse direction after an end of the spline is reached.\n")]
	private SplineAnimate.LoopMode _animationLoopMode = SplineAnimate.LoopMode.Loop;

	[SerializeField]
	[Tooltip("The method used to animate the GameObject along the spline.\n" +
			"Time - The spline is traversed in a given amount of seconds.\n" +
			"Speed - The spline is traversed at a given maximum speed.")]
	private SplineAnimate.Method _animationMethodMode = SplineAnimate.Method.Time;

	[SerializeField]
	[Tooltip("The period of time that it takes for the GameObject to complete its animation along the spline.")]
	private float _animationDuration = 1f;

	[SerializeField]
	[Tooltip("The speed in meters/second that the GameObject animates along the spline at.")]
	private float _animationMaxSpeed = 10f;

	[SerializeField]
	[Tooltip("The easing mode used when the GameObject animates along the spline.\n" +
			"None - Apply no easing to the animation. The animation speed is linear.\n" +
			"Ease In Only - Apply easing to the beginning of animation.\n" +
			"Ease Out Only - Apply easing to the end of animation.\n" +
			"Ease In-Out - Apply easing to the beginning and end of animation.\n")]
	private SplineAnimate.EasingMode _animationEasingMode = SplineAnimate.EasingMode.None;

	[SerializeField]
	[Tooltip("The coordinate space that the GameObject's up and forward axes align to.")]
	private SplineAnimate.AlignmentMode _objectAlignMode = SplineAnimate.AlignmentMode.SplineElement;

	[SerializeField]
	[Tooltip("Which axis of the GameObject is treated as the forward axis.")]
	private AlignAxis _objectForwardAxis = AlignAxis.ZAxis;

	[SerializeField]
	[Tooltip("Which axis of the GameObject is treated as the up axis.")]
	private AlignAxis _objectUpAxis = AlignAxis.YAxis;

	[SerializeField]
	[Tooltip("Normalized distance [0;1] offset along the spline at which the GameObject should be placed when the animation begins.")]
	private float _startOffset;

	[NonSerialized]
	private float _startOffsetT;

	private float m_SplineLength = -1;

	private bool _isPlaying;

	private float _normalizedTime;

	private float _elapsedTime;

#if UNITY_EDITOR
	private double m_LastEditorUpdateTime;
#endif

	private bool m_EndReached = false;

	private SplinePath<Spline> m_SplinePath;

	public SplineContainer SplineContainer
	{
		get => _splineContainer;
		set
		{
			_splineContainer = value;

			if (enabled && _splineContainer != null && _splineContainer.Splines != null)
			{
				for (int i = 0; i < _splineContainer.Splines.Count; i++)
					OnSplineChange(_splineContainer.Splines[i], -1, SplineModification.Default);
			}

			UpdateStartOffsetT();
		}
	}

	public TargetMode TargetComponentMode
	{
		get => _targetComponentMode;
		set => _targetComponentMode = value;
	}

	public Transform TargetTransform
	{
		get => _targetTransform;
		set => _targetTransform = value;
	}

	public Rigidbody TargetRigidbody
	{
		get => _targetRigidbody;
		set => _targetRigidbody = value;
	}

	public bool PlayOnAwake
	{
		get => _playOnAwake;
		set => _playOnAwake = value;
	}

	public SplineAnimate.LoopMode AnimationLoopMode
	{
		get => _animationLoopMode;
		set => _animationLoopMode = value;
	}
	
	/// <summary> The method used to traverse the Spline. See <see cref="SplineAnimate.Method"/> for details. </summary>
	public SplineAnimate.Method AnimationMethodMode
	{
		get => _animationMethodMode;
		set => _animationMethodMode = value;
	}

	/// <summary> The time (in seconds) it takes to traverse the Spline once. </summary>
	/// <remarks>
	/// When animation method is set to <see cref="SplineAnimate.Method.Time"/> this setter will set the <see cref="AnimationDuration"/> value and automatically recalculate <see cref="AnimationMaxSpeed"/>,
	/// otherwise, it will have no effect.
	/// </remarks>
	public float AnimationDuration
	{
		get => _animationDuration;
		set
		{
			if (_animationMethodMode == SplineAnimate.Method.Time)
			{
				_animationDuration = Mathf.Max(0f, value);
				CalculateMaxSpeed();
			}
		}
	}

	/// <summary> The maxSpeed speed (in Unity units/second) that the Spline traversal will advance in. </summary>
	/// <remarks>
	/// If <see cref="EasingMode"/> is to <see cref="SplineAnimate.EasingMode.None"/> then the Spline will be traversed at MaxSpeed throughout its length.
	/// Otherwise, the traversal speed will range from 0 to MaxSpeed throughout the Spline's length depending on the easing mode set.
	/// When animation method is set to <see cref="SplineAnimate.Method.Speed"/> this setter will set the <see cref="AnimationMaxSpeed"/> value and automatically recalculate <see cref="AnimationDuration"/>,
	/// otherwise, it will have no effect.
	/// </remarks>
	public float AnimationMaxSpeed
	{
		get => _animationMaxSpeed;
		set
		{
			if (_animationMethodMode == SplineAnimate.Method.Speed)
			{
				_animationMaxSpeed = Mathf.Max(0f, value);
				CalculateDuration();
			}
		}
	}

	/// <summary> Easing mode used when animating the object along the Spline. See <see cref="SplineAnimate.EasingMode"/> for details. </summary>
	public SplineAnimate.EasingMode AnimationEaseMode
	{
		get => _animationEasingMode;
		set => _animationEasingMode = value;
	}

	/// <summary> The way the object should align when animating along the Spline. See <see cref="SplineAnimate.AlignmentMode"/> for details. </summary>
	public SplineAnimate.AlignmentMode ObjectAlignMode
	{
		get => _objectAlignMode;
		set => _objectAlignMode = value;
	}

	/// <summary> Object space axis that should be considered as the object's forward vector. </summary>
	public AlignAxis ObjectForwardAxis
	{
		get => _objectForwardAxis;
		set => _objectUpAxis = SetObjectAlignAxis(value, ref _objectForwardAxis, _objectUpAxis);
	}

	/// <summary> Object space axis that should be considered as the object's up vector. </summary>
	public AlignAxis ObjectUpAxis
	{
		get => _objectUpAxis;
		set => _objectForwardAxis = SetObjectAlignAxis(value, ref _objectUpAxis, _objectForwardAxis);
	}

	/// <summary>
	/// Normalized time of the Spline's traversal. The integer part is the number of times the Spline has been traversed.
	/// The fractional part is the % (0-1) of progress in the current loop.
	/// </summary>
	public float NormalizedTime
	{
		get => _normalizedTime;
		set
		{
			_normalizedTime = value;
			if (_animationLoopMode == SplineAnimate.LoopMode.PingPong)
			{
				var currentDirection = (int)(_elapsedTime / _animationDuration);
				_elapsedTime = _animationDuration * _normalizedTime + ((currentDirection % 2 == 1) ? _animationDuration : 0f);
			}
			else
				_elapsedTime = _animationDuration * _normalizedTime;

			UpdateTarget();
		}
	}

	/// <summary> Total time (in seconds) since the start of Spline's traversal. </summary>
	public float ElapsedTime
	{
		get => _elapsedTime;
		set
		{
			_elapsedTime = value;
			CalculateNormalizedTime(0f);
			UpdateTarget();
		}
	}

	/// <summary> Normalized distance [0;1] offset along the spline at which the object should be placed when the animation begins. </summary>
	public float StartOffset
	{
		get => _startOffset;
		set
		{
			if (m_SplineLength < 0f)
				RebuildSplinePath();

			_startOffset = Mathf.Clamp01(value);
			UpdateStartOffsetT();
		}
	}

	internal float StartOffsetT
		=> _startOffsetT;

	/// <summary> Returns true if object is currently animating along the Spline. </summary>
	public bool IsPlaying
		=> _isPlaying;

	/// <summary> Invoked each time object's animation along the Spline is updated.</summary>
	[SerializeField]
	private UnityEvent<Vector3, Quaternion> onUpdated;

	/// <summary> Invoked every time the object's animation reaches the end of the Spline.
	/// In case the animation loops, this event is called at the end of each loop.</summary>
	public UnityEvent onCompleted;

	/// <summary> Invoked each time object's animation along the Spline is updated.</summary>
	public event Action<Vector3, Quaternion> Updated;

	/// <summary> Invoked every time the object's animation reaches the end of the Spline.
	/// In case the animation loops, this event is called at the end of each loop.</summary>
	public event Action Completed;


	// Initialize
	private void Awake()
	{
#if UNITY_EDITOR
		if (EditorApplication.isPlaying)
#endif
			Restart(_playOnAwake);
#if UNITY_EDITOR
		else // Place the animated object back at the animation start position.
			Restart(false);
#endif
	}

	private void OnEnable()
	{
		RecalculateAnimationParameters();
		Spline.Changed += OnSplineChange;
	}

	private void OnDisable()
	{
		Spline.Changed -= OnSplineChange;
	}

	private void OnValidate()
	{
		_animationDuration = Mathf.Max(0f, _animationDuration);
		_animationMaxSpeed = Mathf.Max(0f, _animationMaxSpeed);
		RecalculateAnimationParameters();
	}

	internal void RecalculateAnimationParameters()
	{
		RebuildSplinePath();

		switch (_animationMethodMode)
		{
			case SplineAnimate.Method.Time:
			CalculateMaxSpeed();
			break;

			case SplineAnimate.Method.Speed:
			CalculateDuration();
			break;

			default:
			Debug.Log($"{_animationMethodMode} animation method is not supported!", this);
			break;
		}
	}

	private bool IsNullOrEmptyContainer()
	{
		if (_splineContainer == null || _splineContainer.Splines.Count == 0)
		{
			if (Application.isPlaying)
				Debug.LogError("SplineAnimate does not have a valid SplineContainer set.", this);
			return true;
		}
		return false;
	}

	/// <summary> Begin animating object along the Spline. </summary>
	public void Play()
	{
		if (IsNullOrEmptyContainer())
			return;

		_isPlaying = true;
#if UNITY_EDITOR
		m_LastEditorUpdateTime = EditorApplication.timeSinceStartup;
#endif
	}

	/// <summary> Pause object's animation along the Spline. </summary>
	public void Pause()
	{
		_isPlaying = false;
	}

	/// <summary> Stop the animation and place the object at the beginning of the Spline. </summary>
	/// <param name="autoplay"> If true, the animation along the Spline will start over again. </param>
	public void Restart(bool autoplay)
	{
		// [SPLB-269]: Early exit if the container is null to remove log error when initializing the spline animate object from code
		if (SplineContainer == null)
			return;

		if (IsNullOrEmptyContainer())
			return;

		_isPlaying = false;
		_elapsedTime = 0f;
		NormalizedTime = 0f;

		switch (_animationMethodMode)
		{
			case SplineAnimate.Method.Time:
			CalculateMaxSpeed();
			break;

			case SplineAnimate.Method.Speed:
			CalculateDuration();
			break;

			default:
			Debug.Log($"{_animationMethodMode} animation method is not supported!", this);
			break;
		}
		UpdateTarget();
		UpdateStartOffsetT();

		if (autoplay)
			Play();
	}

	/// <summary>
	/// Evaluates the animation along the Spline based on deltaTime.
	/// </summary>
	public void Update()
	{
		if (!_isPlaying || (_animationLoopMode == SplineAnimate.LoopMode.Once && _normalizedTime >= 1f))
			return;

		var dt = Time.deltaTime;

#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
		{
			dt = (float)(EditorApplication.timeSinceStartup - m_LastEditorUpdateTime);
			m_LastEditorUpdateTime = EditorApplication.timeSinceStartup;
		}
#endif
		CalculateNormalizedTime(dt);
		UpdateTarget();
	}

	private void CalculateNormalizedTime(float deltaTime)
	{
		var previousElapsedTime = _elapsedTime;
		_elapsedTime += deltaTime;
		var currentDuration = _animationDuration;

		var t = 0f;
		switch (_animationLoopMode)
		{
			case SplineAnimate.LoopMode.Once:
			t = Mathf.Min(_elapsedTime, currentDuration);
			break;

			case SplineAnimate.LoopMode.Loop:
			t = _elapsedTime % currentDuration;
			UpdateEndReached(previousElapsedTime, currentDuration);
			break;

			case SplineAnimate.LoopMode.LoopEaseInOnce:
			/* If the first loop had an ease in, then our velocity is double that of linear traversal.
			 Therefore time to traverse subsequent loops should be half of the first loop. */
			if ((_animationEasingMode == SplineAnimate.EasingMode.EaseIn || _animationEasingMode == SplineAnimate.EasingMode.EaseInOut) &&
				_elapsedTime >= currentDuration)
				currentDuration *= 0.5f;
			t = _elapsedTime % currentDuration;
			UpdateEndReached(previousElapsedTime, currentDuration);
			break;

			case SplineAnimate.LoopMode.PingPong:
			t = Mathf.PingPong(_elapsedTime, currentDuration);
			UpdateEndReached(previousElapsedTime, currentDuration);
			break;

			default:
			Debug.Log($"{_animationLoopMode} animation loop mode is not supported!", this);
			break;
		}
		t /= currentDuration;

		if (_animationLoopMode == SplineAnimate.LoopMode.LoopEaseInOnce)
		{
			// Apply ease in for the first loop and continue linearly for remaining loops
			if ((_animationEasingMode == SplineAnimate.EasingMode.EaseIn || _animationEasingMode == SplineAnimate.EasingMode.EaseInOut) &&
				_elapsedTime < currentDuration)
				t = EaseInQuadratic(t);
		}
		else
		{
			switch (_animationEasingMode)
			{
				case SplineAnimate.EasingMode.EaseIn:
				t = EaseInQuadratic(t);
				break;

				case SplineAnimate.EasingMode.EaseOut:
				t = EaseOutQuadratic(t);
				break;

				case SplineAnimate.EasingMode.EaseInOut:
				t = EaseInOutQuadratic(t);
				break;
			}
		}

		// forcing reset to 0 if the m_NormalizedTime reach the end of the spline previously (1).
		_normalizedTime = t == 0 ? 0f : Mathf.Floor(_normalizedTime) + t;
		if (_normalizedTime >= 1f && _animationLoopMode == SplineAnimate.LoopMode.Once)
		{
			m_EndReached = true;
			_isPlaying = false;
		}
	}

	private void UpdateEndReached(float previousTime, float currentDuration)
	{
		m_EndReached = Mathf.FloorToInt(previousTime / currentDuration) < Mathf.FloorToInt(_elapsedTime / currentDuration);
	}

	private void UpdateStartOffsetT()
	{
		if (m_SplinePath != null)
			_startOffsetT = m_SplinePath.ConvertIndexUnit(_startOffset * m_SplineLength, PathIndexUnit.Distance, PathIndexUnit.Normalized);
	}

	private void UpdateTarget()
	{
		if (_splineContainer == null)
			return;

		EvaluatePositionAndRotation(out var position, out var rotation);

#if UNITY_EDITOR
		if (EditorApplication.isPlaying)
		{
#endif
			switch (_targetComponentMode)
			{
				case TargetMode.Transform:
				{
					_targetTransform.position = position;
					if (_objectAlignMode != SplineAnimate.AlignmentMode.None)
						_targetTransform.rotation = rotation;
				}
				break;

				case TargetMode.Rigidbody:
					_targetRigidbody.position = position;
					if (_objectAlignMode != SplineAnimate.AlignmentMode.None)
						_targetRigidbody.rotation = rotation;
				break;

				default:
				Debug.Log($"{_targetComponentMode} target controller mode is not supported!", this);
				break;
			}
#if UNITY_EDITOR
		}
#endif
		Updated?.Invoke(position, rotation);
		onUpdated?.Invoke(position, rotation);

		if (m_EndReached)
		{
			m_EndReached = false;
			Completed?.Invoke();
			onCompleted?.Invoke();
		}
	}

	private void EvaluatePositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		var t = GetLoopInterpolation(true);
		position = _splineContainer.EvaluatePosition(m_SplinePath, t);
		rotation = Quaternion.identity;

		// Correct forward and up vectors based on axis remapping parameters
		var remappedForward = GetAxis(_objectForwardAxis);
		var remappedUp = GetAxis(_objectUpAxis);
		var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));

		if (_objectAlignMode != SplineAnimate.AlignmentMode.None)
		{
			var forward = Vector3.forward;
			var up = Vector3.up;

			switch (_objectAlignMode)
			{
				case SplineAnimate.AlignmentMode.SplineElement:
				forward = _splineContainer.EvaluateTangent(m_SplinePath, t);
				if (Vector3.Magnitude(forward) <= Mathf.Epsilon)
				{
					if (t < 1f)
						forward = _splineContainer.EvaluateTangent(m_SplinePath, Mathf.Min(1f, t + 0.01f));
					else
						forward = _splineContainer.EvaluateTangent(m_SplinePath, t - 0.01f);
				}
				forward.Normalize();
				up = _splineContainer.EvaluateUpVector(m_SplinePath, t);
				break;

				case SplineAnimate.AlignmentMode.SplineObject:
				var objectRotation = _splineContainer.transform.rotation;
				forward = objectRotation * forward;
				up = objectRotation * up;
				break;

				case SplineAnimate.AlignmentMode.World:
				break;

				default:
				Debug.Log($"{_objectAlignMode} animation alignment mode is not supported!", this);
				break;
			}

			rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;
		}
		else
		{
			switch (_targetComponentMode)
			{
				case TargetMode.Transform:
				rotation = _targetTransform.rotation;
				break;

				case TargetMode.Rigidbody:
				rotation = _targetRigidbody.rotation;
				break;

				default:
				Debug.Log($"{_targetComponentMode} target controller mode is not supported!", this);
				break;
			}
		}
	}

	private void CalculateDuration()
	{
		if (m_SplineLength < 0f)
			RebuildSplinePath();

		if (m_SplineLength >= 0f)
		{
			switch (_animationEasingMode)
			{
				case SplineAnimate.EasingMode.None:
				_animationDuration = m_SplineLength / _animationMaxSpeed;
				break;

				case SplineAnimate.EasingMode.EaseIn:
				case SplineAnimate.EasingMode.EaseOut:
				case SplineAnimate.EasingMode.EaseInOut:
				_animationDuration = (2f * m_SplineLength) / _animationMaxSpeed;
				break;

				default:
				Debug.Log($"{_animationEasingMode} animation easing mode is not supported!", this);
				break;
			}
		}
	}

	private void CalculateMaxSpeed()
	{
		if (m_SplineLength < 0f)
			RebuildSplinePath();

		if (m_SplineLength >= 0f)
		{
			switch (_animationEasingMode)
			{
				case SplineAnimate.EasingMode.None:
				_animationMaxSpeed = m_SplineLength / _animationDuration;
				break;

				case SplineAnimate.EasingMode.EaseIn:
				case SplineAnimate.EasingMode.EaseOut:
				case SplineAnimate.EasingMode.EaseInOut:
				_animationMaxSpeed = (2f * m_SplineLength) / _animationDuration;
				break;

				default:
				Debug.Log($"{_animationEasingMode} animation easing mode is not supported!", this);
				break;
			}
		}
	}

	private void RebuildSplinePath()
	{
		if (_splineContainer != null)
		{
			m_SplinePath = new SplinePath<Spline>(_splineContainer.Splines);
			m_SplineLength = m_SplinePath != null ? m_SplinePath.GetLength() : 0f;
		}
	}

	private AlignAxis SetObjectAlignAxis(AlignAxis newValue, ref AlignAxis targetAxis, AlignAxis otherAxis)
	{
		// Swap axes if the new value matches that of the other axis
		if (newValue == otherAxis)
		{
			otherAxis = targetAxis;
			targetAxis = newValue;
		}
		// Do not allow configuring object's forward and up axes as opposite
		else if ((int)newValue % 3 != (int)otherAxis % 3)
			targetAxis = newValue;

		return otherAxis;
	}

	private void OnSplineChange(Spline spline, int knotIndex, SplineModification modificationType)
	{
		RecalculateAnimationParameters();
	}

	internal float GetLoopInterpolation(bool offset)
	{
		var t = 0f;
		var normalizedTimeWithOffset = NormalizedTime + (offset ? _startOffsetT : 0f);
		if (Mathf.Floor(normalizedTimeWithOffset) == normalizedTimeWithOffset)
			t = Mathf.Clamp01(normalizedTimeWithOffset);
		else
			t = normalizedTimeWithOffset % 1f;

		return t;
	}

	private float EaseInQuadratic(float t)
	{
		return t * t;
	}

	private float EaseOutQuadratic(float t)
	{
		return t * (2f - t);
	}

	private float EaseInOutQuadratic(float t)
	{
		var eased = 2f * t * t;
		if (t > 0.5f)
			eased = 4f * t - eased - 1f;
		return eased;
	}
}


#if UNITY_EDITOR

public sealed partial class SplineAnimateV2
{ }


#endif
