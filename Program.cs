// See https://aka.ms/new-console-template for more information

using XmlConverterConsole;

const bool runner = true;
Console.WriteLine(" Welcome to this simple Xml-Converter! \n Enter 'start' to begin or exit to quit");

while (runner)
{
    var input = Console.ReadLine();


    if (input is null or "")
    {
        Console.WriteLine("Enter 'start' to begin or exit to quit");
        continue;
    }

    if (input.ToLower().Equals("exit"))
    {
        Console.WriteLine(" Exiting program");
        Environment.Exit(0);
    }


    if (input.ToLower().Equals("start"))
    {
        try
        {
            Console.Write(" File to read: ");
            var file = Console.ReadLine();
            Console.Write("\n Xml-name and path: ");
            var savePath = Console.ReadLine() ?? string.Empty;


            var fileContent = Services.ReadFile(file ?? throw new Exception(" No file found"));

            Services.CreatePeopleFromFile(fileContent, savePath);

            Console.WriteLine(" Xml has been generated from file");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            continue;
        }
    }
}