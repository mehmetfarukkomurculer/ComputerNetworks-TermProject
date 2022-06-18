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

namespace last_server
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();
        bool listening = false;
        bool terminating = false;
        List<string> connectedUsers = new List<string>();
        Dictionary<string, Socket> cnc = new Dictionary<string, Socket>();
        List<string> pendingMessages = new List<string>();
        bool findStatus = false;


        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            
        }
        private void listenbutton_Click(object sender, EventArgs e)
        {
            int serverPort;
            if (Int32.TryParse(porttext.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10);

                listening = true;
                listenbutton.Enabled = false;
                porttext.Enabled = false;

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                serverlogs.AppendText("Started listening on port:" + serverPort + ".\n");
            }
            else
            {
                serverlogs.AppendText("Check your port number!\n");
            }

        }

        private void Accept()
        {
            while (listening)
            {
                
                try
                {
                    Socket newClient = serverSocket.Accept();
                    clientSockets.Add(newClient);
                    
                    Thread receiveThread = new Thread(() => Receive(newClient));
                    receiveThread.Start();
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        serverlogs.AppendText("The socket stopped working!\n");
                        connectedUsers.Clear();
                    }
                }
            }
        }
        //10.50.125.73
        private void Receive(Socket thisClient)
        {
            bool connected = true;
            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[65536];
                    thisClient.Receive(buffer);
                    string incomingData = Encoding.Default.GetString(buffer);
                    incomingData = incomingData.Substring(0, incomingData.IndexOf("\0"));
                    if (incomingData.Substring(0, 9) == "allposts*")
                    {
                        serverlogs.AppendText("Showing all posts for "+ incomingData.Substring(9) + ".\n");
                        try
                        {
                            var lines = File.ReadAllLines(@"../../allposts.txt");
                            string allposts_merged = "";
                            int lineCount = 0;
                            foreach (string item in lines)
                            {
                                lineCount++;
                            }
                            if (lineCount == 0)
                            {
                                string xdd = "allposts* ";
                                Byte[] buffer555 = Encoding.Default.GetBytes(xdd);
                                thisClient.Send(buffer555);
                            }
                            else
                            {
                                foreach (string line in lines)
                                {
                                    string[] splitt = line.Split('%');
                                    if (incomingData.Substring(9) != splitt[0])
                                    {
                                        allposts_merged = allposts_merged + "Username: " + splitt[0] + "\nPostID: " + splitt[1] + "\nPost: " + splitt[2] + "\nTime: " + splitt[3] + "\n\n";
                                    }
                                    else
                                    {
                                        allposts_merged = allposts_merged + "";
                                    }
                                }
                                string last = "allposts*" + allposts_merged;
                                Byte[] buffer55 = Encoding.Default.GetBytes(last);
                                thisClient.Send(buffer55);
                            }
                            
                        }
                        catch (Exception e4)
                        {
                            serverlogs.AppendText(e4.Message);
                        }
                          
                    }
                    else if (incomingData.Substring(0, 7) == "delete*")
                    {

                        if (new FileInfo(@"../../allposts.txt").Length == 0)
                        {
                            Byte[] buffer103 = Encoding.Default.GetBytes("emptyFile*" + "There is no post to be deleted in file.\n");
                            thisClient.Send(buffer103);
                            serverlogs.AppendText("File is empty!. There is no post to be deleted.\n");
                        }
                        else
                        {
                            string[] ayirac = incomingData.Split('*');
                            string[] getUsernameAndPostID = ayirac[1].Split('-');
                            var lines = File.ReadAllLines(@"../../allposts.txt");
                            String last = File.ReadLines(@"../../allposts.txt").Last();
                            string[] lastLine = last.Split('%');

                            List<string> postIDs = new List<string>();
                            foreach (string ln in lines)
                            {
                                string[] getIDs = ln.Split('%');
                                postIDs.Add(getIDs[1]);
                            }
                            if (postIDs.Contains(getUsernameAndPostID[1]))
                            {
                                foreach (string line in lines)
                                {
                                    string[] splitt = line.Split('%');
                                    // gelen delete request username + postid şeklinde (arada '-' var)
                                    // gelen username ve id uyuşuyorsa silme işlemi başlar.
                                    if (splitt[0] == getUsernameAndPostID[0] && splitt[1] == getUsernameAndPostID[1])
                                    {
                                        string tempFile = Path.GetTempFileName();
                                        using (var sr = new StreamReader(@"../../allposts.txt"))
                                        using (var sw = new StreamWriter(tempFile))
                                        {
                                            string delLine;
                                            while ((delLine = sr.ReadLine()) != null)
                                            {
                                                string[] delLineSplit = delLine.Split('%');
                                                if (!(delLineSplit[0] == getUsernameAndPostID[0] && delLineSplit[1] == getUsernameAndPostID[1]))
                                                {
                                                    sw.WriteLine(delLine);
                                                }
                                                else
                                                {
                                                    serverlogs.AppendText("Post with ID "+ delLineSplit[1] + " is deleted.\n");
                                                    Byte[] buffer100 = Encoding.Default.GetBytes("deletionApproved*"+ "Post with ID: " + delLineSplit[1] + " is deleted succesfully.\n");
                                                    thisClient.Send(buffer100);
                                                }

                                            }
                                        }

                                        File.Delete(@"../../allposts.txt");
                                        File.Move(tempFile, @"../../allposts.txt");
                                    }
                                    else if (splitt[1] == getUsernameAndPostID[1] && splitt[0] != getUsernameAndPostID[0])
                                    {
                                        Byte[] buffer101 = Encoding.Default.GetBytes("deletionNotApproved*" + "Post with ID: " + getUsernameAndPostID[1] + " is not yours.\n");
                                        thisClient.Send(buffer101);
                                        serverlogs.AppendText("Post with ID: " + getUsernameAndPostID[1] + " is not " + getUsernameAndPostID[0] + "'s.\n");
                                    }
                                }
                            }
                            else
                            {
                                Byte[] buffer102 = Encoding.Default.GetBytes("noDeletion*" + "There is no post with ID: " + getUsernameAndPostID[1] + ".\n");
                                thisClient.Send(buffer102);
                                serverlogs.AppendText("There is no post with ID: "+ getUsernameAndPostID[1] + ".\n");
                            }
                        }

                    }
                    else if (incomingData.Substring(0, 8) == "myposts*")
                    {
                        serverlogs.AppendText("Showing posts of "+ incomingData.Substring(8) + ".\n");
                        try
                        {
                            var lines = File.ReadAllLines(@"../../allposts.txt");
                            string myposts_merged = "";
                            int lineCount = 0;
                            foreach (string item in lines)
                            {
                                lineCount++;
                            }
                            if (lineCount == 0)
                            {
                                string xdd = "myposts* ";
                                Byte[] buffer555 = Encoding.Default.GetBytes(xdd);
                                thisClient.Send(buffer555);
                            }
                            else
                            {
                                foreach (string line in lines)
                                {
                                    string[] splitt = line.Split('%');
                                    if (incomingData.Substring(8) == splitt[0])
                                    {
                                        myposts_merged = myposts_merged + "Username: " + splitt[0] + "\nPostID: " + splitt[1] + "\nPost: " + splitt[2] + "\nTime: " + splitt[3] + "\n\n";
                                    }
                                    else
                                    {
                                        myposts_merged = myposts_merged + "";
                                    }
                                }
                                string mypostsSend = "myposts*" + myposts_merged;
                                Byte[] buffer104 = Encoding.Default.GetBytes(mypostsSend);
                                thisClient.Send(buffer104);
                            }

                        }
                        catch (Exception e4)
                        {
                            serverlogs.AppendText(e4.Message);
                        }
                    }
                    else if (incomingData.Substring(0, 12) == "friendsPost*")
                    {
                        serverlogs.AppendText("Showing friends posts of " + incomingData.Substring(12) + ".\n");
                        try
                        {
                            var allPostLines = File.ReadAllLines(@"../../allposts.txt");
                            string friendsposts_merged = "";
                            int lineCount = 0;
                            foreach (string item in allPostLines)
                            {
                                lineCount++;
                            }
                            if (lineCount == 0)
                            {
                                string friendsPOST = "friendsposts* ";
                                Byte[] buffer5559 = Encoding.Default.GetBytes(friendsPOST);
                                thisClient.Send(buffer5559);
                            }
                            else
                            {
                                //friends txt ara, arakdaslari list icine salla.
                                var linesFriends = File.ReadAllLines(@"../../friends.txt");
                                List<string> usersFriends = new List<string>();
                                foreach (string line in linesFriends)
                                {
                                    string[] splitFriendsTxtLine = line.Split('-');
                                    if (splitFriendsTxtLine[0] == incomingData.Substring(12))
                                    {
                                        usersFriends.Add(splitFriendsTxtLine[1]);
                                    }
                                }
                                //all posts.txt split at 
                                if (usersFriends.Count !=0)
                                {
                                    var allPosts = File.ReadAllLines(@"../../allposts.txt");
                                    foreach (string allPostsLine in allPosts)
                                    {
                                        string[] allposts_LineSplit = allPostsLine.Split('%');
                                        //username kismiyla listedeki isimler yuluyorsa merge at.
                                        if (usersFriends.Contains(allposts_LineSplit[0]))
                                        {
                                            friendsposts_merged = friendsposts_merged + "Username: " + allposts_LineSplit[0] + "\nPostID: " + allposts_LineSplit[1] + "\nPost: " + allposts_LineSplit[2] + "\nTime: " + allposts_LineSplit[3] + "\n\n";
                                        }
                                        else
                                        {
                                            friendsposts_merged = friendsposts_merged + "";
                                        }
                                    }
                                    string friendspostsSend = "friendsposts*" + friendsposts_merged;
                                    Byte[] buffer1042 = Encoding.Default.GetBytes(friendspostsSend);
                                    thisClient.Send(buffer1042);
                                }
                                else
                                {
                                    string friendsPOSTc = "friendsposts* ";
                                    Byte[] buffer5560 = Encoding.Default.GetBytes(friendsPOSTc);
                                    thisClient.Send(buffer5560);
                                }
                                

                                
                            }

                        }
                        catch (Exception e5)
                        {
                            serverlogs.AppendText(e5.Message);
                        }
                    }
                    else if (incomingData.Substring(0, 7) == "remove*")
                    {
                        string[] removeRequestSplitted = incomingData.Split('*');
                        //removerequestSplitted[0] = "remove"
                        //removerequestSplitted[1] = "silme talebi gonderen kullanici/silinecek user"
                        string[] removeRequest = removeRequestSplitted[1].Split('/');
                        //removeRequest[0] = silme talebi gonderen gonderen kullanici
                        //removeRequest[1] = silinecek user
                        if (new FileInfo(@"../../friends.txt").Length == 0)
                        {
                            serverlogs.AppendText(removeRequest[0] + " and " + removeRequest[1] + " are not friend!\n");
                            string rejectMessage = "removeRejected*You and " + removeRequest[1] + " are not friend!\n"; 
                            Byte[] buffer1042 = Encoding.Default.GetBytes(rejectMessage);
                            thisClient.Send(buffer1042);
                        }
                        else
                        {
                            //search whether they are friend or not
                            var lines = File.ReadAllLines(@"../../friends.txt");
                            bool theyrefriend = false;
                            foreach (string line in lines)
                            {
                                string[] lineSplit = line.Split('-');
                                if (lineSplit[0] == removeRequest[0] && lineSplit[1] == removeRequest[1])
                                {
                                    theyrefriend = true;
                                }
                            }
                            if (theyrefriend == true)
                            {
                                // they are friend, send remove Accepted message
                                serverlogs.AppendText(removeRequest[0] + " removed " + removeRequest[1] + " from his/her friendlist!\n");
                                string rejectMessage = "removeAccepted*You have succesfully removed " + removeRequest[1] + " from your friendlist!\n";
                                Byte[] buffer1042 = Encoding.Default.GetBytes(rejectMessage);
                                thisClient.Send(buffer1042);
                                // if they are friend, update friends.txt 
                                string tempFile = Path.GetTempFileName();
                                using (var sr = new StreamReader(@"../../friends.txt"))
                                using (var sw = new StreamWriter(tempFile))
                                {
                                    string delLine;
                                    while ((delLine = sr.ReadLine()) != null)
                                    {
                                        string[] splityt = delLine.Split('-');
                                        if (!(splityt[0] == removeRequest[0] && splityt[1] == removeRequest[1]) && !(splityt[0] == removeRequest[1] && splityt[1] == removeRequest[0]))
                                        {
                                            sw.WriteLine(delLine);
                                        }
                                    }
                                }
                                File.Delete(@"../../friends.txt");
                                File.Move(tempFile, @"../../friends.txt");

                                // if silinen kişi online'sa kimin onu removeladigini da gonder
                                foreach (KeyValuePair<string, Socket> res in cnc)
                                {
                                    if (res.Key == removeRequest[1])
                                    {
                                        Byte[] buffer1950 = Encoding.Default.GetBytes("removedBy*" + removeRequest[0] + " removed you from his/her friendlist!\n");
                                        res.Value.Send(buffer1950);
                                        //send current friendlist to removed user
                                        try
                                        {
                                            var otherUserFriendList = File.ReadAllLines(@"../../friends.txt");
                                            string otherUserFriends = "";
                                            foreach (string ii in otherUserFriendList)
                                            {
                                                string[] idkk = ii.Split('-');
                                                if (idkk[0] == res.Key)
                                                {
                                                    otherUserFriends = otherUserFriends + idkk[1] + ",";
                                                }
                                            }
                                            if (otherUserFriends != "")
                                            {
                                                Byte[] buffer1951 = Encoding.Default.GetBytes("friendList*" + otherUserFriends);
                                                res.Value.Send(buffer1951);
                                            }
                                            else
                                            {
                                                Byte[] buffer1989 = Encoding.Default.GetBytes("friendList*X");
                                                res.Value.Send(buffer1989);
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            serverlogs.AppendText(e.Message);
                                        }

                                    }
                                }
                                //send current friendlist to client
                                var sendFriendList = File.ReadAllLines(@"../../friends.txt");
                                string friendss = "";
                                foreach (string yy in sendFriendList)
                                {
                                    string[] idk = yy.Split('-');
                                    if (idk[0] == removeRequest[0])
                                    {
                                        friendss = friendss + idk[1] + ",";
                                    }
                                }
                                if (friendss != "")
                                {
                                    Byte[] buffer1952 = Encoding.Default.GetBytes("friendList*" + friendss);
                                    thisClient.Send(buffer1952);
                                }
                                else
                                {
                                    Byte[] buffer1953 = Encoding.Default.GetBytes("friendList*X");
                                    thisClient.Send(buffer1953);
                                }

                                // eger pendingmessage olucaksa remove list icin, pendingmessagesremove.txt file olustur
                                // ve username approved kismindan ilgili kişi baglaninca listi gonder. 
                            }
                            else
                            {
                                // they are not friend, send remove request rejected message to talebi gonderen kullanici
                                serverlogs.AppendText(removeRequest[0] + " and " + removeRequest[1] + " are not friend!\n");
                                string rejectMessage2 = "removeRejected*You and " + removeRequest[1] + " are not friend!\n";
                                Byte[] buffer1071 = Encoding.Default.GetBytes(rejectMessage2);
                                thisClient.Send(buffer1071);
                            }
                        }
                    }
                    else
	                {                       
                        string[] piece = incomingData.Split('*');
                        if (piece[0] == "usernameNotApproved")
                        {
                            serverlogs.AppendText(piece[1]);
                        }
                        else if (piece[0] == "usernameApproved")
                        {
                            if (connectedUsers.Contains(piece[2]))
                            {
                                Byte[] buffer105 = Encoding.Default.GetBytes("alreadyConnected* ");
                                thisClient.Send(buffer105);
                                serverlogs.AppendText(piece[2] + " tried to connect but he/she has already connected.\n");
                                thisClient.Close();
                                clientSockets.Remove(thisClient);
                                connected = false;

                            }
                            else
                            {
                                serverlogs.AppendText(piece[1]);
                                connectedUsers.Add(piece[2]);
                                cnc.Add(piece[2], thisClient);
                                string firstMessage;
                                string friendss1 = "";
                                string lastFriendsMessage = "";
                                string offlineMessage = "";
                                string lastOfflineMessage = "";
                                string connectionMessage = "Hello " + piece[2] + "! You are connected to the server.\n";
                                try
                                {
                                    var sendFriendList = File.ReadAllLines(@"../../friends.txt");
                                    
                                    if (!(new FileInfo(@"../../friends.txt").Length == 0))
                                    {
                                        foreach (string yy in sendFriendList)
                                        {
                                            string[] fff = yy.Split('-');
                                            if (fff[0] == piece[2])
                                            {
                                                friendss1 = friendss1 + fff[1] + ",";
                                            }
                                        }
                                        if (friendss1 != "")
                                        {
                                            lastFriendsMessage = "ilkfriendList*" + friendss1;
                                        }
                                        else
                                        {
                                            lastFriendsMessage = "ilkfriendList*X";
                                        }
                                    }
                                    else
                                    {
                                        lastFriendsMessage = "ilkfriendList*X";
                                    }
                                }
                                
                                catch (Exception e)
                                {
                                    serverlogs.AppendText(e.Message);
                                }
                                
                                try
                                {
                                    int countss = 1;
                                    var lineslines = File.ReadAllLines(@"../../pendingMessages.txt");
                                    if (!(new FileInfo(@"../../pendingMessages.txt").Length == 0))
                                    {
                                        foreach (string tt in lineslines)
                                        {
                                            string[] t = tt.Split(',');
                                            if (t[0] == piece[2])
                                            {
                                                offlineMessage = offlineMessage + countss + "." + t[1] + ",";
                                                countss++;
                                            }
                                        }
                                        if (offlineMessage != "")
                                        {
                                            lastOfflineMessage = "offlineMessage*" + offlineMessage;
                                            try
                                            {
                                                string tempFile = Path.GetTempFileName();
                                                using (var sr = new StreamReader(@"../../pendingMessages.txt"))
                                                using (var sw = new StreamWriter(tempFile))
                                                {
                                                    string line;
                                                    while ((line = sr.ReadLine()) != null)
                                                    {
                                                        string[] deletionPendingMessage = line.Split(',');
                                                        if (deletionPendingMessage[0] != piece[2])
                                                        {
                                                            sw.WriteLine(line);
                                                        }
                                                    }
                                                }
                                                File.Delete(@"../../pendingMessages.txt");
                                                File.Move(tempFile, @"../../pendingMessages.txt");
                                            }
                                            catch (Exception e)
                                            {
                                                serverlogs.AppendText(e.Message);
                                            }
                                            
                                        }
                                        else
                                        {
                                            lastOfflineMessage = "offlineMessage*X";
                                        }
                                    }
                                    else
                                    {
                                        lastOfflineMessage = "offlineMessage*X";
                                    }
                                }
                                catch 
                                {
                                    serverlogs.AppendText("hata");
                                }
                                
                                firstMessage = "connectionApproved*" + connectionMessage + "*" + lastFriendsMessage + "*" + lastOfflineMessage;
                                Byte[] buffer120 = Encoding.Default.GetBytes(firstMessage);
                                thisClient.Send(buffer120);
                            }
                        }
                        else if (piece[0] == "post")
                        {
                            string[] postpieces = piece[1].Split('?');
                            serverlogs.AppendText(postpieces[0] + " has sent a post: " + "\n" + postpieces[1] + "\n");
                            string clientlogsPost = "post*" + postpieces[0] + ": " + postpieces[1] + "\n";
                            Byte[] buffer4 = Encoding.Default.GetBytes(clientlogsPost);                            
                            thisClient.Send(buffer4);
                            var lines = File.ReadAllLines(@"../../allposts.txt");
                            List<string> allposts = new List<string>();
                            int count = 0;                  
                            foreach (string line in lines)
                            {
                                count++;
                            }
                            if (count == 0)
                            {
                                using (StreamWriter writer = new StreamWriter("../../allposts.txt", append: true))
                                {
                                    writer.Write(postpieces[0] + "%" + count + "%" + postpieces[1] + "%" + postpieces[2] + "T" + postpieces[3] + "\n");
                                    writer.Close();
                                }
                            }
                            else
                            {
                                String last = File.ReadLines(@"../../allposts.txt").Last();
                                string[] lastLine = last.Split('%');
                                int newId = Int16.Parse(lastLine[1]) +1;

                                using (StreamWriter writer = new StreamWriter("../../allposts.txt", append: true))
                                {
                                    writer.Write(postpieces[0] + "%" + newId + "%" + postpieces[1] + "%" + postpieces[2] + "T" + postpieces[3] + "\n");
                                    writer.Close();
                                }
                            }
                            
                           
                        }
                        else if (piece[0] == "disconnect")
                        {
                            serverlogs.AppendText(piece[1] + "\n");
                            thisClient.Close();
                            clientSockets.Remove(thisClient);
                            connected = false;
                            connectedUsers.Remove(piece[2]);
                            if (cnc.ContainsKey(piece[2]))
                            {
                                cnc.Remove(piece[2]);
                            }
                            else
                            {
                                serverlogs.AppendText("not in dictionary!\n");
                            }
                        }
                        else if (piece[0] == "addfriend")
                        {
                            //serverlogs.AppendText(piece[0] + "\n");
                            //serverlogs.AppendText(piece[1] + ": kim ekledi\n");
                            //serverlogs.AppendText(piece[2] + ": kimi ekledi\n");
                            var linesFriendsTxt = File.ReadAllLines(@"../../friends.txt");
                            if(new FileInfo(@"../../friends.txt").Length == 0)
                            {
                                using (StreamWriter writer = new StreamWriter("../../friends.txt", append: true))
                                {
                                    writer.Write(piece[1] + "-" + piece[2] + "\n");
                                    writer.Write(piece[2] + "-" + piece[1] + "\n");
                                    writer.Close();
                                }

                                Byte[] buffer9 = Encoding.Default.GetBytes("addfriendApproved*"+ piece[1] + " added " + piece[2] + " as a friend succesfully!\n");
                                thisClient.Send(buffer9);
                                try
                                {
                                    foreach (KeyValuePair<string, Socket> res in cnc)
                                    {
                                        if (res.Key == piece[2])
                                        {
                                            findStatus = true;
                                            Byte[] buffer10 = Encoding.Default.GetBytes("otherAdded*" + piece[1] + " added you as a friend!\n");
                                            res.Value.Send(buffer10);
                                            try
                                            {
                                                var otherUserFriendList = File.ReadAllLines(@"../../friends.txt");
                                                string otherUserFriends = "";
                                                foreach (string ii in otherUserFriendList)
                                                {
                                                    string[] idkk = ii.Split('-');
                                                    if (idkk[0] == res.Key)
                                                    {
                                                        otherUserFriends = otherUserFriends + idkk[1] + ",";
                                                    }
                                                }
                                                if (otherUserFriends != "")
                                                {
                                                    Byte[] buffer11 = Encoding.Default.GetBytes("friendList*" + otherUserFriends);
                                                    res.Value.Send(buffer11);
                                                }
                                                else
                                                {
                                                    Byte[] buffer115 = Encoding.Default.GetBytes("friendList*X");
                                                    thisClient.Send(buffer115);
                                                }

                                            }
                                            catch (Exception e)
                                            {
                                                serverlogs.AppendText(e.Message);
                                            }
                                        }
                                    }
                                }
                                catch (Exception e){
                                    serverlogs.AppendText(e.Message);
                                }
                                try
                                {
                                    if (findStatus == false)
                                    {
                                        using (StreamWriter writer = new StreamWriter("../../pendingMessages.txt", append: true))
                                        {
                                            writer.Write(piece[2] + "," + piece[1] + "\n");// (eklenen kisi, ekleyen)
                                            writer.Close();
                                        } 
                                    }
                                }
                                catch (Exception e)
                                {
                                    serverlogs.AppendText(e.Message);
                                }
                                var sendFriendList = File.ReadAllLines(@"../../friends.txt");
                                string friendss = "";
                                foreach (string yy in sendFriendList)
                                {
                                    string[] idk = yy.Split('-');
                                    if (idk[0] == piece[1])
                                    {
                                        friendss = friendss + idk[1] + ",";
                                    }
                                }
                                if (friendss != "")
                                {
                                    Byte[] buffer11 = Encoding.Default.GetBytes("friendList*" + friendss);
                                    thisClient.Send(buffer11);
                                }
                                else
                                {
                                    Byte[] buffer11555 = Encoding.Default.GetBytes("friendList*X");
                                    thisClient.Send(buffer11555);
                                }
                            }
                            else
                            {
                                bool friendOrNot = false;
                                foreach (string lnn in linesFriendsTxt)
                                {
                                    string[] split_lines = lnn.Split('-');
                                    if (split_lines[0] == piece[1] && split_lines[1] == piece[2])
                                    {
                                        serverlogs.AppendText(piece[1] + " and " + piece[2] + " are already friend!\n");
                                        friendOrNot = true;
                                    }
                                }
                                if (friendOrNot == false)
                                {
                                    using (StreamWriter writer = new StreamWriter("../../friends.txt", append: true))
                                    {
                                        writer.Write(piece[1] + "-" + piece[2] + "\n");
                                        writer.Write(piece[2] + "-" + piece[1] + "\n");
                                        writer.Close();
                                    }
                                    Byte[] buffer9 = Encoding.Default.GetBytes("addfriendApproved*" + piece[1] + " added " + piece[2] + " as a friend succesfully!\n");
                                    thisClient.Send(buffer9);
                                    serverlogs.AppendText(piece[1] + " added " + piece[2] + " as a friend\n");
                                    try
                                    {
                                        foreach (KeyValuePair<string, Socket> res in cnc)
                                        {
                                            if (res.Key == piece[2])
                                            { 
                                                Byte[] buffer10 = Encoding.Default.GetBytes("otherAdded*" + piece[1] + " added you as a friend!\n");
                                                res.Value.Send(buffer10);
                                                try
                                                {
                                                    var otherUserFriendList = File.ReadAllLines(@"../../friends.txt");
                                                    string otherUserFriends = "";
                                                    foreach (string ii in otherUserFriendList)
                                                    {
                                                        string[] idkk = ii.Split('-');
                                                        if (idkk[0] == res.Key)
                                                        {
                                                            otherUserFriends = otherUserFriends + idkk[1] + ",";
                                                        }
                                                    }
                                                    if (otherUserFriends != "")
                                                    {
                                                        Byte[] buffer11 = Encoding.Default.GetBytes("friendList*" + otherUserFriends);
                                                        res.Value.Send(buffer11);
                                                    }
                                                    else
                                                    {
                                                        Byte[] buffer115 = Encoding.Default.GetBytes("friendList*X");
                                                        thisClient.Send(buffer115);
                                                    }

                                                }
                                                catch (Exception e)
                                                {
                                                    serverlogs.AppendText(e.Message);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        serverlogs.AppendText(e.Message);
                                    }
                                    try
                                    {
                                        if (findStatus == false)
                                        {
                                            using (StreamWriter writer = new StreamWriter("../../pendingMessages.txt", append: true))
                                            {
                                                writer.Write(piece[2] + "," + piece[1] + "\n");// (eklenen kisi, ekleyen)
                                                writer.Close();
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        serverlogs.AppendText(e.Message);
                                    }
                                    try
                                    {
                                        var sendFriendList = File.ReadAllLines(@"../../friends.txt");
                                        string friendss = "";
                                        foreach (string yy in sendFriendList)
                                        {
                                            string[] idk = yy.Split('-');
                                            if (idk[0] == piece[1])
                                            {
                                                friendss = friendss + idk[1] + ",";
                                            }
                                        }
                                        if (friendss != "")
                                        {
                                            Byte[] buffer11 = Encoding.Default.GetBytes("friendList*" + friendss);
                                            thisClient.Send(buffer11);
                                        }
                                        else
                                        {
                                            Byte[] buffer1156 = Encoding.Default.GetBytes("friendList*X");
                                            thisClient.Send(buffer1156);
                                        }
                                    }
                                    catch 
                                    {
                                        serverlogs.AppendText("friends.txt file does not exist!\n");
                                    }
                                }
                                else
                                {
                                    serverlogs.AppendText("");
                                }
                            }
                        }
                        else
                        {
                            serverlogs.AppendText("Unknown error\n");
                        }
                    }
                }
                catch
                {
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;  
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            string disconnected_message_server = "The server has disconnected!";
            Byte[] buffer66 = Encoding.Default.GetBytes(disconnected_message_server);
            foreach (Socket client in clientSockets)
            {
                client.Send(buffer66);
            }
            connectedUsers.Clear();
            cnc.Clear();
            Environment.Exit(0);
        }
    }
}
