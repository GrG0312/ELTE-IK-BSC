using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bomberman_Prototype1.Persistence
{
    public class ProfileLoader
    {
		private const string PATH = "../../../../Model/Persistence/Profiles/profiles.txt";
		private List<Profile> Profiles=new List<Profile>();
		private List<string> Pictures = new List<string>();
		private int profileIndex;
		private int pictureIndex;
        public ProfileLoader()
        {
			FillPicturesList();
        }
		public Profile GetCurrentIndexedProfile()
		{
			return Profiles[profileIndex];
		}
        public string GetCurrentIndexedPicture()
        {
            return Pictures[pictureIndex];
        }
        public Profile GetNextProfileByIndex()
		{
			profileIndex++;
			if(profileIndex < Profiles.Count)
			{
				return Profiles[profileIndex];
			} else
			{
				profileIndex = 0;
				return Profiles.First();
			}
		}
        public string GetNextPictureByIndex()
        {
            pictureIndex++;
            if (pictureIndex>=Pictures.Count)
            {
                pictureIndex = 0;
            }
            return Pictures[pictureIndex];
        }
        public void Load()
        {
			try
			{
				using (StreamReader reader = new StreamReader(AppContext.BaseDirectory + PATH))
				{
					Profiles = new List<Profile>();
					while (!reader.EndOfStream)
					{
						string? line=reader.ReadLine();
						string[] data = line!.Split(';', StringSplitOptions.RemoveEmptyEntries);
						Profile profile = new Profile(data[1], int.Parse(data[2]), int.Parse(data[3]), data[4]);
						Profiles.Add(profile);
						if (data[0] == "True")
						{
							profileIndex = Profiles.Count - 1;
							pictureIndex = Pictures.IndexOf(data[4]);
						}
					}
					if (Profiles.Count == 0)
					{
						Profiles.Add(new Profile("Default", 0, 0, "../View/Resources/Characters/Char1Front2.png"));
						pictureIndex = 0;
						profileIndex = 0;
					}
                }
			}
			catch
			{
				throw new System.IO.FileNotFoundException("Error loading the profile data");
			}
        }
		public void Save(Profile profile)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(AppContext.BaseDirectory + PATH, false))
				{
					foreach (Profile prof in Profiles)
					{
						if (prof.Name != profile.Name)
						{
							writer.WriteLine($"False;{prof.Name};{prof.PlayedGames};{prof.GamesWon};{prof.SpritePath};");
						}
					}
					writer.WriteLine($"True;{profile.Name};{profile.PlayedGames};{profile.GamesWon};{profile.SpritePath};");
					Profiles.Add(profile);
				}
			}
			catch
			{
				throw new System.IO.FileNotFoundException("Error saving the profile data");
			}
        }
		public void Update()
		{
            try
            {
                using (StreamWriter writer = new StreamWriter(AppContext.BaseDirectory + PATH, false))
                {
					for (int i = 0; i < Profiles.Count; i++)
					{
						Profile prof = Profiles[i];
                        writer.WriteLine($"{ i == profileIndex };{prof.Name};{prof.PlayedGames};{prof.GamesWon};{prof.SpritePath};");
                    }
                }
            }
            catch
            {
                throw new System.IO.FileNotFoundException("Error saving the profile data");
            }
        }
        private void FillPicturesList()
        {
            for (int i = 1; i < 4; i++)
			{
				Pictures.Add("../View/Resources/Characters/Char"+i+"Front2.png");
			}
		}
    }
}
