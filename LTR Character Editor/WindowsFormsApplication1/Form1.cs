using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CharacterEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Bone_listBox.SelectedIndex = 0;
            Animation_listBox.SelectedIndex = 0;
            KeyFrame_listBox.SelectedIndex = 0;
        }

        private void Bone_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedBone = 0;

            ListBox combo = (ListBox)sender;

            selectedBone = combo.SelectedIndex;
            EditorWindow.selectedBone = selectedBone;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }


    }
}
