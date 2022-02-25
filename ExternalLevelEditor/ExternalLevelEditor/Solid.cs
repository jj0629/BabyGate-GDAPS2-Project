using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLevelEditor
{
    class Solid
    {

        #region Fields

        bool isInvisible;
        int x;
        int y;
        int priority;
        string texture;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets this solid's x position.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Gets this solid's y position.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Gets what type of solid this is.
        /// </summary>
        public bool IsInvisible
        {
            get
            {
                return isInvisible;
            }
        }

        /// <summary>
        /// Gets this solid's priority.
        /// </summary>
        public int Priority
        {
            get
            {
                return priority;
            }
        }

        /// <summary>
        /// Gets this solid's texture.
        /// </summary>
        public string Texture
        {
            get
            {
                return texture;
            }
        }

        #endregion Properties

        /// <summary>
        /// Makes a shell-class of a solid, that contains information about what is in this solid.
        /// </summary>
        /// <param name="x">This solid's x position.</param>
        /// <param name="y">This solid's y position.</param>
        public Solid(int x, int y, bool i, int p, string t)
        {
            this.x = x;
            this.y = y;
            isInvisible = i;
            priority = p;
            texture = t;
        }
    }
}
