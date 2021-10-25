using System;
namespace tic_tac_toe_network.console
{
    class Game
    {   
        public char[,] field {get; private set;}
        public int fieldSize {get;set;}
        private const char emptyCell = '-';
        private NetworkPlayer playerX;
        private NetworkPlayer playerY;
        public NetworkPlayer activePlayer {get;set;}
        public NetworkPlayer winner {get;set;}
        public Boolean DrawExist {get;set;}

        public Game(int fieldSize ) {
            this.fieldSize = fieldSize;
        }

        private void fillField() {
            for(int i =  0; i < fieldSize; i++) {
                    for (int j = 0; j < fieldSize; j++)
                    {
                        field[i,j] = emptyCell;
                    }
                }
        }
        private void createField() {
            field = new char[fieldSize, fieldSize];
            fillField();
        }
        public void createGame(NetworkPlayer playerX, NetworkPlayer playerY) {
            this.playerX = playerX;
            this.playerY = playerY;
            this.activePlayer = playerX;
            createField();
        }   
        private void changePlayer() {
            if(activePlayer == playerX) activePlayer = playerY;
            else activePlayer = playerX; 
        }
        private Boolean isDrawExists() {
            int count = 0;
            foreach(char c in field) {
                if (c == emptyCell) count++;
            }
            return count == 0 ? true : false;
        }
        private void isWinner() {

            char[] winArray = new char[fieldSize];
            for (int i = 0; i < fieldSize; i++) winArray[i] = activePlayer.symbol;

            for(int i = 0; i < fieldSize; i++) {
                
                Boolean isWinOnRow = true;
                Boolean isWinOnColumn = true;
                Boolean isWinOnGlobalDiagonal = true;
                Boolean isWinOnDiagonal = true;
                
                for(int j = 0; j < fieldSize; j++) {
                    if(winArray[j] != field[i,j]) isWinOnRow = false;
                    if(winArray[j] != field[j,i]) isWinOnColumn = false;
                    if(winArray[j] != field[j,j]) isWinOnGlobalDiagonal = false;
                    if(winArray[j] != field[j,fieldSize-1-j]) isWinOnDiagonal = false;
                }

                if(isWinOnRow || isWinOnColumn || isWinOnGlobalDiagonal || isWinOnDiagonal) {
                winner = activePlayer;
                break;
                }
            }
            if(isDrawExists()) DrawExist = true;  
            Console.WriteLine(DrawExist);
            
        }
        public void makeStep(int x, int y) {
            
            if(field[x,y] == emptyCell) {
                field[x,y] = activePlayer.symbol;
                isWinner();
                changePlayer();
            }
            
            
            
        }
        public Boolean gameOver() {
            if(DrawExist || winner != null) return true;
            else return false;
        }
        public string seeResult() {
            if(winner != null) return $"Winner: {winner.name} ({winner.symbol})";
            if(winner == null) if(DrawExist) return $"Draw";
            return null;
        }
        public NetworkPlayer getOtherPlayer() {
            if(activePlayer == playerX) return playerY;
            else return playerX; 
        }
    }
}