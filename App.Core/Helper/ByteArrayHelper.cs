using System.Text.Json;

namespace App.Core.Helper;

public static class ByteArrayHelper
{
    public static byte[] ToByteArray<T>(this T obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        return JsonSerializer.SerializeToUtf8Bytes(obj);
    }
    
    public static T FromByteArray<T>(this byte[] byteArray)
    {
        if (byteArray == null || byteArray.Length == 0) throw new ArgumentNullException(nameof(byteArray));
        return JsonSerializer.Deserialize<T>(byteArray);
    }
}