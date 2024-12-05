using System;
	
namespace Microwave
{
	class Program
	{
		public static void Main(string[] args)
		{
			Micro m = new Micro();
			Thread.Sleep(500);
			m.Write();
			// lamp on
			m.door.Open();
			Thread.Sleep(500);
			m.Write();
			// lamp off
			m.door.Close();
			Thread.Sleep(500);
			m.Write();
			// lamp on, magn on
			m.button.Press();
			Thread.Sleep(500);
			m.Write();
			// lamp on, magn off
			m.door.Open();
			Thread.Sleep(500);
			m.Write();
			m.Stop();
			Thread.Sleep(500);
		}
	}
}