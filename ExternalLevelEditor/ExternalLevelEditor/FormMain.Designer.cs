namespace ExternalLevelEditor
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxLevel = new System.Windows.Forms.GroupBox();
            this.panelLevelObjects = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.groupBoxLevelOptions = new System.Windows.Forms.GroupBox();
            this.numericUpDownPriority = new System.Windows.Forms.NumericUpDown();
            this.TextureSelector = new System.Windows.Forms.ComboBox();
            this.buttonGoal = new System.Windows.Forms.Button();
            this.buttonErase = new System.Windows.Forms.Button();
            this.buttonInvisibleSolid = new System.Windows.Forms.Button();
            this.buttonSolid = new System.Windows.Forms.Button();
            this.buttonPlayer = new System.Windows.Forms.Button();
            this.buttonEnemy = new System.Windows.Forms.Button();
            this.groupBoxSize = new System.Windows.Forms.GroupBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.buttonSizeConfirm = new System.Windows.Forms.Button();
            this.pictureBoxCurrent = new System.Windows.Forms.PictureBox();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.labelSelected = new System.Windows.Forms.Label();
            this.lablePlayerHealth = new System.Windows.Forms.Label();
            this.textBoxPlayerHealth = new System.Windows.Forms.TextBox();
            this.groupBoxHoverInfo = new System.Windows.Forms.GroupBox();
            this.panelHoverInfo = new System.Windows.Forms.Panel();
            this.labelHoverInfo = new System.Windows.Forms.Label();
            this.labelObjectNum = new System.Windows.Forms.Label();
            this.groupBoxLevel.SuspendLayout();
            this.groupBoxLevelOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).BeginInit();
            this.groupBoxSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).BeginInit();
            this.groupBoxHoverInfo.SuspendLayout();
            this.panelHoverInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLevel
            // 
            this.groupBoxLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLevel.Controls.Add(this.panelLevelObjects);
            this.groupBoxLevel.Location = new System.Drawing.Point(315, 13);
            this.groupBoxLevel.Name = "groupBoxLevel";
            this.groupBoxLevel.Size = new System.Drawing.Size(529, 477);
            this.groupBoxLevel.TabIndex = 1;
            this.groupBoxLevel.TabStop = false;
            this.groupBoxLevel.Text = "Level Labels";
            // 
            // panelLevelObjects
            // 
            this.panelLevelObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLevelObjects.AutoScroll = true;
            this.panelLevelObjects.Location = new System.Drawing.Point(6, 20);
            this.panelLevelObjects.Name = "panelLevelObjects";
            this.panelLevelObjects.Size = new System.Drawing.Size(517, 451);
            this.panelLevelObjects.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(12, 13);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(12, 44);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 3;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // groupBoxLevelOptions
            // 
            this.groupBoxLevelOptions.Controls.Add(this.labelObjectNum);
            this.groupBoxLevelOptions.Controls.Add(this.numericUpDownPriority);
            this.groupBoxLevelOptions.Controls.Add(this.TextureSelector);
            this.groupBoxLevelOptions.Controls.Add(this.buttonGoal);
            this.groupBoxLevelOptions.Controls.Add(this.buttonErase);
            this.groupBoxLevelOptions.Controls.Add(this.buttonInvisibleSolid);
            this.groupBoxLevelOptions.Controls.Add(this.buttonSolid);
            this.groupBoxLevelOptions.Controls.Add(this.buttonPlayer);
            this.groupBoxLevelOptions.Controls.Add(this.buttonEnemy);
            this.groupBoxLevelOptions.Location = new System.Drawing.Point(12, 73);
            this.groupBoxLevelOptions.Name = "groupBoxLevelOptions";
            this.groupBoxLevelOptions.Size = new System.Drawing.Size(204, 292);
            this.groupBoxLevelOptions.TabIndex = 4;
            this.groupBoxLevelOptions.TabStop = false;
            this.groupBoxLevelOptions.Text = "Level Options";
            // 
            // numericUpDownPriority
            // 
            this.numericUpDownPriority.Location = new System.Drawing.Point(79, 151);
            this.numericUpDownPriority.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.numericUpDownPriority.Name = "numericUpDownPriority";
            this.numericUpDownPriority.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownPriority.TabIndex = 7;
            // 
            // TextureSelector
            // 
            this.TextureSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TextureSelector.FormattingEnabled = true;
            this.TextureSelector.Items.AddRange(new object[] {
            "bottom",
            "bottomleft",
            "bottomright",
            "center",
            "column_bottom",
            "column_middle",
            "column_top",
            "end",
            "end_left",
            "inward_bottomleft",
            "inward_bottomright",
            "inward_topleft",
            "inward_topright",
            "left",
            "middle",
            "right",
            "single",
            "tiny",
            "top",
            "topleft",
            "topright"});
            this.TextureSelector.Location = new System.Drawing.Point(79, 172);
            this.TextureSelector.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.TextureSelector.MaxDropDownItems = 3;
            this.TextureSelector.Name = "TextureSelector";
            this.TextureSelector.Size = new System.Drawing.Size(117, 21);
            this.TextureSelector.TabIndex = 6;
            this.TextureSelector.SelectedIndexChanged += new System.EventHandler(this.TextureSelector_SelectedIndexChanged);
            // 
            // buttonGoal
            // 
            this.buttonGoal.BackColor = System.Drawing.Color.Yellow;
            this.buttonGoal.Location = new System.Drawing.Point(79, 86);
            this.buttonGoal.Name = "buttonGoal";
            this.buttonGoal.Size = new System.Drawing.Size(68, 58);
            this.buttonGoal.TabIndex = 5;
            this.buttonGoal.Text = "Trigger";
            this.buttonGoal.UseVisualStyleBackColor = false;
            this.buttonGoal.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // buttonErase
            // 
            this.buttonErase.BackColor = System.Drawing.Color.White;
            this.buttonErase.Location = new System.Drawing.Point(79, 20);
            this.buttonErase.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonErase.Name = "buttonErase";
            this.buttonErase.Size = new System.Drawing.Size(64, 58);
            this.buttonErase.TabIndex = 4;
            this.buttonErase.Text = "Eraser";
            this.buttonErase.UseVisualStyleBackColor = false;
            this.buttonErase.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // buttonInvisibleSolid
            // 
            this.buttonInvisibleSolid.BackColor = System.Drawing.Color.LightGray;
            this.buttonInvisibleSolid.Location = new System.Drawing.Point(7, 218);
            this.buttonInvisibleSolid.Name = "buttonInvisibleSolid";
            this.buttonInvisibleSolid.Size = new System.Drawing.Size(68, 61);
            this.buttonInvisibleSolid.TabIndex = 3;
            this.buttonInvisibleSolid.Text = "Invisible Solid";
            this.buttonInvisibleSolid.UseVisualStyleBackColor = false;
            this.buttonInvisibleSolid.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // buttonSolid
            // 
            this.buttonSolid.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.buttonSolid.BackColor = System.Drawing.Color.Black;
            this.buttonSolid.ForeColor = System.Drawing.Color.White;
            this.buttonSolid.Location = new System.Drawing.Point(7, 151);
            this.buttonSolid.Name = "buttonSolid";
            this.buttonSolid.Size = new System.Drawing.Size(68, 61);
            this.buttonSolid.TabIndex = 2;
            this.buttonSolid.Text = "Solid";
            this.buttonSolid.UseVisualStyleBackColor = false;
            this.buttonSolid.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // buttonPlayer
            // 
            this.buttonPlayer.BackColor = System.Drawing.Color.Green;
            this.buttonPlayer.Location = new System.Drawing.Point(7, 85);
            this.buttonPlayer.Name = "buttonPlayer";
            this.buttonPlayer.Size = new System.Drawing.Size(68, 59);
            this.buttonPlayer.TabIndex = 1;
            this.buttonPlayer.Text = "Player";
            this.buttonPlayer.UseVisualStyleBackColor = false;
            this.buttonPlayer.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // buttonEnemy
            // 
            this.buttonEnemy.BackColor = System.Drawing.Color.Blue;
            this.buttonEnemy.Location = new System.Drawing.Point(7, 20);
            this.buttonEnemy.Name = "buttonEnemy";
            this.buttonEnemy.Size = new System.Drawing.Size(68, 58);
            this.buttonEnemy.TabIndex = 0;
            this.buttonEnemy.Text = "Enemy";
            this.buttonEnemy.UseVisualStyleBackColor = false;
            this.buttonEnemy.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // groupBoxSize
            // 
            this.groupBoxSize.Controls.Add(this.textBoxHeight);
            this.groupBoxSize.Controls.Add(this.textBoxWidth);
            this.groupBoxSize.Controls.Add(this.labelHeight);
            this.groupBoxSize.Controls.Add(this.labelWidth);
            this.groupBoxSize.Controls.Add(this.buttonSizeConfirm);
            this.groupBoxSize.Location = new System.Drawing.Point(19, 371);
            this.groupBoxSize.Name = "groupBoxSize";
            this.groupBoxSize.Size = new System.Drawing.Size(126, 94);
            this.groupBoxSize.TabIndex = 5;
            this.groupBoxSize.TabStop = false;
            this.groupBoxSize.Text = "Level Size";
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(51, 39);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(56, 20);
            this.textBoxHeight.TabIndex = 4;
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(51, 14);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(56, 20);
            this.textBoxWidth.TabIndex = 3;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(6, 45);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(41, 13);
            this.labelHeight.TabIndex = 2;
            this.labelHeight.Text = "Height:";
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(7, 18);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(38, 13);
            this.labelWidth.TabIndex = 1;
            this.labelWidth.Text = "Width:";
            // 
            // buttonSizeConfirm
            // 
            this.buttonSizeConfirm.Location = new System.Drawing.Point(6, 65);
            this.buttonSizeConfirm.Name = "buttonSizeConfirm";
            this.buttonSizeConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonSizeConfirm.TabIndex = 0;
            this.buttonSizeConfirm.Text = "Confirm";
            this.buttonSizeConfirm.UseVisualStyleBackColor = true;
            this.buttonSizeConfirm.Click += new System.EventHandler(this.buttonSizeConfirm_Click);
            // 
            // pictureBoxCurrent
            // 
            this.pictureBoxCurrent.BackColor = System.Drawing.Color.White;
            this.pictureBoxCurrent.Location = new System.Drawing.Point(189, 12);
            this.pictureBoxCurrent.Name = "pictureBoxCurrent";
            this.pictureBoxCurrent.Size = new System.Drawing.Size(50, 53);
            this.pictureBoxCurrent.TabIndex = 6;
            this.pictureBoxCurrent.TabStop = false;
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Location = new System.Drawing.Point(105, 13);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(78, 13);
            this.labelCurrent.TabIndex = 7;
            this.labelCurrent.Text = "Current Object:";
            // 
            // labelSelected
            // 
            this.labelSelected.AutoSize = true;
            this.labelSelected.Location = new System.Drawing.Point(105, 33);
            this.labelSelected.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSelected.Name = "labelSelected";
            this.labelSelected.Size = new System.Drawing.Size(0, 13);
            this.labelSelected.TabIndex = 8;
            // 
            // lablePlayerHealth
            // 
            this.lablePlayerHealth.AutoSize = true;
            this.lablePlayerHealth.Location = new System.Drawing.Point(19, 474);
            this.lablePlayerHealth.Name = "lablePlayerHealth";
            this.lablePlayerHealth.Size = new System.Drawing.Size(70, 13);
            this.lablePlayerHealth.TabIndex = 9;
            this.lablePlayerHealth.Text = "PlayerHealth:";
            // 
            // textBoxPlayerHealth
            // 
            this.textBoxPlayerHealth.Location = new System.Drawing.Point(95, 471);
            this.textBoxPlayerHealth.Name = "textBoxPlayerHealth";
            this.textBoxPlayerHealth.Size = new System.Drawing.Size(50, 20);
            this.textBoxPlayerHealth.TabIndex = 10;
            // 
            // groupBoxHoverInfo
            // 
            this.groupBoxHoverInfo.Controls.Add(this.panelHoverInfo);
            this.groupBoxHoverInfo.Location = new System.Drawing.Point(152, 371);
            this.groupBoxHoverInfo.Name = "groupBoxHoverInfo";
            this.groupBoxHoverInfo.Size = new System.Drawing.Size(157, 120);
            this.groupBoxHoverInfo.TabIndex = 11;
            this.groupBoxHoverInfo.TabStop = false;
            this.groupBoxHoverInfo.Text = "Hover Info";
            // 
            // panelHoverInfo
            // 
            this.panelHoverInfo.AutoScroll = true;
            this.panelHoverInfo.Controls.Add(this.labelHoverInfo);
            this.panelHoverInfo.Location = new System.Drawing.Point(6, 13);
            this.panelHoverInfo.Name = "panelHoverInfo";
            this.panelHoverInfo.Size = new System.Drawing.Size(145, 100);
            this.panelHoverInfo.TabIndex = 0;
            // 
            // labelHoverInfo
            // 
            this.labelHoverInfo.AutoSize = true;
            this.labelHoverInfo.Location = new System.Drawing.Point(3, 5);
            this.labelHoverInfo.Name = "labelHoverInfo";
            this.labelHoverInfo.Size = new System.Drawing.Size(0, 13);
            this.labelHoverInfo.TabIndex = 0;
            // 
            // labelObjectNum
            // 
            this.labelObjectNum.AutoSize = true;
            this.labelObjectNum.Location = new System.Drawing.Point(82, 200);
            this.labelObjectNum.Name = "labelObjectNum";
            this.labelObjectNum.Size = new System.Drawing.Size(64, 13);
            this.labelObjectNum.TabIndex = 8;
            this.labelObjectNum.Text = "Object num:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 502);
            this.Controls.Add(this.groupBoxHoverInfo);
            this.Controls.Add(this.textBoxPlayerHealth);
            this.Controls.Add(this.lablePlayerHealth);
            this.Controls.Add(this.labelSelected);
            this.Controls.Add(this.labelCurrent);
            this.Controls.Add(this.pictureBoxCurrent);
            this.Controls.Add(this.groupBoxSize);
            this.Controls.Add(this.groupBoxLevelOptions);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBoxLevel);
            this.Name = "FormMain";
            this.Text = "Main Level Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.groupBoxLevel.ResumeLayout(false);
            this.groupBoxLevelOptions.ResumeLayout(false);
            this.groupBoxLevelOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).EndInit();
            this.groupBoxSize.ResumeLayout(false);
            this.groupBoxSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrent)).EndInit();
            this.groupBoxHoverInfo.ResumeLayout(false);
            this.panelHoverInfo.ResumeLayout(false);
            this.panelHoverInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxLevel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.GroupBox groupBoxLevelOptions;
        private System.Windows.Forms.Button buttonInvisibleSolid;
        private System.Windows.Forms.Button buttonSolid;
        private System.Windows.Forms.Button buttonPlayer;
        private System.Windows.Forms.Button buttonEnemy;
        private System.Windows.Forms.GroupBox groupBoxSize;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Button buttonSizeConfirm;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Panel panelLevelObjects;
        private System.Windows.Forms.PictureBox pictureBoxCurrent;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Label labelSelected;
        private System.Windows.Forms.Label lablePlayerHealth;
        private System.Windows.Forms.TextBox textBoxPlayerHealth;
        private System.Windows.Forms.Button buttonErase;
        private System.Windows.Forms.Button buttonGoal;
        private System.Windows.Forms.ComboBox TextureSelector;
        private System.Windows.Forms.NumericUpDown numericUpDownPriority;
        private System.Windows.Forms.GroupBox groupBoxHoverInfo;
        private System.Windows.Forms.Panel panelHoverInfo;
        private System.Windows.Forms.Label labelHoverInfo;
        private System.Windows.Forms.Label labelObjectNum;
    }
}

