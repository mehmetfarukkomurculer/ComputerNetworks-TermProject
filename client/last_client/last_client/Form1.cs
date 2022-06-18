using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace last_client
{
    public partial class listBox : Form
    {
        bool disconnected = false;
        bool connected = false;
        Socket clientSocket;
        string traceUsername;
        string removedUser;
        public listBox()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void connectbutton_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip;
            string ipaddress = iptext.Text;
            int PortNum;
            string username = usernametext.Text;
            if (Int32.TryParse(porttext.Text, out PortNum))
            {
                if (IPAddress.TryParse(ipaddress, out ip))
                {
                    var lines = File.ReadAllLines(@"../../user-db.txt");
                    List<string> usernames = new List<string>();
                    foreach (string line in lines)
                    {
                        usernames.Add(line);
                    }
                    if (usernames.Contains(username))
                    {
                        try
                        {
                            clientSocket.Connect(ipaddress, PortNum);
                            connected = true;
                            //define the username to send posts. 
                            traceUsername = username;
                            //buttons are enabled/disabled
                            connectbutton.Enabled = false;
                            iptext.Enabled = false;
                            porttext.Enabled = false;
                            usernametext.Enabled = false;
                            disconnectbutton.Enabled = true;
                            posttext.Enabled = true;
                            sendbutton.Enabled = true;
                            allpostsbutton.Enabled = true;
                            removeButton.Enabled = true;
                            addButton.Enabled = true;
                            addTextBox.Enabled = true;
                            deletePost.Enabled = true;
                            deleteText.Enabled = true;
                            friendsPostsButton.Enabled = true;
                            myPostsButton.Enabled = true;
                            listBoxFriends.Enabled = true;
                            //connection status is posted to the clientlogs
                            //sending the status message to the server
                            // message type is specified as usernameApproved and - is written to  
                            // make easy the message to be splitted by the server.
                            string status = "usernameApproved*" + username + " has connected!\n*" + username;
                            Byte[] buffer = Encoding.Default.GetBytes(status);
                            clientSocket.Send(buffer);
                            Thread receiveThread = new Thread(Receive);
                            receiveThread.Start();
                        }
                        catch
                        {
                            clientlogs.AppendText("Could not connect to the server!\n");
                        }
                    }
                    else
                    {
                        try
                        {
                            clientSocket.Connect(ipaddress, PortNum);
                            //connection status is posted to the clientlogs
                            clientlogs.AppendText("Please enter a valid username.\n");
                            connected = false;
                            //buttons are enabled/disabled
                            connectbutton.Enabled = true;
                            iptext.Enabled = true;
                            porttext.Enabled = true;
                            usernametext.Enabled = true;
                            disconnectbutton.Enabled = false;
                            posttext.Enabled = false;
                            sendbutton.Enabled = false;
                            allpostsbutton.Enabled = false;
                            removeButton.Enabled = false;
                            addButton.Enabled = false;
                            addTextBox.Enabled = false;
                            deletePost.Enabled = false;
                            deleteText.Enabled = false;
                            friendsPostsButton.Enabled = false;
                            myPostsButton.Enabled = false;
                            listBoxFriends.Enabled = false;
                            try
                            {
                                string status2 = "usernameNotApproved*" + username + " tried to connect to the server but cannot.\n";
                                Byte[] buffer1 = Encoding.Default.GetBytes(status2);
                                clientSocket.Send(buffer1);
                            }
                            catch
                            {
                                clientlogs.AppendText("Username Not Approved message could not send to the server!\n");
                            }
                            Thread receiveThread = new Thread(Receive);
                            receiveThread.Start();
                        }
                        catch
                        {
                            clientlogs.AppendText("Wrong port number!\n");
                        }    
                    }
                }
                else
                {
                    clientlogs.AppendText("Check the IP address!\n");
                }
            }
            else
            {
                clientlogs.AppendText("Check the port number!\n");
            }
        }

        private void Receive()
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[65536];
                    clientSocket.Receive(buffer);
                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    if (incomingMessage == "The server has disconnected!")
                    {
                        clientlogs.AppendText(incomingMessage + "\n");
                        connected = false;
                        clientSocket.Close();

                        connectbutton.Enabled = true;
                        iptext.Enabled = true;
                        porttext.Enabled = true;
                        usernametext.Enabled = true;
                        disconnectbutton.Enabled = false;
                        posttext.Enabled = false;
                        sendbutton.Enabled = false;
                        allpostsbutton.Enabled = false;
                        removeButton.Enabled = false;
                        addButton.Enabled = false;
                        addTextBox.Enabled = false;
                        deletePost.Enabled = false;
                        deleteText.Enabled = false;
                        friendsPostsButton.Enabled = false;
                        myPostsButton.Enabled = false;
                        listBoxFriends.Enabled = false;
                    }
                    else
                    {
                        string[] messageSplit = incomingMessage.Split('*');
                        if (messageSplit[0] == "connectionApproved")
                        {
                            if (messageSplit[4] == "offlineMessage")
                            {
                                if (messageSplit[5] != "X")
                                {
                                    messageSplit[5] = messageSplit[5].Remove(messageSplit[5].Length - 1);
                                    if (messageSplit[5].Contains(','))
                                    {
                                        string[] tt = messageSplit[5].Split(',');
                                        foreach (string t in tt)
                                        {
                                            clientlogs.AppendText(t + "\n");
                                        }
                                    }
                                    else
                                    {
                                        clientlogs.AppendText(messageSplit[5] + "\n");
                                    }
                                }
                                else
                                {
                                    clientlogs.AppendText("");
                                }

                            }
                            if (messageSplit[0] == "connectionApproved")
                            {
                                clientlogs.AppendText(messageSplit[1]);
                            }
                            if (messageSplit[2] == "ilkfriendList")
                            {
                                if (messageSplit[3] != "X")
                                {
                                    messageSplit[3] = messageSplit[3].Remove(messageSplit[3].Length - 1);
                                    listBoxFriends.Items.Clear();
                                    if (messageSplit[3].Contains(','))
                                    {
                                        string[] kk = messageSplit[3].Split(',');
                                        foreach (string k in kk)
                                        {
                                            listBoxFriends.Items.Add(k);
                                        }
                                    }
                                    else
                                    {
                                        listBoxFriends.Items.Add(messageSplit[3]);
                                    }
                                }
                                else
                                {
                                    listBoxFriends.Items.Clear();
                                }
                            }
                        }
                        else if (messageSplit[0] == "removeRejected")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "removeAccepted")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "allposts")
                        {
                            clientlogs.AppendText("Showing all posts from clients:\n");
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "friendsposts")
                        {
                            clientlogs.AppendText("Showing all posts of your friends:\n");
                            clientlogs.AppendText(messageSplit[1]);
                        }

                        else if (messageSplit[0] == "post")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "emptyFile")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "deletionApproved")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "deletionNotApproved")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "noDeletion")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "myposts")
                        {
                            clientlogs.AppendText("Showing my posts:\n");
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        
                        else if (messageSplit[0] == "alreadyConnected")
                        {
                            clientlogs.AppendText(traceUsername +" has already connected to the server!\n");
                            connected = false;
                            clientSocket.Close();

                            connectbutton.Enabled = true;
                            iptext.Enabled = true;
                            porttext.Enabled = true;
                            usernametext.Enabled = true;
                            disconnectbutton.Enabled = false;
                            posttext.Enabled = false;
                            listBoxFriends.Enabled = false;
                            sendbutton.Enabled = false;
                            allpostsbutton.Enabled = false;
                            removeButton.Enabled = false;
                            addButton.Enabled = false;
                            addTextBox.Enabled = false;
                            deletePost.Enabled = false;
                            deleteText.Enabled = false;
                            friendsPostsButton.Enabled = false;
                            myPostsButton.Enabled = false;
                        }
                        else if (messageSplit[0] == "otherAdded")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "removedBy")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        else if (messageSplit[0] == "addfriendApproved")
                        {
                            clientlogs.AppendText(messageSplit[1]);
                        }
                        
                        else if (messageSplit[0] == "friendList")
                        {
                            if (messageSplit[1] != "X")
                            {
                                messageSplit[1] = messageSplit[1].Remove(messageSplit[1].Length - 1);
                                listBoxFriends.Items.Clear();
                                if (messageSplit[1].Contains(','))
                                {
                                    string[] xx = messageSplit[1].Split(',');
                                    foreach (string x in xx)
                                    {
                                        listBoxFriends.Items.Add(x);
                                    }
                                }
                                else
                                {
                                    listBoxFriends.Items.Add(messageSplit[1]);
                                }
                            }
                            else
                            {
                                listBoxFriends.Items.Clear();
                            }  
                        }
                    }     
                }
                catch
                {
                    if (!disconnected)
                    {
                        connectbutton.Enabled = true;
                        iptext.Enabled = true;
                        porttext.Enabled = true;
                        usernametext.Enabled = true;
                        disconnectbutton.Enabled = false;
                        posttext.Enabled = false;
                        sendbutton.Enabled = false;
                        allpostsbutton.Enabled = false;
                        removeButton.Enabled = false;
                        addButton.Enabled = false;
                        addTextBox.Enabled = false;
                        deletePost.Enabled = false;
                        deleteText.Enabled = false;
                        friendsPostsButton.Enabled = false;
                        myPostsButton.Enabled = false;
                        listBoxFriends.Enabled = false;
                        listBoxFriends.Items.Clear();
                    }
                    clientSocket.Close();
                    connected = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (connected == true)
            {
                connected = false;
                disconnected = true;
                clientlogs.AppendText("Succesfully disconnected!\n");
                string disconnected_message = ("disconnect*" + traceUsername + " has disconnected!*"+traceUsername);
                Byte[] buffer44 = Encoding.Default.GetBytes(disconnected_message);
                clientSocket.Send(buffer44);
                listBoxFriends.Items.Clear();
                Environment.Exit(0);
            }
            else
            {
                disconnected = true;
                listBoxFriends.Items.Clear();
                Environment.Exit(0);
            }

            
        }

        private void sendbutton_Click(object sender, EventArgs e)
        {
            string post = posttext.Text;
            if (post != "")
            {
                clientlogs.AppendText("You have succesfully sent a post.\n");
                string serverpost = "post*" + traceUsername + "?" + post + "?" + DateTime.Now.ToLongDateString() + "?" + DateTime.Now.ToLongTimeString();
                Byte[] buffer2 = Encoding.Default.GetBytes(serverpost);
                clientSocket.Send(buffer2);
            }
            else
            {
                clientlogs.AppendText("Post cannot be empty\n");
            }
        }

        private void allpostsbutton_Click(object sender, EventArgs e)
        {
            string require = "allposts*" + traceUsername;
            Byte[] buffer3 = Encoding.Default.GetBytes(require);
            clientSocket.Send(buffer3);
        }

        private void disconnectbutton_Click(object sender, EventArgs e)
        {
            connected = false;
            disconnected = true;
            connectbutton.Enabled = true;
            iptext.Enabled = true;
            porttext.Enabled = true;
            usernametext.Enabled = true;
            disconnectbutton.Enabled = false;
            posttext.Enabled = false;
            sendbutton.Enabled = false;
            allpostsbutton.Enabled = false;
            removeButton.Enabled = false;
            addButton.Enabled = false;
            addTextBox.Enabled = false;
            deletePost.Enabled = false;
            deleteText.Enabled = false;
            friendsPostsButton.Enabled = false;
            myPostsButton.Enabled = false;
            listBoxFriends.Enabled = false;
            listBoxFriends.Items.Clear();
            clientlogs.AppendText("Succesfully disconnected!\n");
            string disconnected_message = ("disconnect*"+ traceUsername+" has disconnected!*"+traceUsername);
            Byte[] buffer44 = Encoding.Default.GetBytes(disconnected_message);
            clientSocket.Send(buffer44);
        }

        private void deletePost_Click(object sender, EventArgs e)
        {
            if (connected == true)
            {
                if (deleteText.Text != "")
                {
                    string requireDelete = "delete*" + traceUsername + "-" + deleteText.Text;
                    Byte[] buffer45 = Encoding.Default.GetBytes(requireDelete);
                    clientSocket.Send(buffer45);
                }
                else
                {
                    clientlogs.AppendText("Please fill the ID field!\n");
                }
            }
            else
            {
                clientlogs.AppendText("not connected idk error.\n");
            }
        }

        private void myPostsButton_Click(object sender, EventArgs e)
        {
            string requireMyPosts = "myposts*" + traceUsername;
            Byte[] buffer50 = Encoding.Default.GetBytes(requireMyPosts);
            clientSocket.Send(buffer50);
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFriends.SelectedItem != null)
            {
                removedUser = listBoxFriends.SelectedItem.ToString();
            }  
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string friendRequestUsername = addTextBox.Text;
            var lines = File.ReadAllLines(@"../../user-db.txt");
            List<string> checkUsernames = new List<string>();
            foreach (string line in lines)
            {
                checkUsernames.Add(line);
            }
            if (friendRequestUsername == traceUsername)
            {
                clientlogs.AppendText("You cannot add yourself as friend.\n");
            }
            else
            {
                if (checkUsernames.Contains(friendRequestUsername))
                {
                    Byte[] buffer50 = Encoding.Default.GetBytes("addfriend*"+traceUsername+"*"+friendRequestUsername);
                    clientSocket.Send(buffer50);
                }
                else
                {
                    clientlogs.AppendText("There is not a user with username: " + friendRequestUsername + ".\n");
                }
            }
            

        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (removedUser!= null)
            {
                string removeFriendRequest = "remove*" + traceUsername + "/" + removedUser;
                Byte[] buffer1453 = Encoding.Default.GetBytes(removeFriendRequest);
                clientSocket.Send(buffer1453);
            }
            else
            {
                clientlogs.AppendText("Please select a friend to remove!\n");
            }
        }

        private void friendsPostsButton_Click(object sender, EventArgs e)
        {
            string requireFriendsPosts = "friendsPost*" + traceUsername;
            Byte[] buffer505 = Encoding.Default.GetBytes(requireFriendsPosts);
            clientSocket.Send(buffer505);
        }
    }
}
