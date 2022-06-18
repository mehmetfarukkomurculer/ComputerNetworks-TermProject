namespace last_server
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
            this.porttext = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listenbutton = new System.Windows.Forms.Button();
            this.serverlogs = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // porttext
            // 
            this.porttext.Location = new System.Drawing.Point(161, 27);
            this.porttext.Name = "porttext";
            this.porttext.Size = new System.Drawing.Size(100, 20);
            this.porttext.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port: ";
            // 
            // listenbutton
            // 
            this.listenbutton.Location = new System.Drawing.Point(324, 30);
            this.listenbutton.Name = "listenbutton";
            this.listenbutton.Size = new System.Drawing.Size(75, 23);
            this.listenbutton.TabIndex = 2;
            this.listenbutton.Text = "Listen";
            this.listenbutton.UseVisualStyleBackColor = true;
            this.listenbutton.Click += new System.EventHandler(this.listenbutton_Click);
            // 
            // serverlogs
            // 
            this.serverlogs.Location = new System.Drawing.Point(135, 79);
            this.serverlogs.Name = "serverlogs";
            this.serverlogs.ReadOnly = true;
            this.serverlogs.Size = new System.Drawing.Size(404, 248);
            this.serverlogs.TabIndex = 3;
            this.serverlogs.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 421);
            this.Controls.Add(this.serverlogs);
            this.Controls.Add(this.listenbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.porttext);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox porttext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button listenbutton;
        private System.Windows.Forms.RichTextBox serverlogs;
    }
}

