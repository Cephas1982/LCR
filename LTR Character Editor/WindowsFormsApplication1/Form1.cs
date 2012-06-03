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
            ListBox listBox = (ListBox)sender;

            EditorWindow.selectedBone = listBox.SelectedIndex;
            Bone_trackBar.Value = (int)EditorWindow.SelectedBoneAngle;


            //update stuff when a bone is selected
            BoneAngle_textBox_AutoUpdate(Bone_trackBar.Value.ToString());
            
        }

        private void BoneAngle_textBox_Update(object sender, KeyPressEventArgs e)
        {
            //todo: write code for when a user inputs value
        }

        //helpers
        private void BoneAngle_textBox_AutoUpdate(string text)
        {
            BoneAngle_textBox.Text = text;
          
        }

        private void Bone_trackBar_SelectedChanged(object sender, EventArgs e)
        {
            TrackBar trackBar = (TrackBar)sender;

            //change angle of the selected bone (will be converted to radians in C_Skeleton)
            EditorWindow.SelectedBoneAngle = trackBar.Value;

            //reload skeleton
            EditorWindow.DumpTree();
            EditorWindow.LoadKeyFrames();

        }

        private void SaveButton_Update(object sender, EventArgs e)
        {
            EditorWindow.DumpTree();
        }

        

    

    }
}
