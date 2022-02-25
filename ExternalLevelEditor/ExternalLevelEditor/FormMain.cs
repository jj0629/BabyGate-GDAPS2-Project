using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ExternalLevelEditor
{
    enum TileOptions { Empty, Solid, InvisibleSolid, Player, Enemy, Goal }

    public partial class FormMain : Form
    {

        #region Fields

        private int levelWidth;
        private int levelHeight;
        PictureBox[,] cells;
        bool unsavedChanges;

        private FormEnemyOptions enemy;
        private Player p;

        string filePath;

        int objectNum;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the 2D array of PictureBoxes used as the central way of presenting/storing data.
        /// </summary>
        public PictureBox[,] Cells
        {
            get
            {
                return cells;
            }
        }

        #endregion Properties

        public FormMain()
        {
            InitializeComponent();
            
            cells = new PictureBox[0,0];
            p = null;
            TextureSelector.SelectedItem = "single";
            objectNum = 0;
        }

        /// <summary>
        /// Used to confirm/change the amount of labels in the main panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSizeConfirm_Click(object sender, EventArgs e)
        {
            int levelWidthTemp = Int32.Parse(textBoxWidth.Text);
            int levelHeightTemp = Int32.Parse(textBoxHeight.Text);

            // Handle error checking to see if the new level sizes are valid.

            if (levelWidthTemp < 0 || levelHeightTemp < 0)
            {
                MessageBox.Show("Error: You can not have a level with negative space", "Size Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (levelWidthTemp < levelWidth || levelHeightTemp < levelHeight)
            {
                DialogResult result = MessageBox.Show("Warning: This new size is smaller than the previous one. Some data may be lost. Would you like to continue?", "Size Loss", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    levelWidth = levelWidthTemp;
                    levelHeight = levelHeightTemp;
                    PictureBox[,] tempCells = new PictureBox[levelWidth, levelHeight];
                    for (int i = 0; i < levelWidth; i++)
                    {
                        for (int j = 0; j < levelHeight; j++)
                        {
                            tempCells[i, j] = cells[i, j];
                        }
                    }
                    cells = tempCells;
                }
            }
            else
            {
                levelWidth = levelWidthTemp;
                levelHeight = levelHeightTemp;
                PictureBox[,] tempCells = new PictureBox[levelWidth, levelHeight];
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    for (int j = 0; j < cells.GetLength(1); j++)
                    {
                        tempCells[i, j] = cells[i, j];
                    }
                }
                cells = tempCells;
            }
            LoadPictureBoxes();           
        }

        /// <summary>
        /// Loads the picture boxes into the main panel.
        /// </summary>
        private void LoadPictureBoxes()
        {
            panelLevelObjects.Controls.Clear();
            cells = new PictureBox[levelWidth, levelHeight];
            int picSide = 25;

            // Loop  through and make the labels in the panel.
            for (int w = 0; w < levelWidth; w++)
            {
                for (int h = 0; h < levelHeight; h++)
                {
                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(picSide, picSide);
                    pic.Location = new Point(picSide * w, picSide * h);
                    pic.BackColor = Color.White;
                    pic.Click += PictureBox_Click;
                    panelLevelObjects.Controls.Add(pic);
                    pic.MouseHover += ShowData;
                    cells[w, h] = pic;
                }
            }
        }

        /// <summary>
        /// Changes the contents of the picture box to whatever is currently selected.
        /// </summary>
        /// <param name="sender">The object sending this event.</param>
        /// <param name="e"></param>
        private void PictureBox_Click(object sender, EventArgs e)
        {
            // Change the number of objects before  ANY edits are made.
            if (pictureBoxCurrent.BackColor == Color.White)
            {
                // The case if the eraser is being used. This number check is in case the user has the eraser active, and clicks on nothing.
                if (objectNum > 0)
                {
                    objectNum--;
                }
            }
            else
            {
                // If the eraser isn't being used, there are two cases to check for: a totally new object, or one object replacing another.

                // This is the case where it's a new object. Here, we increment the number. In the other case, we do nothing.
                if (((PictureBox)sender).BackColor == Color.White)
                {
                    objectNum++;
                }
            }

            labelObjectNum.Text = "Object num:" + objectNum;

            ((PictureBox)sender).BackColor = pictureBoxCurrent.BackColor;
            unsavedChanges = true;
            if (!this.Text.Contains("*"))
            {
                this.Text += "*";
            }

            // Unbound the mouse for the click and drag feature.
            ((PictureBox)sender).Capture = false;

            // Checks if the eraser is being used, or something else. then it changes the object count accordingly.
            if (((PictureBox)sender).BackColor == Color.White)
            {
                // Clear the contents of the picture box
                ((PictureBox)sender).Image = null;
                ((PictureBox)sender).ImageLocation = null;
                ((PictureBox)sender).Tag = null;
            }
            // Checks if the object being placed is an enemy, and if so calls a small form to create data for the enemy.
            else if (((PictureBox)sender).BackColor == Color.Blue)
            {
                Enemy temp = new Enemy();
                ((PictureBox)sender).Tag = temp;
                // Search through every cell to find this enemy, so we can get the x and y positions of the enemy and write it into the enemy class.
                for (int i = 0; i < levelWidth; i++)
                {
                    for (int j = 0; j < levelHeight; j++)
                    {
                        if (cells[i, j].Tag == temp)
                        {
                            enemy = new FormEnemyOptions(this, i, j);
                            enemy.ShowDialog();
                        }
                    }
                }
            }
            // The case for when a player is added to the level.
            else if (((PictureBox)sender).BackColor == Color.Green)
            {
                // Check to see if there is already a player object in the level. If so, ask if the user wishes to replace it.
                if (p == null)
                {
                    for (int i = 0; i < levelWidth; i++)
                    {
                        for (int j = 0; j < levelHeight; j++)
                        {
                            if (cells[i, j] == ((PictureBox)sender))
                            {
                                p = new Player(i, j);
                            }
                        }
                    }
                    // Make the new cell a player cell.
                    ((PictureBox)sender).Tag = p;
                }
                else
                {
                    // Clear the previous player cell.
                    for (int i = 0; i < levelWidth; i++)
                    {
                        for (int j = 0; j < levelHeight; j++)
                        {
                            if (cells[i, j].Tag is Player)
                            {
                                cells[i, j].Tag = null;
                                cells[i, j].BackColor = Color.White;
                            }
                        }
                    }

                    // Fill in the new one.
                    ((PictureBox)sender).Tag = p;
                    ((PictureBox)sender).BackColor = Color.Green;
                    for (int i = 0; i < levelWidth; i++)
                    {
                        for (int j = 0; j < levelHeight; j++)
                        {
                            if (cells[i, j].Tag is Player)
                            {
                                p = new Player(i, j);
                            }
                        }
                    }
                }
            }
            // The case for when a normal solid or invisible solid is placed on the level.
            else if (((PictureBox)sender).BackColor == Color.Black || ((PictureBox)sender).BackColor == Color.LightGray)
            {
                // Find the picture box that we are currently on, to determine its x and y position.
                for (int i = 0; i < levelWidth; i++)
                {
                    for (int j = 0; j < levelHeight; j++)
                    {
                        if (((PictureBox)sender) == cells[i, j])
                        {
                            // Next, determine if it's an invisible solid or not.
                            bool isSolid;
                            if (((PictureBox)sender).BackColor == Color.LightGray)
                            {
                                isSolid = true;
                            }
                            else
                            {
                                isSolid = false;
                                // If it's a solid, insert its selected texture.
                                String imageLocation = @"../../Platform Images/platform_" + TextureSelector.SelectedItem + ".png";
                                pictureBoxCurrent.ImageLocation = imageLocation;
                                cells[i, j].Image = Image.FromFile(imageLocation);
                                cells[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                            }

                            // Load all of the information into a shell-object that organizes said information.
                            Solid temp = new Solid(i, j, isSolid, (int)numericUpDownPriority.Value, (String)TextureSelector.SelectedItem);

                            cells[i, j].Tag = temp;
                        }
                    }
                }
            }
            // The case for when a goal is placed on the level.
            else if (((PictureBox)sender).BackColor == Color.Yellow)
            {
                // Find the picture box we are currently on to determine the x and y position.
                for (int i = 0; i < levelWidth; i++)
                {
                    for (int j = 0; j < levelHeight; j++)
                    {
                        if (((PictureBox)sender) == cells[i, j])
                        {
                            Trigger temp = new Trigger(i, j, (int)numericUpDownPriority.Value);

                            cells[i, j].Tag = temp;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Whenever the sending button is clicked, the current picture box will be updated with the sender's background color.
        /// </summary>
        /// <param name="sender">The object sending this event.</param>
        /// <param name="e"></param>
        private void OptionButton_Click(object sender, EventArgs e)
        {
            // Reset the image of pictureBoxCurrent
            pictureBoxCurrent.Image = null;
            pictureBoxCurrent.ImageLocation = null;

            pictureBoxCurrent.BackColor = ((Button)sender).BackColor;
            labelSelected.Text = ((Button)sender).Text;
            if (((Button)sender).BackColor == Color.Green || ((Button)sender).BackColor == Color.White)
            {
                numericUpDownPriority.Enabled = false;
            }
            else
            {
                numericUpDownPriority.Enabled = true;
            }
        }

        /// <summary>
        /// Saves the current file, and labels that the file is saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxPlayerHealth.Text == null)
            {
                textBoxPlayerHealth.Text = "1";
            }

            unsavedChanges = false;
            // Sets up the new file to be written to, as well as a save dialog (because it looks nice :P)
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save a level file";
            save.Filter = "Level files |*.level;";
            Stream sStream = null;
            BinaryWriter writer = null;
            string fileName;
            DialogResult result = save.ShowDialog();
            if (result != DialogResult.OK)
            {
                unsavedChanges = true;

            }
            else
            {
                fileName = save.FileName;
                this.Text = "Level editor - " + filePath;
                // Checks to see if the current save will be replacing an already existing file.
                if (save.FileName == filePath)
                {
                    File.Delete(filePath);
                }
                try
                {
                    sStream = File.OpenWrite(fileName);
                    writer = new BinaryWriter(sStream);

                    // Save the necessary data here.
                    // Start by writing the level dimensions
                    writer.Write(levelWidth);
                    writer.Write(levelHeight);

                    // Write how many objects are in the file. This number is used for file reading.
                    writer.Write(objectNum);

                    // Set a defualt player health of 1 if no player health has been entered.
                    if (textBoxPlayerHealth.Text == "")
                    {
                        writer.Write(1);
                    }
                    else
                    {
                        int playerHealth = Int32.Parse(textBoxPlayerHealth.Text);
                        writer.Write(playerHealth);
                    }                    

                    int length0 = cells.GetLength(0);
                    int length1 = cells.GetLength(1);

                    // Loop through every "cell" in the main 2D array, and save different objects in different ways.
                    for (int i = 0; i < cells.GetLength(0); i++)
                    {
                        for (int j = 0; j < cells.GetLength(1); j++)
                        {
                            // Each cell type has something different in its tag property, and must be read slightly differently.
                            // The following if-else statement shall do that.

                            // The first case is to check if the block is empty.
                            if (cells[i, j].Tag is null)
                            {
                                // Do nothing. This statement is here just in case!
                            }
                            // The next case is to check for solids, then write them to the file.
                            if (cells[i, j].Tag is Solid)
                            {
                                Solid temp = (Solid)cells[i, j].Tag;

                                writer.Write("Solid");

                                // Write the solid's position.
                                writer.Write(temp.X);
                                writer.Write(temp.Y);

                                // Next, write the solid's properties.
                                writer.Write(temp.Priority);
                                writer.Write(temp.IsInvisible);
                                writer.Write(temp.Texture);
                                
                            }
                            // The case for if the block is a trigger.
                            else if (cells[i, j].Tag is Trigger)
                            {
                                Trigger temp = (Trigger)cells[i, j].Tag;

                                writer.Write("Goal");

                                // Write the trigger's position.
                                writer.Write(temp.X);
                                writer.Write(temp.Y);

                                // Next, write the trigger's priority.
                                writer.Write(temp.Priority);
                            }
                            // Enemies have a few different things that need to be written: their position (x and y), player attack options and their attack options.
                            else if (cells[i, j].Tag is Enemy)
                            {
                                Enemy temp = (Enemy)cells[i, j].Tag;
                                writer.Write("Enemy");
                                writer.Write(temp.X);
                                writer.Write(temp.Y);

                                // Write the enemy's type.
                                writer.Write(temp.EnemyType.ToLower());

                                // Write the enemy's priority.
                                writer.Write(temp.Priority);

                                // Write the number of each type of options before the options, so that file reading is a little easier.
                                writer.Write(temp.EnemyOptions.Count);
                                writer.Write(temp.PlayerOptions.Count);

                                foreach (String s in temp.EnemyOptions)
                                {
                                    writer.Write(s);
                                }
                                
                                foreach (String s in temp.PlayerOptions)
                                {
                                    writer.Write(s);
                                }
                            }
                            // Players only have a position that needs to be written to the file.
                            else if (cells[i, j].Tag is Player)
                            {
                                Player temp = (Player)cells[i, j].Tag;
                                writer.Write("Player");
                                writer.Write(temp.X);
                                writer.Write(temp.Y);
                            }
                        }
                    }
                    MessageBox.Show("The file was saved successfully.", "Save successful.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
                finally
                {
                    sStream.Close();
                }
            }
            
        }

        /// <summary>
        /// Checks if there are any unsaved changes, then acts based on the answer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (unsavedChanges)
            {
                DialogResult result = MessageBox.Show("Warning: You may unsaved changes. Would you like to save before closing this program?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    buttonSave_Click(sender, e);
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Loads a level file and puts it in the editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            unsavedChanges = false;
            file.Title = "Open a level file";
            file.Filter = "Level files |*.level;*.alevel";
            DialogResult result = file.ShowDialog();
            Stream sr;
            if (result == DialogResult.OK)
            {
                filePath = Path.GetFileName(file.FileName);
                sr = File.OpenRead(file.FileName);
                BinaryReader reader = new BinaryReader(sr);

                // Read the level data into the editor.

                levelWidth = reader.ReadInt32();
                levelHeight = reader.ReadInt32();
                textBoxWidth.Text = levelWidth.ToString();
                textBoxHeight.Text = levelHeight.ToString();

                objectNum = reader.ReadInt32();

                labelObjectNum.Text = "Object num: " + objectNum;

                // Read the level's player health.
                textBoxPlayerHealth.Text = reader.ReadInt32().ToString();

                LoadPictureBoxes();

                // Loop through every cell and determine what is in it from the file.
                for (int i = 0; i < objectNum; i++)
                {
                    string s;
                    s = reader.ReadString();

                    switch (s)
                    {
                        case "Solid":
                            // First, read in the solid's position.
                            int solidX = reader.ReadInt32();
                            int solidY = reader.ReadInt32();

                            // Next, read in the solid's properties.
                            int priority = reader.ReadInt32();
                            bool isInvisible = reader.ReadBoolean();
                            string texture = reader.ReadString();

                            // Next, create the solid object and assign it to the cells array.
                            Solid tempSolid = new Solid(solidX, solidY, isInvisible, priority, texture);

                            cells[solidX, solidY].Tag = tempSolid;

                            // Finally, determine what color the picture box should be.
                            // Gray for invisible solid, black for solid.
                            if (tempSolid.IsInvisible)
                            {
                                cells[solidX, solidY].BackColor = Color.LightGray;
                            }
                            else
                            {
                                cells[solidX, solidY].BackColor = Color.Black;
                                // Write in the image based off of what was in the file.
                                String imageLocation = @"../../Platform Images/platform_" + texture + ".png";
                                cells[solidX, solidY].ImageLocation = imageLocation;
                                cells[solidX, solidY].Image = Image.FromFile(imageLocation);
                                cells[solidX, solidY].SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            break;
                        case "Goal":
                            // First, read in the goal's position
                            int goalX = reader.ReadInt32();
                            int goalY = reader.ReadInt32();

                            // Next, read in the goal's priority.
                            int goalPriority = reader.ReadInt32();

                            // Finally, create a trigger object and assign it to the cells array while changing the proper picture box's color.
                            Trigger tempTrigger = new Trigger(goalX, goalY, goalPriority);
                            cells[goalX, goalY].Tag = tempTrigger;
                            cells[goalX, goalY].BackColor = Color.Yellow;
                            break;
                        case "Enemy":
                            Enemy temp = new Enemy();
                            temp.X = reader.ReadInt32();
                            temp.Y = reader.ReadInt32();

                            // Read the enemy's type.
                            temp.EnemyType = reader.ReadString();

                            // Read the enemy's priority
                            temp.Priority = reader.ReadInt32();

                            // Read in the battle options for the player and the enemy.
                            int enemyOptionNum = reader.ReadInt32();
                            int playerOptionNum = reader.ReadInt32();

                            for (int n = 0; n < enemyOptionNum; n++)
                            {
                                temp.EnemyOptions.Add(reader.ReadString());
                            }
                            for (int n = 0; n < playerOptionNum; n++)
                            {
                                temp.PlayerOptions.Add(reader.ReadString());
                            }
                            cells[temp.X, temp.Y].Tag = temp;
                            cells[temp.X, temp.Y].BackColor = Color.Blue;
                            break;
                        case "Player":
                            int pX = reader.ReadInt32();
                            int pY = reader.ReadInt32();
                            cells[pX, pY].Tag = new Player(pX, pY);
                            cells[pX, pY].BackColor = Color.Green;
                            break;
                    }
                }
                this.Text = "Level editor - " + filePath;
                sr.Close();
                MessageBox.Show("Level file loaded correctly.", "Load successful.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Shows the picture box's data in a tooltip if the user hovers over a picture box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowData(object sender, EventArgs e)
        {
            PictureBox current = ((PictureBox)sender);
            if (current.Tag is Enemy)
            {
                Enemy temp = (Enemy)current.Tag;
                string enemyType = temp.EnemyType;
                string enemyOptions = "";
                foreach (string s in temp.EnemyOptions)
                {
                    enemyOptions += s + ", ";
                }
                string playerOptions = "";
                foreach (string s in temp.PlayerOptions)
                {
                    playerOptions += s + ", ";
                }
                labelHoverInfo.Text = ("Enemy type: " + enemyType + "\r\nEnemy options: " + enemyOptions + "\r\nPlayerOptions: " + playerOptions + "\r\nPriority: " + temp.Priority);
            }
            else if (current.Tag is Player)
            {
                labelHoverInfo.Text = ("Player");
            }
            else if (current.Tag is Solid)
            {
                string finalSolidData = "Solid";

                Solid tempSolid = (Solid)current.Tag;

                finalSolidData += "\r\nX: " + tempSolid.X;
                finalSolidData += "\r\nY: " + tempSolid.Y;
                finalSolidData += "\r\nPriority: " + tempSolid.Priority;
                finalSolidData += "\r\nIsInvisible?: " + tempSolid.IsInvisible;
                finalSolidData += "\r\nTexture: " + tempSolid.Texture;

                labelHoverInfo.Text = (finalSolidData);
            }
            else if (current.Tag is Trigger)
            {
                string finalTriggerData = "Trigger";

                Trigger tempTrigger = (Trigger)current.Tag;

                finalTriggerData += "\r\nX: " + tempTrigger.X;
                finalTriggerData += "\r\nY: " + tempTrigger.Y;
                finalTriggerData += "\r\nPriority: " + tempTrigger.Priority;
                finalTriggerData += "\r\nIsGoal?: " + tempTrigger.IsGoal;

                labelHoverInfo.Text = (finalTriggerData);
            }
        }

        /// <summary>
        /// This will change the picture used in the picture box for currently selected, so you can see what image you're using.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextureSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            String imageLocation = @"../../Platform Images/platform_" + TextureSelector.SelectedItem + ".png";
            pictureBoxCurrent.ImageLocation = imageLocation;
            pictureBoxCurrent.Image = Image.FromFile(imageLocation);
            pictureBoxCurrent.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
