using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;

namespace SetPathLibrary
{
    public class Model
    {
        public List<Node> AllNodes;
        public Node NodeStart;
        public Node NodeFinish;
        public int[,] Array;
        private List<Bitmap> Maps;

        public Model()
        {
            AllNodes = new List<Node>();
            XmlWork xmlWork = new XmlWork();
            Array = xmlWork.XmlRead(AllNodes);
            //Maps = SetMaps();
            Maps = new List<Bitmap>();
        }

        private void SetStartFinish(Node start, Node finish)
        {
            NodeStart = start;
            NodeFinish = finish;
        }

        private void SetMaps()
        {
            Maps.Clear();
            Maps.Add(new Bitmap("pic//1_etazh.bmp"));
            Maps.Add(new Bitmap("pic//2_etazh.bmp"));
            Maps.Add(new Bitmap("pic//3_etazh.bmp"));
            Maps.Add(new Bitmap("pic//4_etazh.bmp"));

        }

        private List<Node> GetRout()
        {
            if (NodeStart == null || NodeFinish == null)
            {
                return null;
            }
            int s = FindId(NodeStart);
            int f = FindId(NodeFinish);
            List<Node> Result = new List<Node>();
            int[] NodAr = new int[AllNodes.Count];
            List<int> Lar = new List<int>();
            for (int i = 0; i < NodAr.Length; ++i)
            {
                NodAr[i] = Int32.MaxValue;
            }

            NodAr[s] = 0;
            for (int i = 0; i < NodAr.Length; ++i)
            {
                if (NodAr[i] != Int32.MaxValue && !Lar.Contains(i))
                {
                    for (int j = 0; j < NodAr.Length; ++j)
                    {
                        if (Array[i, j] != 0 && !Lar.Contains(j))
                        {
                            if (NodAr[j] > (NodAr[i] + Array[i, j]))
                            {
                                NodAr[j] = NodAr[i] + Array[i, j];

                            }
                        }
                    }
                    Lar.Add(i);
                    i = 0;
                }
            }
            int min = int.MaxValue;
            int res = f;
            int a = f;
            Result.Add(AllNodes[res]);
            for (; ; )
            {
                min = int.MaxValue;
                for (int i = 0; i < NodAr.Length; ++i)
                {
                    if (Array[a, i] != 0)
                    {
                        if (NodAr[i] + Array[a, i] < min)
                        {
                            min = NodAr[i] + Array[a, i];
                            res = i;
                        }
                    }

                }
                a = res;
                Result.Add(AllNodes[res]);
                if (res == s)
                {
                    break;
                }

            }
            return Result;
        }

        private int FindId(Node node)
        {
            int i = 0;
            foreach (Node n in AllNodes)
            {
                if (n.X == node.X && n.Y == node.Y)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        private Node FindByName(string name)
        {
            foreach (Node n in AllNodes)
            {
                if (n.Name == name)
                {
                    return n;
                }
            }
            return null;
        }

        public List<Bitmap> GetMap(string startName, string finishName)
        {
            AllNodes = new List<Node>();
            XmlWork xmlWork = new XmlWork();
            Array = xmlWork.XmlRead(AllNodes);
            SetMaps();

            SetStartFinish(FindByName(startName), FindByName(finishName));
            List<Node> result = null;
            if (startName != "" || finishName != "")
            {
                result = GetRout();
            }

            Brush brush = Brushes.Black;
            System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
            Font f = new Font("Arial", 25);
            int count = 0;

            foreach (Bitmap b in Maps)
            {

                Graphics Graph = Graphics.FromImage(b);
                foreach (Node n in AllNodes)
                {
                    if (n.Floor == count)
                    {
                        Graph.DrawEllipse(myPen, n.X - 2, n.Y - 2, 10, 10);
                        Graph.DrawString("" + n.Id, f, brush, n.X + 4, n.Y + 4);
                    }
                }

                foreach (Node n in AllNodes)
                {
                    if (n.Floor == count)
                    {
                        foreach (Node m in n.Nodes)
                        {
                            Graph.DrawLine(myPen, n.X, n.Y, m.X, m.Y);
                        }
                    }
                }

                if (result != null)
                {
                    myPen.Color = Color.Orange;
                    myPen.Width = 10;
                    for (int i = 0; i < result.Count - 1; ++i)
                    {
                        if (result[i].Floor == count && result[i + 1].Floor == count)
                        {
                            Graph.DrawLine(myPen, result[i].X, result[i].Y, result[i + 1].X, result[i + 1].Y);
                        }
                    }
                    myPen.Width = 1;
                    myPen.Color = Color.Red;
                }
                count++;
            }

            //AllNodes = null;
            xmlWork = null;
            Array = null;
            return Maps;
        }

        public List<string> GetRoom()
        {
            List<string> rooms=new List<string>();
            foreach (Node n in AllNodes)
            {
                if (n.Name != "")
                {
                    rooms.Add(n.Name);
                }
            }
            return rooms;
        }
    }
}
