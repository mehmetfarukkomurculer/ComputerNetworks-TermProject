namespace last_client
{
    partial class listBox
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.iptext = new System.Windows.Forms.TextBox();
            this.porttext = new System.Windows.Forms.TextBox();
            this.usernametext = new System.Windows.Forms.TextBox();
            this.posttext = new System.Windows.Forms.TextBox();
            this.clientlogs = new System.Windows.Forms.RichTextBox();
            this.connectbutton = new System.Windows.Forms.Button();
            this.disconnectbutton = new System.Windows.Forms.Button();
            this.sendbutton = new System.Windows.Forms.Button();
            this.allpostsbutton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.addTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.deleteText = new System.Windows.Forms.TextBox();
            this.deletePost = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.friendsPostsButton = new System.Windows.Forms.Button();
            this.myPostsButton = new System.Windows.Forms.Button();
            this.listBoxFriends = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Username: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 301);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Post: ";
            // 
            // iptext
            // 
            this.iptext.Location = new System.Drawing.Point(98, 19);
            this.iptext.Name = "iptext";
            this.iptext.Size = new System.Drawing.Size(100, 20);
            this.iptext.TabIndex = 4;
            // 
            // porttext
            // 
            this.porttext.Location = new System.Drawing.Point(98, 47);
            this.porttext.Name = "porttext";
            this.porttext.Size = new System.Drawing.Size(100, 20);
            this.porttext.TabIndex = 5;
            // 
            // usernametext
            // 
            this.usernametext.Location = new System.Drawing.Point(98, 75);
            this.usernametext.Name = "usernametext";
            this.usernametext.Size = new System.Drawing.Size(100, 20);
            this.usernametext.TabIndex = 6;
            // 
            // posttext
            // 
            this.posttext.Enabled = false;
            this.posttext.Location = new System.Drawing.Point(88, 301);
            this.posttext.Name = "posttext";
            this.posttext.Size = new System.Drawing.Size(100, 20);
            this.posttext.TabIndex = 7;
            // 
            // clientlogs
            // 
            this.clientlogs.Location = new System.Drawing.Point(324, 17);
            this.clientlogs.Name = "clientlogs";
            this.clientlogs.ReadOnly = true;
            this.clientlogs.Size = new System.Drawing.Size(261, 308);
            this.clientlogs.TabIndex = 8;
            this.clientlogs.Text = "";
            // 
            // connectbutton
            // 
            this.connectbutton.Location = new System.Drawing.Point(218, 17);
            this.connectbutton.Name = "connectbutton";
            this.connectbutton.Size = new System.Drawing.Size(75, 23);
            this.connectbutton.TabIndex = 9;
            this.connectbutton.Text = "Connect";
            this.connectbutton.UseVisualStyleBackColor = true;
            this.connectbutton.Click += new System.EventHandler(this.connectbutton_Click);
            // 
            // disconnectbutton
            // 
            this.disconnectbutton.Enabled = false;
            this.disconnectbutton.Location = new System.Drawing.Point(217, 69);
            this.disconnectbutton.Name = "disconnectbutton";
            this.disconnectbutton.Size = new System.Drawing.Size(75, 23);
            this.disconnectbutton.TabIndex = 10;
            this.disconnectbutton.Text = "Disconnect";
            this.disconnectbutton.UseVisualStyleBackColor = true;
            this.disconnectbutton.Click += new System.EventHandler(this.disconnectbutton_Click);
            // 
            // sendbutton
            // 
            this.sendbutton.Enabled = false;
            this.sendbutton.Location = new System.Drawing.Point(217, 301);
            this.sendbutton.Name = "sendbutton";
            this.sendbutton.Size = new System.Drawing.Size(75, 23);
            this.sendbutton.TabIndex = 11;
            this.sendbutton.Text = "Send";
            this.sendbutton.UseVisualStyleBackColor = true;
            this.sendbutton.Click += new System.EventHandler(this.sendbutton_Click);
            // 
            // allpostsbutton
            // 
            this.allpostsbutton.Enabled = false;
            this.allpostsbutton.Location = new System.Drawing.Point(324, 340);
            this.allpostsbutton.Name = "allpostsbutton";
            this.allpostsbutton.Size = new System.Drawing.Size(75, 23);
            this.allpostsbutton.TabIndex = 12;
            this.allpostsbutton.Text = "All Posts";
            this.allpostsbutton.UseVisualStyleBackColor = true;
            this.allpostsbutton.Click += new System.EventHandler(this.allpostsbutton_Click);
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Location = new System.Drawing.Point(217, 266);
            this.addButton.Margin = new System.Windows.Forms.Padding(2);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 21);
            this.addButton.TabIndex = 14;
            this.addButton.Text = "Add Friend";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Enabled = false;
            this.removeButton.Location = new System.Drawing.Point(110, 224);
            this.removeButton.Margin = new System.Windows.Forms.Padding(2);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(109, 27);
            this.removeButton.TabIndex = 15;
            this.removeButton.Text = "Remove Friend";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // addTextBox
            // 
            this.addTextBox.Enabled = false;
            this.addTextBox.Location = new System.Drawing.Point(88, 270);
            this.addTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.addTextBox.Name = "addTextBox";
            this.addTextBox.Size = new System.Drawing.Size(100, 20);
            this.addTextBox.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 272);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Username";
            // 
            // deleteText
            // 
            this.deleteText.Enabled = false;
            this.deleteText.Location = new System.Drawing.Point(88, 338);
            this.deleteText.Name = "deleteText";
            this.deleteText.Size = new System.Drawing.Size(100, 20);
            this.deleteText.TabIndex = 18;
            // 
            // deletePost
            // 
            this.deletePost.Enabled = false;
            this.deletePost.Location = new System.Drawing.Point(217, 335);
            this.deletePost.Name = "deletePost";
            this.deletePost.Size = new System.Drawing.Size(75, 23);
            this.deletePost.TabIndex = 19;
            this.deletePost.Text = "Delete";
            this.deletePost.UseVisualStyleBackColor = true;
            this.deletePost.Click += new System.EventHandler(this.deletePost_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 340);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Post ID: ";
            // 
            // friendsPostsButton
            // 
            this.friendsPostsButton.Enabled = false;
            this.friendsPostsButton.Location = new System.Drawing.Point(468, 340);
            this.friendsPostsButton.Name = "friendsPostsButton";
            this.friendsPostsButton.Size = new System.Drawing.Size(116, 23);
            this.friendsPostsButton.TabIndex = 21;
            this.friendsPostsButton.Text = "Friends\' Posts";
            this.friendsPostsButton.UseVisualStyleBackColor = true;
            this.friendsPostsButton.Click += new System.EventHandler(this.friendsPostsButton_Click);
            // 
            // myPostsButton
            // 
            this.myPostsButton.Enabled = false;
            this.myPostsButton.Location = new System.Drawing.Point(394, 369);
            this.myPostsButton.Name = "myPostsButton";
            this.myPostsButton.Size = new System.Drawing.Size(75, 23);
            this.myPostsButton.TabIndex = 22;
            this.myPostsButton.Text = "My Posts";
            this.myPostsButton.UseVisualStyleBackColor = true;
            this.myPostsButton.Click += new System.EventHandler(this.myPostsButton_Click);
            // 
            // listBoxFriends
            // 
            this.listBoxFriends.Enabled = false;
            this.listBoxFriends.FormattingEnabled = true;
            this.listBoxFriends.Location = new System.Drawing.Point(23, 101);
            this.listBoxFriends.Name = "listBoxFriends";
            this.listBoxFriends.Size = new System.Drawing.Size(270, 108);
            this.listBoxFriends.TabIndex = 23;
            this.listBoxFriends.SelectedIndexChanged += new System.EventHandler(this.listBoxFriends_SelectedIndexChanged);
            // 
            // listBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 400);
            this.Controls.Add(this.listBoxFriends);
            this.Controls.Add(this.myPostsButton);
            this.Controls.Add(this.friendsPostsButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.deletePost);
            this.Controls.Add(this.deleteText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.addTextBox);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.allpostsbutton);
            this.Controls.Add(this.sendbutton);
            this.Controls.Add(this.disconnectbutton);
            this.Controls.Add(this.connectbutton);
            this.Controls.Add(this.clientlogs);
            this.Controls.Add(this.posttext);
            this.Controls.Add(this.usernametext);
            this.Controls.Add(this.porttext);
            this.Controls.Add(this.iptext);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "listBox";
            this.Text = "ListBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox iptext;
        private System.Windows.Forms.TextBox porttext;
        private System.Windows.Forms.TextBox usernametext;
        private System.Windows.Forms.TextBox posttext;
        private System.Windows.Forms.RichTextBox clientlogs;
        private System.Windows.Forms.Button connectbutton;
        private System.Windows.Forms.Button disconnectbutton;
        private System.Windows.Forms.Button sendbutton;
        private System.Windows.Forms.Button allpostsbutton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.TextBox addTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox deleteText;
        private System.Windows.Forms.Button deletePost;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button friendsPostsButton;
        private System.Windows.Forms.Button myPostsButton;
        private System.Windows.Forms.ListBox listBoxFriends;
    }
}

