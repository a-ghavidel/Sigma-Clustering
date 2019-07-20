namespace PhylogeneticTree
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbtnPearsonCorrelation = new System.Windows.Forms.RadioButton();
            this.rbtnEuclidean = new System.Windows.Forms.RadioButton();
            this.rbtnCoVariance = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPP = new System.Windows.Forms.TextBox();
            this.chbGapIgnore = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbEqualLengths = new System.Windows.Forms.CheckBox();
            this.txtNumberOfSequences = new System.Windows.Forms.TextBox();
            this.cbNotification = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbPriority = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.tpAlgorithm = new System.Windows.Forms.TabPage();
            this.tpTree = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cmbImageType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chbShowAdditionalinfo = new System.Windows.Forms.CheckBox();
            this.txtIDLength = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbGapOpenPenalty = new System.Windows.Forms.TextBox();
            this.cmbGapExtendPenalty = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpAlgorithm.SuspendLayout();
            this.tpTree.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbGapExtendPenalty);
            this.groupBox1.Controls.Add(this.cmbGapOpenPenalty);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmbAlgorithm);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(579, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alignment Settings";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Gap Extend Penalty:";
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.DropDownHeight = 100;
            this.cmbAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.IntegralHeight = false;
            this.cmbAlgorithm.ItemHeight = 13;
            this.cmbAlgorithm.Items.AddRange(new object[] {
            "Smith–Waterman",
            "Needleman-Wunsch"});
            this.cmbAlgorithm.Location = new System.Drawing.Point(152, 28);
            this.cmbAlgorithm.MaxDropDownItems = 3;
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(141, 21);
            this.cmbAlgorithm.TabIndex = 1;
            this.cmbAlgorithm.Text = global::PhylogeneticTree.Settings1.Default.Algorithm;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.rbtnPearsonCorrelation);
            this.panel3.Controls.Add(this.rbtnEuclidean);
            this.panel3.Controls.Add(this.rbtnCoVariance);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(359, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(212, 106);
            this.panel3.TabIndex = 4;
            // 
            // rbtnPearsonCorrelation
            // 
            this.rbtnPearsonCorrelation.AutoSize = true;
            this.rbtnPearsonCorrelation.Checked = global::PhylogeneticTree.Settings1.Default.Pearson;
            this.rbtnPearsonCorrelation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnPearsonCorrelation.Location = new System.Drawing.Point(6, 80);
            this.rbtnPearsonCorrelation.Name = "rbtnPearsonCorrelation";
            this.rbtnPearsonCorrelation.Size = new System.Drawing.Size(117, 17);
            this.rbtnPearsonCorrelation.TabIndex = 43;
            this.rbtnPearsonCorrelation.Text = "Pearson Correlation";
            this.rbtnPearsonCorrelation.UseVisualStyleBackColor = true;
            // 
            // rbtnEuclidean
            // 
            this.rbtnEuclidean.AutoSize = true;
            this.rbtnEuclidean.Checked = global::PhylogeneticTree.Settings1.Default.Euclidean;
            this.rbtnEuclidean.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnEuclidean.Location = new System.Drawing.Point(6, 54);
            this.rbtnEuclidean.Name = "rbtnEuclidean";
            this.rbtnEuclidean.Size = new System.Drawing.Size(117, 17);
            this.rbtnEuclidean.TabIndex = 42;
            this.rbtnEuclidean.TabStop = true;
            this.rbtnEuclidean.Text = "Euclidean Distance";
            this.rbtnEuclidean.UseVisualStyleBackColor = true;
            // 
            // rbtnCoVariance
            // 
            this.rbtnCoVariance.AutoSize = true;
            this.rbtnCoVariance.Checked = global::PhylogeneticTree.Settings1.Default.CoVariance;
            this.rbtnCoVariance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnCoVariance.Location = new System.Drawing.Point(6, 27);
            this.rbtnCoVariance.Name = "rbtnCoVariance";
            this.rbtnCoVariance.Size = new System.Drawing.Size(80, 17);
            this.rbtnCoVariance.TabIndex = 41;
            this.rbtnCoVariance.Text = "CoVariance";
            this.rbtnCoVariance.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Distance Function Type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Algorithm:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Gap Open Penalty:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtThreshold);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtPP);
            this.groupBox2.Controls.Add(this.chbGapIgnore);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.groupBox2.Location = new System.Drawing.Point(6, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(579, 64);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cluster Settings";
            // 
            // txtThreshold
            // 
            this.txtThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThreshold.Location = new System.Drawing.Point(361, 28);
            this.txtThreshold.MaxLength = 5;
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.Size = new System.Drawing.Size(50, 20);
            this.txtThreshold.TabIndex = 6;
            this.txtThreshold.Text = global::PhylogeneticTree.Settings1.Default.Threshold_Size;
            this.txtThreshold.TextChanged += new System.EventHandler(this.txtThreshold_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(182, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "%";
            // 
            // txtPP
            // 
            this.txtPP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPP.Location = new System.Drawing.Point(152, 28);
            this.txtPP.MaxLength = 2;
            this.txtPP.Name = "txtPP";
            this.txtPP.Size = new System.Drawing.Size(30, 20);
            this.txtPP.TabIndex = 5;
            this.txtPP.Text = global::PhylogeneticTree.Settings1.Default.Presence_Percentage;
            // 
            // chbGapIgnore
            // 
            this.chbGapIgnore.AutoSize = true;
            this.chbGapIgnore.Checked = true;
            this.chbGapIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbGapIgnore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chbGapIgnore.Location = new System.Drawing.Point(452, 29);
            this.chbGapIgnore.Name = "chbGapIgnore";
            this.chbGapIgnore.Size = new System.Drawing.Size(98, 17);
            this.chbGapIgnore.TabIndex = 7;
            this.chbGapIgnore.Text = "Ignore All Gaps";
            this.chbGapIgnore.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(253, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Variance Threshold:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Presence Percentage:";
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnDefault.Location = new System.Drawing.Point(451, 170);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(110, 27);
            this.btnDefault.TabIndex = 9;
            this.btnDefault.Text = "Default Settings";
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnSave.Location = new System.Drawing.Point(138, 279);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 25);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save and close";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnCancel.Location = new System.Drawing.Point(375, 279);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbEqualLengths);
            this.groupBox3.Controls.Add(this.txtNumberOfSequences);
            this.groupBox3.Controls.Add(this.cbNotification);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.btnDefault);
            this.groupBox3.Controls.Add(this.cmbPriority);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.groupBox3.Location = new System.Drawing.Point(6, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(579, 205);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General Settings";
            // 
            // cbEqualLengths
            // 
            this.cbEqualLengths.AutoSize = true;
            this.cbEqualLengths.Checked = true;
            this.cbEqualLengths.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEqualLengths.Location = new System.Drawing.Point(9, 90);
            this.cbEqualLengths.Name = "cbEqualLengths";
            this.cbEqualLengths.Size = new System.Drawing.Size(270, 17);
            this.cbEqualLengths.TabIndex = 20;
            this.cbEqualLengths.Text = "Show notification if sequences have different length";
            this.cbEqualLengths.UseVisualStyleBackColor = true;
            // 
            // txtNumberOfSequences
            // 
            this.txtNumberOfSequences.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PhylogeneticTree.Settings1.Default, "NumberOfSequences", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtNumberOfSequences.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberOfSequences.Location = new System.Drawing.Point(361, 55);
            this.txtNumberOfSequences.MaxLength = 3;
            this.txtNumberOfSequences.Name = "txtNumberOfSequences";
            this.txtNumberOfSequences.Size = new System.Drawing.Size(37, 20);
            this.txtNumberOfSequences.TabIndex = 18;
            this.txtNumberOfSequences.Text = global::PhylogeneticTree.Settings1.Default.NumberOfSequences;
            // 
            // cbNotification
            // 
            this.cbNotification.AutoSize = true;
            this.cbNotification.Checked = true;
            this.cbNotification.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNotification.Location = new System.Drawing.Point(9, 35);
            this.cbNotification.Name = "cbNotification";
            this.cbNotification.Size = new System.Drawing.Size(260, 17);
            this.cbNotification.TabIndex = 17;
            this.cbNotification.Text = "Show notification about the number of sequences";
            this.cbNotification.UseVisualStyleBackColor = true;
            this.cbNotification.CheckedChanged += new System.EventHandler(this.cbNotification_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(44, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(253, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Show message if the number of sequences exceeds";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.854546F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Process Priority:";
            // 
            // cmbPriority
            // 
            this.cmbPriority.DropDownHeight = 100;
            this.cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriority.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPriority.FormattingEnabled = true;
            this.cmbPriority.IntegralHeight = false;
            this.cmbPriority.ItemHeight = 13;
            this.cmbPriority.Items.AddRange(new object[] {
            "RealTime",
            "High",
            "AboveNormal",
            "Normal"});
            this.cmbPriority.Location = new System.Drawing.Point(117, 149);
            this.cmbPriority.MaxDropDownItems = 3;
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(141, 21);
            this.cmbPriority.TabIndex = 8;
            this.cmbPriority.SelectedIndexChanged += new System.EventHandler(this.cmbPriority_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGeneral);
            this.tabControl1.Controls.Add(this.tpAlgorithm);
            this.tabControl1.Controls.Add(this.tpTree);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(602, 263);
            this.tabControl1.TabIndex = 12;
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.groupBox3);
            this.tpGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(594, 237);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            // 
            // tpAlgorithm
            // 
            this.tpAlgorithm.Controls.Add(this.groupBox1);
            this.tpAlgorithm.Controls.Add(this.groupBox2);
            this.tpAlgorithm.Location = new System.Drawing.Point(4, 22);
            this.tpAlgorithm.Name = "tpAlgorithm";
            this.tpAlgorithm.Padding = new System.Windows.Forms.Padding(3);
            this.tpAlgorithm.Size = new System.Drawing.Size(594, 237);
            this.tpAlgorithm.TabIndex = 1;
            this.tpAlgorithm.Text = "Algorithm";
            this.tpAlgorithm.UseVisualStyleBackColor = true;
            // 
            // tpTree
            // 
            this.tpTree.Controls.Add(this.groupBox5);
            this.tpTree.Controls.Add(this.groupBox4);
            this.tpTree.Location = new System.Drawing.Point(4, 22);
            this.tpTree.Name = "tpTree";
            this.tpTree.Size = new System.Drawing.Size(594, 237);
            this.tpTree.TabIndex = 2;
            this.tpTree.Text = "Tree";
            this.tpTree.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cmbImageType);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Location = new System.Drawing.Point(6, 125);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(579, 100);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Save";
            // 
            // cmbImageType
            // 
            this.cmbImageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImageType.FormattingEnabled = true;
            this.cmbImageType.Items.AddRange(new object[] {
            "JPEG",
            "Bitmap"});
            this.cmbImageType.Location = new System.Drawing.Point(123, 40);
            this.cmbImageType.MaxDropDownItems = 2;
            this.cmbImageType.MaxLength = 10;
            this.cmbImageType.Name = "cmbImageType";
            this.cmbImageType.Size = new System.Drawing.Size(58, 21);
            this.cmbImageType.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Preferred  Image type:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chbShowAdditionalinfo);
            this.groupBox4.Controls.Add(this.txtIDLength);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Location = new System.Drawing.Point(6, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(579, 100);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output Result";
            // 
            // chbShowAdditionalinfo
            // 
            this.chbShowAdditionalinfo.AutoSize = true;
            this.chbShowAdditionalinfo.Location = new System.Drawing.Point(9, 55);
            this.chbShowAdditionalinfo.Name = "chbShowAdditionalinfo";
            this.chbShowAdditionalinfo.Size = new System.Drawing.Size(363, 17);
            this.chbShowAdditionalinfo.TabIndex = 7;
            this.chbShowAdditionalinfo.Text = "Show additional information (Such as number of clusters, variance, etc.)";
            this.chbShowAdditionalinfo.UseVisualStyleBackColor = true;
            // 
            // txtIDLength
            // 
            this.txtIDLength.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PhylogeneticTree.Settings1.Default, "IDLength", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtIDLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIDLength.Location = new System.Drawing.Point(241, 26);
            this.txtIDLength.MaxLength = 2;
            this.txtIDLength.Name = "txtIDLength";
            this.txtIDLength.Size = new System.Drawing.Size(30, 20);
            this.txtIDLength.TabIndex = 6;
            this.txtIDLength.Text = global::PhylogeneticTree.Settings1.Default.IDLength;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(229, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "The length of the name of each sequence (ID):";
            // 
            // cmbGapOpenPenalty
            // 
            this.cmbGapOpenPenalty.Location = new System.Drawing.Point(152, 60);
            this.cmbGapOpenPenalty.Name = "cmbGapOpenPenalty";
            this.cmbGapOpenPenalty.Size = new System.Drawing.Size(141, 19);
            this.cmbGapOpenPenalty.TabIndex = 17;
            // 
            // cmbGapExtendPenalty
            // 
            this.cmbGapExtendPenalty.Location = new System.Drawing.Point(152, 96);
            this.cmbGapExtendPenalty.Name = "cmbGapExtendPenalty";
            this.cmbGapExtendPenalty.Size = new System.Drawing.Size(141, 19);
            this.cmbGapExtendPenalty.TabIndex = 17;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 312);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpAlgorithm.ResumeLayout(false);
            this.tpTree.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbtnPearsonCorrelation;
        private System.Windows.Forms.RadioButton rbtnEuclidean;
        private System.Windows.Forms.RadioButton rbtnCoVariance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPP;
        private System.Windows.Forms.CheckBox chbGapIgnore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbPriority;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpAlgorithm;
        private System.Windows.Forms.TextBox txtNumberOfSequences;
        private System.Windows.Forms.CheckBox cbNotification;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbEqualLengths;
        private System.Windows.Forms.TabPage tpTree;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtIDLength;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chbShowAdditionalinfo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbImageType;
        private System.Windows.Forms.TextBox cmbGapExtendPenalty;
        private System.Windows.Forms.TextBox cmbGapOpenPenalty;
    }
}