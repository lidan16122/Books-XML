using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace server.Models.Constants
{
    public class BooksXML
    {
        private readonly string? XMLPath;
        public XDocument Doc;
        public BooksXML()
        {
            XMLPath = Environment.GetEnvironmentVariable("BOOKS_XML_PATH");

            if (string.IsNullOrWhiteSpace(XMLPath))
            {
                throw new Exception("could not find the books xml path");
            }

            if (!File.Exists(XMLPath))
            {
                Doc = new XDocument(new XElement("bookstore"));
                Doc.Save(XMLPath);
            }
            else
            {
                Doc = XDocument.Load(XMLPath);
            }
        }
        public void SaveDoc() => Doc.Save(XMLPath);
    }
}
