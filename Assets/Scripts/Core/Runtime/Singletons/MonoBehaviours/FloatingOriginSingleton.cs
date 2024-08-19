using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class FloatingOriginSingleton : MonoBehaviourSingletonBase<FloatingOriginSingleton>
{
	#region FloatingOriginSingleton Floating Origin

	[SerializeField]
	private Rigidbody selfRigidbody;

	[SerializeField]
	private Rigidbody alignRigidbody;

	private readonly HashSet<Rigidbody> childRigidbodiesSet = new();


	#endregion


	// Update
	/// <remarks> Do not use frequently </remarks>
	public void Shift()
	{
		var shiftPosition = alignRigidbody.position;
		selfRigidbody.position -= shiftPosition;

		childRigidbodiesSet.RemoveWhere((x) => !x);
		foreach (var childRigidbody in childRigidbodiesSet)
		{
			if (childRigidbody.isKinematic)
				continue;

			if (!childRigidbody.IsSleeping() && !childRigidbody.IsMovingApproximately())
				childRigidbody.Sleep();

			childRigidbody.position -= shiftPosition;
		}

		alignRigidbody.position = Vector3.zero;
	}

	public void RegisterChildRigidbody(Rigidbody childRigidbody)
		=> childRigidbodiesSet.Add(childRigidbody);

	public void UnRegisterChildRigidbody(Rigidbody childRigidbody)
		=> childRigidbodiesSet.Remove(childRigidbody);
}


#if UNITY_EDITOR

public partial class FloatingOriginSingleton { }

#endif