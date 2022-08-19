namespace ExclusiveProgram
{
    partial class Control
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.buttonInit = new System.Windows.Forms.Button();
            this.pictureBoxMain = new System.Windows.Forms.PictureBox();
            this.checkBoxShowMessage = new System.Windows.Forms.CheckBox();
            this.buttonReady = new System.Windows.Forms.Button();
            this.buttonGetImage = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownPocketY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPocketX = new System.Windows.Forms.NumericUpDown();
            this.radioButtonPocket5 = new System.Windows.Forms.RadioButton();
            this.radioButtonPocket2 = new System.Windows.Forms.RadioButton();
            this.radioButtonPocket4 = new System.Windows.Forms.RadioButton();
            this.radioButtonPocket1 = new System.Windows.Forms.RadioButton();
            this.radioButtonPocket3 = new System.Windows.Forms.RadioButton();
            this.radioButtonPocket0 = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPocketY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPocketX)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 161);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(29, 58);
            this.buttonInit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(71, 48);
            this.buttonInit.TabIndex = 1;
            this.buttonInit.Text = "Init";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // pictureBoxMain
            // 
            this.pictureBoxMain.Location = new System.Drawing.Point(264, 28);
            this.pictureBoxMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBoxMain.Name = "pictureBoxMain";
            this.pictureBoxMain.Size = new System.Drawing.Size(968, 611);
            this.pictureBoxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxMain.TabIndex = 2;
            this.pictureBoxMain.TabStop = false;
            // 
            // checkBoxShowMessage
            // 
            this.checkBoxShowMessage.AutoSize = true;
            this.checkBoxShowMessage.Checked = true;
            this.checkBoxShowMessage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowMessage.Location = new System.Drawing.Point(29, 28);
            this.checkBoxShowMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxShowMessage.Name = "checkBoxShowMessage";
            this.checkBoxShowMessage.Size = new System.Drawing.Size(137, 22);
            this.checkBoxShowMessage.TabIndex = 3;
            this.checkBoxShowMessage.Text = "Show Message";
            this.checkBoxShowMessage.UseVisualStyleBackColor = true;
            // 
            // buttonReady
            // 
            this.buttonReady.Location = new System.Drawing.Point(29, 112);
            this.buttonReady.Margin = new System.Windows.Forms.Padding(2);
            this.buttonReady.Name = "buttonReady";
            this.buttonReady.Size = new System.Drawing.Size(71, 44);
            this.buttonReady.TabIndex = 0;
            this.buttonReady.Text = "Ready";
            this.buttonReady.UseVisualStyleBackColor = true;
            this.buttonReady.Click += new System.EventHandler(this.buttonReady_Click);
            // 
            // buttonGetImage
            // 
            this.buttonGetImage.Location = new System.Drawing.Point(6, 47);
            this.buttonGetImage.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGetImage.Name = "buttonGetImage";
            this.buttonGetImage.Size = new System.Drawing.Size(112, 44);
            this.buttonGetImage.TabIndex = 0;
            this.buttonGetImage.Text = "拍照";
            this.buttonGetImage.UseVisualStyleBackColor = true;
            this.buttonGetImage.Click += new System.EventHandler(this.buttonGetImage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownPocketY);
            this.groupBox1.Controls.Add(this.numericUpDownPocketX);
            this.groupBox1.Controls.Add(this.radioButtonPocket5);
            this.groupBox1.Controls.Add(this.buttonGetImage);
            this.groupBox1.Controls.Add(this.radioButtonPocket2);
            this.groupBox1.Controls.Add(this.radioButtonPocket4);
            this.groupBox1.Controls.Add(this.radioButtonPocket1);
            this.groupBox1.Controls.Add(this.radioButtonPocket3);
            this.groupBox1.Controls.Add(this.radioButtonPocket0);
            this.groupBox1.Location = new System.Drawing.Point(29, 232);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(228, 437);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "球袋位置調整";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 280);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "X:";
            // 
            // numericUpDownPocketY
            // 
            this.numericUpDownPocketY.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPocketY.Location = new System.Drawing.Point(38, 314);
            this.numericUpDownPocketY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericUpDownPocketY.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numericUpDownPocketY.Name = "numericUpDownPocketY";
            this.numericUpDownPocketY.Size = new System.Drawing.Size(135, 29);
            this.numericUpDownPocketY.TabIndex = 1;
            this.numericUpDownPocketY.ValueChanged += new System.EventHandler(this.numericUpDownPocketY_ValueChanged);
            // 
            // numericUpDownPocketX
            // 
            this.numericUpDownPocketX.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownPocketX.Location = new System.Drawing.Point(38, 277);
            this.numericUpDownPocketX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericUpDownPocketX.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numericUpDownPocketX.Name = "numericUpDownPocketX";
            this.numericUpDownPocketX.Size = new System.Drawing.Size(135, 29);
            this.numericUpDownPocketX.TabIndex = 1;
            this.numericUpDownPocketX.ValueChanged += new System.EventHandler(this.numericUpDownPocketX_ValueChanged);
            // 
            // radioButtonPocket5
            // 
            this.radioButtonPocket5.AutoSize = true;
            this.radioButtonPocket5.Location = new System.Drawing.Point(7, 247);
            this.radioButtonPocket5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonPocket5.Name = "radioButtonPocket5";
            this.radioButtonPocket5.Size = new System.Drawing.Size(92, 22);
            this.radioButtonPocket5.TabIndex = 0;
            this.radioButtonPocket5.Text = "右下 紫";
            this.radioButtonPocket5.UseVisualStyleBackColor = true;
            this.radioButtonPocket5.CheckedChanged += new System.EventHandler(this.radioButtonPocket5_CheckedChanged);
            // 
            // radioButtonPocket2
            // 
            this.radioButtonPocket2.AutoSize = true;
            this.radioButtonPocket2.Location = new System.Drawing.Point(7, 157);
            this.radioButtonPocket2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonPocket2.Name = "radioButtonPocket2";
            this.radioButtonPocket2.Size = new System.Drawing.Size(92, 22);
            this.radioButtonPocket2.TabIndex = 0;
            this.radioButtonPocket2.Text = "右上 黃";
            this.radioButtonPocket2.UseVisualStyleBackColor = true;
            this.radioButtonPocket2.CheckedChanged += new System.EventHandler(this.radioButtonPocket2_CheckedChanged);
            // 
            // radioButtonPocket4
            // 
            this.radioButtonPocket4.AutoSize = true;
            this.radioButtonPocket4.Location = new System.Drawing.Point(7, 217);
            this.radioButtonPocket4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonPocket4.Name = "radioButtonPocket4";
            this.radioButtonPocket4.Size = new System.Drawing.Size(92, 22);
            this.radioButtonPocket4.TabIndex = 0;
            this.radioButtonPocket4.Text = "中下 藍";
            this.radioButtonPocket4.UseVisualStyleBackColor = true;
            this.radioButtonPocket4.CheckedChanged += new System.EventHandler(this.radioButtonPocket4_CheckedChanged);
            // 
            // radioButtonPocket1
            // 
            this.radioButtonPocket1.AutoSize = true;
            this.radioButtonPocket1.Location = new System.Drawing.Point(7, 127);
            this.radioButtonPocket1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonPocket1.Name = "radioButtonPocket1";
            this.radioButtonPocket1.Size = new System.Drawing.Size(92, 22);
            this.radioButtonPocket1.TabIndex = 0;
            this.radioButtonPocket1.Text = "中上 橙";
            this.radioButtonPocket1.UseVisualStyleBackColor = true;
            this.radioButtonPocket1.CheckedChanged += new System.EventHandler(this.radioButtonPocket1_CheckedChanged);
            // 
            // radioButtonPocket3
            // 
            this.radioButtonPocket3.AutoSize = true;
            this.radioButtonPocket3.Location = new System.Drawing.Point(7, 187);
            this.radioButtonPocket3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonPocket3.Name = "radioButtonPocket3";
            this.radioButtonPocket3.Size = new System.Drawing.Size(92, 22);
            this.radioButtonPocket3.TabIndex = 0;
            this.radioButtonPocket3.Text = "左下 綠";
            this.radioButtonPocket3.UseVisualStyleBackColor = true;
            this.radioButtonPocket3.CheckedChanged += new System.EventHandler(this.radioButtonPocket3_CheckedChanged);
            // 
            // radioButtonPocket0
            // 
            this.radioButtonPocket0.AutoSize = true;
            this.radioButtonPocket0.Checked = true;
            this.radioButtonPocket0.Location = new System.Drawing.Point(7, 97);
            this.radioButtonPocket0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonPocket0.Name = "radioButtonPocket0";
            this.radioButtonPocket0.Size = new System.Drawing.Size(92, 22);
            this.radioButtonPocket0.TabIndex = 0;
            this.radioButtonPocket0.TabStop = true;
            this.radioButtonPocket0.Text = "左上 紅";
            this.radioButtonPocket0.UseVisualStyleBackColor = true;
            this.radioButtonPocket0.CheckedChanged += new System.EventHandler(this.radioButtonPocket0_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(127, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 48);
            this.button2.TabIndex = 5;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxShowMessage);
            this.Controls.Add(this.pictureBoxMain);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.buttonReady);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Control";
            this.Size = new System.Drawing.Size(1281, 700);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPocketY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPocketX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.PictureBox pictureBoxMain;
        private System.Windows.Forms.CheckBox checkBoxShowMessage;
        private System.Windows.Forms.Button buttonReady;
        private System.Windows.Forms.Button buttonGetImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonPocket5;
        private System.Windows.Forms.RadioButton radioButtonPocket2;
        private System.Windows.Forms.RadioButton radioButtonPocket4;
        private System.Windows.Forms.RadioButton radioButtonPocket1;
        private System.Windows.Forms.RadioButton radioButtonPocket3;
        private System.Windows.Forms.RadioButton radioButtonPocket0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownPocketY;
        private System.Windows.Forms.NumericUpDown numericUpDownPocketX;
        private System.Windows.Forms.Button button2;
    }
}
