using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace tic_tac_toe_network.console
{
    public class NetworkPlayer : Player
    {   
        TcpClient client;
        NetworkStream stream;
        StreamReader reader;       
        StreamWriter writer;

        public NetworkPlayer(TcpClient client) : base(default, default)
        {   
            this.client = client;
            this.stream = client.GetStream();
            this.reader = new StreamReader(this.stream);
            this.writer = new StreamWriter(this.stream);
            this.writer.AutoFlush = true;
        }
        public void write(string text) {
            this.writer.Write(text);
        }
        public string read() {
            return reader.ReadLine();
        }
        public void writeLine(string text) {
            this.writer.WriteLine(text);
        }
        public void closeConnect() {
            client.Close();
        }
        
    }
}