using System;
using System.Windows.Forms;
using FortyOne.AudioSwitcher.HotKeyData;

namespace FortyOne.AudioSwitcher
{
    public class CycleHotKeyForm : Form
    {
        private TextBox txtHotKey;
        private Button btnSave;
        private Button btnClear;
        private Button btnCancel;
        private Label lblInstruction;

        public Keys SelectedKey { get; private set; }
        public Modifiers SelectedModifiers { get; private set; }

        public CycleHotKeyForm(string title, Keys currentKey, Modifiers currentModifiers)
        {
            SelectedKey = currentKey;
            SelectedModifiers = currentModifiers;

            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new System.Drawing.Size(280, 110);

            lblInstruction = new Label
            {
                Text = "Press a key combination:",
                Location = new System.Drawing.Point(12, 12),
                AutoSize = true
            };

            txtHotKey = new TextBox
            {
                Location = new System.Drawing.Point(12, 32),
                Size = new System.Drawing.Size(256, 20),
                ReadOnly = true,
                Text = currentKey == Keys.None ? "" : new HotKey { Key = currentKey, Modifiers = currentModifiers }.HotKeyString
            };
            txtHotKey.KeyUp += TxtHotKey_KeyUp;

            btnSave = new Button
            {
                Text = "Save",
                Location = new System.Drawing.Point(12, 72),
                Size = new System.Drawing.Size(75, 25),
                DialogResult = DialogResult.OK
            };
            btnSave.Click += (s, e) => Close();

            btnClear = new Button
            {
                Text = "Clear",
                Location = new System.Drawing.Point(100, 72),
                Size = new System.Drawing.Size(75, 25)
            };
            btnClear.Click += (s, e) =>
            {
                SelectedKey = Keys.None;
                SelectedModifiers = Modifiers.None;
                txtHotKey.Text = "";
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(193, 72),
                Size = new System.Drawing.Size(75, 25),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += (s, e) => Close();

            Controls.Add(lblInstruction);
            Controls.Add(txtHotKey);
            Controls.Add(btnSave);
            Controls.Add(btnClear);
            Controls.Add(btnCancel);

            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            AudioSwitcher.Instance.DisableHotKeyFunction = true;
            txtHotKey.Focus();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            AudioSwitcher.Instance.DisableHotKeyFunction = false;
        }

        private void TxtHotKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu)
                return;

            SelectedKey = e.KeyCode;
            SelectedModifiers = Modifiers.None;

            if (e.Control) SelectedModifiers |= Modifiers.Control;
            if (e.Alt)     SelectedModifiers |= Modifiers.Alt;
            if (e.Shift)   SelectedModifiers |= Modifiers.Shift;

            txtHotKey.Text = new HotKey { Key = SelectedKey, Modifiers = SelectedModifiers }.HotKeyString;
        }
    }
}