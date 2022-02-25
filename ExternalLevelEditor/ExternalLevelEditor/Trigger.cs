using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLevelEditor
{
    class Trigger
    {

        #region Fields

        bool isGoal;
        int x;
        int y;
        int priority;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets if this trigger is a goal or not.
        /// </summary>
        public bool IsGoal
        {
            get
            {
                return isGoal;
            }
        }

        /// <summary>
        /// Gets the x position of this trigger.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Gets the y position of this trigger.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Gets the priority of this trigger.
        /// </summary>
        public int Priority
        {
            get
            {
                return priority;
            }
        }

        #endregion Properties

        /// <summary>
        /// Makes a shell-class of a trigger, that contains information about what is in this trigger.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="p"></param>
        /// <param name="g"></param>
        public Trigger(int x, int y, int p)
        {
            this.x = x;
            this.y = y;
            priority = p;

            // If the trigger's priority is 0, then it's a goal.
            if (priority == 0)
            {
                isGoal = true;
            }
            else
            {
                isGoal = false;
            }
        }
    }
}
