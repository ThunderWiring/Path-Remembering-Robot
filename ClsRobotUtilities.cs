using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotNamespace
{
    enum Steps
    {
        Forward, BackWard, Left, Right, Stop
    }
    class ClsStepNode
    {
        #region Variables
        /// <summary>
        /// the interval of the pressed position
        /// </summary>
        private int interval;
        /// <summary>
        /// the direction of the motion
        /// </summary>
        private Steps step;
        public List<ClsStepNode> SubMapList = new List<ClsStepNode>();
        #endregion // variables
        #region constructor
        public ClsStepNode(Steps step)
        {
            this.step = step;
            this.interval = 0;
        }
        #endregion//constructor
        #region Properties
        public int Interval
        {
            set { this.interval = value; }
            get { return this.interval; }
        }// SetInterval
        public Steps Step
        {
            set { this.step = value; }
            get { return this.step; }
        }//Step
        #endregion//Property
    }
}
