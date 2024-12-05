using System;
	
namespace Microwave
{
	class Program
	{
		public static void Main(string[] args)
		{
			Micro m = new Micro();
			m.Write();
			// lamp on
			m.door.Open();
			m.Write();
			// lamp off
			m.door.Close();
			m.Write();
			// lamp on, magn on
			m.button.Press();
			m.Write();
			// lamp on, magn off
			m.door.Open();
			m.Write();
		}
	}
}