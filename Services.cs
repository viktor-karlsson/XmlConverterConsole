using System.Xml.Linq;


namespace XmlConverterConsole
{
    internal class Services
    {
        public static string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static void CreatePeopleFromFile(string data, string savePath = @"C:\MyXmlFile.xml")
        {
            try
            {
                var dataRows = data.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var document = new XDocument(new XElement("people"));
                var previousNodeType = "";

                foreach (var parts in dataRows.Select(row => row.Split('|')))
                {
                    previousNodeType = parts[0] switch
                    {
                        "P" => "person",
                        "F" => "family",
                        _ => previousNodeType
                    };
                    switch (parts[0])
                    {
                        case "P":
                            XmlAddPerson(parts, document);
                            break;

                        case "T":
                            XmlAddPhone(parts, previousNodeType, document);
                            break;

                        case "A":
                            XmlAddAddress(parts, previousNodeType, document);
                            break;

                        case "F":
                            XmlAddFamily(parts, previousNodeType, document);
                            break;
                    }
                }

                document.Save(savePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private static void XmlAddPerson(string[] data, XContainer document)
        {
            var element = document.Element("people") ?? throw new Exception("Element People does not exist");
            element.Add(
                new XElement("person",
                    new XElement("firstname", data[1]),
                    new XElement("lastname", data[2])));
        }

        private static void XmlAddPhone(string[] data, string previousNodeType, XContainer document)
        {
            var lastPerson = document.Descendants(previousNodeType).LastOrDefault() ??
                             throw new Exception($"Node {previousNodeType} does not exist");

            foreach (var (part, index) in data.Select((value, index) => (value, index)))
            {
                if (index == 0) continue;

                switch (index)
                {
                    case 1:
                        lastPerson.Add(new XElement("phone",
                            new XElement("mobile", part)));
                        break;
                    case 2:
                        lastPerson = document.Descendants("phone").LastOrDefault() ??
                                     throw new Exception($"Node {previousNodeType} does not exist");
                        lastPerson.Add(new XElement("landline", part));
                        break;
                }
            }
        }

        private static void XmlAddAddress(string[] data, string previousNodeType, XContainer document)
        {
            var lastPerson = document.Descendants(previousNodeType).LastOrDefault() ??
                             throw new Exception($"Node {previousNodeType} does not exist");
            foreach (var (part, index) in data.Select((value, index) => (value, index)))
            {
                if (index == 0) continue;

                switch (index)
                {
                    case 1:
                        lastPerson.Add(new XElement("address",
                            new XElement("street", part)));
                        break;
                    case 2:
                        lastPerson = document.Descendants("address").LastOrDefault() ??
                                     throw new Exception($"Node {previousNodeType} does not exist");
                        lastPerson.Add(new XElement("city", part));
                        break;
                    case 3:
                        lastPerson.Add(new XElement("zipcode", part));
                        break;
                }
            }
        }

        private static void XmlAddFamily(string[] data, string previousNodeType, XContainer document)
        {
            var lastPerson = document.Descendants("person").LastOrDefault() ??
                             throw new Exception($"Node {previousNodeType} does not exist");
            foreach (var (part, index) in data.Select((value, index) => (value, index)))
            {
                if (index == 0) continue;

                switch (index)
                {
                    case 1:
                        lastPerson.Add(new XElement("family",
                            new XElement("name", part)));
                        break;
                    case 2:
                        lastPerson = document.Descendants("family").LastOrDefault() ??
                                     throw new Exception($"Node {previousNodeType} does not exist");
                        lastPerson.Add(new XElement("born", part));
                        break;
                }
            }
        }
    }
}