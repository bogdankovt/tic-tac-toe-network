namespace tic_tac_toe_network.console
{
    public class Player
    {
        public string name {get; set;}
        public char symbol {get; set;}
        
        public Player(string name, char symbol) {
            this.name = name;
            this.symbol = symbol;
        } 

    }
}