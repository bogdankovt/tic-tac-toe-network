using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace tic_tac_toe_network.console
{
    class Program
    {   
        private static string askName(NetworkPlayer player) {
            player.write("\nWhat is u name: ");
            string s = player.read();
            return s;
        }
        public static char askSymbol(NetworkPlayer player) {
            player.write("What is u symbol: ");
            char s = Convert.ToChar(player.read());
            return s;
        }
        static void printField(NetworkPlayer player, char[,] field, int fieldSize) {
            player.writeLine("\n\n");
            for(int i =  0; i < fieldSize; i++) {
                for (int j = 0; j < fieldSize; j++)
                {
                    if(j < fieldSize - 1) {
                        player.write(field[i,j] + " | ");
                    }else player.write(field[i,j].ToString());
                }
                if(i != fieldSize -1) player.writeLine("\n" + new string('—',fieldSize*3));

            }
            player.writeLine("\n\n");
        }
        public static String[] askStepCoords(NetworkPlayer player) {
            player.write("Make step: ");
            return player.read().Split(" ");
        }
        public static void printFieldToAllPlayers(List<NetworkPlayer> players, char[,] field, int fieldsize) {
            foreach (var player in players)
            {
                printField(player,field, fieldsize );
            }
        }
        public static void seeInfoPlayers(List<NetworkPlayer> players) {
            foreach (var player in players)
            {
                player.write("\nPlayers: \n");
                foreach(var playerWrite in players) {
                    player.writeLine($"- {playerWrite.name} | {playerWrite.symbol}");
                }
            }
        }
        public static void printToAll(List<NetworkPlayer> players, string s) {
            foreach (var player in players)
            {
                player.writeLine($"\n{s}");
            }
        }
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                IPAddress localAddr = IPAddress.Parse(args[0]);
                Int32 port = Int32.Parse(args[1]);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client1 requests.
                server.Start();
                
                System.Console.WriteLine("Server Starting....Wait for Players...");

                while(true) {
                    List<NetworkPlayer> players = new List<NetworkPlayer>();
                    
                    while(players.Count < 2 ) {

                        TcpClient client = server.AcceptTcpClient();

                        // Get a stream object for reading and writing
                        NetworkPlayer player = new NetworkPlayer(client);
                        player.name = askName(player);
                        player.symbol = askSymbol(player);

                        System.Console.WriteLine($"Player {player.name} connected | {player.symbol}");
                        players.Add(player);
                        if(players.Count < 2) player.writeLine("\nWaiting other players....");

                    }

                    
                    Game game = new Game(3);
                    game.createGame(players[0], players[1]);
                    seeInfoPlayers(players);
                    printFieldToAllPlayers(players, game.field, 3);

                    while (!game.gameOver()) {
                        
                        game.activePlayer.write($"Player: {game.activePlayer.name} ({game.activePlayer.symbol})\n");
                        game.getOtherPlayer().write($"Waiting for step other player....");

                        String[] stepCoords = askStepCoords(game.activePlayer);
                        game.makeStep(Convert.ToInt32(stepCoords[0]) - 1, Convert.ToInt32(stepCoords[1]) - 1);
                        printFieldToAllPlayers(players, game.field, 3);
                    }
                    printToAll(players, game.seeResult());
                    players.ForEach(x => x.closeConnect());
                }
                
                
                
            }catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

       
        }
    }
}
