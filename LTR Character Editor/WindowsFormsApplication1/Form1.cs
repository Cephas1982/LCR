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
            //KeyFrame_listBox.SelectedIndex = 0;

        }

        private void Bone_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Highlights a selected bone
            ListBox listBox = (ListBox)sender;
            EditorWindow.selectedBone = listBox.SelectedIndex;
            Bone_trackBar.Value = (int)EditorWindow.SelectedBoneAngle;


            //update stuff when a bone is selected
            BoneAngle_textBox_AutoUpdate(Bone_trackBar.Value.ToString());
            
        }

        private void Animation_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //when clicking on an animation, all keyframes for that animation should load in keyframe box
            ListBox listBox = (ListBox)sender;
            KeyFrame_listBox_Update(listBox.SelectedItem.ToString());
            //AnimationList_Fill();//removed b/c if causes errors on startup (created init button instead)
        }

       private void KeyFrame_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if keyframe is selected - jump to that keyframe
            ListBox listBox = (ListBox)sender;
            EditorWindow.KeyFrame = listBox.SelectedIndex;
            
            
        }
       private void KeyFrame_listBox_Update(string animation)//updates keyFrame listbox based on animation parameter
       {
           //Clear the list
           KeyFrame_listBox.Items.Clear();

           //Populate the list
           List<int> keyList = EditorWindow.GetKeysForAnimation(animation);
           foreach (int key in keyList)
               KeyFrame_listBox.Items.Add("KeyFrame " + key);
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

            //update BoneAngle Text box
            BoneAngle_textBox.Text = trackBar.Value.ToString();


        }

        private void SaveButton_Update(object sender, EventArgs e)
        {
            EditorWindow.DumpTree();

            
        }

        private void AnimationList_Fill()//this function will get all the different animation types  todo: rename to listbox type function
        {            
            
            //empty the list box
            Animation_listBox.Items.Clear();

            //fills the list box
            if(EditorWindow.animationList != null)
                foreach(String animationName in EditorWindow.animationList)
                    this.Animation_listBox.Items.Add(animationName);
        }

        private void InitEditor_button_Clicked(object sender, EventArgs e)
        {
            AnimationList_Fill();
        }


        

 

        

    

    }
}
