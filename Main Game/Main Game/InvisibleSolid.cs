using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Main_Game
{
    /// <summary>
    /// this class is a (usually) invisible obstacle only enemies collide with
    /// </summary>


    public class InvisibleSolid : Solid
    {
        #region fields

        /// <summary>
        /// used during testing to put a basic texture in order to see the wall
        /// </summary>
        private bool visible;

        #endregion fields

        #region properties

        #endregion properties


        /// <summary>
        /// initializes the wall with a texture and a rectangle position
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="pos"></param>
        public InvisibleSolid(Texture2D tex, Rectangle pos, bool isSolid = false) : base(pos, new Animation(tex, pos.Width), isSolid)
        {
            visible = false; //defaults to being invisible
        }

        /// <summary>
        /// returns whether or not an enemy is colliding with the invisible wall
        /// if not an enemy, returns false - can't collide with the wall
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public override bool ActorIsCollide(Actor check)
        {
            if(check is Enemy)
            {
                return base.ActorIsCollide(check);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// only draws the wall if it is set to visible, for testing
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if (!visible)
            {
                base.Draw(sb);
            }
            
        }


    }
}
