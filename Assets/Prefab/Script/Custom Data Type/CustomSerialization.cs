using System;
using System.Linq;
using System.Text;

public class CustomSerialization
{
    public int NumberValue { get; private set; } = -1;
    public string StringValue { get; private set; } = string.Empty;

    public void SetValues(int numberValue)
    {
        this.NumberValue = numberValue;
        this.StringValue = string.Empty;
    }
    public void SetValues(string stringValue)
    {
        this.NumberValue = -1;
        this.StringValue = stringValue;
    }
    public void SetValues(int numberValue, string stringValue)
    {
        this.NumberValue = numberValue;
        this.StringValue = stringValue;
    }

    public static byte[] Serialize(object obj)
    {
        CustomSerialization data = (CustomSerialization)obj;

        byte[] numberBytes = BitConverter.GetBytes(data.NumberValue);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(numberBytes);
        }

        byte[] stringBytes = Encoding.ASCII.GetBytes(data.StringValue);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(stringBytes);
        }

        return JoinBytes(numberBytes, stringBytes);
    }

    public static object Deserialize(byte[] bytes)
    {
        CustomSerialization data = new CustomSerialization();

        byte[] numberBytes = new byte[4];

        Array.Copy(bytes, 0, numberBytes, 0, numberBytes.Length);

        if(BitConverter.IsLittleEndian)
        {
            Array.Reverse(numberBytes);
        }

        data.NumberValue = BitConverter.ToInt32(numberBytes, 0);

        byte[] stringBytes = new byte[bytes.Length - 4];

        if(stringBytes.Length > 0)
        {
            Array.Copy(bytes, 4, stringBytes, 0, stringBytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(stringBytes);
            }

            data.StringValue = Encoding.UTF8.GetString(stringBytes);
        }
        else
        {
            data.StringValue = string.Empty;
        }

        return data;
    }

    private static byte[] JoinBytes(params byte[][] arrays)
    {
        byte[] rv = new byte[arrays.Sum(a => a.Length)];

        int offset = 0;

        foreach (byte[] array in arrays)
        {
            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            offset += array.Length;
        }

        return rv;
    }
}
