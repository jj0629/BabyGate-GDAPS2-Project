using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_Game
{
	public class Animation
	{
		Texture2D frames;
		int frameWidth;
		int frame;
		private static int fps = 6;

		public Texture2D Texture
		{
			get
			{
				return frames;
			}
		}

        /// <summary>
        /// Creates an animation given a spritesheet and the width of each frame.
        /// </summary>
        /// <param name="inStrip">The spritesheet of the animation. THIS MUST BE HORIZONTAL ONLY</param>
        /// <param name="inFrameWidth">The width of each frame. Used in slicing the spritesheet</param>
		public Animation(Texture2D inStrip, int inFrameWidth)
		{
			frames = inStrip;
			frameWidth = inFrameWidth;
			frame = 0;
		}

        /// <summary>
        /// Gets the spritesheet
        /// </summary>
		public Texture2D SpriteSheet
		{
			get
			{
				return frames;
			}
		}

        /// <summary>
        /// Gets and sets the current frame number if the frame number is within the possible number of frames.
        /// </summary>
		public int Frame
		{
			get
			{
				return frame;
			}
			set
			{
				if(value < frames.Width / frameWidth)
				{
					frame = value;
				}
			}
		}

		public int FrameWidth
		{
			get
			{
				return frameWidth;
			}
		}

		public bool isLastFrame()
		{
			return (frame > frames.Width / frameWidth * fps - 3);
		}

        /// <summary>
        /// Draws each frame of this.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="destination"></param>
		public void Draw(SpriteBatch sb, Rectangle destination, SpriteEffects spef = SpriteEffects.None, float scale = 2.0f)
		{
			//draws the current frame.
			sb.Draw(frames, new Vector2(destination.X, destination.Y), new Rectangle((frame / fps) * frameWidth, 0, frameWidth, frames.Height), Color.White, 0.0f, new Vector2(0, 0), scale, spef, 0.0f);
			//sb.Draw(frames, destination, new Rectangle((frame / fps) * frameWidth, 0, frameWidth, frames.Height), Color.White, 0.0f,  null, spef, 0.0f);
			// This counts up to 6, then moves to the next frame so that each frame of animation has more screen time and doesn't cycle too fast
			if (isLastFrame())
				frame = 0;
			else
				frame++;
			// frame = (frame/fps) + 2 > frames.Width / frameWidth ? 0 : frame + 1;
		}

        /// <summary>
        /// Same as above, but only draws the specified frame of the animation
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="destination"></param>
        public void DrawSingleFrame(SpriteBatch sb, Rectangle destination, int frame, SpriteEffects spef = SpriteEffects.None, float scale = 2.0f)
        {
            //draws the current frame.
            sb.Draw(frames, new Vector2(destination.X, destination.Y), new Rectangle((frame / fps) * frameWidth, 0, frameWidth, frames.Height), Color.White, 0.0f, new Vector2(0, 0), scale, spef, 0.0f);
        }
	}
}
