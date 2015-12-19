using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace SetPathLibrary
{
    class XmlWork
    {
        private void SetFileAtribute(XmlDocument xd, XmlNode xn, Node node)
        {
            XmlAttribute attribute = xd.CreateAttribute("Id"); // создаём атрибут
            attribute.Value = "" + node.Id; // устанавливаем значение атрибута
            xn.Attributes.Append(attribute);

            attribute = xd.CreateAttribute("Name"); // создаём атрибут
            attribute.Value = node.Name; // устанавливаем значение атрибута
            xn.Attributes.Append(attribute);

            attribute = xd.CreateAttribute("Floor"); // создаём атрибут
            attribute.Value = "" + node.Floor; // устанавливаем значение атрибута
            xn.Attributes.Append(attribute);

            attribute = xd.CreateAttribute("X"); // создаём атрибут
            attribute.Value = "" + node.X; // устанавливаем значение атрибута
            xn.Attributes.Append(attribute);

            attribute = xd.CreateAttribute("Y"); // создаём атрибут
            attribute.Value = "" + node.Y; // устанавливаем значение атрибута
            xn.Attributes.Append(attribute);

        }

        private XmlDocument CreateXmlFile(string fullName)
        {
            XmlTextWriter textWritter = new XmlTextWriter(fullName, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("head");
            textWritter.WriteEndElement();
            textWritter.Close();

            return null;
        }

        private Node FindNod(int id,List<Node> nodes)
        {
            foreach (Node n in nodes)
            {
                if (n.Id == id)
                {
                    return n;
                }
            }
            return null;
        }

        public void XmlPrint(List<Node> nodes)
        {
            XmlDocument XmlDocument = new XmlDocument();
            XmlNode xmlNodeFile;
            XmlNode xmlNode;

            string path = "Map.xml";
            CreateXmlFile(path);
            XmlDocument.Load(path);
            System.IO.FileStream file = System.IO.File.Create(path);

            foreach (Node n in nodes)
            {
                xmlNodeFile = XmlDocument.CreateElement("Point");
                SetFileAtribute(XmlDocument, xmlNodeFile, n);
                foreach (Node m in n.Nodes)
                {
                    xmlNode = XmlDocument.CreateElement("P");
                    XmlAttribute attribute = XmlDocument.CreateAttribute("Id"); // создаём атрибут
                    attribute.Value = "" + m.Id; // устанавливаем значение атрибута
                    xmlNode.Attributes.Append(attribute);
                    xmlNodeFile.AppendChild(xmlNode);
                }
                XmlDocument.DocumentElement.AppendChild(xmlNodeFile);
            }

            XmlDocument.Save(file);
            file.Close();
        }

        public int[,] XmlRead(List<Node> nodes)
        {
            XmlDocument XmlDocument = new XmlDocument();

            string path = "11.xml";

            XmlDocument.Load(path);

            foreach (XmlNode xn in XmlDocument.DocumentElement.ChildNodes)
            {


                nodes.Add(new Node(Convert.ToInt32(xn.Attributes.GetNamedItem("Id").Value), xn.Attributes.GetNamedItem("Name").Value, 
                    Convert.ToInt32(xn.Attributes.GetNamedItem("Floor").Value),
                    Convert.ToInt32(xn.Attributes.GetNamedItem("X").Value), 
                    Convert.ToInt32(xn.Attributes.GetNamedItem("Y").Value)));
            }

            Node node;
            foreach (XmlNode xn in XmlDocument.DocumentElement.ChildNodes)
            {
                node = FindNod(Convert.ToInt32(xn.Attributes.GetNamedItem("Id").Value),nodes);
                foreach (XmlNode m in xn.ChildNodes)
                {
                    node.Nodes.Add(FindNod(Convert.ToInt32(m.Attributes.GetNamedItem("Id").Value),nodes));
                }
            }

            int[,] ar = new int[nodes.Count, nodes.Count];

            foreach (Node n in nodes)
            {
                foreach (Node m in n.Nodes)
                {
                    ar[n.Id, m.Id] = Convert.ToInt32(Math.Sqrt((n.X - m.X) * (n.X - m.X) + (n.Y - m.Y) * (n.Y - m.Y)));
                }
            }

            return ar;
        }
    }
}
