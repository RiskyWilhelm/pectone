using Newtonsoft.Json;
using System;
using UnityEngine;

/// <summary> A Guid that can be serialized by Unity </summary>
/// <remarks> 128-bit Guid is stored as two 64-bit(8 byte) ulongs which named low and high </remarks>
[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public struct GuidSerializable : IEquatable<GuidSerializable>
{
	private static readonly GuidSerializable k_Empty = Guid.Empty;

	[SerializeField]
	[HideInInspector]
	[JsonProperty]
	private ulong m_GuidLow;

	[SerializeField]
	[HideInInspector]
	[JsonProperty]
	private ulong m_GuidHigh;

	/// <summary> Represents <see cref="Guid.Empty"/>, a GUID where the value is all zero </summary>
	public static GuidSerializable Empty => k_Empty;

	/// <summary> Reconstructs a <see cref="System.Guid"/> from two <see cref="ulong"/> values representing the low and high bytes </summary>
	public readonly Guid Guid => GuidUtils.Compose(m_GuidLow, m_GuidHigh);


	/// <summary> Constructs a <see cref="GuidSerializable"/> from two 64-bit(8 byte) <see cref="ulong"/> values </summary>
	/// <param name="guidLow"> The low 8 bytes of the <see cref="System.Guid"/> </param>
	/// <param name="guidHigh"> The high 8 bytes of the <see cref="System.Guid"/> </param>
	[JsonConstructor]
	public GuidSerializable(ulong guidLow, ulong guidHigh)
	{
		m_GuidLow = guidLow;
		m_GuidHigh = guidHigh;
	}

	/// <summary> Decomposes two values from <see cref="System.Guid"/> representing the low and high 64-bit(8 byte) <see cref="ulong"/> values and constructs a <see cref="GuidSerializable"/> </summary>
	public GuidSerializable(Guid guid)
	{
		guid.Decompose(out m_GuidLow, out m_GuidHigh);
	}

	/// <summary> Uses <see cref="Guid.NewGuid"/> </summary>
	public static GuidSerializable NewGuid() => new GuidSerializable(Guid.NewGuid());

	public override readonly int GetHashCode() => HashCode.Combine(m_GuidLow, m_GuidHigh);

	public override readonly bool Equals(object obj)
	{
		if (obj is GuidSerializable serializableGuid)
			Equals(serializableGuid);

		return false;
	}

	/// <summary> Generates a string representation of the <see cref="Guid"/>. Same as <see cref="Guid.ToString()"/> </summary>
	public override readonly string ToString() => Guid.ToString();

	/// <summary> Generates a string representation of the <see cref="Guid"/>. Same as <see cref="Guid.ToString(string)"/> </summary>
	/// <param name="format"> A single format specifier that indicates how to format the value of the <see cref="Guid"/> </param>
	public readonly string ToString(string format) => Guid.ToString(format);

	/// /// <summary> Generates a string representation of the <see cref="Guid"/>. Same as <see cref="Guid.ToString(string, IFormatProvider)"/> </summary>
	/// <param name="format"> A single format specifier that indicates how to format the value of the <see cref="Guid"/> </param>
	/// <param name="provider"> An object that supplies culture-specific formatting information </param>
	public readonly string ToString(string format, IFormatProvider provider) => Guid.ToString(format, provider);

	public readonly bool Equals(GuidSerializable other)
	{
		return m_GuidLow == other.m_GuidLow &&
			m_GuidHigh == other.m_GuidHigh;
	}

	public static bool operator ==(GuidSerializable lhs, GuidSerializable rhs) => lhs.Equals(rhs);
	public static bool operator !=(GuidSerializable lhs, GuidSerializable rhs) => !(lhs == rhs);

	public static implicit operator GuidSerializable(Guid guid) => new GuidSerializable(guid);

	public static implicit operator Guid(GuidSerializable serializableGuid) => serializableGuid.Guid;
}
