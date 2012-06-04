namespace CharacterEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Bone_listBox = new System.Windows.Forms.ListBox();
            this.BoneList_Lable = new System.Windows.Forms.Label();
            this.Animation_listBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.KeyFrame_listBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.KeyFrame_AddButton = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.Bone_trackBar = new System.Windows.Forms.TrackBar();
            this.button7 = new System.Windows.Forms.Button();
            this.BoneAngle_textBox = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.InitEditor_button = new System.Windows.Forms.Button();
            this.Play_button = new System.Windows.Forms.Button();
            this.Loop_checkbox = new System.Windows.Forms.CheckBox();
            this.EditorWindow = new CharacterEditor.C_Skeleton();
            this.AnimationSpeed_trackbar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AnimationSpeed_textbox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Bone_trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationSpeed_trackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // Bone_listBox
            // 
            this.Bone_listBox.FormattingEnabled = true;
            this.Bone_listBox.Items.AddRange(new object[] {
            "head",
            "right arm",
            "right forearm",
            "left arm",
            "left forearm",
            "torso",
            "left upper leg",
            "left lower leg",
            "right upper leg",
            "right lower leg",
            "left foot",
            "right foot"});
            this.Bone_listBox.Location = new System.Drawing.Point(763, 40);
            this.Bone_listBox.Name = "Bone_listBox";
            this.Bone_listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Bone_listBox.Size = new System.Drawing.Size(162, 160);
            this.Bone_listBox.TabIndex = 2;
            this.Bone_listBox.SelectedIndexChanged += new System.EventHandler(this.Bone_listBox_SelectedIndexChanged);
            // 
            // BoneList_Lable
            // 
            this.BoneList_Lable.AutoSize = true;
            this.BoneList_Lable.Location = new System.Drawing.Point(763, 10);
            this.BoneList_Lable.Name = "BoneList_Lable";
            this.BoneList_Lable.Size = new System.Drawing.Size(51, 13);
            this.BoneList_Lable.TabIndex = 3;
            this.BoneList_Lable.Text = "Bone List";
            // 
            // Animation_listBox
            // 
            this.Animation_listBox.FormattingEnabled = true;
            this.Animation_listBox.Items.AddRange(new object[] {
            "Idle -semi working",
            "Walk - not yet",
            "Run - not yet",
            "Murder - not yet"});
            this.Animation_listBox.Location = new System.Drawing.Point(996, 40);
            this.Animation_listBox.Name = "Animation_listBox";
            this.Animation_listBox.Size = new System.Drawing.Size(175, 160);
            this.Animation_listBox.TabIndex = 4;
            this.Animation_listBox.SelectedIndexChanged += new System.EventHandler(this.Animation_listBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(993, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Animation List";
            // 
            // KeyFrame_listBox
            // 
            this.KeyFrame_listBox.FormattingEnabled = true;
            this.KeyFrame_listBox.Items.AddRange(new object[] {
            "TODO: load from file",
            "Frame_1",
            "Frame_2",
            "Frame_3",
            "Frame_4",
            "etc"});
            this.KeyFrame_listBox.Location = new System.Drawing.Point(1207, 40);
            this.KeyFrame_listBox.Name = "KeyFrame_listBox";
            this.KeyFrame_listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.KeyFrame_listBox.Size = new System.Drawing.Size(175, 160);
            this.KeyFrame_listBox.TabIndex = 6;
            this.KeyFrame_listBox.SelectedIndexChanged += new System.EventHandler(this.KeyFrame_listBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1204, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Key Frame List";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(996, 207);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Add Animation";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(996, 236);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(175, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Edit Animation";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(996, 265);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(175, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Delete Animation";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // KeyFrame_AddButton
            // 
            this.KeyFrame_AddButton.Location = new System.Drawing.Point(1207, 265);
            this.KeyFrame_AddButton.Name = "KeyFrame_AddButton";
            this.KeyFrame_AddButton.Size = new System.Drawing.Size(175, 23);
            this.KeyFrame_AddButton.TabIndex = 13;
            this.KeyFrame_AddButton.Text = "Add Key Frame";
            this.KeyFrame_AddButton.UseVisualStyleBackColor = true;
            this.KeyFrame_AddButton.Click += new System.EventHandler(this.KeyFrame_Add_Button);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1207, 236);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(175, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "Move DOWN";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1207, 207);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(175, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "Move UP";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // Bone_trackBar
            // 
            this.Bone_trackBar.LargeChange = 10;
            this.Bone_trackBar.Location = new System.Drawing.Point(763, 207);
            this.Bone_trackBar.Maximum = 360;
            this.Bone_trackBar.Minimum = -360;
            this.Bone_trackBar.Name = "Bone_trackBar";
            this.Bone_trackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Bone_trackBar.Size = new System.Drawing.Size(45, 395);
            this.Bone_trackBar.TabIndex = 14;
            this.Bone_trackBar.Scroll += new System.EventHandler(this.Bone_trackBar_SelectedChanged);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1207, 294);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(175, 23);
            this.button7.TabIndex = 15;
            this.button7.Text = "Delete Key Frame";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // BoneAngle_textBox
            // 
            this.BoneAngle_textBox.Location = new System.Drawing.Point(814, 207);
            this.BoneAngle_textBox.Name = "BoneAngle_textBox";
            this.BoneAngle_textBox.Size = new System.Drawing.Size(55, 20);
            this.BoneAngle_textBox.TabIndex = 16;
            this.BoneAngle_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BoneAngle_textBox_Update);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(1207, 323);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(175, 23);
            this.SaveButton.TabIndex = 18;
            this.SaveButton.Text = "Save Key Frame";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Update);
            // 
            // InitEditor_button
            // 
            this.InitEditor_button.Location = new System.Drawing.Point(1318, 591);
            this.InitEditor_button.Name = "InitEditor_button";
            this.InitEditor_button.Size = new System.Drawing.Size(75, 23);
            this.InitEditor_button.TabIndex = 19;
            this.InitEditor_button.Text = "InitEditor";
            this.InitEditor_button.UseVisualStyleBackColor = true;
            this.InitEditor_button.Click += new System.EventHandler(this.InitEditor_button_Clicked);
            // 
            // Play_button
            // 
            this.Play_button.Location = new System.Drawing.Point(996, 303);
            this.Play_button.Name = "Play_button";
            this.Play_button.Size = new System.Drawing.Size(75, 23);
            this.Play_button.TabIndex = 20;
            this.Play_button.Text = "Play";
            this.Play_button.UseVisualStyleBackColor = true;
            this.Play_button.Click += new System.EventHandler(this.Play_button_Clicked);
            // 
            // Loop_checkbox
            // 
            this.Loop_checkbox.AutoSize = true;
            this.Loop_checkbox.Location = new System.Drawing.Point(1093, 309);
            this.Loop_checkbox.Name = "Loop_checkbox";
            this.Loop_checkbox.Size = new System.Drawing.Size(50, 17);
            this.Loop_checkbox.TabIndex = 21;
            this.Loop_checkbox.Text = "Loop";
            this.Loop_checkbox.UseVisualStyleBackColor = true;
            // 
            // EditorWindow
            // 
            this.EditorWindow.KeyFrame = 0;
            this.EditorWindow.Location = new System.Drawing.Point(1, 1);
            this.EditorWindow.Name = "EditorWindow";
            this.EditorWindow.SelectedBoneAngle = 0;
            this.EditorWindow.Size = new System.Drawing.Size(756, 613);
            this.EditorWindow.TabIndex = 17;
            // 
            // AnimationSpeed_trackbar
            // 
            this.AnimationSpeed_trackbar.LargeChange = 1;
            this.AnimationSpeed_trackbar.Location = new System.Drawing.Point(1207, 384);
            this.AnimationSpeed_trackbar.Name = "AnimationSpeed_trackbar";
            this.AnimationSpeed_trackbar.Size = new System.Drawing.Size(143, 45);
            this.AnimationSpeed_trackbar.TabIndex = 22;
            this.AnimationSpeed_trackbar.Scroll += new System.EventHandler(this.AnimationSpeed_trackbar_Update);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1225, 416);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Animation Speed(WIP)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1027, 575);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(355, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "PRESS INIT BUTTON FIRST TO START - IM ME FOR INSTRUCTIONS";
            // 
            // AnimationSpeed_textbox
            // 
            this.AnimationSpeed_textbox.Location = new System.Drawing.Point(1346, 384);
            this.AnimationSpeed_textbox.Name = "AnimationSpeed_textbox";
            this.AnimationSpeed_textbox.Size = new System.Drawing.Size(36, 20);
            this.AnimationSpeed_textbox.TabIndex = 25;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1394, 614);
            this.Controls.Add(this.AnimationSpeed_textbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AnimationSpeed_trackbar);
            this.Controls.Add(this.Loop_checkbox);
            this.Controls.Add(this.Play_button);
            this.Controls.Add(this.InitEditor_button);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.EditorWindow);
            this.Controls.Add(this.BoneAngle_textBox);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.Bone_trackBar);
            this.Controls.Add(this.KeyFrame_AddButton);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.KeyFrame_listBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Animation_listBox);
            this.Controls.Add(this.BoneList_Lable);
            this.Controls.Add(this.Bone_listBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Bone_trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnimationSpeed_trackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C_Skeleton EditorWindow;//editor window
        private System.Windows.Forms.Label BoneList_Lable;
        private System.Windows.Forms.ListBox Animation_listBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox KeyFrame_listBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button KeyFrame_AddButton;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox BoneAngle_textBox;
        private System.Windows.Forms.ListBox Bone_listBox;
        private System.Windows.Forms.TrackBar Bone_trackBar;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button InitEditor_button;
        private System.Windows.Forms.Button Play_button;
        private System.Windows.Forms.CheckBox Loop_checkbox;
        private System.Windows.Forms.TrackBar AnimationSpeed_trackbar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AnimationSpeed_textbox;
        
    }
}

