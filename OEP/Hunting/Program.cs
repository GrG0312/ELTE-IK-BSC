using System;

namespace Hunting
{
	class Program
	{
		static void Main(string[] args)
		{
			Hunter h = new Hunter("xyz", "1990");
			Read(h, "input.txt");
			Console.WriteLine($"{h.MaleLions()} hím oroszlánt fogott.");
			bool found = h.MaxHornWeight(out double ratio);
			if (found)
			{
				Console.WriteLine($"A legnagyobb szarv / tömeg arány {ratio} volt.");
			} else
			{
				Console.WriteLine("Nem volt orrszarvú!");
			}
			Console.Write(h.SameFangLength() ? "Volt " : "Nem volt ");
			Console.WriteLine("olyan elefánt, melynek megegyeztek az agyarak méretei.");
		}

		static void Read(Hunter h, string fn)
		{
			try
			{
				TextFile.TextFileReader reader = new(fn);
				while (reader.ReadLine(out string line))
				{
					string[] strings = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					Animal a = null!;
					switch (strings[2])
					{
						case "lion":
							a = new Lion(int.Parse(strings[3]), "male" == strings[4]);
							break;
						case "rhino":
							a = new Rhino(int.Parse(strings[3]), "male" == strings[4], int.Parse(strings[5]));
							break;
						case "elephant":
							a = new Elephant(int.Parse(strings[3]), "male" == strings[4], int.Parse(strings[5]), int.Parse(strings[6]));
							break;
						default:
							continue;
					}
					h.Kill(strings[0], strings[1], a);
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Nem található bemeneti fájl!");
			}
		}
	}
}