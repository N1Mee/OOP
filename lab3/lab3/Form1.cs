using System;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Collection of media items shown in the UI.
        /// </summary>
        private readonly MediaItemList mediaList = new MediaItemList();

        public Form1()
        {
            InitializeComponent();
            InitializeDomainBindings();
        }

        /// <summary>
        /// Initializes bindings between domain model and UI controls.
        /// </summary>
        private void InitializeDomainBindings()
        {
            // Bind list box to media items list.
            listItems.DataSource = mediaList.Items;

            // Populate combo box with available media item types.
            cmbType.DataSource = MediaItemTypeRegistry.Types;
        }

        /// <summary>
        /// Handles click on the Add button and creates a new media item.
        /// </summary>
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            if (cmbType.SelectedItem is MediaItemTypeDescriptor descriptor)
            {
                var item = descriptor.Create(
                    txtTitle.Text,
                    txtCreator.Text,
                    (int)nudYear.Value);

                mediaList.Add(item);
            }
        }

        /// <summary>
        /// Handles click on the Remove button and deletes selected item.
        /// </summary>
        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                mediaList.Remove(item);
                ClearEditor();
            }
        }

        /// <summary>
        /// Handles click on the Update button and updates selected item properties.
        /// </summary>
        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                item.Title = txtTitle.Text;
                item.Creator = txtCreator.Text;
                item.Year = (int)nudYear.Value;

                // Force list box to refresh string representation.
                var index = listItems.SelectedIndex;
                mediaList.Items.ResetItem(index);

                UpdateDetails(item);
            }
        }

        /// <summary>
        /// Handles serialization of the list to a BSON file.
        /// </summary>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "BSON files (*.bson)|*.bson|All files (*.*)|*.*";
                dialog.Title = "Save media library to BSON";
                dialog.DefaultExt = "bson";

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    mediaList.SaveToBson(dialog.FileName);
                }
            }
        }

        /// <summary>
        /// Handles deserialization of the list from a BSON file.
        /// </summary>
        private void btnLoad_Click(object sender, System.EventArgs e)
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

        /// <summary>
        /// Updates editor controls when user selects a different item in the list.
        /// </summary>
        private void listItems_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listItems.SelectedItem is MediaItem item)
            {
                txtTitle.Text = item.Title;
                txtCreator.Text = item.Creator;
                nudYear.Value = item.Year >= nudYear.Minimum && item.Year <= nudYear.Maximum
                    ? item.Year
                    : nudYear.Minimum;

                UpdateDetails(item);
            }
            else
            {
                ClearEditor();
            }
        }

        /// <summary>
        /// Clears editor controls.
        /// </summary>
        private void ClearEditor()
        {
            txtTitle.Text = string.Empty;
            txtCreator.Text = string.Empty;
            nudYear.Value = nudYear.Minimum;
            txtDetails.Text = string.Empty;
        }

        /// <summary>
        /// Shows details of the selected item in the details text box.
        /// </summary>
        private void UpdateDetails(MediaItem item)
        {
            txtDetails.Text = $"{item.TypeName}\r\n{item.GetDetails()}";
        }
    }
}
