using System.Windows.Forms;
using ClassmateApi;
using Lab6.Core.Plugins;

namespace ClassmateAdapterPlugin
{
    /// <summary>
    /// Adapter pattern. Translates the host-side <see cref="IProcessingPlugin"/>
    /// contract into the foreign <see cref="IClassmateTransform"/> contract
    /// provided by a classmate. The pipeline keeps talking to its single
    /// interface; only this class knows about the foreign vocabulary.
    ///
    /// Why an Adapter and not direct integration: rewriting the classmate's
    /// assembly is not an option (it is a binary we received as-is). And
    /// changing our <c>IProcessingPlugin</c> would break our three existing
    /// plugins. The Adapter bridges the gap with a single small class.
    /// </summary>
    public class ClassmateTransformAdapter : IProcessingPlugin
    {
        private readonly IClassmateTransform adaptee;

        public ClassmateTransformAdapter(IClassmateTransform adaptee)
        {
            this.adaptee = adaptee;
        }

        public string Name => adaptee.DisplayName;
        public string Description => "Adapted classmate transform";
        public bool Enabled { get; set; }

        // Method-name translation: Encode -> ProcessOnSave, Decode -> ProcessOnLoad.
        public byte[] ProcessOnSave(byte[] input) => adaptee.Encode(input);
        public byte[] ProcessOnLoad(byte[] input) => adaptee.Decode(input);

        public Control BuildSettingsControl()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
            var enabled = new CheckBox { Text = "Enabled", Top = 8, Left = 8, AutoSize = true, Checked = Enabled };
            enabled.CheckedChanged += (s, e) => Enabled = enabled.Checked;
            var label = new Label { Text = "Provided by classmate library: " + adaptee.DisplayName, Top = 40, Left = 8, AutoSize = true };
            panel.Controls.Add(enabled);
            panel.Controls.Add(label);
            return panel;
        }
    }

    /// <summary>
    /// Plugin entry point picked up by the host loader. Wires the classmate
    /// Base64 transform through the adapter. Multiple adapter instances can
    /// be returned by extending this class to expose more transforms.
    /// </summary>
    public class ClassmateAdapterPluginEntry : IProcessingPlugin
    {
        // The adapter we expose. Defaults to Base64; an admin can swap the
        // adaptee for InvertTransform by editing this single line.
        private readonly ClassmateTransformAdapter adapter = new ClassmateTransformAdapter(new Base64Transform());

        public string Name => adapter.Name;
        public string Description => adapter.Description;
        public bool Enabled { get => adapter.Enabled; set => adapter.Enabled = value; }
        public byte[] ProcessOnSave(byte[] input) => adapter.ProcessOnSave(input);
        public byte[] ProcessOnLoad(byte[] input) => adapter.ProcessOnLoad(input);
        public Control BuildSettingsControl() => adapter.BuildSettingsControl();
    }
}
