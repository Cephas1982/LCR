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

            if(listBox.SelectedItem != null)
                KeyFrame_listBox_Update(listBox.SelectedItem.ToString());
            //AnimationList_Fill();//removed b/c if causes errors on startup (created init button instead)
        }

       private void KeyFrame_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           //Grab data from listbox
            int keyframeStart = KeyFrame_listBox.SelectedIndex;
            int endKeyFrame = KeyFrame_listBox.Items.Count-1;
            bool loop = Loop_checkbox.Checked;

            //when clicked play through all keyframes in keyframe listbox
            //couldn't get public enum working 0 = player, 1= stop, 2 = single frame
            if ( keyframeStart != -1)//make sure a keyframe is selected
                EditorWindow.Play(1, keyframeStart, keyframeStart, endKeyFrame, loop); //Show still of the keyframe
            
        }
       private void KeyFrame_listBox_Update(string animation)//updates keyFrame listbox based on animation parameter
       {
           //Clear the list
           KeyFrame_listBox.Items.Clear();

           //Populate the list
           List<int> keyList = EditorWindow.GetKeysForAnimation(animation);
           foreach (int key in keyList)
               KeyFrame_listBox.Items.Add("KeyFrame " + key);

           //If the list populates select first keyframe
           if(KeyFrame_listBox.Items.Count != 0)
               KeyFrame_listBox.SelectedIndex = 0;
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
            if (KeyFrame_listBox.SelectedIndex != -1)//do NOT update unless an index is selected
            {
                TrackBar trackBar = (TrackBar)sender;

                //Fetch bones to update
                EditorWindow.selectedBoneIndex = KeyFrame_listBox.SelectedIndex;//we have selected bone, but need to make sure it's for the right keyframe    
                EditorWindow.SelectedBoneAngle = trackBar.Value; //change angle of the selected bone (will be converted to radians in C_Skeleton)

                //reload skeleton
                EditorWindow.DumpTree();
                EditorWindow.LoadKeyFrames();

                //update BoneAngle Text box
                BoneAngle_textBox.Text = trackBar.Value.ToString();
            }
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

        private void Play_button_Clicked(object sender, EventArgs e)
        {
            int keyframeStart = KeyFrame_listBox.SelectedIndex;
            int endKeyFrame = KeyFrame_listBox.Items.Count - 1;
            bool loop = Loop_checkbox.Checked;

            //when clicked play through all keyframes in keyframe listbox
            //couldn't get public enum working 0 = player, 1= stop, 2 = single frame

            //plays from selected keyframe to last keyframe of animation
            if (keyframeStart != -1 && keyframeStart != endKeyFrame)//don't try to play something that is unavailable.
                EditorWindow.Play(0, keyframeStart, keyframeStart, endKeyFrame, loop);

            //this keeps player from trying to play past final keyframe
            if (keyframeStart == endKeyFrame && keyframeStart != -1)//dont try to play past this
                EditorWindow.Play(1, keyframeStart, keyframeStart, endKeyFrame, loop);
        }

        private void KeyFrame_Add_Button(object sender, EventArgs e)
        {
            //will copy and add the most recent keyframe to end of keyframe list
            EditorWindow.AddKeyFrame();

            //reload skeleton
            EditorWindow.DumpTree();
            EditorWindow.LoadKeyFrames();

            //update the listbox
            if(Animation_listBox.SelectedIndex != -1)//make sure something is selected
                KeyFrame_listBox_Update(Animation_listBox.SelectedItem.ToString());
        }

        private void AnimationSpeed_trackbar_Update(object sender, EventArgs e)
        {           
            //Update speed
            EditorWindow.i_animationTimer[0] = AnimationSpeed_trackbar.Value/2;
         
            //update AnimationSpeed Text box
            AnimationSpeed_textbox.Text = (AnimationSpeed_trackbar.Value/2).ToString();
        }


        

 

        

    

    }
}
