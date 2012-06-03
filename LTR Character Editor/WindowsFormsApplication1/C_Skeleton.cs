using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

//new for winforms
using System.Diagnostics;
using System.Windows.Forms;

namespace CharacterEditor
{
    class C_Skeleton : GraphicsDeviceControl
    {
        //private SpriteBatch m_spriteBatch;
        //can assume first bone is the root/head and traverse from there
        List<C_Bone> l_bones = new List<C_Bone>();//todo: might not want to use a list down the road
//        C_Bone root; //pointer to current root bone (kinda like an iterator)
        private const uint MAX_CHILD_COUNT = 100;
        //private Texture2D m_nullTexture;
        private int m_keyFrame = 1;//keyframe 
        public const int MAX_CHAR_BONES = 12;//each keyframe contains 6 bones for now. m_keyFrame*MAX_CHAR_BONES is keyframe entry point

        private VertexPositionColor[] vertices;//holds characters vertices for all keyframes
        VertexPositionColor[] m_drawFrame;//holds current vertices calculated by interpolating keyframes
        BasicEffect basicEffect;
        int[] i_keyFrame;//keyframe index
        float[] i_animationTimer;// index --- load from file

        float time;

        //must use this for timing. cannot rely on xna gameTimer when imbedded into winforms
        Stopwatch timer;
        
        public int selectedBone;//for the editor! if bone is selected it will be stored here
        public int selectedBoneAngle;//stores angle of currently selected bone
        public List<string> animationList;//stores unique animation values

        public C_Skeleton()
        {

        }

        //Skeleton functions
        public void AddChild(int keyFrame, C_Bone newBone)
        {
            if (newBone.Parent != null && newBone.Parent.ChildCount < MAX_CHILD_COUNT) //if space for child
            {
                newBone.Parent.ChildCount += 1;//increment parents child count
                newBone.Position = newBone.Parent.PositionEnd;//set x/y and this is relative to the end of parent bone
            }
           
            //set data  
            if (newBone.Name.Length == 0)                
                newBone.Name = "bone " + l_bones.Count().ToString();

            l_bones.Add(newBone);
        }


        public void DumpTree()
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter("../../../Content1/testing.txt");            
            for (int i = 0; i < l_bones.Count(); i++)
            {
                //name, position, positionEnd, length, angle, childCount
                if(i%MAX_CHAR_BONES == 0)
                    writer.Write("///////// NEW KEY /////////////// \r\n");

                //writer.Write(l_bones[i].AnimatonName);
                writer.Write(l_bones[i].AnimationName + "\r\n");            //animation name
                writer.Write(l_bones[i].KeyFrame.ToString() + "\r\n");      //key frame it belongs to
                

                writer.Write(l_bones[i].Name + "\r\n");                     //Bone Name
                writer.Write(l_bones[i].Position.X.ToString() + "\r\n");
                writer.Write(l_bones[i].Position.Y.ToString() + "\r\n");
                writer.Write(l_bones[i].PositionEnd.X.ToString() + "\r\n");
                writer.Write(l_bones[i].PositionEnd.Y.ToString() + "\r\n");
                writer.Write(l_bones[i].Length.ToString() + "\r\n");
                writer.Write(l_bones[i].Angle.ToString() + "\r\n");
                writer.Write(l_bones[i].ChildCount.ToString() + "\r\n");
                writer.Write(l_bones[i].ParentNumber.ToString() + "\r\n");//parent #, ghetto way to determine which bone it connects to
            }

            writer.Close();
            writer.Dispose();
        }

        protected override void Initialize()
        {
            //create ortho viewport with standard screen coordinates
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
                (0, GraphicsDevice.Viewport.Width,     // left, right
                 GraphicsDevice.Viewport.Height, 0,    // bottom, top
                 0, 1);   //near, far

            timer = Stopwatch.StartNew();
            time = 0;

            //will load assets here b/c Update() is part of Game class ( not used in winforms)
            Load();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        public void Load()
        {
            LoadKeyFrames();//hard coded vertices. Will eventually load from file....gary

            //Load keyframe and animation indices
            i_keyFrame = new int[vertices.Length / (MAX_CHAR_BONES * 2)];//create index array for each keyframe
            for (int i = 0; i < i_keyFrame.Length; i++)
                i_keyFrame[i] = i * MAX_CHAR_BONES * 2;

            i_animationTimer = new float[vertices.Length / (MAX_CHAR_BONES * 2)];
            //will load from file, testing w/hardcoding now
            i_animationTimer[0] = .1f;//going from key0 to key1 should take 2 seconds  TODO: better variable name?

            m_drawFrame = new VertexPositionColor[MAX_CHAR_BONES * 2];//this is the final positon of the model for the current buffer     

        }

        public new void Update()
        {
            Vector3 moveSpeed = new Vector3(0, 0, 0);
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                time = 0;
                m_keyFrame = 1;
            }
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                time = 0;
                m_keyFrame = 0;
            }
            //TODO  ADD a play function of some sort. Will end up removing one of the "if(m_keyframe....)" blocks and adding variables for the i_keyFrame array index

            //if keyframe is 0, lerp to 1
            if (m_keyFrame == 0)
            {
                float lerpTime;
                lerpTime = time * i_animationTimer[0];
                if (time * i_animationTimer[0] > 1)
                    lerpTime = 1;
                for (int i = 0; i < MAX_CHAR_BONES * 2; i++)
                {
                    m_drawFrame[i].Position = Vector3.Lerp(vertices[i + i_keyFrame[0]].Position, vertices[i + i_keyFrame[1]].Position, lerpTime);
                    m_drawFrame[i].Position = Vector3.Transform(m_drawFrame[i].Position, Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2 - 100, 0)));
                }
            }
            //if keyframe is 1, lerp to 0
            if (m_keyFrame == 1)
            {
                float lerpTime;
                lerpTime = time * i_animationTimer[0];
                if (time * i_animationTimer[0] > 1)
                    lerpTime = 1;
                for (int i = 0; i < MAX_CHAR_BONES * 2; i++)
                {
                    m_drawFrame[i].Position = Vector3.Lerp(vertices[i + i_keyFrame[1]].Position, vertices[i + i_keyFrame[0]].Position, lerpTime);
                    m_drawFrame[i].Position = Vector3.Transform(m_drawFrame[i].Position, Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 100, 0)));
                }
            }

        }

        public int SelectedBoneAngle
        {
            get
            {
                if (i_keyFrame == null || l_bones == null)
                    return 0;

                return (int)MathHelper.ToDegrees(l_bones[i_keyFrame[0] + selectedBone].Angle);//TODO!!!!!!!!!!!! check selected keyframe in editor when returning this value
            }
            set
            {
                if (i_keyFrame != null && l_bones != null)
                    l_bones[i_keyFrame[0] + selectedBone].Angle = MathHelper.ToRadians(value);
            }
        }
        public int KeyFrame
        {
            get
            {
                return m_keyFrame;
            }
            set
            {
                m_keyFrame = value;
            }
        }

        public List<int> GetKeysForAnimation(string animationName)
        {

            List<int> keyList = new List<int>();

            for (int i = 0; i < l_bones.Count(); i+= MAX_CHAR_BONES)//this function checks the root bone for each keyframe. If it matches the requestion animation
                if (l_bones[i].AnimationName == animationName)      //the keyframe will be added to the list
                    keyList.Add(l_bones[i].KeyFrame);

            return keyList;
        }


        protected override void Draw()
        {           

            //editor only!! -- when selected
            for(int i = 0; i < m_drawFrame.Length; i++)//set all vertices to black
                m_drawFrame[i].Color = Color.Black;
            
            m_drawFrame[selectedBone * 2].Color = Color.Yellow;//highlight whatever is selected in the editor
            m_drawFrame[selectedBone * 2 + 1].Color = Color.Yellow;

            selectedBoneAngle = (int)MathHelper.ToDegrees(l_bones[i_keyFrame[0] + selectedBone].Angle);//TODO!!!!!!!!!!!! check selected keyframe in editor when returning this value



            time += (float)timer.Elapsed.Milliseconds /(1000 * 60);
            Update();//Udate function will be called here since winforms cannot use xna.Game.Update                
            
    
            //Drawing happens here
            GraphicsDevice.Clear(Color.CornflowerBlue);//background color
            basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                m_drawFrame, 0, MAX_CHAR_BONES);           
        }


        public void LoadKeyFrames()
        {
            
            l_bones.Clear();
            animationList = new List<string>();


            System.IO.StreamReader reader = new System.IO.StreamReader("../../../Content1/testing.txt");
            while(!reader.EndOfStream)
            {
                 C_Bone tempBone = new C_Bone();
                //name, position, positionEnd, length, angle, childCount
                Vector3 tempVector = new Vector3(0,0,0);
                if (l_bones.Count() % MAX_CHAR_BONES == 0)
                    reader.ReadLine();
                
                tempBone.AnimationName = reader.ReadLine();//bone animation name
                tempBone.KeyFrame = int.Parse(reader.ReadLine());

                tempBone.Name = reader.ReadLine();//bone name

                tempVector.X = float.Parse(reader.ReadLine());
                tempVector.Y = float.Parse(reader.ReadLine());
                tempBone.Position = tempVector;

                tempVector.X = float.Parse(reader.ReadLine());
                tempVector.Y = float.Parse(reader.ReadLine());
                tempBone.PositionEnd = tempVector;

                tempBone.Length = float.Parse(reader.ReadLine());
                tempBone.Angle = float.Parse(reader.ReadLine());
                tempBone.ChildCount = uint.Parse(reader.ReadLine());

                tempBone.ParentNumber = int.Parse(reader.ReadLine());
                
                if(tempBone.ParentNumber != -1)
                    tempBone.Position = l_bones[tempBone.ParentNumber].PositionEnd;

                //done parsing, time to add stuff

                //see if animation name is listed. If not add it to list
                bool animationListed = false;
                for (int i = 0; i < animationList.Count; i++)
                    if (animationList[i] == tempBone.AnimationName)
                        animationListed = true;
                if (!animationListed)
                    animationList.Add(tempBone.AnimationName);

                AddChild(0, tempBone);
            }

            reader.Close();
            reader.Dispose();

/* 
            #region HARDCODED

            /////////////////////////////////
            //NEW FRAME --- KEYFRAME 0
            /////////////////////////////////
            //TODO: this is hard coded now, but will eventually read from file
            C_Bone tempBone = new C_Bone();
            tempBone.Name = "head";
            tempBone.Length = 30;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            AddChild(0, tempBone);


            tempBone = new C_Bone();
            tempBone.Name = "right arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(20);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[0];
            tempBone.ParentNumber = 0;
            AddChild(0, tempBone);

            tempBone = new C_Bone(); 
            tempBone.Name = "right forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(45);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[1];
            tempBone.ParentNumber = 1; 
            AddChild(0, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "left arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(160);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[0];
            tempBone.ParentNumber = 0;
            AddChild(0, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "left forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(135);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[3];
            tempBone.ParentNumber = 3;
            AddChild(0, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "torso";//bone 6 or [5]
            tempBone.Length = 100;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[0];
            tempBone.ParentNumber = 0;
            AddChild(0, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "left upper leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(120);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[5];
            tempBone.ParentNumber = 5;
            AddChild(0, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "left lower leg";//bone 8
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(100);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[6];
            tempBone.ParentNumber = 6;
            AddChild(0, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "right upper leg";//bone 9
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(60); 
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[5];
            tempBone.ParentNumber = 5;
            AddChild(0, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "right lower leg";//bone 10
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(80); 
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[8];
            tempBone.ParentNumber = 8;
            AddChild(0, tempBone);


            tempBone = new C_Bone();
            tempBone.Name = "left foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(180); 
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[7];
            tempBone.ParentNumber = 7;
            AddChild(0, tempBone);


            tempBone = new C_Bone();
            tempBone.Name = "right foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(0);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[9];
            tempBone.ParentNumber = 9;
            AddChild(0, tempBone);


            /////////////////////////////////
            //NEW FRAME --- KEYFRAME 1
            /////////////////////////////////


            tempBone = new C_Bone();
            tempBone.Name = "head";
            tempBone.Length = 30;
            tempBone.Angle = MathHelper.ToRadians(45);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            AddChild(12, tempBone);


            tempBone = new C_Bone();
            tempBone.Name = "right arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(-20);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 0];
            tempBone.ParentNumber = 12 + 0;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "right forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(155);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 1];
            tempBone.ParentNumber = 12 + 1;
            AddChild(12, tempBone);
            

            tempBone = new C_Bone();
            tempBone.Name = "left arm";
            tempBone.Length = 60;
            tempBone.Angle = MathHelper.ToRadians(155);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 0];
            tempBone.ParentNumber = 12 + 0;
            AddChild(12, tempBone);


            tempBone = new C_Bone();
            tempBone.Name = "left forearm";
            tempBone.Length = 50;
            tempBone.Angle = MathHelper.ToRadians(130);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 3];
            tempBone.ParentNumber = 12 + 3;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "torso";
            tempBone.Length = 100;
            tempBone.Angle = MathHelper.ToRadians(90);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 0];
            tempBone.ParentNumber = 12 + 0;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "left upper leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(205);
            tempBone.Position = Vector3.Zero;// Only matters for root/head  5 6 5 8 7 9
            tempBone.Parent = l_bones[12 + 5];
            tempBone.ParentNumber = 12 + 5;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "left ower leg";//bone 8
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(180);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 6];
            tempBone.ParentNumber = 12 + 6;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "right upper leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(-45);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 5];
            tempBone.ParentNumber = 12 + 5;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "right lower leg";
            tempBone.Length = 75;
            tempBone.Angle = MathHelper.ToRadians(170);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 8];
            tempBone.ParentNumber = 12 + 8;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "left foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(235);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 7];
            tempBone.ParentNumber = 12 + 7;
            AddChild(12, tempBone);

            tempBone = new C_Bone();
            tempBone.Name = "right foot";
            tempBone.Length = 35;
            tempBone.Angle = MathHelper.ToRadians(235);
            tempBone.Position = Vector3.Zero;// Only matters for root/head
            tempBone.Parent = l_bones[12 + 9];
            tempBone.ParentNumber = 12 + 9;
            AddChild(12, tempBone);
 #endregion          
*/
            vertices = new VertexPositionColor[l_bones.Count() * 2];//2 vertices per bone

            //after reading bone data load vertices
            for (int i = 0; i < l_bones.Count(); i++)
            {
                vertices[i * 2].Position = l_bones[i].Position;
                vertices[i * 2].Color = Color.Black;
                vertices[i * 2 + 1].Position = l_bones[i].PositionEnd;
                vertices[i * 2 + 1].Color = Color.Black;
            }
        }

 
    }
}
