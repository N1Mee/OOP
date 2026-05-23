using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Lab5.Core.Plugins;

namespace AesPlugin
{
    /// <summary>
    /// Symmetric AES encryption plugin. Derives a 256-bit key from a
    /// user-supplied passphrase via PBKDF2 so users do not have to type or
    /// paste raw key material. A random IV is prepended to each ciphertext.
    /// </summary>
    public class AesProcessingPlugin : IProcessingPlugin
    {
        public string Name => "AES Encryption";
        public string Description => "Encrypts the stream with AES-256-CBC.";
        public bool Enabled { get; set; }

        /// <summary>Passphrase chosen by the user in settings.</summary>
        public string Passphrase { get; set; } = "change-me";

        /// <summary>Key size in bits; user picks 128 / 192 / 256.</summary>
        public int KeySizeBits { get; set; } = 256;

        // Fixed salt so the same passphrase derives the same key across runs.
        // The 10-point variant is configuration, not full PKI; using a fixed
        // salt keeps the demo simple while still requiring a passphrase.
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("lab5-aes-salt-v1");

        private byte[] DeriveKey()
        {
            using (var kdf = new Rfc2898DeriveBytes(Passphrase ?? string.Empty, Salt, 10000))
                return kdf.GetBytes(KeySizeBits / 8);
        }

        public byte[] ProcessOnSave(byte[] input)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySizeBits;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = DeriveKey();
                aes.GenerateIV();

                using (var output = new MemoryStream())
                {
                    output.Write(aes.IV, 0, aes.IV.Length); // IV is not secret
                    using (var enc = aes.CreateEncryptor())
                    using (var cs = new CryptoStream(output, enc, CryptoStreamMode.Write))
                        cs.Write(input, 0, input.Length);
                    return output.ToArray();
                }
            }
        }

        public byte[] ProcessOnLoad(byte[] input)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySizeBits;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = DeriveKey();

                var iv = new byte[16];
                Buffer.BlockCopy(input, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var inStream = new MemoryStream(input, iv.Length, input.Length - iv.Length))
                using (var dec = aes.CreateDecryptor())
                using (var cs = new CryptoStream(inStream, dec, CryptoStreamMode.Read))
                using (var output = new MemoryStream())
                {
                    cs.CopyTo(output);
                    return output.ToArray();
                }
            }
        }

        public Control BuildSettingsControl()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };

            var enabled = new CheckBox { Text = "Enabled", Top = 8, Left = 8, AutoSize = true, Checked = Enabled };
            enabled.CheckedChanged += (s, e) => Enabled = enabled.Checked;

            var pwLabel = new Label { Text = "Passphrase:", Top = 40, Left = 8, AutoSize = true };
            var pwBox = new TextBox { Top = 60, Left = 8, Width = 250, UseSystemPasswordChar = true, Text = Passphrase };
            pwBox.TextChanged += (s, e) => Passphrase = pwBox.Text;

            var ksLabel = new Label { Text = "Key size (bits):", Top = 95, Left = 8, AutoSize = true };
            var ksCombo = new ComboBox { Top = 115, Left = 8, Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            ksCombo.Items.AddRange(new object[] { 128, 192, 256 });
            ksCombo.SelectedItem = KeySizeBits;
            ksCombo.SelectedIndexChanged += (s, e) => KeySizeBits = (int)ksCombo.SelectedItem;

            panel.Controls.Add(enabled);
            panel.Controls.Add(pwLabel);
            panel.Controls.Add(pwBox);
            panel.Controls.Add(ksLabel);
            panel.Controls.Add(ksCombo);
            return panel;
        }
    }
}
