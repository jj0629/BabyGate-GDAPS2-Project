using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main_Game
{
	public class Particle
	{
		//particle velocity
		Point v;
		//particle acceleration
		Point a;

		//the position and size of the particle
		Rectangle pos;

		Texture2D spark;

		Color col;

		public int X
		{
			get
			{
				return pos.X;
			}
			set
			{
				pos.X = value;
			}
		}

		public int Y
		{
			get
			{
				return pos.Y; 
			}
			set
			{
				pos.Y = value;
			}
		}

		public Point Vector
		{
			set
			{
				v = value;
			}
		}

		/// <summary>
		/// Creates a particle
		/// </summary>
		/// <param name="vector">the original directional movement of the particle</param>
		/// <param name="location">The starting location of the particle</param>
		/// <param name="sparkle">the texture of the particle</param>
		/// <param name="c">the color of the particle</param>
		/// <param name="acceleration">basically for which way gravity is. this allows for floating particles like smoke or ashes. also can allow for wind.
		/// Leave null for default value</param>
		private Particle(Point vector, Rectangle location, Texture2D sparkle, Color c, Point acceleration)
		{
			//acceleration = new Point(0, 1);
			a = acceleration;
			v = vector;
			pos = location;
			spark = sparkle;
			col = c;
		}

		/// <summary>
		/// applies the acceleration and velocity to the particle
		/// </summary>
		public void Update()
		{
			v.X += a.X;
			v.Y += a.Y;

			pos.X += v.X;
			pos.Y += v.Y;
		}

		/// <summary>
		/// Draws the particle
		/// </summary>
		/// <param name="sb"></param>
		public void Draw(SpriteBatch sb)
		{
			sb.Draw(spark, pos, col);
		}

		/// <summary>
		/// Generates several particles. Starting velocity and color will be randomized between the upper and lower bounds. if you don't want
		/// it to be random, set them to the same thing or set one of the two to null.
		/// </summary>
		/// <param name="vectorLower">The lower starting velocity of the particle. Defaults to vectorUpper or (0, 1) if vectorUpper is null</param>
		/// <param name="vectorUpper">The upper starting velocity of the particle. Defaults to vectorLower or (0, 1) if vectorLower is null</param>
		/// <param name="acceleration">The acceleration of the particles. This can be gravity (0, 1) or reverse gravity (0, -1) or wherever you want the particles to end up</param>
		/// <param name="origin">The starting position of the particles</param>
		/// <param name="particleTextures">The texture(s) of the particle. This method will pick a random one each time.</param>
		/// <param name="lowerColorBound">The lower range of the color. Defaults to upperColorBound or white if upperColorBound is null.</param>
		/// <param name="upperColorBound">The upper range of the color. Defaults to lowerColorBound or white if lowerColorBound is null</param>
		/// <param name="numParticles">The number of particles to be generated</param>
		/// <param name="rng">Random object</param>
		/// <returns>A List containing the generated particles.</returns>
		public static List<Particle> GenerateParticles(Point vectorLower, Point vectorUpper, Point acceleration, Point origin, List<Texture2D> particleTextures, Color lowerColorBound, Color upperColorBound, int numParticles, Random rng, bool allowUnmoving = false)
		{
			List<Particle> particles = new List<Particle>();

			//default color options
			if (upperColorBound == null)
				upperColorBound = lowerColorBound;
			if (lowerColorBound == null)
				lowerColorBound = upperColorBound;
			if (lowerColorBound == null)
				lowerColorBound = upperColorBound = Color.White;

			//default velocity options
			if (vectorLower == null)
				vectorLower = vectorUpper;
			if (vectorUpper == null)
				vectorUpper = vectorLower;
			if (vectorUpper == null)
				vectorUpper = vectorLower = new Point(0, 1);

			int temp;
			if (upperColorBound.R < lowerColorBound.R)
			{
				temp = upperColorBound.R;
				upperColorBound.R = lowerColorBound.R;
				lowerColorBound.R = (byte)temp;
			}
			if (upperColorBound.G < lowerColorBound.G)
			{
				temp = upperColorBound.G;
				upperColorBound.G = lowerColorBound.G;
				lowerColorBound.G = (byte)temp;
			}
			if (upperColorBound.B < lowerColorBound.B)
			{
				temp = upperColorBound.B;
				upperColorBound.B = lowerColorBound.B;
				lowerColorBound.B = (byte)temp;
			}

			//Generates the particles
			for(int i = 0; i < numParticles; i++)
			{
				//gets the texture to use
				int textureIndex = rng.Next(particleTextures.Count);
				int colorAdjustment = (rng.Next(lowerColorBound.R, upperColorBound.R) + rng.Next(lowerColorBound.G, upperColorBound.G) + rng.Next(lowerColorBound.B, upperColorBound.B))/3;

				Point v = new Point(rng.Next(vectorLower.X, vectorUpper.X), rng.Next(vectorLower.Y, vectorUpper.Y));
				if(v.X == 0 && v.Y == 0 && !allowUnmoving && acceleration.X == 0 && acceleration.Y == 0)
				{
					v = vectorLower;
				}

				particles.Add(new Particle(
					//creates the starting velocity of this particle
					v,
					//the starting position and size of the particle
					new Rectangle(origin, new Point(particleTextures[textureIndex].Width, particleTextures[textureIndex].Height)),
					//the texture of the particle
					particleTextures[textureIndex],
					//generates the color of the particle
					new Color(
						rng.Next(lowerColorBound.R, upperColorBound.R),
						rng.Next(lowerColorBound.G, upperColorBound.G),
						rng.Next(lowerColorBound.B, lowerColorBound.B)),
						//lowerColorBound.R + colorAdjustment,
						//lowerColorBound.G + colorAdjustment,
						//lowerColorBound.B + colorAdjustment),
					//the acceleration of the particle
					acceleration));
			}

			return particles;
		}
	}
}
