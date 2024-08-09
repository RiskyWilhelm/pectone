using System;

public static class GuidExtensions
{
	/// <summary> Decomposes a 16-byte <see cref="Guid"/> into two 8-byte <see cref="ulong"/>s. Recompose with <see cref="GuidUtils.Compose(ulong, ulong)"/> </summary>
	/// <param name="low"> The lower 8 bytes of the guid </param>
	/// <param name="high"> The upper 8 bytes of the guid </param>
	public static void Decompose(this Guid guid, out ulong low, out ulong high)
	{
		var bytes = guid.ToByteArray();
		low = BitConverter.ToUInt64(bytes, 0);
		high = BitConverter.ToUInt64(bytes, 8);
	}
}