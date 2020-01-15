using System;
using System.Collections.Generic;
using System.Text;

namespace Td6
{
    public class Location
    {
        private String name;
        //private float price;
        //private int position;
        //private Player owner;
        //private bool property;
        public String Name { set; get; }
        //public float Price { set; get; }
        //public int Position { set; get; }
        //public Player Owner { set; get; }
        //public bool Property { set; get; }

        public override string ToString()
        {
            return this.Name;
        }

        public Location(String name)
        {
          //  this.Position = position;
            this.Name = name;
            //this.Price = price;
          //  this.Property = property;
        }

    }
}
