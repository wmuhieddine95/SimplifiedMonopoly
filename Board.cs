using System;
using System.Collections.Generic;
using System.Text;

namespace Td6
{
    public class Board
    {
        int pnum;
        
        //Dictionary for free people having same dice
        private static Dictionary<String,int> freeMonitor;

        //Dictionary for prisoned players
        private static Dictionary<String,int> listOfPrisoners;

        //List of Players in the game
         public static List<Player> players;

        //Circular Linked List of 40 tiles
         public static LocationLinkedList<Location> locations;

        private Board(int pl) {
            //Declaration
            locations = new LocationLinkedList<Location>();
            freeMonitor=new Dictionary<string, int>();
            listOfPrisoners = new Dictionary<string, int>();
            //Circular LinkedList Filled with 40 Location Nodes
            for (int i = 0; i < 40; i++)
            {
                if (i == 0)
                    locations.addNode(new Node<Location>(new Location("GO")));
                else if (i == 9)
                    locations.addNode(new Node<Location>(new Location("Jail/Visit")));
                else if (i == 29)
                    locations.addNode(new Node<Location>(new Location("2Jail")));
                else { locations.addNode(new Node<Location>(new Location("Tile"+i))); }
            }
            locations.print();
            //Insert Number of Players
            pnum = pl;
            players = new List<Player>(3);
            for (int i = 0; i < pnum; i++)
            {
                String name = "Player" + i;
                Player player = new Player(name,0);
                players.Add(player);
            }
        }
            private static Board _instance;

            public static Board GetInstance(int pl)
            {
                if (_instance == null)
                {
                    _instance = new Board(pl);
                }
                return _instance;
            }

            // Player playing under the Controller's rules
            public static void PlayerTurn(Player toWorkOn)
            {
            // ...
                //Rolling
                Random dice1 = new Random();
                Random dice2 = new Random();

                String pname = toWorkOn.Pname;
                
                int rand1 = dice1.Next(1, 7);
                int rand2 = dice2.Next(1, 7);
                int rand = rand1 + rand2;
                Console.WriteLine("1st Dice is " + rand1);
                Console.WriteLine("2nd Dice is " + rand2);
                Node<Location> newLocation;
                if (toWorkOn.Free)
                {
                    if (rand1 == rand2)
                    {
                    //After the three consecutive moves player goes to jail 
                    newLocation=locations.MoveForward(toWorkOn.Current_Pos, rand);
                    toWorkOn.Current_Pos = locations.getNode(newLocation);
                    Console.WriteLine("Player is under Free Control");
                    ExecuteFree(toWorkOn);
                    }
                    else
                    {
                    Console.WriteLine("Player is under Remove Free Control");
                    ExecuteRemoveFree(toWorkOn);
                    newLocation=locations.MoveForward(toWorkOn.Current_Pos, rand);
                    toWorkOn.Current_Pos = locations.getNode(newLocation);
                    }
                if (toWorkOn.Current_Pos == 29)
                    toWorkOn.GoToJail();
                }
                //In Jail
                else
                {
                    //Condition for freedom
                    if (rand1 == rand2)
                    {
                    toWorkOn.OutOfJail();
                    newLocation= locations.MoveForward(toWorkOn.Current_Pos,rand);
                    toWorkOn.Current_Pos = locations.getNode(newLocation);
                    Console.WriteLine("Player moved " + rand);
                    }
                    
                    //Update ListOfPrisoners
                    Console.WriteLine("Player is under Prison Control");
                    bool removedFromPrison=ExecutePrison(toWorkOn);
                    if (removedFromPrison)
                    {
                    newLocation = locations.MoveForward(toWorkOn.Current_Pos, rand);
                    toWorkOn.Current_Pos = locations.getNode(newLocation);
                    Console.WriteLine("Player moved " + rand);
                    }
                }
            }
        

        // Prisoner Rolling
        public static bool ExecutePrison(Player toWorkOn)
        {
            //Got Same Values in Dice
            if (toWorkOn.Free && listOfPrisoners.ContainsKey(toWorkOn.Pname))
            {
                listOfPrisoners.Remove(toWorkOn.Pname);
                return true;
            }
            //Couldn't get the same values
            else
            {
                //Condition for freedom (Failed 3 times)
                if (listOfPrisoners.ContainsKey(toWorkOn.Pname) && listOfPrisoners[toWorkOn.Pname] == 3)
                {
                    Console.WriteLine("OUT OF JAIL From Prison Control");
                    toWorkOn.OutOfJail();
                    listOfPrisoners.Remove(toWorkOn.Pname);
                    return true;
                }
                //Dictionary To Count failure for freedom
                else
                {
                    if (listOfPrisoners.ContainsKey(toWorkOn.Pname))
                        listOfPrisoners[toWorkOn.Pname] = listOfPrisoners[toWorkOn.Pname] + 1;
                    else
                        listOfPrisoners[toWorkOn.Pname] = 1;
                    Console.WriteLine("Player " + toWorkOn.Pname + " in jail for " + listOfPrisoners[toWorkOn.Pname] + " times");
                    return false;
                }
            }
        }

        //Check if free player previously got same values then he didn't in order to remove from dictionary
        public static void ExecuteRemoveFree(Player toWorkOn)
        {
            //Player toWorkOn = this.toWorkOn;
            //Dictionary<String, int> monitor = (Dictionary<String, int>)this.monitor;
            if (freeMonitor.ContainsKey(toWorkOn.Pname))
            {
                if (freeMonitor[toWorkOn.Pname] == 3)
                {
                    toWorkOn.GoToJail();
                    freeMonitor.Remove(toWorkOn.Pname);
                    Console.WriteLine("Removed from dictionary");
                }

                else {
                    freeMonitor.Remove(toWorkOn.Pname);
                    Console.WriteLine("Removed from dictionary");
                }
            }
            else
            {
                Console.WriteLine("Player didn't get same value before");
            }
        }

        //Free people getting same value dice check if they go to Jail
        public static void ExecuteFree(Player toWorkOn)
        {
            //Player toWorkOn = this.toWorkOn;
            //Dictionary<String, int> monitor = (Dictionary<String, int>)this.monitor;
            
            //Exist in Dictionary
            if (freeMonitor.ContainsKey(toWorkOn.Pname))
                //Condition to jail
                if (freeMonitor[toWorkOn.Pname] == 3)
                {
                    toWorkOn.GoToJail();
                    freeMonitor.Remove(toWorkOn.Pname);
                    Console.WriteLine("Removed from dictionary");
                }
                //Add Count
                else
                {
                    freeMonitor[toWorkOn.Pname] = freeMonitor[toWorkOn.Pname] + 1;
                    Console.WriteLine("Player " + toWorkOn.Pname + " got same value on both dices " + freeMonitor[toWorkOn.Pname] + " times");
                    //toWorkOn.Roll();
                }
            //Not found -> New to dictionary
            else
            {
                freeMonitor[toWorkOn.Pname] = 1;
                //toWorkOn.Roll();
                Console.WriteLine("Player " + toWorkOn.Pname + " got same value on both dices " + freeMonitor[toWorkOn.Pname] + " times");
            }
        }

        public static void StartGame() {
            View v = new View();
        }
    }
}

