using System;
using System.Text;

namespace ClassmateApi
{
    /// <summary>
    /// Simulated foreign API delivered by a fellow student. It does not know
    /// anything about <c>IProcessingPlugin</c>, exposes its own method names
    /// (Encode/Decode), and uses an Apply/Revert vocabulary that does not
    /// map one-to-one onto our pipeline. This intentional mismatch is what
    /// motivates the Adapter pattern in <c>ClassmateAdapterPlugin</c>.
    /// </summary>
    public interface IClassmateTransform
    {
        string DisplayName { get; }
        byte[] Encode(byte[] data);
        byte[] Decode(byte[] data);
    }

    /// <summary>
    /// Reversible Base64 encoding. Naive but useful as a smoke test for the
    /// adapter — output is always ASCII and round-trips identically.
    /// </summary>
    public class Base64Transform : IClassmateTransform
    {
        public string DisplayName => "Classmate / Base64";

        public byte[] Encode(byte[] data) => Encoding.ASCII.GetBytes(Convert.ToBase64String(data));
        public byte[] Decode(byte[] data) => Convert.FromBase64String(Encoding.ASCII.GetString(data));
    }

    /// <summary>
    /// Symmetric byte-inversion ("not" each byte). Used to verify that the
    /// adapter calls Decode on the way back even when Encode is not a no-op.
    /// </summary>
    public class InvertTransform : IClassmateTransform
    {
        public string DisplayName => "Classmate / Invert";

        public byte[] Encode(byte[] data) => Transform(data);
        public byte[] Decode(byte[] data) => Transform(data);

        private static byte[] Transform(byte[] data)
        {
            var output = new byte[data.Length];
            for (int i = 0; i < data.Length; i++) output[i] = (byte)~data[i];
            return output;
        }
    }
}
