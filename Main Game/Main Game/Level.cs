using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

// Jusitn Neft
// 2/18/19
namespace Main_Game
{
    /// <summary>
    /// This class will include everything needed to read a file and generate a level
    /// with terrain, textures and collisions.
    /// </summary>
    public class Level
    {

        #region Fields

        private int currentLevel;

        private Dictionary<String, Texture2D> textures;

		private Dictionary<String, Texture2D> enemyTextures;

		private List<Texture2D> goalFireworks;

        // A list of all solid objects, so the level can know what's where.
        private List<Solid> levelObstacles;

        // A list of all enemies within a level, so the level can check collisions with the player.
        private List<Enemy> levelEnemies;

        // A list of lists of destroyable solids with the given priorities.
        private List<List<Solid>> levelDestructibles;

        public QuadTreeNode root;

        public Boolean levelFinished;

        // The player, so that the level is aware of where they are
        private Player player;

        private int levelWidth;
        private int levelHeight;
        private string filePath;
        private int playerHealth;

        Game1 game;

        #endregion Fields

        #region PlayerVars

        private bool playerFloored = false;
        public Matrix scrollMatrix;

        #endregion

        #region Properties

        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
        }

        public int PlayerHealth
        {
            get
            {
                return playerHealth;
            }
        }

		public Player User
		{
			get
			{
				return player;
			}
		}

		public String LevelPath
		{
			get
			{
				return filePath;
			}
		}
        #endregion Properties

        public Level(GraphicsDevice gd, Game1 game)
        {
            textures = new Dictionary<string, Texture2D>();
			enemyTextures = new Dictionary<string, Texture2D>();

			goalFireworks = new List<Texture2D>();
            levelEnemies = new List<Enemy>();
            levelDestructibles = new List<List<Solid>>();
            levelDestructibles.Add(new List<Solid>());
            scrollMatrix = Matrix.CreateTranslation(0, 0, 0);
            this.game = game;
            currentLevel = 1;
            
        }

        /// <summary>
        /// Draws all blocks on the level.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            //We need to draw using a transformation matrix, so kill the SpriteBatch and reinitialize it with the matrix
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, scrollMatrix);

            //Draw everything in the level

            // Check if we're debugging or not.
            if (game.IsDebugging)
            {
                root.DrawDebug(textures["dog"], sb);
                
            }
            else
            {
                root.Draw(sb);
            }
            player.Draw(sb, Color.White, game.IsDebugging);

            foreach (Enemy e in levelEnemies)
			{
				e.Draw(sb);
			}
			foreach (Particle p in Game1.sparkles)
			{
				p.Draw(sb);
			}

			//Kill the SpriteBatch and reinitialize it with no parameters
			sb.End();
			sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);//SamplerState.PointClamp);

        }

        public void LoadContent(ContentManager content, GraphicsDevice gd)
        {
            textures = new Dictionary<string, Texture2D>();
            enemyTextures = new Dictionary<string, Texture2D>();

            textures.Add("platform", content.Load<Texture2D>("platform"));
            textures.Add("platform_bottom", content.Load<Texture2D>("platform_bottom"));
            textures.Add("platform_bottomleft", content.Load<Texture2D>("platform_bottomleft"));
            textures.Add("platform_bottomright", content.Load<Texture2D>("platform_bottomright"));
            textures.Add("platform_center", content.Load<Texture2D>("platform_center"));
            textures.Add("platform_column_bottom", content.Load<Texture2D>("platform_column_bottom"));
            textures.Add("platform_column_middle", content.Load<Texture2D>("platform_column_middle"));
            textures.Add("platform_column_top", content.Load<Texture2D>("platform_column_top"));
            textures.Add("platform_end", content.Load<Texture2D>("platform_end"));
            textures.Add("platform_end_left", content.Load<Texture2D>("platform_end_left"));
            textures.Add("platform_inward_bottomleft", content.Load<Texture2D>("platform_inward_bottomleft"));
            textures.Add("platform_inward_bottomright", content.Load<Texture2D>("platform_inward_bottomright"));
            textures.Add("platform_inward_topleft", content.Load<Texture2D>("platform_inward_topleft"));
            textures.Add("platform_inward_topright", content.Load<Texture2D>("platform_inward_topright"));
            textures.Add("platform_left", content.Load<Texture2D>("platform_left"));
            textures.Add("platform_middle", content.Load<Texture2D>("platform_middle"));
            textures.Add("platform_right", content.Load<Texture2D>("platform_right"));
            textures.Add("platform_single", content.Load<Texture2D>("platform_single"));
            textures.Add("platform_tiny", content.Load<Texture2D>("platform"));
            textures.Add("platform_top", content.Load<Texture2D>("platform_top"));
            textures.Add("platform_topleft", content.Load<Texture2D>("platform_topleft"));
            textures.Add("platform_topright", content.Load<Texture2D>("platform_topright"));
            textures.Add("player_kawaiigoom_idle-Sheet", content.Load<Texture2D>("player_kawaiigoom_idle-Sheet"));
            textures.Add("player_kawaiigoom_walk-Sheet", content.Load<Texture2D>("player_kawaiigoom_walk-Sheet"));
            textures.Add("dog", content.Load<Texture2D>("dog"));
            textures.Add("goal_unclaimed", content.Load<Texture2D>("goal_unclaimed"));
            textures.Add("goal_claimed", content.Load<Texture2D>("goal_claimed"));
            textures.Add("goal-Sheet", content.Load<Texture2D>("goal-Sheet"));

            goalFireworks.Add(content.Load<Texture2D>("sparkle_blue"));
            goalFireworks.Add(content.Load<Texture2D>("sparkle_yellow"));

			enemyTextures.Add("enemy_paper_idle-Sheet", content.Load<Texture2D>("enemy_paper_idle-Sheet"));
			enemyTextures.Add("enemy_paper_walk-Sheet", content.Load<Texture2D>("enemy_paper_walk-Sheet"));
			enemyTextures.Add("enemy_scissors_idle-Sheet", content.Load<Texture2D>("enemy_scissors_idle-Sheet"));
			enemyTextures.Add("enemy_scissors_walk-Sheet", content.Load<Texture2D>("enemy_scissors_walk-Sheet"));
			enemyTextures.Add("enemy_rock_idle-Sheet", content.Load<Texture2D>("enemy_rock_idle-Sheet"));
			enemyTextures.Add("enemy_rock_walk-Sheet", content.Load<Texture2D>("enemy_rock_walk-Sheet"));
			enemyTextures.Add("enemy_lizard_idle-Sheet", content.Load<Texture2D>("enemy_lizard_idle"));
			enemyTextures.Add("enemy_lizard_walk-Sheet", content.Load<Texture2D>("enemy_lizard_walk-Sheet"));
			enemyTextures.Add("enemy_spock_idle-Sheet", content.Load<Texture2D>("enemy_spock_idle"));
			enemyTextures.Add("enemy_spock_walk-Sheet", content.Load<Texture2D>("enemy_spock_walking-Sheet"));

			player = new Player(new Animation(textures["player_kawaiigoom_idle-Sheet"], textures["player_kawaiigoom_idle-Sheet"].Height), new Animation(textures["player_kawaiigoom_walk-Sheet"], textures["player_kawaiigoom_walk-Sheet"].Height), new Rectangle(0, 0, 64, 64), new KeyboardManager(), gd);
        }

        /// <summary>
        /// Reads an external .lvl file into the game, builds it and adds all the objects to various parts of the level object.
        /// Solids of all types are added to the root, and levelDestructibles if need be.
        /// The player is set up for where they spawn here.
        /// Goals are given events here.
        /// 
        /// </summary>
        /// <param name="selectedFile"></param>
        public void LoadLevel(string selectedFile)
        {

			currentLevel++;

			// Add file reading, and all required fields (see milestone 2 tasks notes for 2/18/19 for more details).
			filePath = selectedFile;

            Stream sr;
            sr = File.OpenRead(filePath);
            BinaryReader reader = new BinaryReader(sr);

            // Read the level data into the editor.

            levelWidth = reader.ReadInt32();
            levelHeight = reader.ReadInt32();

            // Make the root node that'll contain all the solids in the level.
            root = new QuadTreeNode(new Rectangle(0, 0, levelWidth * 64, levelHeight * 64));

            levelEnemies = new List<Enemy>();

            // Read how many non-empty tiles there are in the file.
            int objectNum = reader.ReadInt32();

            // Read the level's player health.
            playerHealth = reader.ReadInt32();

            // Loop through every object in the file, and place it where it needs to be in the level.
            for (int i = 0; i < objectNum; i++)
            {
                string s = null;
                try
                {
                    s = reader.ReadString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                // Check to see what kind of object has to be generated.
                switch (s)
                {
                    case "Solid":
                        // First read in this solid's position
                        int solidX = reader.ReadInt32();
                        int solidY = reader.ReadInt32();

                        // Next read in this solid's properties.
                        int priority = reader.ReadInt32();
                        bool isInvisible = reader.ReadBoolean();
                        string texture = "platform_" + reader.ReadString();

                        Solid temp;

                        if (isInvisible)
                        {
                            temp = new InvisibleSolid(textures["dog"], new Rectangle(solidX * 64, solidY * 64, 64, 64));
                        }
                        else
                        {
                            temp = new Solid(new Rectangle(solidX * 64, solidY * 64, 64, 64), new Animation(textures[texture], textures[texture].Width));
                        }
                        root.AddSolid(temp);
                        // ADD TO PRIORITY LIST

                        if (priority >= levelDestructibles.Count - 1)
                        {
                            for (int j = levelDestructibles.Count - 1; j < priority; j++)
                            {
                                levelDestructibles.Add(new List<Solid>());
                            }
                        }
                        levelDestructibles[priority].Add(temp);
                        break;
                    case "Enemy":
                        // Generates a null enemy, then slowly gathers its information.
                        int enemyX = reader.ReadInt32();
                        int enemyY = reader.ReadInt32();
                        
                        string enemyType = reader.ReadString().ToLower();

                        //Determine the animations needed for the enemy.
                        Animation enemyIdle = new Animation(enemyTextures["enemy_" + enemyType + "_idle-Sheet"], enemyTextures["enemy_" + enemyType + "_idle-Sheet"].Height);
                        Animation enemyWalk = new Animation(enemyTextures["enemy_" + enemyType + "_walk-Sheet"], enemyTextures["enemy_" + enemyType + "_walk-Sheet"].Height);

                        Enemy enemyTemp = new Enemy(enemyIdle, enemyWalk, new Rectangle(enemyX * 64, enemyY * 64, 64, 64), new Vector2(3, 0));

                        int enemyPriority = reader.ReadInt32();

                        // Read in all the combat options for the enemy.
                        int enemyOptionNum = reader.ReadInt32();
                        int playerOptionNum = reader.ReadInt32();
                        for (int n = 0; n < enemyOptionNum; n++)
                        {
                            string option = reader.ReadString();
                            enemyTemp.EnemyOptions.Add(option);
                        }

                        // Load in the player combat options.
                        for (int n = 0; n < playerOptionNum; n++)
                        {
                            enemyTemp.PlayerOptions.Add(reader.ReadString());
                        }

                        levelEnemies.Add(enemyTemp);

                        break;
                    case "Player":
                        player.X = reader.ReadInt32() * 64;
                        player.Y = reader.ReadInt32() * 64;
                        break;
                    case "Goal":

                        int goalX = reader.ReadInt32();
                        int goalY = reader.ReadInt32();

                        int goalPriority = reader.ReadInt32();

                        Goal<string> tempGoalString;
                        Goal<int> tempGoalInt;

                        


                        // Determine what the goal does based off its priority. If it's zero, then it loads the next level. Otherwise it destroys solids.
                        if (goalPriority == 0)
                        {
                            // change the current filepath to the next level's
                            string nextFilePath = "Content/level" + (currentLevel) + ".level"; //+ 1) + ".level";
                            Action<string> action = LoadNextLevel;
                            // Create goal animations

                            tempGoalString = new Goal<string>(new Rectangle(goalX * 64, goalY * 64, 64, 64), new Animation(textures["goal_unclaimed"], textures["goal_unclaimed"].Height), action, nextFilePath, new Animation(textures["goal-Sheet"], textures["goal-Sheet"].Height), new Animation(textures["goal_claimed"], textures["goal_claimed"].Height), goalFireworks);
                            root.AddSolid(tempGoalString);
                        }
                        else
                        {
                            Action<int> action = DestroyPrioritizedSolids;
                            tempGoalInt = new Goal<int>(new Rectangle(goalX * 64, goalY * 64, 64, 64), null, action, goalPriority, new Animation(textures["goal-Sheet"], textures["goal-Sheet"].Height), new Animation(textures["goal_claimed"], textures["goal_claimed"].Height), goalFireworks);
                            
                            root.AddSolid(tempGoalInt);
                            // Add this goal to the proper destructibles list.
                            if (goalPriority >= levelDestructibles.Count - 1)
                            {
                                for (int j = levelDestructibles.Count - 1; j < goalPriority; j++)
                                {
                                    levelDestructibles.Add(new List<Solid>());
                                }
                            }
                            levelDestructibles[goalPriority].Add(tempGoalInt);
                        }
                        break;
                }
            }
            player.ResetVelocities();
        }

        protected void DestroyPrioritizedSolids(int priority)
        {
            foreach(Solid solid in levelDestructibles[priority])
            {
				solid.Enabled = false;
            }
        }

        protected void LoadNextLevel(string nextLevel)
        {
			//currentLevel++;
			LoadLevel(nextLevel);
            //try
            //{
            //    currentLevel++;
            //    LoadLevel(nextLevel);
            //}
            //catch(Exception e)
            //{
				
            //    Game1.State = MainGameState.StartMenu;
            //}
        }

        /// <summary>
        /// Executes all collision handling between all objects every tick.
        /// </summary>
        public void Update()
        {
            //Update the player (and enemies, and other actors) before executing collision
            player.Update();

            // Check that the player is still in the bounds of the level.
            if (player.X < 0)
            {
                player.X = 0;
            }
            if (player.X > ((levelWidth * 64) - 64))
            {
                player.X = ((levelWidth * 64) - 64);
            }
            // This condition checks mainly for bottomless pits, which count as an instant kill.
            if (player.Y > ((levelHeight * 64) - 64))
            {
                playerHealth = 0;
                player.Y = ((levelHeight * 64) - 64);
            }
            if (player.Y < 0)
            {
                player.CollideWithCeiling(0);
                player.Y = 0;
            }

            //For every solid in the solid list
            foreach (Solid solid in root.SolidsInNodesIntersecting(player.ColliderCheck))
            {
				solid.Update();

                if(solid.IsCollide && solid.Enabled)
                {

                    //If the player's floor is below/at this solid, the downwards collider is intersecting with a solid, and the player is fully above that solid
                    if (solid.Bounds.Y <= player.Floor && solid.Bounds.Intersects(player.YCollider) && solid.Bounds.Y > player.Y + player.Height - 1)
                    {
                        //Set the floor to the solid's Y and mark the player as floored
                        player.Floor = solid.Bounds.Y;
                        playerFloored = true;
                    }
                    //Check for left/right collision detection if the Y collider doesn't intersect
                    else if (solid.ActorIsCollide(player))
                    {
                        if (player.Y > solid.Bounds.Y + solid.Bounds.Height / 2)
                        {
                            player.CollideWithCeiling(solid.Bounds.Y + solid.Bounds.Height);
                        }
                        else if (player.X > solid.Bounds.X)
                        {
                            player.X = solid.Bounds.X + solid.Bounds.Width;
                        }
                        else if (player.X < solid.Bounds.X)
                        {
                            player.X = solid.Bounds.X - player.Width;
                        }
                    }
                }
                //this test is dumb as SHIT but it kind of works
                //doing "solid is goal" is impossible because goal is generic
                else if(solid.GetType().Name.Substring(0,4).Equals("Goal"))
                {
                    if(solid.ActorIsCollide(player))
                    {
                        solid.Trigger();

                    }
                }
            }
            //If the player was not floored this tick, set the floor to off the screen
            if (!playerFloored)
            {
                player.Floor = int.MaxValue;
            }

            //Currently not fully implemented: check enemy collision against solids in the level
            foreach (Enemy enemy in levelEnemies)
            {
                bool enemyFloored = false;
                if(!enemy.ColliderCheck.Intersects(root.rect))
                {
                    enemy.Dead = true;
                    continue;
                }
                foreach (Solid solid in root.SolidsInNodesIntersecting(enemy.ColliderCheck))
                {
                    if ((solid.IsCollide && solid.Enabled) || solid is InvisibleSolid)
                    {

                        //If the enemy's floor is below/at this solid, the downwards collider is intersecting with a solid, and the enemy is fully above that solid
                        if (solid.Bounds.Y <= enemy.Floor && solid.Bounds.Intersects(enemy.YCollider) && solid.Bounds.Y > enemy.Y + enemy.Height - 1)
                        {
                            //Set the floor to the solid's Y and mark the player as floored
                            enemy.Floor = solid.Bounds.Y;
                            enemyFloored = true;
                        }
                        //Check for left/right collision detection if the Y collider doesn't intersect
                        else if (solid.ActorIsCollide(enemy))
                        {
                            if (enemy.Y > solid.Bounds.Y + solid.Bounds.Height / 2)
                            {
                                enemy.CollideWithCeiling(solid.Bounds.Y + solid.Bounds.Height);
                            }
                            else if (solid.ActorIsCollide(enemy))
                            {
                                enemy.ReverseDirection();
                                break;
                            }
                        }
                    }
                }
                if(!enemyFloored)
                {
                    enemy.Floor = int.MaxValue;
                }
            }
            //Generate a translation matrix based on the player X and Y center, and the screen offsets
            //Should the resolution ever change, these should not be hardcoded
            int playerXCenter = player.X + player.Width / 2;
            int playerYCenter = player.Y + player.Height / 2;

            int scrollOffsetX = -MathHelper.Clamp(playerXCenter - 800, 0, levelWidth * 64 - 1600);
            int scrollOffsetY = -MathHelper.Clamp(playerYCenter - 450, 0, levelHeight * 64 - 900);

            scrollMatrix = Matrix.CreateTranslation(scrollOffsetX, scrollOffsetY, 0);

            //If R is pressed or the player falls off the screen, reset the player's location and scroll matrix
            //This maybe should be handled by some sort of PrepareLevel() method instead of containing it within the update loop
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                player.X = 50;
                player.Y = 50;
                scrollMatrix = Matrix.CreateTranslation(0, 0, 0);
            }
            //Collision detection is over; reset collision related variables
            playerFloored = false;

            // Check if the player is dead, and then change the main game state if that's the case.
            if (playerHealth == 0)
            {
                Game1.State = MainGameState.GameOver;
            }

            // Check for collision with every enemy in the level and trigger battle
            foreach(Enemy enemy in levelEnemies)
            {
                if(!enemy.Dead)
                {
                    if(player.Bounds.Intersects(enemy.Bounds))
                    {
                        Game1.State = MainGameState.BattleTransition;
                        game.CurrentCombatEnemy = enemy;
                    }
                    enemy.Update();
                }
                
            }
        }

	}
}
