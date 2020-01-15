using System;
using System.Collections.Generic;
using System.Text;

namespace Td6
{
    public class View
    {

        public View() {

            Console.WriteLine("Enter Number of Players");
            int pl = int.Parse(Console.ReadLine());
            Board.GetInstance(pl);
            Console.WriteLine("Game Initialized");
            int round = 1;
            while (round<10)
            {
                int test=Board.players.Count;
                Console.WriteLine("In View number of players in List " + test);
                Console.WriteLine("Round: "+round);
                

                foreach (var p in Board.players)
                {    
                    p.ToString();
                    Board.PlayerTurn(p);
                }
                round++;
                DisplayLocations();
            }
        }

        public void DisplayLocations()
        { 
            foreach(Player p in Board.players)
            {
                Console.WriteLine(p.Pname + "is on" + Board.locations.getNode(p.Current_Pos).data.ToString());
            }
        }
}        
}