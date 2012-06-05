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
        private SpriteBatch m_spriteBatch;
        ContentManager m_content;
        Texture2D myTexture;

        //can assume first bone is the root/head and traverse from there
        List<C_Bone> l_bones = new List<C_Bone>();//todo: might not want to use a list down the road
//        C_Bone root; //pointer to current root bone (kinda like an iterator)
        private const uint MAX_CHILD_COUNT = 100;
        //private Texture2D m_nullTexture;
        private int m_keyFrame = 0;//keyframe 
        public int m_keyFrame_start, m_keyFrame_end;
        public const int MAX_CHAR_BONES = 12;//each keyframe contains 6 bones for now. m_keyFrame*MAX_CHAR_BONES is keyframe entry point

        private VertexPositionColor[] vertices;//holds characters vertices for all keyframes
        VertexPositionColor[] m_drawFrame;//holds current vertices calculated by interpolating keyframes
        BasicEffect basicEffect;
        private int[] i_keyFrame;//keyframe index
        public float[] i_animationTimer;// index --- load from file

        private float time;
        private float elapsedAnimationTime;
        private float currentTime;
        private bool m_loopAnimation = false;

        //must use this for timing. cannot rely on xna gameTimer when imbedded into winforms
        Stopwatch timer;
        
        public int selectedBone;//for the editor! if bone is selected it will be stored here
        public int selectedBoneAngle;//stores angle of currently selected bone
        public int selectedBoneIndex;//stores which keyframe the bone belongs to
        public float animationSpeed = 3;//how fast animation plays  TODO/////////////// get rid of this and set animation speed for each keyframe
        public List<string> animationList;//stores unique animation values

        public enum e_AnimationState { PLAY, STOP};
        private int m_animationState = (int)e_AnimationState.STOP;

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
            System.IO.StreamWriter writer = new System.IO.StreamWriter("../../Content/KeyframeData.txt");            
            for (int i = 0; i < l_bones.Count(); i++)
            {
                //name, position, positionEnd, length, angle, childCount
                if(i%MAX_CHAR_BONES == 0)
                    writer.Write("///////// NEW KEY /////////////// \r\n");

                //writer.Write(l_bones[i].AnimatonName);
                writer.Write(l_bones[i].AnimationName + "\r\n");            //animation name
                writer.Write((l_bones[i].KeyFrame).ToString() + "\r\n");      //key frame it belongs to
                

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
            //init content
            m_content = new ContentManager(Services, "CEContent");

            //create spritebatch for textures
            m_spriteBatch = new SpriteBatch(GraphicsDevice);

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

            

            i_animationTimer = new float[vertices.Length / (MAX_CHAR_BONES * 2)];//todo needeD????
            //will load from file, testing w/hardcoding now
            i_animationTimer[0] = animationSpeed;//going from key0 to key1 should take 2 seconds  TODO: better variable name? TODO: FIX THIS

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
       
            
            Play(m_animationState, m_keyFrame, m_keyFrame_start, m_keyFrame_end, m_loopAnimation);
  
        }

        public void Play(int state, int keyFrame_current, int keyFrame_start, int keyFrame_end, bool loop)
        {

            //todo reset elapsed time if stop/play state is changed. This should solve animations not starting from origin
            if(m_animationState != state)
                currentTime = timer.ElapsedMilliseconds;//reset the timer if state changes

            m_keyFrame = keyFrame_current;
            m_keyFrame_start = keyFrame_start;
            m_keyFrame_end = keyFrame_end;
            m_animationState = state;
            m_loopAnimation = loop;

            elapsedAnimationTime = timer.ElapsedMilliseconds - currentTime;
            elapsedAnimationTime /= 1000;

            float lerpTime;
            lerpTime = elapsedAnimationTime * i_animationTimer[0];
            if (elapsedAnimationTime * i_animationTimer[0] > 1)
            {
                lerpTime = 1;
                if (m_animationState != (int)e_AnimationState.STOP)//advance keyframe UNLESS only displaying 1 frame
                    m_keyFrame++;//advance to next frame
                currentTime = timer.ElapsedMilliseconds;//reset the timer. this will give a current time to check how much time has passed
                if (m_keyFrame >= m_keyFrame_end && loop == true)
                    m_keyFrame = m_keyFrame_start;
                else if(m_keyFrame == m_keyFrame_end)
                {
                    m_keyFrame = m_keyFrame_end;
                    m_animationState = (int)e_AnimationState.STOP;
                }
            }
            if (m_animationState == (int)e_AnimationState.PLAY)//play state
                for (int i = 0; i < MAX_CHAR_BONES * 2; i++)
                {
                    m_drawFrame[i].Position = Vector3.Lerp(vertices[i + i_keyFrame[m_keyFrame]].Position, vertices[i + i_keyFrame[m_keyFrame + 1]].Position, lerpTime);
                    m_drawFrame[i].Position = Vector3.Transform(m_drawFrame[i].Position, Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 100, 0)));
                }
            if (m_animationState == (int)e_AnimationState.STOP)//stop state
                for (int i = 0; i < MAX_CHAR_BONES * 2; i++)
                {
                    m_drawFrame[i].Position = vertices[i + i_keyFrame[m_keyFrame]].Position;
                    m_drawFrame[i].Position = Vector3.Transform(m_drawFrame[i].Position, Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 100, 0)));
                }
           
        }

        public int SelectedBoneAngle//todo: change to funciton to idiot proof??
        {
            get
            {
                if (i_keyFrame == null || l_bones == null)
                    return 0;

                return (int)MathHelper.ToDegrees(l_bones[(selectedBoneIndex * MAX_CHAR_BONES) + selectedBone].Angle);//TODO!!!!!!!!!!!! check selected keyframe in editor when returning this value
            }
            set
            {
                if (i_keyFrame != null && l_bones != null)
                    l_bones[(selectedBoneIndex * MAX_CHAR_BONES) + selectedBone].Angle = MathHelper.ToRadians(value);
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

        public void AddKeyFrame()
        {
            int start = l_bones.Count - MAX_CHAR_BONES;//start at first bone of the last keyframe
            int end = start + MAX_CHAR_BONES;//end at the last bone of the last keyframe
            

            string name, animationName;// bone name, and the animation bone belongs to
            Vector3 position, positionEnd;//bone start positon
            float length, angle;//bone length and angle (radians)
            uint childCount;//how many child bones does this bone have?
            int parentNumber;
            int keyFrame;//which key this bone belongs to

            //copy data from prior keyframe
            for (int i = start; i < end; i++)
            {
                C_Bone tempBone = new C_Bone();


                //C# copies by reference. There's a clone function, but for now I'll do this so I don't overwrite data ='(
                animationName = l_bones[i].AnimationName;
                keyFrame = l_bones[i].KeyFrame +1;//IMPORTANT because this is the next keyframe
                name = l_bones[i].Name;
                position = l_bones[i].Position;
                positionEnd = l_bones[i].PositionEnd;
                length = l_bones[i].Length;
                angle = l_bones[i].Angle;
                childCount = l_bones[i].ChildCount;
                parentNumber = l_bones[i].ParentNumber;

                tempBone.AnimationName = animationName;
                tempBone.KeyFrame = keyFrame;
                tempBone.Name = name;
                tempBone.Position = position;
                tempBone.PositionEnd = positionEnd;
                tempBone.Length = length;
                tempBone.Angle = angle;
                tempBone.ChildCount = childCount;
                if(parentNumber != -1)
                    tempBone.ParentNumber = parentNumber + MAX_CHAR_BONES;//also very important. Tells which bone it is attached to;

                AddChild(keyFrame, tempBone);
            }
                
        }
        public void LoadKeyFrames()
        {
            
            l_bones.Clear();
            animationList = new List<string>();


            System.IO.StreamReader reader = new System.IO.StreamReader("../../Content/KeyframeData.txt");
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

 
            #region HARDCODED
/*
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
 */
 #endregion          

            vertices = new VertexPositionColor[l_bones.Count() * 2];//2 vertices per bone

            //after reading bone data load vertices
            for (int i = 0; i < l_bones.Count(); i++)
            {
                vertices[i * 2].Position = l_bones[i].Position;
                vertices[i * 2].Color = Color.Black;
                vertices[i * 2 + 1].Position = l_bones[i].PositionEnd;
                vertices[i * 2 + 1].Color = Color.Black;
            }

            //Load keyframe and animation indices
            i_keyFrame = new int[vertices.Length / (MAX_CHAR_BONES * 2)];//create index array for each keyframe
            for (int i = 0; i < i_keyFrame.Length; i++)
                i_keyFrame[i] = i * MAX_CHAR_BONES * 2;
        }

 
    }
}
