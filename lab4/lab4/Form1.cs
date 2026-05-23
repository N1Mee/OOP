using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lab4.Core;
using Lab4.Core.Plugins;

namespace lab4
{
    public partial class Form1 : Form
    {
        private readonly MediaItemList mediaList = new MediaItemList();

        public Form1()
        {
            InitializeComponent();
            LoadPluginsFromDefaultFolder();
            InitializeDomainBindings();
        }

        /// <summary>
        /// Looks for a Plugins folder next to the executable and loads every
        /// signed plugin DLL from it. Failures are aggregated and shown so the
        /// student/operator can react instead of silently losing a plugin.
        /// </summary>
        private void LoadPluginsFromDefaultFolder()
        {
            var exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            var pluginsDir = Path.Combine(exeDir, "Plugins");
            var keyPath = Path.Combine(exeDir, "trusted_public_key.xml");
            if (!File.Exists(keyPath))
            {
                MessageBox.Show(this,
                    "Trusted public key not found next to the executable.\n" +
                    "Plugins will not be loaded.",
                    "Plugin loader",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var publicKeyXml = File.ReadAllText(keyPath);
            var loader = new PluginLoader(publicKeyXml);
            var report = new StringBuilder();
            foreach (var entry in loader.LoadFromFolder(pluginsDir))
            {
                report.AppendLine($"{entry.FileName}: {entry.Message}");
                if (!entry.Loaded) continue;

                // Register descriptors and BSON maps for every new item type.
                foreach (var descriptor in entry.Instance.GetTypeDescriptors())
                    MediaItemTypeRegistry.Register(descriptor);
                foreach (var type in entry.Instance.GetItemTypes())
                    MediaItemList.RegisterPluginType(type);
            }

            if (report.Length > 0)
                lblPluginReport.Text = report.ToString().TrimEnd();
        }

        private void InitializeDomainBindings()
        {
            listItems.DataSource = mediaList.Items;
            // Rebuild the combobox source explicitly to include plugin types.
            cmbType.DataSource = MediaItemTypeRegistry.Types.ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem is MediaItemTypeDescriptor descriptor)
            {
                var item = descriptor.Create(txtTitle.Text, txtCreator.Text, (int)nudYear.Value);
                mediaList.Add(item);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                mediaList.Remove(item);
                ClearEditor();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                item.Title = txtTitle.Text;
                item.Creator = txtCreator.Text;
                item.Year = (int)nudYear.Value;
                var index = listItems.SelectedIndex;
                mediaList.Items.ResetItem(index);
                UpdateDetails(item);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "BSON files (*.bson)|*.bson|All files (*.*)|*.*";
                dialog.Title = "Save media library to BSON";
                dialog.DefaultExt = "bson";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    mediaList.SaveToBson(dialog.FileName);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "BSON files (*.bson)|*.bson|All files (*.*)|*.*";
                dialog.Title = "Load media library from BSON";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    mediaList.LoadFromBson(dialog.FileName);
                    ClearEditor();
                }
            }
        }

        private void listItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                txtTitle.Text = item.Title;
                txtCreator.Text = item.Creator;
                nudYear.Value = item.Year >= nudYear.Minimum && item.Year <= nudYear.Maximum
                    ? item.Year : nudYear.Minimum;
                UpdateDetails(item);
            }
            else ClearEditor();
        }

        private void ClearEditor()
        {
            txtTitle.Text = string.Empty;
            txtCreator.Text = string.Empty;
            nudYear.Value = nudYear.Minimum;
            txtDetails.Text = string.Empty;
        }

        private void UpdateDetails(MediaItem item)
        {
            txtDetails.Text = $"{item.TypeName}\r\n{item.GetDetails()}";
        }
    }
}
