using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Lab6.Core.Plugins;

namespace GZipPlugin
{
    /// <summary>
    /// Compresses the BSON byte stream with GZip on save and decompresses it
    /// on load. The level is exposed through the settings UI so users can
    /// trade speed vs. size without code changes.
    /// </summary>
    public class GZipProcessingPlugin : IProcessingPlugin
    {
        public string Name => "GZip Compression";
        public string Description => "Compresses the stream with GZip on save.";
        public bool Enabled { get; set; }

        /// <summary>Compression level selected by the user in settings.</summary>
        public CompressionLevel Level { get; set; } = CompressionLevel.Optimal;

        public byte[] ProcessOnSave(byte[] input)
        {
            using (var output = new MemoryStream())
            {
                using (var gz = new GZipStream(output, Level, leaveOpen: true))
                    gz.Write(input, 0, input.Length);
                return output.ToArray();
            }
        }

        public byte[] ProcessOnLoad(byte[] input)
        {
            using (var inStream = new MemoryStream(input))
            using (var gz = new GZipStream(inStream, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gz.CopyTo(output);
                return output.ToArray();
            }
        }

        /// <summary>
        /// Builds the settings panel вЂ” a single combo box for compression
        /// level plus an Enabled checkbox. Keeping the control tiny lets the
        /// host stack many plugins in the same dialog without scrolling.
        /// </summary>
        public Control BuildSettingsControl()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };

            var enabled = new CheckBox { Text = "Enabled", Top = 8, Left = 8, AutoSize = true, Checked = Enabled };
            enabled.CheckedChanged += (s, e) => Enabled = enabled.Checked;

            var label = new Label { Text = "Compression level:", Top = 40, Left = 8, AutoSize = true };
            var combo = new ComboBox { Top = 60, Left = 8, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(new object[] { CompressionLevel.NoCompression, CompressionLevel.Fastest, CompressionLevel.Optimal });
            combo.SelectedItem = Level;
            combo.SelectedIndexChanged += (s, e) => Level = (CompressionLevel)combo.SelectedItem;

            panel.Controls.Add(enabled);
            panel.Controls.Add(label);
            panel.Controls.Add(combo);
            return panel;
        }
    }
}
