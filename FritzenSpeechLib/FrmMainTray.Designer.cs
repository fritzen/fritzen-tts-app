namespace FritzenSpeech
{
    partial class FrmMainTray
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainTray));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxLang = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxPTBR = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxEN = new System.Windows.Forms.ToolStripMenuItem();
            this.label9 = new System.Windows.Forms.Label();
            this.ctxStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipText = "aaaa";
            this.trayIcon.BalloonTipTitle = "aaaa";
            this.trayIcon.Text = "notifyIcon1";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            // 
            // ctxStatus
            // 
            this.ctxStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxAbout,
            this.toolStripSeparator1,
            this.ctxRestore,
            this.ctxExit,
            this.ctxLang});
            this.ctxStatus.Name = "ctxStatus";
            this.ctxStatus.ShowImageMargin = false;
            this.ctxStatus.Size = new System.Drawing.Size(156, 120);
            // 
            // ctxAbout
            // 
            this.ctxAbout.Name = "ctxAbout";
            this.ctxAbout.Size = new System.Drawing.Size(155, 22);
            this.ctxAbout.Text = "&Sobre";
            this.ctxAbout.Visible = false;
            this.ctxAbout.Click += new System.EventHandler(this.ctxAbout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
            // 
            // ctxRestore
            // 
            this.ctxRestore.DoubleClickEnabled = true;
            this.ctxRestore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ctxRestore.Name = "ctxRestore";
            this.ctxRestore.Size = new System.Drawing.Size(155, 22);
            this.ctxRestore.Text = "&Restore";
            this.ctxRestore.Click += new System.EventHandler(this.ctxRestore_Click);
            // 
            // ctxExit
            // 
            this.ctxExit.Name = "ctxExit";
            this.ctxExit.Size = new System.Drawing.Size(155, 22);
            this.ctxExit.Text = "E&xit";
            this.ctxExit.Click += new System.EventHandler(this.ctxExit_Click);
            // 
            // ctxLang
            // 
            this.ctxLang.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxPTBR,
            this.ctxEN});
            this.ctxLang.Name = "ctxLang";
            this.ctxLang.Size = new System.Drawing.Size(155, 22);
            this.ctxLang.Text = "Idioma / Language";
            this.ctxLang.Visible = false;
            // 
            // ctxPTBR
            // 
            this.ctxPTBR.Checked = true;
            this.ctxPTBR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ctxPTBR.Name = "ctxPTBR";
            this.ctxPTBR.Size = new System.Drawing.Size(180, 22);
            this.ctxPTBR.Text = "Português";
            this.ctxPTBR.Click += new System.EventHandler(this.ctxPTBR_Click);
            // 
            // ctxEN
            // 
            this.ctxEN.Name = "ctxEN";
            this.ctxEN.Size = new System.Drawing.Size(180, 22);
            this.ctxEN.Text = "English";
            this.ctxEN.Click += new System.EventHandler(this.ctxEN_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(30, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(180, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Copyright © 2020 FRITZEN.IO";
            // 
            // FrmMainTray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(242, 72);
            this.ControlBox = false;
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmMainTray";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Fritzen Speech Monitor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMainTray_FormClosed);
            this.Load += new System.EventHandler(this.FrmMainTray_Load);
            this.ctxStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip ctxStatus;
        private System.Windows.Forms.ToolStripMenuItem ctxAbout;
        private System.Windows.Forms.ToolStripMenuItem ctxRestore;
        private System.Windows.Forms.ToolStripMenuItem ctxExit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ctxLang;
        private System.Windows.Forms.ToolStripMenuItem ctxPTBR;
        private System.Windows.Forms.ToolStripMenuItem ctxEN;
    }
}

