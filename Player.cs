using System;
using System.Collections.Generic;
using System.Text;

namespace Td6
{
    public class Player
    {
        private String pname;
        private int current_Pos;
        private bool free;
        //private float bank_Account;

        public String Pname { get; set; }
        public int Current_Pos { get; set; }
        public bool Free { get; set; }
        //public float Bank_Account { get; set; }

        public Player(String pname, int current_Pos)
        {
            this.Pname = pname;
            this.Current_Pos = current_Pos;
            this.Free = true;
        }

        //Methods 
        public void GoToJail()
        {
            Console.WriteLine("Go Back To Jail");
            this.Free = false;
            this.Current_Pos = 9;
        }
        public void OutOfJail()
        {
            Console.WriteLine("You are Free Again, Enjoy.");
            this.Free = true;
        }

        public void ToString() {
            Console.WriteLine("" + this.Pname + " " + this.Current_Pos + " " + this.Free);
        }

        //Moved to Board
        /*public void Roll()
        {
            PlayerMonitor pm = new PlayerMonitor(this);
            
            //Rolling
            Random dice1 = new Random();
            Random dice2 = new Random();
            
            String pname = this.Pname;

            int rand1 = dice1.Next(1, 7);
            int rand2 = dice2.Next(1, 7);
            int rand = rand1 + rand2;
            Console.WriteLine("1st Dice is " + rand1);
            Console.WriteLine("2nd Dice is " + rand2);
            if (this.Free)
            {
                if (rand1 == rand2)
                {
                    //After the three consecutive moves player goes to jail 
                    Move(rand);
                    pm.executeFree();
                }
                else {
                    pm.executeRemoveFree();
                    Move(rand);
                }
                Console.WriteLine("Moved: " + rand);
            }
            //In Jail
            else
            {
                //Condition for freedom
                if (rand1 == rand2)
                {
                    this.OutOfJail();
                    Move(rand);
                    Console.WriteLine("Player moved " + rand);
                }
                //Go To 2nd Condition
                pm.executePrison();
            }
            
        }*/

        /*  public void Move(int steps)
          {
              Console.WriteLine("Before moving: " + Current_Pos);
              this.Current_Pos = this.Current_Pos + steps;
              if (this.Current_Pos >= 40)
              {
                  this.Current_Pos = this.Current_Pos - 40;
                  this.Bank_Account = this.Bank_Account + 200;
              }
              Console.WriteLine("After moving: " + Current_Pos);
    }
    public void Operation()
        {
        }*/

    }
}
