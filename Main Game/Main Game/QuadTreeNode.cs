using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Justin Neft
// 2/28/19
// These will be the nodes that hold data in the game for collision with other items.
namespace Main_Game
{
    public class QuadTreeNode
    {
        #region Constants

        const int TILE_SIZE = 64;
        const int MAX_SOLIDS_IN_NODE = 10;

        #endregion Constants

        #region Fields

        public Rectangle rect;

        public QuadTreeNode[] divisions;

        public List<Solid> solids;

        int nodeTileWidth;
        int nodeTileHeight;

        #endregion Fields

        #region Properties

        public List<Solid> Solids { get { return solids; } }

        #endregion

        /// <summary>
        /// Constructs the next node in the tree.
        /// </summary>
        /// <param name="r">This node's rectangle in the game world.</param>
        /// <param name="w">The width of this node in tiles.</param>
        /// <param name="h">The height of this node in tiles.</param>
        public QuadTreeNode(Rectangle r)
        {
            rect = r;
            divisions = null;
            solids = new List<Solid>();    
        }

        /// <summary>
        /// Divides the current node into 4 subnodes.
        /// </summary>
        public void Divide()
        {
            
            // Begin by simply filling out the array with the standard size of 4 subnodes.
            divisions = new QuadTreeNode[4];

            //Width and height for each new division
            int width = rect.Width / 2;
            int height = rect.Height / 2;
            //Index we're at in the array
            int i = 0;
            //Create the divisions
            for(int y = rect.Y; y < rect.Height + rect.Y; y += height)
            {
                for(int x = rect.X; x < rect.Width + rect.X; x += width)
                {
                    divisions[i] = new QuadTreeNode(new Rectangle(x,y,width,height));
                    i++;
                }
            }
            List<Solid> dividedSolids = new List<Solid>();
            //Loop through each solid in this node and see if it can be put into a division
            foreach(Solid solid in solids)
            {
                foreach(QuadTreeNode node in divisions)
                {
                    if(node.rect.Contains(solid.Bounds))
                    {
                        node.AddSolid(solid);
                        dividedSolids.Add(solid);
                        break;
                    }
                }
            }
            //Loop through all of the divided solids and remove them from this node
            foreach(Solid solid in dividedSolids)
            {
                solids.Remove(solid);
            }
        }

        /// <summary>
        /// Adds a solid to this node's internal list of solids.
        /// </summary>
        /// <param name="s">The solid to be added in.</param>
        public void AddSolid(Solid s)
        {
            //If this node contains the given solid...
            if(s != null && rect.Contains(s.Bounds))
            {
                //...and divisions are already established OR the node needs to divide with this new entry...
                if(divisions != null || solids.Count + 1 > MAX_SOLIDS_IN_NODE)
                {
                    //If there are no divisions, divide the node
                    if (divisions == null)
                        Divide();
                    //Loop through the divisions to find the node that this object will fit in, and add it to it
                    foreach(QuadTreeNode node in divisions)
                    {
                        if(node.rect.Contains(s.Bounds))
                        {
                            node.AddSolid(s);
                        }
                    }
                }
                //Add the solid to this node's list regardless
                solids.Add(s);
            }
        }

        /// <summary>
        /// Draws this node into the level.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            // Checks if this node should do the drawing, or the divisions should.
            if (divisions != null)
            {
                foreach(QuadTreeNode n in divisions)
                {
                    n.Draw(sb);
                }
            }
            // Loop through this node's solids and draw them all into the game world.
            foreach(Solid solid in Solids)
            {
				//Don't draw if it's an invisible solid
				if (!(solid is InvisibleSolid))
					solid.Draw(sb);
					//sb.Draw(solid.Ani.Texture, solid.Bounds, Color.White);
            }
        }

        /// <summary>
        /// Draws this node's contents normally, as well as drawing lines around each node using a given texture for debugging purposes
        /// </summary>
        /// <param name="line">A texture to draw lines with (can be virtually anything)</param>
        /// <param name="sb">The SpriteBatch to draw using</param>
        public void DrawDebug(Texture2D line, SpriteBatch sb)
        {
            // Checks if this node should do the drawing, or the divisions should.
            if (divisions != null)
            {
                foreach (QuadTreeNode n in divisions)
                {
                    n.DrawDebug(line, sb);
                }
            }
            // Loop through this node's solids and draw them all into the game world.
            foreach (Solid solid in Solids)
            {
                //Don't draw if it's an invisible solid
                if (!(solid is InvisibleSolid))
                    sb.Draw(solid.Ani.Texture, solid.Bounds, Color.White);
            }

            //Draw lines at the node's edges for debugging purposes
            sb.Draw(line, new Rectangle(rect.X, rect.Y, rect.Width, 1), Color.Red);
            sb.Draw(line, new Rectangle(rect.X, rect.Y, 1, rect.Height), Color.Red);
            sb.Draw(line, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, 1), Color.Red);
            sb.Draw(line, new Rectangle(rect.X + rect.Width, rect.Y, 1, rect.Height), Color.Red);
        }

        /// <summary>
        /// Check which node contains an actor inside of a QuadTree
        /// <para>This method is no longer used for collision, but is maintained in case it becomes useful later</para>
        /// </summary>
        /// <param name="actor">The actor to check</param>
        /// <returns>The smallest node that fully contains the actor</returns>
        public QuadTreeNode GetNodeContainingActor(Actor actor)
        {

            //If this node fully ontains the actor...
            if(rect.Contains(actor.Bounds))
            {
                //...and it has divisions...
                if(divisions != null)
                {
                    //Check each division to see if it FULLY contains the actor's bounding box.
                    //Those nodes will also check their children using this same process.
                    //If one returns something not null, immediately return that result
                    foreach(QuadTreeNode node in divisions)
                    {
                        QuadTreeNode result = node.GetNodeContainingActor(actor);
                        if (result != null)
                            return result;
                    }
                }
                //...and this node does not have divisions or no divisions had a non-null result, return this node
                return this;
            }
            //If this node does not fully contain the actor, return null
            return null;
        }

        /// <summary>
        /// <para>(my naming scheme for these is getting progressively worse)</para>
        /// Returns a List of all Solids in all Nodes intersected by the given Actor's bounds
        /// </summary>
        /// <param name="actor">The actor to check</param>
        /// <returns>List of Solids</returns>
        public List<Solid> SolidsInNodesIntersecting(Actor actor)
        {
            return SolidsInNodesIntersecting(actor.Bounds);
        }

        /// <summary>
        /// Returns a List of all Solids in all Nodes intersected by the given Rectangle
        /// </summary>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        public List<Solid> SolidsInNodesIntersecting(Rectangle toCheck)
        {
            //Make sure the rectangle is inside this node
            if(rect.Intersects(toCheck))
            {
                //Generate a List to store all Solids to pass out
                List<Solid> finalList = new List<Solid>();
                //If this node has divisions, loop through them
                if (divisions != null)
                {
                    foreach (QuadTreeNode division in divisions)
                    {
                        //If the given division intersects this rectangle, add its solids (recursively) to this node's list
                        if (division.rect.Intersects(toCheck))
                        {
                            List<Solid> temp = division.SolidsInNodesIntersecting(toCheck);
                            //Make sure it didn't return null before adding them
                            if (temp != null)
                                finalList.AddRange(temp);
                        }
                    }
                }
                //Add this node's solids to the List and return it
                finalList.AddRange(solids);
                return finalList;
            }
            else
            {
                //If it's not intersecting this node (somehow), return null
                return null;
            }
        }
    }
}
