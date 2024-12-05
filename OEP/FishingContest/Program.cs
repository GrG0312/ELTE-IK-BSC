using System;
using TextFile;

namespace FishingContest
{
	class Prgoram
	{
		public static void Main(string[] args)
		{
			Organization org = new Organization();
			try
			{
				TextFileReader reader = new("versenyek.txt");
				reader.ReadLine(out string line);
				string[] nevek = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string n in nevek)
				{
					org.Join(n);
				}
				while (reader.ReadString(out string verseny))
				{
					TextFileReader vf = new(verseny);
					vf.ReadLine(out string adatok);
					string[] strings = adatok.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					Contest contest = org.Organize(strings[0], DateTime.Parse(strings[1]));
					vf.ReadLine(out string horgaszok);
					string[] horgasztomb = horgaszok.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string horgasz in  horgasztomb)
					{
						contest.SignUp(org.Search(horgasz));
					}
					while (vf.ReadString(out string horgasz))
					{
						vf.ReadString(out string ora);
						vf.ReadString(out string fajta);
						vf.ReadDouble(out double tomeg);
						Fisher f = org.Search(horgasz);
						switch (fajta)
						{
							case "keszeg":
								f.Catch(DateTime.Parse(ora), Bream.Instance(), tomeg, contest);
								break;
							case "harcsa":
									f.Catch(DateTime.Parse(ora), Catfish.Instance(), tomeg, contest);
									break;
							case "ponty":
									f.Catch(DateTime.Parse(ora), Carp.Instance(), tomeg, contest);
									break;
							}
					}
				}

				if (org.BestContest(out Contest c))
				{
					Console.WriteLine($"Legjobb verseny: {c.place}");
				} else
				{
					Console.WriteLine("Nem volt megfelelő verseny!");
				}
			}
			catch (Contest.AlreadyRegisteredException)
			{
				Console.WriteLine("Már regisztrált a versenyre");
			}
			catch (Exception)
			{
				Console.WriteLine("Hiba történt!");
			}
		}
	}
}