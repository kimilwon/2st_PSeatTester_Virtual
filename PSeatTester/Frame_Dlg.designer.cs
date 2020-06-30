namespace PSeatTester
{
    partial class Frame_Dlg
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "0x01",
            "2",
            "SubscriberAutoLength",
            "8",
            "Enhanced"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("");
            this.lvGFT = new System.Windows.Forms.ListView();
            this.ColGFTID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColGFTPID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColGFTDirect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColGFTLen = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColGFTChecksum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.pgGFTDef = new System.Windows.Forms.PropertyGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.imageButton1 = new UserImageButton.ImageButton();
            this.SuspendLayout();
            // 
            // lvGFT
            // 
            this.lvGFT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvGFT.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColGFTID,
            this.ColGFTPID,
            this.ColGFTDirect,
            this.ColGFTLen,
            this.ColGFTChecksum});
            this.lvGFT.FullRowSelect = true;
            this.lvGFT.HideSelection = false;
            this.lvGFT.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvGFT.Location = new System.Drawing.Point(14, 23);
            this.lvGFT.Name = "lvGFT";
            this.lvGFT.ShowItemToolTips = true;
            this.lvGFT.Size = new System.Drawing.Size(394, 486);
            this.lvGFT.TabIndex = 1;
            this.lvGFT.UseCompatibleStateImageBehavior = false;
            this.lvGFT.View = System.Windows.Forms.View.Details;
            this.lvGFT.SelectedIndexChanged += new System.EventHandler(this.lvGFT_SelectedIndexChanged);
            this.lvGFT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvGFT_KeyDown);
            // 
            // ColGFTID
            // 
            this.ColGFTID.Text = "ID";
            this.ColGFTID.Width = 41;
            // 
            // ColGFTPID
            // 
            this.ColGFTPID.Text = "PID";
            this.ColGFTPID.Width = 38;
            // 
            // ColGFTDirect
            // 
            this.ColGFTDirect.Text = "Direction";
            this.ColGFTDirect.Width = 124;
            // 
            // ColGFTLen
            // 
            this.ColGFTLen.Text = "Length";
            this.ColGFTLen.Width = 50;
            // 
            // ColGFTChecksum
            // 
            this.ColGFTChecksum.Text = "CST";
            this.ColGFTChecksum.Width = 61;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Global Frame Table:";
            // 
            // pgGFTDef
            // 
            this.pgGFTDef.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgGFTDef.Location = new System.Drawing.Point(413, 23);
            this.pgGFTDef.Name = "pgGFTDef";
            this.pgGFTDef.Size = new System.Drawing.Size(299, 486);
            this.pgGFTDef.TabIndex = 3;
            this.pgGFTDef.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgGFTDef_PropertyValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(415, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Properties:";
            // 
            // imageButton1
            // 
            this.imageButton1.BackColor = System.Drawing.Color.Transparent;
            this.imageButton1.ButtonColor = System.Drawing.Color.Blue;
            this.imageButton1.ButtonText = "닫기";
            this.imageButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.imageButton1.Image = global::PSeatTester.Properties.Resources.Folder___Home;
            this.imageButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.imageButton1.ImageSize = new System.Drawing.Size(28, 28);
            this.imageButton1.Location = new System.Drawing.Point(248, 519);
            this.imageButton1.Name = "imageButton1";
            this.imageButton1.Size = new System.Drawing.Size(233, 46);
            this.imageButton1.TabIndex = 4;
            this.imageButton1.Click += new System.EventHandler(this.ImageButton1_Click);
            // 
            // Frame_Dlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 572);
            this.Controls.Add(this.imageButton1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pgGFTDef);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvGFT);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(742, 275);
            this.Name = "Frame_Dlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Global Frame Table";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvGFT;
        private System.Windows.Forms.ColumnHeader ColGFTID;
        private System.Windows.Forms.ColumnHeader ColGFTPID;
        private System.Windows.Forms.ColumnHeader ColGFTDirect;
        private System.Windows.Forms.ColumnHeader ColGFTLen;
        private System.Windows.Forms.ColumnHeader ColGFTChecksum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid pgGFTDef;
        private System.Windows.Forms.Label label2;
        private UserImageButton.ImageButton imageButton1;
    }
}