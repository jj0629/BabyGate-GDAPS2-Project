using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_Game
{
    public class Player : Actor
    {

        Texture2D yColliderTex;

        #region PhysicsVars

        //Unless these are replaced (as floor should be), all of these should initially be included alongside the physics code
        //Constants can be changed to modify the properties of the jump

        //Jump-related variables
        int yVelocity = 0;
        int jumpVelocity = 0;
        const int jumpVelocityInitial = 6;
        const int gravity = -1;
        const int jumpVelocityDecrease = -1;
        //Instances of floor should be replaced by the Y of whatever Solid is below the Player
        int floor = 500;
        bool grounded = true;

        //Left-right movement related variables
        int xVelocity = 0;
        const int runVelocityInitial = 2;
        const int runVelocityIncrease = 1;
        const int runVelocityMax = 6;

        //Landing-related objects
        Rectangle yCollider;

        #endregion

        #region Fields
        KeyboardManager kbManager;
        #endregion

        #region Properties
        //X and Y properties overriden to move the yCollider along with the player
        public override int X
        {
            get { return pos.X; }
            set { pos.X = value; yCollider.X = value + pos.Width / 8; }
        }

        public override int Y
        {
            get { return pos.Y; }
            set { pos.Y = value; yCollider.Y = value + pos.Height; }
        }

        //Make yCollider available to other classes
        public Rectangle YCollider
        {
            get { return yCollider; }
        }

        //Set floor correctly
        public int Floor
        {
            get { return floor + pos.Height; }
            set { floor = value - pos.Height; }
        }

        /// <summary>
        /// Returns the smallest possible Rectangle containing both the Player bounds and the YCollider
        /// <para>Useful for determining large areas to check against the player at once</para>
        /// </summary>
        public Rectangle ColliderCheck
        {
            get
            {
                return new Rectangle(Math.Min(pos.X, YCollider.X),
                                     Math.Min(pos.Y, YCollider.Y),
                                     Math.Max(pos.Width, YCollider.Width),
                                     Math.Max(pos.Height, YCollider.Height));
            }
        }

        #endregion

        //Basic constructor chained to Actor constructor
        public Player(Animation texture, Animation walk, Rectangle position, KeyboardManager kbManager, GraphicsDevice gd) : base(texture, walk, position)
        {
            this.kbManager = kbManager;
            //the Y collider is a rectangle 3/4 of the player's width and 300 pixels tall, centered on the X axis and immediately below the player on Y
            //This is used to detect floors below the player before it reaches them
            yCollider = new Rectangle(position.X + position.Width/8, position.Y + position.Height, (int)(position.Width*0.75), 300);
            yColliderTex = new Texture2D(gd, 1, 1);
            yColliderTex.SetData(new[] {Color.White});
        }

        /// <summary>
        /// Handles updates and inputs for the player class.
        /// </summary>
        public void Update()
        {
            //Get the state of Jump key
            switch (kbManager.GetKeyState(Inputs.Jump))
            {
                //If it's just being tapped, set the jump velocity to the initial jump velocity and set the y velocity to the jump velocity
                case KeyState.FirstTap:
                    if(grounded)
                    {
                        jumpVelocity = jumpVelocityInitial;
                        yVelocity = jumpVelocity;
						MoveState = MovementType.Idle;
                    }
                    break;
                //If it's being held and the current jump velocity isn't zero, decay the jump velocity and add the new jump velocity to the y velocity
                case KeyState.Down:
                    if (!(jumpVelocity <= 0))
                        jumpVelocity += jumpVelocityDecrease;
                    yVelocity += jumpVelocity;
                    break;
            }
            //If the actor isn't grounded, subtract gravity from the velocity
            if (!grounded)
            {
                yVelocity += gravity;				
            }
            //Modify the actor's position by the calculated velocity
            Y -= yVelocity;

            //If the actor's FALLING through the floor and not grounded:
            // - Set their Y value to the floor
            // - Set their Y velocity to 0
            // - Make them grounded
            if (pos.Y >= floor && !grounded && yVelocity < 0)
            {
                pos.Y = floor;
                yVelocity = 0;
                grounded = true;
            }
            //If the actor is above the floor, they're no longer grounded
            else if (pos.Y < floor)
            {
                grounded = false;
            }
            //Otherwise, keep the current grounded value

            //The two switches below this look complicated, but make the movement feel ever so slightly more familiar
            //Take input for leftwards movement
            switch(kbManager.GetKeyState(Inputs.Left))
            {
                case KeyState.FirstTap:
                    //If the player is already moving in the opposite direction, decelerate them
                    if(xVelocity > 0)
                        xVelocity -= runVelocityIncrease;
                    //Otherwise, start at the initial negative run velocity
                    else
                        xVelocity = -runVelocityInitial;
					mt = MovementType.MovingLeft;
                    break;

                case KeyState.Down:
					if (mt == MovementType.Idle)
						WalkingAnimation.Frame = 0;
					//If the player is still moving in the opposite direction, decelerate them
					if (xVelocity > 0)
                        xVelocity -= runVelocityIncrease;
                    //Otherwise, accelerate them in this direction as long as the velocity isn't already at the max and the opposite direction isn't being pressed
                    else if (xVelocity > -runVelocityMax && kbManager.GetKeyState(Inputs.Right) != KeyState.Down)
                        xVelocity -= runVelocityIncrease;
					mt = MovementType.MovingLeft;
					break;

                case KeyState.Up:
                    //If the player is still moving in this direction, decelerate them
                    if (xVelocity < 0)
                        xVelocity += runVelocityIncrease;
                    break;
            }

            //Take input for rightwards movement
            //All functions are identical to the above, with switched signs
            switch (kbManager.GetKeyState(Inputs.Right))
            {
                case KeyState.FirstTap:
                    if (xVelocity < 0)
                        xVelocity += runVelocityIncrease;
                    else
                        xVelocity = runVelocityInitial;
					mt = MovementType.MovingRight;
					break;

                case KeyState.Down:
					if (mt == MovementType.Idle)
						WalkingAnimation.Frame = 0;
                    if (xVelocity < 0)
                        xVelocity += 2 * runVelocityIncrease;
                    else if (xVelocity < runVelocityMax && kbManager.GetKeyState(Inputs.Left) != KeyState.Down)
                        xVelocity += runVelocityIncrease;
					mt = MovementType.MovingRight;
					break;

                case KeyState.Up:
                    if (xVelocity > 0)
                        xVelocity -= runVelocityIncrease;
					break;
            }
            // Make it so the player can't leave the level bounds.
            X += xVelocity;

			if (kbManager.GetKeyState(Inputs.Right) == KeyState.Up && kbManager.GetKeyState(Inputs.Left) == KeyState.Up)
			{
				if (mt != MovementType.Idle)
					IdleAnimation.Frame = 0;
				mt = MovementType.Idle;
			}
        }

        public override void Draw(SpriteBatch sb, Color color, bool debug)
        {
            if (debug)
            {
                sb.Draw(yColliderTex, yCollider, Color.Red * 0.25f);
            }
            
            base.Draw(sb, color, debug);
        }

        /// <summary>
        /// Alter the player's Y velocity based on a given "ceiling" Y value
        /// <para>This should be used in conjunction with a well-written collision detection system</para>
        /// </summary>
        /// <param name="ceilingY">Y-value of the ceiling</param>
        public void CollideWithCeiling(int ceilingY)
        {
            //Make sure the player isn't *already* falling
            //This prevents "hitching" when the player is falling and pushing against the side of a block
            if(yVelocity >= 0)
            {
                yVelocity = 0;
                Y = ceilingY;
            }
        }

        public void ResetVelocities()
        {
            xVelocity = 0;
            yVelocity = 0;
        }
    }
}
