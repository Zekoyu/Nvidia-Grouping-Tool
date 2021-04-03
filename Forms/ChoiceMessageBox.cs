using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nvidia_Grouping_Tool {
    public partial class ChoiceMessageBox : Form {
       public enum ButtonClick {
            Left,
            Right
       }

        private ButtonClick buttonClicked;  // Which choice the user has made
        public ButtonClick ButtonClicked { get => buttonClicked; }

        public ChoiceMessageBox(string text, string leftButton, string rightButton) {
            InitializeComponent();
            this.textLabel.Text = text;
            this.leftButton.Text = leftButton;
            this.rightButton.Text = rightButton;
            this.ActiveControl = this.textLabel;    // So that no button is selected by default
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle |= 0x200;  // Disable close button
                return myCp;
            }
        }

        private void OnRightButtonClick(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.buttonClicked = ButtonClick.Right;
            this.Close();
        }

        private void OnLeftButtonClick(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.buttonClicked = ButtonClick.Left;
            this.Close();
        }
    }
}
