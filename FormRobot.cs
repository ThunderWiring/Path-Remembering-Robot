using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RobotNamespace;
using System.Threading;

namespace RobotNamespace
{
    public partial class FormRobot : Form
    {
        #region Variables
        private RobotHandler robot;
        private Point robotInitialPosition;
        private int index, tmpInterval;
        private int subPathIndex = 0;
        private bool bAddPath = false;
        private List<int> subPathSerialIndex = new List<int>(); //containes the positioning of sub-pathes in "robot.mapList"
        private List<ClsStepNode> pathPatternStore = new List<ClsStepNode>(); //storing the wanted path from the mapList.
        private string selected;
        private string current;
        private string selectedPathName;
        private int selectedPathIndex , selectedPositionIndex ;
        private int currentPathIndex , currentPositionIndex;
        private string currentPathName;
        private Interface control = new Interface();
       
        #endregion
        #region constructor
        public FormRobot()
        {
            InitializeComponent();
            
            // initialize robot
            try
            {
                robot = new RobotHandler();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            // set robot initial position
            this.robotInitialPosition = pictureBoxRobot.Location;
        }// public FormRobot()
        #endregion // constructor
        #region Event
        private void FormRobot_KeyDown(object sender, KeyPressEventArgs e)
        {
            keyDown(e);
        }// FormRobot_KeyDown
        private void FormRobot_KeyUp(object sender, KeyEventArgs e)
        {
            keyUp(e);
        }//FormRobot_KeyUp

        private void buttonRecord_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Manual";
            labelLight.ImageIndex = imageList1.Images.IndexOfKey("redLight");
            timerBlinkLight.Start();

            // enable/disable key perview
            this.KeyPreview = !this.KeyPreview;
            // change label index
            labelTime.Text = "0";
            if (buttonRecord.Text == "Record")
            {
                buttonRecord.Text = "Stop";
                if (bAddPath == false)
                {
                    treeViewPath.Nodes.Add("OriginalPath");
                    //the initial position is Stop
                    robot.mapList.Add(new ClsStepNode(Steps.Stop));
                }
                else
                    robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Stop));
                // start the timer
                timerSteps.Start();
                //change icon
                buttonRecord.ImageIndex = imageList1.Images.IndexOfKey("record");

            }
            else
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                buttonRecord.Text = "Record";
                if (robot.mapList.Count > 1)
                {
                    // enable playing after record
                    buttonPlay.Enabled = true;
                    buttonReverse.Enabled = true;
                }
                // stop the timer
                timerSteps.Stop();
                if (bAddPath == false)
                {
                    // print "stop" + time after finishing the recording.
                    listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                    treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                }
                else
                {
                    // print "stop" + time after finishing the recording.
                    listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                    treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                }
                //change icon
                buttonRecord.ImageIndex = imageList1.Images.IndexOfKey("stop");
                // disable additional record
                buttonRecord.Enabled = false;
            }
            control.litLED(1, 0);
            control.litLED(2, 0);
        }
        private void timerSteps_Tick(object sender, EventArgs e)
        {
            Point position = pictureBoxRobot.Location;
            if (bAddPath == false)
            {
                switch (robot.mapList.Last().Step)
                {
                    case Steps.Forward:
                        control.forward();
                        pictureBoxRobot.Location = new Point(position.X, position.Y - 1);
                        break;
                    case Steps.BackWard:
                        control.backward();
                        pictureBoxRobot.Location = new Point(position.X, position.Y + 1);
                        break;
                    case Steps.Left:
                        control.turnLeft();
                        pictureBoxRobot.Location = new Point(position.X - 1, position.Y);
                        break;
                    case Steps.Right:
                        control.turnRight();
                        pictureBoxRobot.Location = new Point(position.X + 1, position.Y);
                        break;
                }
                robot.mapList.Last().Interval += 1;
                labelTime.Text = robot.mapList.Last().Interval.ToString();
                textBoxCurrentPath.Text = treeViewPath.Nodes[0].Text + " " + ((robot.mapList.Count()) - 1).ToString();
            }
            else
            {
                switch (robot.mapList[index + 1].SubMapList.Last().Step)
                {
                    case Steps.Forward:
                        control.forward();
                        pictureBoxRobot.Location = new Point(position.X, position.Y - 1);
                        break;
                    case Steps.BackWard:
                        control.backward();
                        pictureBoxRobot.Location = new Point(position.X, position.Y + 1);
                        break;
                    case Steps.Left:
                        control.turnLeft();
                        pictureBoxRobot.Location = new Point(position.X - 1, position.Y);
                        break;
                    case Steps.Right:
                        control.turnRight();
                        pictureBoxRobot.Location = new Point(position.X + 1, position.Y);
                        break;
                }
                robot.mapList[index + 1].SubMapList.Last().Interval += 1;
                labelTime.Text = robot.mapList[index + 1].SubMapList.Last().Interval.ToString();
            }
        }
            
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            control.litLED(1, 0);
            control.litLED(2, 0);
            labelStatus.Text = "Status: Tracing";
            labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
            timerBlinkLight.Start();
            pictureBoxRobot.Location = this.robotInitialPosition;
            index = 0;
            tmpInterval = -1;
            timerPlay.Start();
        }
        private void timerPlay_Tick(object sender, EventArgs e)
        {
            trace(robot.mapList);
        }

        private void buttonReverse_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Tracing";
            labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
            reverse();
        }
        private void buttonAddPath_Click(object sender, EventArgs e)
        {
            timerBlinkLight.Stop();
            labelStatus.Text = "Status: Stopped";
            labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");

            control.stop();
            bAddPath = true;
            buttonAddPath.Enabled = false;
            buttonRecord.Enabled = true;
            buttonPlay.Enabled = false;
            buttonReverse.Enabled = false;
            timerPlay.Stop();
            timerReverse.Stop();
            subPathIndex += 1;
            treeViewPath.Nodes.Add("SubPath" + " " + subPathIndex.ToString());
            //**********************************************************
            ClsStepNode newNode = new ClsStepNode(robot.mapList[index].Step);
            newNode.Interval = robot.mapList[index].Interval - tmpInterval;
            robot.mapList.Insert(index + 1, newNode);
            robot.mapList[index].Interval = tmpInterval;
            robot.mapList[index + 1].SubMapList = new List<ClsStepNode>();
            subPathSerialIndex.Add(index + 1);
        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            bAddPath = false;
            buttonRecord.Enabled = true;
            buttonPlay.Enabled = false;
            buttonReverse.Enabled = false;
            timerPlay.Stop();
            timerReverse.Stop();
            pictureBoxRobot.Location = this.robotInitialPosition;
            listBoxSteps.Items.Clear();
            treeViewPath.Nodes.Clear();
            robot.mapList.Clear();
            textBoxCurrentPath.Clear();
            textBoxSelectedPath.Clear();
        }

        private void treeViewPath_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            timerPlay.Enabled = false;
            timerReverse.Enabled = false;
            timerSteps.Enabled = false;
            control.stop();
            //----------------------------------------
            int indexOfSelectedSiblingNode = treeViewPath.SelectedNode.Index;
            int indexOfSelectedParentNode = treeViewPath.SelectedNode.Parent.Index;
            
            textBoxSelectedPath.Text = treeViewPath.Nodes[indexOfSelectedParentNode].Text + " " + treeViewPath.Nodes[indexOfSelectedParentNode].Nodes[indexOfSelectedSiblingNode].Index.ToString();
            
            analyzeString();

            traceSelectedPattern(selectedPathName, selectedPathIndex, selectedPositionIndex,
                                 currentPathIndex, currentPositionIndex, currentPathName);

            labelStatus.Text = "Status: Tracing";
            labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
            timerBlinkLight.Start();
        }
       
        #endregion //Event
        #region Functions
        #region initializeOriginalPathTrace 
        /// <summary>
        /// for tracing a part from the Original-Path  
        /// </summary>
        private void initializeOriginalPathTrace()
        {
            if (currentPositionIndex < selectedPositionIndex)
            {
                index = 0;
                if (robot.mapList[selectedPositionIndex].Step == Steps.Stop && robot.mapList[currentPositionIndex].Step == Steps.Stop)
                {
                    tmpInterval = -1;
                    for (int i = currentPositionIndex; i >= 0; i--)
                    {
                        pathPatternStore.Insert(0, robot.mapList[i]);
                    }
                    timerOriginalStraight.Enabled = true;
                }
                else
                {
                    tmpInterval = -1;
                    pathPatternStore = robot.mapList.GetRange(currentPositionIndex + 1, selectedPositionIndex - currentPositionIndex + 1);
                    timerOriginalStraight.Enabled = true;
                }
            }
            if (currentPositionIndex > selectedPositionIndex)
            {
                if (robot.mapList[currentPositionIndex].Step == Steps.Stop)
                {
                    pathPatternStore = robot.mapList.GetRange(selectedPositionIndex, currentPositionIndex - selectedPositionIndex + 1);
                    index = currentPositionIndex - selectedPositionIndex;
                    tmpInterval = -1;
                    timerOriginalReverse.Start();
                }
                else
                {
                    pathPatternStore = robot.mapList.GetRange(selectedPositionIndex, (currentPositionIndex + 1) - selectedPositionIndex + 1);
                    index = currentPositionIndex - selectedPositionIndex + 1;
                    tmpInterval = -1;
                    timerOriginalReverse.Start();
                }
            }
        }
        #endregion//initializeOriginalPathTrace
        #region initializeSameSubPathTrace
        private void initializeSameSubPathTrace()
        {
            labelStatus.Text = "Tracing";
            labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
            if (currentPositionIndex > selectedPositionIndex)
            {
                if (robot.mapList[subPathSerialIndex[selectedPathIndex - 1]].SubMapList[currentPositionIndex].Step == Steps.Stop)
                {
                    pathPatternStore = robot.mapList[subPathSerialIndex[selectedPathIndex - 1]].SubMapList.GetRange(selectedPositionIndex, currentPositionIndex - selectedPositionIndex + 1);
                    index = currentPositionIndex - selectedPositionIndex;
                    tmpInterval = -1;
                    timerOriginalReverse.Start();
                    
                }
                else
                {
                    pathPatternStore = robot.mapList[subPathSerialIndex[selectedPathIndex - 1]].SubMapList.GetRange(selectedPositionIndex, (currentPositionIndex + 1) - selectedPositionIndex + 1);
                    index = currentPositionIndex - selectedPositionIndex + 1;
                    tmpInterval = -1;
                    timerOriginalReverse.Start();
                }
            }
            if (currentPositionIndex < selectedPositionIndex)
            {
                index = 0;
                if (robot.mapList[subPathSerialIndex[selectedPathIndex - 1]].SubMapList[currentPositionIndex].Step == Steps.Stop)
                {
                    tmpInterval = -1;
                    pathPatternStore = robot.mapList[subPathSerialIndex[selectedPathIndex - 1]].SubMapList.GetRange(currentPositionIndex, selectedPositionIndex - currentPositionIndex + 1);
                    timerOriginalStraight.Enabled = true;
                }
                else
                {
                    tmpInterval = -1;
                    pathPatternStore = robot.mapList[subPathSerialIndex[selectedPathIndex - 1]].SubMapList.GetRange(currentPositionIndex + 1, selectedPositionIndex - currentPositionIndex + 1);
                    timerOriginalStraight.Enabled = true;
                }
            }
        }
        #endregion //initializeSameSubPathTrace
        #region traceSelectedPattern
        private void traceSelectedPattern(string selectedPathName, int selectedPathIndex, int selectedPositionIndex,
                                          int currentPathIndex, int currentPositionIndex, string currentPathName)
        {
            int StepCount = 1;
            for (int i = 3; i < robot.mapList.Count(); i += 2)
            {
                if (robot.mapList[i].Step != robot.mapList[1].Step)
                    StepCount++;
            }

            if (currentPathName == "OriginalPath" && selectedPathName == "OriginalPath")
            {
                initializeOriginalPathTrace();
                
            }
            else if (currentPathName == "SubPath" && selectedPathName == "SubPath")
            {
                if (selectedPathIndex == currentPathIndex)
                {
                    initializeSameSubPathTrace();
                    
                }
                else if (selectedPathIndex != currentPathIndex)
                {
                    if (subPathSerialIndex[currentPathIndex - 1] > subPathSerialIndex[selectedPathIndex - 1])
                    {
                        #region  BeginLine "after" FinishLine
                        if (robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[currentPositionIndex].Step == Steps.Stop)
                        {
                            pathPatternStore = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList.GetRange(selectedPositionIndex, currentPositionIndex - selectedPositionIndex + 1);
                        }
                        else
                        {
                            pathPatternStore = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList.GetRange(selectedPositionIndex, (currentPositionIndex + 1) - selectedPositionIndex + 1);
                        }

                        currentPositionIndex = subPathSerialIndex[currentPathIndex - 1];
                        selectedPositionIndex = subPathSerialIndex[selectedPathIndex - 1];
                        //storing the part from OriginalPath to the List.
                        if (StepCount == 1)
                        {
                            for (int i = currentPositionIndex; i >= selectedPositionIndex + 1; i--)
                            {
                                pathPatternStore.Insert(0, robot.mapList[i]);
                            }
                        }
                        else
                        {
                            for (int i = robot.mapList.Count() - 1; i >= currentPositionIndex; i--)
                            {
                                pathPatternStore.Insert(0, robot.mapList[i]);
                            }
                            for (int i = 0; i < selectedPositionIndex; i++)
                            {
                                pathPatternStore.Insert(0, robot.mapList[i]);
                            }
                        }
                        for (int i = 0; i < robot.mapList[selectedPositionIndex].SubMapList.Count(); i++)
                        {
                            switch ((Steps)robot.mapList[selectedPositionIndex].SubMapList[i].Step)
                            {
                                case Steps.Forward:
                                    pathPatternStore.Insert(0, new ClsStepNode(Steps.BackWard));
                                    pathPatternStore[0].Interval = robot.mapList[selectedPositionIndex].SubMapList[i].Interval;
                                    break;
                                case Steps.BackWard:
                                    pathPatternStore.Insert(0, new ClsStepNode(Steps.Forward));
                                    pathPatternStore[0].Interval = robot.mapList[selectedPositionIndex].SubMapList[i].Interval;
                                    break;
                                case Steps.Left:
                                    pathPatternStore.Insert(0, new ClsStepNode(Steps.Right));
                                    pathPatternStore[0].Interval = robot.mapList[selectedPositionIndex].SubMapList[i].Interval;
                                    break;
                                case Steps.Right:
                                    pathPatternStore.Insert(0, new ClsStepNode(Steps.Left));
                                    pathPatternStore[0].Interval = robot.mapList[selectedPositionIndex].SubMapList[i].Interval;
                                    break;
                                case Steps.Stop:
                                    pathPatternStore.Insert(0, new ClsStepNode(Steps.Stop));
                                    pathPatternStore[0].Interval = robot.mapList[selectedPositionIndex].SubMapList[i].Interval;
                                    break;
                            }
                        }
                        timerOriginalReverse.Start();
                        index = pathPatternStore.Count() - 1;
                        tmpInterval = -1;
                        #endregion//BeginLine "after" FinishLine
                        
                    }
                    else if (subPathSerialIndex[currentPathIndex - 1] < subPathSerialIndex[selectedPathIndex - 1])
                    {
                        labelStatus.Text = "Tracing";
                        labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
                        #region BeginLine "before" FinishLine

                        for (int i = 0; i < robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList.Count(); i++)
                        {
                            switch ((Steps)robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Step)
                            {
                                case Steps.BackWard:
                                    pathPatternStore.Add(new ClsStepNode(Steps.Forward));
                                    pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                    break;
                                case Steps.Forward:
                                    pathPatternStore.Add(new ClsStepNode(Steps.BackWard));
                                    pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                    break;
                                case Steps.Left:
                                    pathPatternStore.Add(new ClsStepNode(Steps.Right));
                                    pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                    break;
                                case Steps.Right:
                                    pathPatternStore.Add(new ClsStepNode(Steps.Left));
                                    pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                    break;
                                case Steps.Stop:
                                    pathPatternStore.Add(new ClsStepNode(Steps.Stop));
                                    pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                    break;
                            }
                        }
                        selectedPositionIndex = subPathSerialIndex[selectedPathIndex - 1] + 1;
                        currentPositionIndex = subPathSerialIndex[currentPathIndex - 1];
                        //storing the part from OriginalPath to the List.
                        for (int i = 0; i < currentPositionIndex; i++)
                        {
                            pathPatternStore.Insert(0, robot.mapList[i]);
                        }
                        for (int i = robot.mapList.Count() - 1; i >= selectedPositionIndex; i--)
                        {
                            pathPatternStore.Insert(0, robot.mapList[i]);
                        }

                        //pathPatternStore.InsertRange(pathPatternStore.Count(), robot.mapList.GetRange(currentPositionIndex, selectedPositionIndex - currentPositionIndex + 1));

                        for (int i = robot.mapList[selectedPositionIndex].SubMapList.Count() - 1; i >= 0; i--)
                        {
                            pathPatternStore.Insert(robot.mapList[selectedPositionIndex].SubMapList.Count() - 1 - i, robot.mapList[selectedPositionIndex].SubMapList[i]);
                        }

                        pathPatternStore.Reverse();
                        index = 0;
                        tmpInterval = -1;
                        timerOriginalStraight.Start();
                        #endregion // BeginLine "before" FinishLine
                    }
                }
            }
            else if (currentPathName == "SubPath" && selectedPathName == "OriginalPath")
            {
                if (subPathSerialIndex[currentPathIndex - 1] > selectedPositionIndex)
                {
                    labelStatus.Text = "Tracing";
                    labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
                    #region beginingLine "after" finishLine
                    currentPositionIndex = subPathSerialIndex[currentPathIndex - 1];
                    pathPatternStore = robot.mapList[currentPositionIndex].SubMapList;
                    
                    //storing the part from OriginalPath to the List.
                    for (int i = currentPositionIndex ; i >= 0; i--)
                    {
                        if (i == currentPositionIndex - 1)
                            i -= 1;
                        else
                            pathPatternStore.Insert(0, robot.mapList[i]);
                    }
                    timerOriginalReverse.Start();
                    index = pathPatternStore.Count() - 1;
                    tmpInterval = -1;
                    #endregion //beginingLine "before" finishLine
                }
                else
                {
                    #region beginingLine "before" finishLine
                    for (int i = 0; i < robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList.Count(); i++)
                    {
                        switch ((Steps)robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Step)
                        {
                            case Steps.BackWard:
                                pathPatternStore.Add(new ClsStepNode(Steps.Forward));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Forward:
                                pathPatternStore.Add(new ClsStepNode(Steps.BackWard));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Left:
                                pathPatternStore.Add(new ClsStepNode(Steps.Right));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Right:
                                pathPatternStore.Add(new ClsStepNode(Steps.Left));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Stop:
                                pathPatternStore.Add(new ClsStepNode(Steps.Stop));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                        }
                    }
                    currentPositionIndex = subPathSerialIndex[currentPathIndex - 1];
                    //storing the part from OriginalPath to the List.
                    for (int i = 0; i < currentPositionIndex - 1; i++)
                    {
                        pathPatternStore.Insert(0, robot.mapList[i]);
                    }
                    
                    for (int i = robot.mapList[selectedPositionIndex].SubMapList.Count() - 1; i >= 0; i--)
                    {
                        pathPatternStore.Insert(robot.mapList[selectedPositionIndex].SubMapList.Count() - 1 - i, robot.mapList[selectedPositionIndex].SubMapList[i]);
                    }
                    pathPatternStore.Reverse();

                    index = 0;
                    tmpInterval = -1;
                    timerOriginalStraight.Start();
                    #endregion //beginingLine "after" finishLine   
                }
            }
            else if (currentPathName == "OriginalPath" && selectedPathName == "SubPath")
            {
                if (subPathSerialIndex[selectedPathIndex - 1] > currentPositionIndex)
                {
                    labelStatus.Text = "Tracing";
                    labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
                    #region beginingLine "before" finishLine
                    currentPositionIndex = subPathSerialIndex[currentPathIndex - 1];
                    pathPatternStore = robot.mapList[currentPositionIndex].SubMapList;

                    //storing the part from OriginalPath to the List.
                    for (int i = currentPositionIndex; i >= 0; i--)
                    {
                        if (i == currentPositionIndex - 1)
                            i -= 1;
                        else
                            pathPatternStore.Insert(0, robot.mapList[i]);
                    }

                    timerOriginalReverse.Start();
                    index = pathPatternStore.Count() - 1;
                    tmpInterval = -1;
                    #endregion //beginingLine "before" finishLine
                    
                }
                else if (subPathSerialIndex[selectedPathIndex - 1] < currentPositionIndex)
                {
                    labelStatus.Text = "Tracing";
                    labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
                    #region beginingLine "after" finishLine

                    for (int i = 0; i < robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList.Count(); i++)
                    {
                        switch ((Steps)robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Step)
                        {
                            case Steps.BackWard:
                                pathPatternStore.Add(new ClsStepNode(Steps.Forward));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Forward:
                                pathPatternStore.Add(new ClsStepNode(Steps.BackWard));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Left:
                                pathPatternStore.Add(new ClsStepNode(Steps.Right));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Right:
                                pathPatternStore.Add(new ClsStepNode(Steps.Left));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                            case Steps.Stop:
                                pathPatternStore.Add(new ClsStepNode(Steps.Stop));
                                pathPatternStore.Last().Interval = robot.mapList[subPathSerialIndex[currentPathIndex - 1]].SubMapList[i].Interval;
                                break;
                        }
                    }
                    selectedPositionIndex = subPathSerialIndex[selectedPathIndex - 1] + 1;
                    currentPositionIndex = subPathSerialIndex[currentPathIndex - 1];
                    //storing the part from OriginalPath to the List.
                    for (int i = 0; i < currentPositionIndex; i++)
                    {
                        pathPatternStore.Insert(0, robot.mapList[i]);
                    }
                    for (int i = robot.mapList.Count() - 1; i >= selectedPositionIndex; i--)
                    {
                        pathPatternStore.Insert(0, robot.mapList[i]);
                    }


                    //pathPatternStore.InsertRange(pathPatternStore.Count(), robot.mapList.GetRange(currentPositionIndex, selectedPositionIndex - currentPositionIndex + 1));
                    for (int i = robot.mapList[selectedPositionIndex].SubMapList.Count() - 1; i >= 0; i--)
                    {
                        pathPatternStore.Insert(robot.mapList[selectedPositionIndex].SubMapList.Count() - 1 - i, robot.mapList[selectedPositionIndex].SubMapList[i]);
                    }
                    pathPatternStore.Reverse();
                    index = 0;
                    tmpInterval = -1;
                    timerOriginalStraight.Start();
                    #endregion //beginingLine "before" finishLine
                    
                }
            }
        }
        private void timerOriginalStraight_Tick(object sender, EventArgs e)
        {
            originalToOriginalStraight(selectedPositionIndex, currentPositionIndex, pathPatternStore);
        }
        private void timerOriginalReverse_Tick(object sender, EventArgs e)
        {
            OriginalToOriginalReverse(selectedPositionIndex, currentPositionIndex, pathPatternStore);
        }
        #endregion //traceSelectedPattern
        #region OriginalToOriginalReverse
        private void OriginalToOriginalReverse(int selectedPositionIndex, int currentPositionIndex, List<ClsStepNode> Path)
        {
            Point position = pictureBoxRobot.Location;

            if (Path.Count <= index)
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                
                timerOriginalReverse.Stop();
                control.stop();
                buttonAddPath.Enabled = true;
                textBoxCurrentPath.Text = selected;
                return;
            }
            if (index < 0)
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                
                timerOriginalReverse.Stop();
                control.stop();
                buttonAddPath.Enabled = true;
                textBoxCurrentPath.Text = selected;
                return;
            }

            if (tmpInterval < 0)
                tmpInterval = Path[index].Interval;

            switch ((Steps)Path[index].Step)
            {
                case Steps.Forward:
                    control.backward();
                    pictureBoxRobot.Location = new Point(position.X, position.Y + 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.BackWard:
                    control.forward();
                    pictureBoxRobot.Location = new Point(position.X, position.Y - 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Left:
                    control.turnRight();
                    pictureBoxRobot.Location = new Point(position.X + 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Right:
                    control.turnLeft();
                    pictureBoxRobot.Location = new Point(position.X - 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Stop:
                    control.stop();
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
            }
        }
        #endregion //OriginalToOriginalReverse
        #region originalToOriginalStraight
        private void originalToOriginalStraight(int selectedPositionIndex, int currentPositionIndex, List<ClsStepNode> Path)
        {
            buttonAddPath.Enabled = true;
            Point position = pictureBoxRobot.Location;
            if (Path.Count == 0 || Path.Count <= index)
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                timerPlay.Stop();
                buttonAddPath.Enabled = true;
                textBoxCurrentPath.Text = selected;
                timerPlay.Stop();
                return;
            }
            if (tmpInterval < 0)
                tmpInterval = Path[index].Interval;

            switch ((Steps)Path[index].Step)
            {
                case Steps.Forward:
                    pictureBoxRobot.Location = new Point(position.X, position.Y - 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.BackWard:
                    pictureBoxRobot.Location = new Point(position.X, position.Y + 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Left:
                    control.turnLeft();
                    pictureBoxRobot.Location = new Point(position.X - 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Right:
                    control.turnRight();
                    pictureBoxRobot.Location = new Point(position.X + 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Stop:
                    control.stop();
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
            }
        }
        #endregion//originalToOriginalStraight
        #region analyzeString
        /// <summary>
        /// takes the textbox.string and put the path name in a string and pathIndex and positionIndex in an int
        /// </summary>
        private void analyzeString()
        {
            selected = textBoxSelectedPath.Text;
            current = textBoxCurrentPath.Text;
            char[] space = { ' ' };
            string[] a = selected.Split(space);
            selectedPathName = a[0];
            if (selectedPathName != "OriginalPath")
            {
                selectedPathIndex = int.Parse(a[1]);
                selectedPositionIndex = int.Parse(a[2]);
            }
            else if (selectedPathName == "OriginalPath")
            {
                selectedPositionIndex = int.Parse(a[1]);
            }
            //----------------------------------------
           
            a = current.Split(space);
            currentPathName = a[0];
            if (currentPathName != "OriginalPath")
            {
                currentPathIndex = int.Parse(a[1]);
                currentPositionIndex = int.Parse(a[2]);
            }
            else if (currentPathName == "OriginalPath")
            {
                currentPositionIndex = int.Parse(a[1]);
            }
            
        }
        #endregion //analyzeString
        #region Trace
        /// <summary>
        /// this function traces the recorded path from start to end
        /// </summary>
        private void trace(List<ClsStepNode>Path)
        {
            buttonAddPath.Enabled = true;
            Point position = pictureBoxRobot.Location;
            if (Path.Count == 0 || Path.Count <= index)
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                control.stop();
                timerPlay.Stop();
                buttonAddPath.Enabled = true;
                return;
            }
            if (listBoxSteps.Items.Count == 0)
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                control.stop();
                timerPlay.Stop();
                buttonAddPath.Enabled = true;
                return;
            }

            if (tmpInterval < 0)
                tmpInterval = Path[index].Interval;
            listBoxSteps.SelectedIndex = index;
            textBoxCurrentPath.Text = treeViewPath.Nodes[0].Text + " " + index.ToString();

            switch ((Steps)Path[index].Step)
            {
                case Steps.Forward:
                    control.forward();
                    pictureBoxRobot.Location = new Point(position.X, position.Y - 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.BackWard:
                    control.backward();
                    pictureBoxRobot.Location = new Point(position.X, position.Y + 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Left:
                    control.turnLeft();
                    pictureBoxRobot.Location = new Point(position.X - 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Right:
                    control.turnRight();
                    pictureBoxRobot.Location = new Point(position.X + 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Stop:
                    control.stop();
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index++;
                        tmpInterval = -1;
                    }
                    break;
            }
        }
        #endregion //Trace
        #region Reverse
        private void reverse()
        {
            labelStatus.Text = "Status: Tracing";
            labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
            timerBlinkLight.Start();
            buttonAddPath.Enabled = true;
            index = robot.mapList.Count() - 1;
            tmpInterval = -1;
            timerReverse.Start();
        }
        private void timerReverse_Tick(object sender, EventArgs e)
        {
            Point position = pictureBoxRobot.Location;

            if (robot.mapList.Count <= index)
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                timerReverse.Stop();
                buttonAddPath.Enabled = true;
                control.stop();
                return;
            }
            if (index < 0)
            {
                timerBlinkLight.Stop();
                labelStatus.Text = "Status: Stopped";
                labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                control.stop();
                buttonAddPath.Enabled = true;
                timerReverse.Stop();
                return;
            }

            if (tmpInterval < 0)
                tmpInterval = robot.mapList[index].Interval;

            switch ((Steps)robot.mapList[index].Step)
            {
                case Steps.Forward:
                    control.backward();
                    pictureBoxRobot.Location = new Point(position.X, position.Y + 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.BackWard:
                    control.forward();
                    pictureBoxRobot.Location = new Point(position.X, position.Y - 1);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Left:
                    control.turnRight();
                    pictureBoxRobot.Location = new Point(position.X + 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Right:
                    control.turnLeft();
                    pictureBoxRobot.Location = new Point(position.X - 1, position.Y);
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
                case Steps.Stop:
                    control.stop();
                    if (tmpInterval > 0)
                        tmpInterval -= 1;
                    else
                    {
                        index--;
                        tmpInterval = -1;
                    }
                    break;
            }
        }
#endregion //Reverse
        #region KeyUp
        /// <summary>
        /// this function is called when a key is up - released
        /// it adds a "Stop" node to the list && prints on the "ListBox" the recent recording with time
        /// </summary>
        /// <param name="e"></param>
        private void keyUp(KeyEventArgs e)
        {
            if (bAddPath == false)
            {
                switch ((Keys)e.KeyCode)
                {
                    case Keys.W:
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        // now record the stop interval
                        robot.mapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                    case Keys.S:
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        robot.mapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                    case Keys.A:
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        robot.mapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                    case Keys.D:
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        robot.mapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                }
            }
            else
            {
                switch ((Keys)e.KeyCode)
                {
                    case Keys.W:
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        // now record the stop interval
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                    case Keys.S:
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                    case Keys.A:
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                    case Keys.D:
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Stop));
                        break;
                }
            }
            control.stop();
        }
        #endregion //KeyUp
        #region KeyDown
        /// <summary>
        /// this function is called when a key is pressed
        /// it prints the recent direction with time && adds a node to the list according to the pressed button.
        /// </summary>
        /// <param name="e"></param>
        private void keyDown(KeyPressEventArgs e)
        {
            if (bAddPath == false)
            {
                if (robot.mapList.Last().Step == ConvertDirection(e.KeyChar))
                {
                    return;
                }
                switch (e.KeyChar.ToString().ToLower())
                {
                    case "w":
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());//printing "stop" + time on listBox
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        robot.mapList.Add(new ClsStepNode(Steps.Forward));
                        break;
                    case "s":
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        robot.mapList.Add(new ClsStepNode(Steps.BackWard));
                        break;
                    case "a":
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        robot.mapList.Add(new ClsStepNode(Steps.Left));
                        break;
                    case "d":
                        listBoxSteps.Items.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        treeViewPath.Nodes[0].Nodes.Add(robot.mapList.Last().Step.ToString() + " " + robot.mapList.Last().Interval.ToString());
                        robot.mapList.Add(new ClsStepNode(Steps.Right));
                        break;
                }
            }
            else//if Add Path button is clicked.
            {
                if (robot.mapList[index + 1].SubMapList.Last().Step == ConvertDirection(e.KeyChar))
                {
                    return;
                }
                switch (e.KeyChar.ToString().ToLower())
                {
                    case "w":
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());//printing "stop" + time on listBox
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Forward));
                        break;
                    case "s":
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.BackWard));
                        break;
                    case "a":
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Left));
                        break;
                    case "d":
                        listBoxSteps.Items.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        treeViewPath.Nodes[subPathIndex].Nodes.Add(robot.mapList[index + 1].SubMapList.Last().Step.ToString() + " " + robot.mapList[index + 1].SubMapList.Last().Interval.ToString());
                        textBoxCurrentPath.Text = treeViewPath.Nodes[subPathIndex].Text + " " + treeViewPath.Nodes[subPathIndex].LastNode.Index.ToString();
                        robot.mapList[index + 1].SubMapList.Add(new ClsStepNode(Steps.Right));
                        break;
                }
            }
        }
        #endregion //KeyDown
        #region Convert Directions
        /// <summary>
        /// returns the direction according to the pressed button(char p)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Steps ConvertDirection(char p)
        {
            switch ((char)p)
            {
                case 'w':
                    return Steps.Forward;
                case 's':
                    return Steps.BackWard;
                case 'a':
                    return Steps.Left;
                case 'd':
                    return Steps.Right;
            }
            return Steps.Stop;
        }
        #endregion // Convert Directions
        #region DrawLines
        private void drawLines(Point initialPoint, Point currentPoint)
        {
            int initialX = initialPoint.X;
            int initialY = initialPoint.Y;
            int currentX = currentPoint.X;
            int currentY = currentPoint.Y;

            Graphics route = this.groupBoxBoard.CreateGraphics();
            Pen liner = new Pen(Color.Black, 5);
            route.DrawLine(liner, new Point(initialX, initialY), new Point(currentX, currentY));
        }
        #endregion//DrawLines
        #region blink Light
        private void timerBlinkLight_Tick(object sender, EventArgs e)
        {
            blinkLight();
        }

        private void blinkLight()
        {
            if (labelStatus.Text == "Status: Manual")
            {
                if (labelLight.ImageIndex == imageList1.Images.IndexOfKey("redLight"))
                {
                    labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                    control.litLED(2, 0);
                }
                else if (labelLight.ImageIndex == imageList1.Images.IndexOfKey("grayLight"))
                {
                    labelLight.ImageIndex = imageList1.Images.IndexOfKey("redLight");
                    control.litLED(2, 1);
                }
            }
            if (labelStatus.Text == "Status: Tracing")
            {
                if (labelLight.ImageIndex == imageList1.Images.IndexOfKey("greenLight"))
                {
                    labelLight.ImageIndex = imageList1.Images.IndexOfKey("grayLight");
                    control.litLED(1, 0);
                }
                else if (labelLight.ImageIndex == imageList1.Images.IndexOfKey("grayLight"))
                {
                    labelLight.ImageIndex = imageList1.Images.IndexOfKey("greenLight");
                    control.litLED(1,1);
                }
            }
        }
        #endregion //blink Light

        #endregion //Functions
    }
}
