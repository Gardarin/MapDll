using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetPathLibrary
{
    public class Node
    {
        public int Id;
        public string Name;
        public int Floor;
        public int X;
        public int Y;
        public List<Node> Nodes;

        public Node(int id, string name, int floor, int x, int y)
        {
            Id = id;
            Name = name;
            Floor = floor;
            X = x;
            Y = y;
            Nodes = new List<Node>();
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }
    }
}
