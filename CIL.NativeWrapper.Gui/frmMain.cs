#region Copyright (c) 2012-2020 CIL
/*
CIL.NativeWrapper Software Component Product
Copyright (c) 2012-2020 CIL


Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

	1.	Redistributions of source code must retain the above copyright notice,
			this list of conditions and the following disclaimer.

	2.	Redistributions in binary form must reproduce the above copyright notice,
			this list of conditions and the following disclaimer in the documentation
			and/or other materials provided with the distribution.

	3.	The names of the authors may not be used to endorse or promote products derived
			from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED “AS IS” AND ANY EXPRESSED OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL CIL NativeWrapper
OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Runtime;
using System.Windows.Forms;
using System.Diagnostics;

using CIL.NativeWrapper;
using CIL.NativeWrapper.Api;

namespace CIL.NativeWrapper.Gui
{
	/// <summary>
	/// NativeWrapper main user interface form.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private Label lblHeader;
		private Label lblFirstStep;
		private Label lblSecondStep;
		private Label lblThirdStep;

		private Button btnOpen;
		private Button btnPack;
		private Button btnRun;

		private OpenFileDialog dSource;
		private SaveFileDialog dDestination;

		GuiState state;
		string[] files;
		string packedFile;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblRuntime;
		private System.Windows.Forms.ComboBox cbRuntime;

		enum GuiState
		{
			FirstStep,
			SecondStep,
			ThirdStep
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
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
			this.btnOpen = new System.Windows.Forms.Button();
			this.dSource = new System.Windows.Forms.OpenFileDialog();
			this.dDestination = new System.Windows.Forms.SaveFileDialog();
			this.btnPack = new System.Windows.Forms.Button();
			this.btnRun = new System.Windows.Forms.Button();
			this.lblHeader = new System.Windows.Forms.Label();
			this.lblFirstStep = new System.Windows.Forms.Label();
			this.lblSecondStep = new System.Windows.Forms.Label();
			this.lblThirdStep = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblRuntime = new System.Windows.Forms.Label();
			this.cbRuntime = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnOpen
			// 
			this.btnOpen.Location = new System.Drawing.Point(252, 68);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.TabIndex = 0;
			this.btnOpen.Text = "Open";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// dSource
			// 
			this.dSource.Multiselect = true;
			// 
			// btnPack
			// 
			this.btnPack.Location = new System.Drawing.Point(252, 104);
			this.btnPack.Name = "btnPack";
			this.btnPack.TabIndex = 1;
			this.btnPack.Text = "Pack";
			this.btnPack.Click += new System.EventHandler(this.btnPack_Click);
			// 
			// btnRun
			// 
			this.btnRun.Location = new System.Drawing.Point(252, 144);
			this.btnRun.Name = "btnRun";
			this.btnRun.TabIndex = 2;
			this.btnRun.Text = "Run";
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// lblHeader
			// 
			this.lblHeader.AutoSize = true;
			this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblHeader.Location = new System.Drawing.Point(18, 9);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(144, 16);
			this.lblHeader.TabIndex = 3;
			this.lblHeader.Text = "CIL Software NativeWrapper";
			// 
			// lblFirstStep
			// 
			this.lblFirstStep.AutoSize = true;
			this.lblFirstStep.Location = new System.Drawing.Point(30, 72);
			this.lblFirstStep.Name = "lblFirstStep";
			this.lblFirstStep.Size = new System.Drawing.Size(183, 16);
			this.lblFirstStep.TabIndex = 4;
			this.lblFirstStep.Text = "1. Step - Open your application files";
			// 
			// lblSecondStep
			// 
			this.lblSecondStep.AutoSize = true;
			this.lblSecondStep.Location = new System.Drawing.Point(30, 112);
			this.lblSecondStep.Name = "lblSecondStep";
			this.lblSecondStep.Size = new System.Drawing.Size(181, 16);
			this.lblSecondStep.TabIndex = 5;
			this.lblSecondStep.Text = "2. Step - Pack your application files";
			// 
			// lblThirdStep
			// 
			this.lblThirdStep.AutoSize = true;
			this.lblThirdStep.Location = new System.Drawing.Point(30, 152);
			this.lblThirdStep.Name = "lblThirdStep";
			this.lblThirdStep.Size = new System.Drawing.Size(153, 16);
			this.lblThirdStep.TabIndex = 6;
			this.lblThirdStep.Text = "3. Step - Run your application";
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(252, 188);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lblRuntime
			// 
			this.lblRuntime.Location = new System.Drawing.Point(124, 36);
			this.lblRuntime.Name = "lblRuntime";
			this.lblRuntime.Size = new System.Drawing.Size(76, 16);
			this.lblRuntime.TabIndex = 8;
			this.lblRuntime.Text = "Runtime:";
			// 
			// cbRuntime
			// 
			this.cbRuntime.Location = new System.Drawing.Point(208, 32);
			this.cbRuntime.Name = "cbRuntime";
			this.cbRuntime.Size = new System.Drawing.Size(121, 21);
			this.cbRuntime.TabIndex = 9;
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(334, 221);
			this.Controls.Add(this.cbRuntime);
			this.Controls.Add(this.lblRuntime);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblThirdStep);
			this.Controls.Add(this.lblSecondStep);
			this.Controls.Add(this.lblFirstStep);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.btnRun);
			this.Controls.Add(this.btnPack);
			this.Controls.Add(this.btnOpen);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.Text = "App Packer";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.ResumeLayout(false);

		}
		#endregion

		void OpenFiles()
		{
			DialogResult result = dSource.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				files = dSource.FileNames;
				btnPack.Enabled = true;
				Step(GuiState.SecondStep);
			}
		}
		void PackFiles()
		{
			DialogResult result = dDestination.ShowDialog(this);
			if (result == DialogResult.OK)
			{
				try
				{
					packedFile = dDestination.FileName;
					PackFiles(files, packedFile);
				}
				finally
				{
					Step(GuiState.ThirdStep);
				}
			}
		}
		void PackFiles(string[] files, string dstPath)
		{
			Packer packer = new Packer();
			string runtime = cbRuntime.SelectedItem as string;
			packer.Pack(files, dstPath, DefaultPacker.TypeName, runtime);
		}
		void Run(string path)
		{
			try
			{
				if (path == null || path.Length == 0)
					return;
				Process p = new Process();
				ProcessStartInfo startInfo = new ProcessStartInfo(path, String.Empty);
				p.StartInfo = startInfo;
				p.Start();
			}
			finally
			{
				files = null;
				packedFile = null;
				Step(GuiState.FirstStep);
			}
		}

		// ui steps...
		void Step(GuiState state)
		{
			switch (state)
			{
				case GuiState.FirstStep:
					FirstStep();
					break;

				case GuiState.SecondStep:
					SecondStep();
					break;

				case GuiState.ThirdStep:
					ThirdStep();
					break;
			}
		}
		void FirstStep()
		{
			state = GuiState.FirstStep;
			btnOpen.Enabled = true;
			btnPack.Enabled = false;
			btnRun.Enabled = false;
		}
		void SecondStep()
		{
			state = GuiState.SecondStep;
			btnOpen.Enabled = false;
			btnPack.Enabled = true;
			btnRun.Enabled = false;
		}
		void ThirdStep()
		{
			state = GuiState.ThirdStep;
			btnOpen.Enabled = false;
			btnPack.Enabled = false;
			btnRun.Enabled = true;
		}
		void LoadRuntimeVersions()
		{
			string[] versions = FrameworkHelper.GetRuntimeVersions();
			cbRuntime.Items.AddRange(versions);
			cbRuntime.SelectedIndex = 0;
		}

		// event handlers...
		void frmMain_Load(object sender, System.EventArgs e)
		{
			LoadRuntimeVersions();
			Step(GuiState.FirstStep);
		}
		void btnOpen_Click(object sender, System.EventArgs e)
		{
			OpenFiles();
		}
		void btnPack_Click(object sender, System.EventArgs e)
		{
			PackFiles();
		}
		void btnRun_Click(object sender, System.EventArgs e)
		{
			Run(packedFile);
		}
		void btnClose_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}