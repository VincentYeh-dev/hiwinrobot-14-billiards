namespace SimulationProgram
{
    partial class Form1
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
            this.pictureBoxMain = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownCueBallX = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCueBallY = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownObjBallY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownObjBallX = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCueBallX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCueBallY)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjBallY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjBallX)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxMain
            // 
            this.pictureBoxMain.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxMain.Name = "pictureBoxMain";
            this.pictureBoxMain.Size = new System.Drawing.Size(1512, 890);
            this.pictureBoxMain.TabIndex = 0;
            this.pictureBoxMain.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownCueBallY);
            this.groupBox1.Controls.Add(this.numericUpDownCueBallX);
            this.groupBox1.Location = new System.Drawing.Point(1576, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 162);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CueBall";
            // 
            // numericUpDownCueBallX
            // 
            this.numericUpDownCueBallX.Location = new System.Drawing.Point(45, 50);
            this.numericUpDownCueBallX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownCueBallX.Name = "numericUpDownCueBallX";
            this.numericUpDownCueBallX.Size = new System.Drawing.Size(120, 35);
            this.numericUpDownCueBallX.TabIndex = 0;
            this.numericUpDownCueBallX.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownCueBallX.ValueChanged += new System.EventHandler(this.numericUpDownCueBallX_ValueChanged);
            // 
            // numericUpDownCueBallY
            // 
            this.numericUpDownCueBallY.Location = new System.Drawing.Point(45, 104);
            this.numericUpDownCueBallY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownCueBallY.Name = "numericUpDownCueBallY";
            this.numericUpDownCueBallY.Size = new System.Drawing.Size(120, 35);
            this.numericUpDownCueBallY.TabIndex = 0;
            this.numericUpDownCueBallY.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDownCueBallY.ValueChanged += new System.EventHandler(this.numericUpDownCueBallY_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownObjBallY);
            this.groupBox2.Controls.Add(this.numericUpDownObjBallX);
            this.groupBox2.Location = new System.Drawing.Point(1576, 281);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(240, 162);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ObjBall";
            // 
            // numericUpDownObjBallY
            // 
            this.numericUpDownObjBallY.Location = new System.Drawing.Point(45, 104);
            this.numericUpDownObjBallY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownObjBallY.Name = "numericUpDownObjBallY";
            this.numericUpDownObjBallY.Size = new System.Drawing.Size(120, 35);
            this.numericUpDownObjBallY.TabIndex = 0;
            this.numericUpDownObjBallY.Value = new decimal(new int[] {
            140,
            0,
            0,
            0});
            this.numericUpDownObjBallY.ValueChanged += new System.EventHandler(this.numericUpDownObjBallY_ValueChanged);
            // 
            // numericUpDownObjBallX
            // 
            this.numericUpDownObjBallX.Location = new System.Drawing.Point(45, 50);
            this.numericUpDownObjBallX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownObjBallX.Name = "numericUpDownObjBallX";
            this.numericUpDownObjBallX.Size = new System.Drawing.Size(120, 35);
            this.numericUpDownObjBallX.TabIndex = 0;
            this.numericUpDownObjBallX.Value = new decimal(new int[] {
            230,
            0,
            0,
            0});
            this.numericUpDownObjBallX.ValueChanged += new System.EventHandler(this.numericUpDownObjBallX_ValueChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1919, 914);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBoxMain);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCueBallX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCueBallY)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjBallY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjBallX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxMain;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownCueBallY;
        private System.Windows.Forms.NumericUpDown numericUpDownCueBallX;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownObjBallY;
        private System.Windows.Forms.NumericUpDown numericUpDownObjBallX;
    }
}

