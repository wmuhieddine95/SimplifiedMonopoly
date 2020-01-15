using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Td6
{
    // This source code shows how to implement an Iterator pattern by extending the native C# IEnumerable and IEnumerator interfaces,
    // so to allow our custom collection SingleLinkedList to be used within a for-each construct
    public class Node<T> : IEquatable<Node<T>>
    {
        public Node(T val)
        {
            data = val;
            prev = null;
            next = null;
        }

        public T data { get; set; }
        public Node<T> next { get; set; }
        public Node<T> prev { get; set; }

        public bool Equals(Node<T> other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return data.GetHashCode();
        }
    }


    // This is the actual iterator object (the so-called ConcreteIterator in the Iterator pattern UML diagram)
    // (in C# iterators are called enumerators)
    public class LocationLinkedListEnumerator<T> : IEnumerator<T>
    {
        public LocationLinkedListEnumerator(LocationLinkedList<T> l)
        {
            list = l;
            current_obj_idx = -1; // At the beginning, the iterator is set before the first element (see IEnumerator documentation)
        }

        public T Current
        {
            get
            {
                if (list.size() > 0)
                    return list.getNode(current_obj_idx).data;

                // Return null for an empty list
                return default(T);
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            current_obj_idx++;

            if (current_obj_idx >= list.size())
                // All elements of the list have already been scanned
                return false;

            return true;
        }

        public void Reset()
        {
            current_obj_idx = -1; // At the beginning, the iterator is set before the first element (see IEnumerator documentation)
        }

        void IDisposable.Dispose() { }

        private LocationLinkedList<T> list;
        private int current_obj_idx;
    }


    // Our collection SingleLinkedList is the actual aggregate object (the so-called ConcreateAggregator in the Iterator pattern UML diagram)
    // (in C# aggregate objects, which are basically collections of objects, are said to be enumerable (i.e. they are implementing IEnumerable), 
    // as they can be scanned by an enumerator (i.e. an object that implements IEnumerator)
    public class LocationLinkedList<T> : IEnumerable<T>
    {
        public Node<T> head { get; set; }
        public Node<T> tail { get; set; }
        public IEnumerator<T> GetEnumerator()
        {
            return new LocationLinkedListEnumerator<T>(this);
        }

        // To be compliant to IEnumerable<T>, a class must also implement IEnumerable.GetEnumerator,
        //but implemented as a private method.
        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        public int size()
        {
            int count = 0;

            Node<T> current = head;
            while (current.next != head)
            {
                count++;
                current = current.next;
            }
            count++;

            return count;
        }

        //Trace New Location after roll
        public Node<T> MoveForward(int current_pos,int rolling)
        {
            Node<T> n = this.getNode(current_pos);
            for (int i = 0; i < rolling; i++)
                n = n.next;
            return n;
        }

        public void addNode(Node<T> node)
        {
            Node<T> new_node = new Node<T>(node.data);

            if (head == null)
            {
                head = new_node;
                tail = new_node;
            }

            else
            {
                if (head == tail)
                {
                    tail = new_node;
                    head.next = tail;
                    tail.next = head;
                    head.prev = tail;
                    tail.prev = head;
                }

                else
                {
                    // Determine the last element of the list...
                    Node<T> current = tail;
                    Node<T> temp = new_node;
                    temp.prev = current;
                    current.next = temp;
                    temp.next = head;
                    head.prev = temp;
                    tail = temp;
                }
            }
        }

        public Node<T> getNode(int idx)
        {
            Node<T> current = head;
            int pos = 0;

            if (pos == idx)
                return head;

            else
            {
                pos = 1;
                current = current.next;
                while (current != head)
                {
                    if (pos == idx)
                        return current;

                    current = current.next;
                    pos++;
                }
            }
            return null;
        }

        /*public void setNode(int idx, Node<T> node)
        {
            Node<T> current = head;
            Node<T> prev_current = null;
            int pos = 0;

            while (current.next != head)
            {
                if (pos == idx)
                {
                    Node<T> new_node = new Node<T>(node.data);
                    new_node.next = current.next;
                    prev_current.next = new_node;
                    break;
                }

                prev_current = current;
                current = current.next;
                pos++;
            }
        }*/

        // Override operator [] for SingleLinkedList objects
        /* public Node<T> this[int index]
         {
             get
             {
                 return getNode(index);
             }

             set
             {
                 setNode(index, value);
             }
         }*/

        public int getNode(Node<T> elem)
                {
                    Node<T> current = head;
                    int index = 0;
                    if (current.Equals(elem))
                        return index;

                    else {
                        index++;
                        current = current.next;
                        while (current != head)
                        {
                            if (current.Equals(elem))
                                return index;

                            current = current.next;
                            index++;
                        }
                    }
                    return -1;
                }

                public void print()
                {
                    Node<T> current = head;

                    if (current == head)
                        Console.WriteLine(current.data + " ");
                    
                    current = current.next;
                    while (current != head)
                    {
                        Console.Write(current.data + " ");
                        current = current.next;
                    }
                    

                }

            }


    //Tested
    /*public class Program1
    {
        public static void Main()
        {
            LocationLinkedList<string> l = new LocationLinkedList<string>();

            l.addNode(new Node<string>("Ciao,"));
            l.addNode(new Node<string>("Mondo\n"));
            Console.WriteLine(l.size());
            l.print();

            l.addNode(new Node<string>("World!\n"));
            l.print();

            Node<string> n1 = l.getNode(0);
            Console.WriteLine(n1.data);
            Console.WriteLine(n1.prev.data);
            Console.WriteLine(n1.next.data);
            Node<string> n2 = l.getNode(1);
            Console.WriteLine(n2.data);
            Console.WriteLine(n2.next.data);
            Console.WriteLine(n2.prev.data);
            // Print "World\n"

            Node<string> n3 = new Node<string>("Ciao,");
            //Node<string> n4 = l.getNode(n3);
            /*if (n4 != null)
                Console.WriteLine(n4.data);
            else
                Console.WriteLine("n4 is null\n");

            l[0].data = "Hello,";
            l.print(); // Print "Hello, World!\n"

            // Using the foreach construct on the SingleLinkedList object, as it has been now endowed with the enumerable capability
            // and a proper enumerator (iterator) has been defined
            Console.WriteLine("Scanning list useing a foreach:");
            foreach (string s in l)
                Console.WriteLine(s);
        }*/

    // Tested
    /*public class Program1
    {
        public static void Main()
        {
            LocationLinkedList<Location> l = new LocationLinkedList<Location>();

            l.addNode(new Node<Location>(new Location("Go")));
            l.addNode(new Node<Location>(new Location("Tile1")));
            l.addNode(new Node<Location>(new Location("Tile2")));
            l.addNode(new Node<Location>(new Location("Tile3")));
            Console.WriteLine("Size is:"+l.size());
            l.print();

            Node<Location> n1 = l.getNode(0);
            Console.WriteLine(n1.data);
            Console.WriteLine(n1.prev.data);
            Console.WriteLine(n1.next.data);

            Node<Location> n2 = l.MoveForward(0, 6);
            Console.WriteLine(n2.data.ToString());
            Console.WriteLine(n2.next.data.ToString());
            Console.WriteLine(n2.prev.data.ToString());
            // Print "World\n"
            //Node<string> n4 = l.getNode(n3);
            /*if (n4 != null)
                Console.WriteLine(n4.data);
            else
                Console.WriteLine("n4 is null\n");

            l[0].data = "Hello,";
            l.print(); // Print "Hello, World!\n"

            // Using the foreach construct on the SingleLinkedList object, as it has been now endowed with the enumerable capability
            // and a proper enumerator (iterator) has been defined
            Console.WriteLine("Scanning list useing a foreach:");
            foreach (string s in l)
                Console.WriteLine(s);
        }
        */
    

}


