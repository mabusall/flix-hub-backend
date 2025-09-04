namespace Tasheer.Caching.Helpers;

public static class Serializer
{
    public static byte[] Serialize<T>(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stream = new MemoryStream();
        serializer.Serialize(stream, obj);

        return stream.ToArray();
    }

    /// <summary>
    /// ReadOnlySpan is better for memory perfomance
    /// in other terms called pinnable array reference
    /// so after method scope is exit, the GC remove the array from
    /// the memory
    /// </summary>
    public static T Deserialize<T>(ReadOnlySpan<byte> arr)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stream = new MemoryStream(arr.ToArray());

        return (T)serializer.Deserialize(stream);
    }
}