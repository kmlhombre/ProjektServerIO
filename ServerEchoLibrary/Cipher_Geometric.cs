using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace szyfr_przestawieniowy_geo
{
	class Szyfr_geo
	{
		#region Fields

		static int arrayx = 0;
		static int arrayy = 0;
		//static String key, msg;
		static List<int> lines = new List<int>();
		static List<List<char>> array = new List<List<char>>();
		static List<char> answer = new List<char>();

		private
			static string msg, key;

		#endregion

		#region Methods

		#region Comment
		/// <summary>
		/// Seting number of available cells to use in process of ciphering.
		/// </summary>
		/// <param name="key">
		/// Key for ciphering.
		/// </param>
		/// <returns>
		/// Array with informations about available space.
		/// </returns>
		#endregion
		static protected List<int> setLines(String key)
		{
			int intx;
			char x;
			List<int> lines = new List<int>();
			for (int i = 0; i < key.Length; i++)
			{
				x = (char)(key[i]);
				intx = (int)x - 48;
				intx -= 48;
				lines.Add(intx);
			}
			return lines;
		}

		#region Comment
		/// <summary>
		/// Checking for biggest ASCII value in key to set 2D array x size.
		/// </summary>
		/// <param name="key">
		/// Key for ciphering.
		/// </param>
		/// <param name="msg">
		/// Message to encrypt.
		/// </param>
		/// <returns>
		/// 2D array x size.
		/// </returns>
		#endregion
		static protected int arraySizeX(String key, String msg)
		{
			char x = 'a';
			for (int i = 0; i < key.Length; i++)
			{
				if ((char)(key[i]) > x)
				{
					x = (char)(key[i]);
				}
			}
			int intx = (int)x - 48;
			intx -= 48;
			return intx;

		}

		#region Comment
		/// <summary>
		/// Finding how many rows are needed to set 2D array y size.
		/// </summary>
		/// <param name="vec">
		/// Array with informations about available space.
		/// </param>
		/// <param name="key">
		/// Key for ciphering.
		/// </param>
		/// <param name="msg">
		/// Message to encrypt.
		/// </param>
		/// <returns>
		/// 2D array y size.
		/// </returns>
		#endregion
		static protected int arraySizeY(List<int> vec, String key, String msg)
		{
			int a = msg.Length;
			int y = 0;
			for (int i = 0; i < vec.Count; i++)
			{
				a -= vec[i];
				y++;
				if (a < 0) break;
			}
			//double c = ceil(a / x);

			return y;
		}

		#region Comment
		/// <summary>
		/// Check: Is in 2D array enought space for message?
		/// </summary>
		/// <param name="vec">
		/// Array with informations about available space.
		/// </param>
		/// <param name="msg">
		/// Message to cipher.
		/// </param>
		/// <returns>
		/// (true)	- enough space
		/// (false)	- not enough space (key is to short)
		/// </returns>
		#endregion
		static protected Boolean enoughSpace(List<int> vec, String msg)
		{
			int d = 0;
			for (int i = 0; i < vec.Count; i++)
			{
				d += d + vec[i];
			}
			//double s = d / vec.size();
			//double x = ceil(msg.length() / s);
			if (d < msg.Length) return false;
			else return true;
		}

		#region Comment
		/// <summary>
		/// Adding new line(list) to list of lists.
		/// </summary>
		/// <param name="array">
		/// List to which we add list.
		/// </param>
		#endregion
		static protected void createLine(List<List<char>> array)
		{
			List<char> line = new List<char>();
			for (int i = 0; i < arrayx; i++)
			{
				line.Add('X');
			}
			array.Add(line);
		}


		#endregion

		#region Constructor

		public Szyfr_geo(string add_key, string add_msg)
		{
			msg = add_msg;
			key = add_key;
		}

		#endregion

		static void Run()
		{	
			//Wyznaczanie parametrow
			System.Console.WriteLine("Wiadomosc:\n" + msg + "\nKlucz:\n" + key + "\n");
			int msg_size = msg.Count();
			lines = setLines(key);
			arrayx = arraySizeX(key, msg);
			arrayy = arraySizeY(lines, key, msg);

			//Sprawdzenie klucza
			Boolean check_ = enoughSpace(lines, msg);
			if (!check_)
			{
				System.Console.WriteLine("Za krotki klucz\n");
			}

			//Tworzenie tablicy
			for (int i = 0; i < arrayy; i++)
			{
				createLine(array);
			}

			//Uzupelnianie tablicy
			int check = 0;
			bool cpp_jest_podjezykiem = false;
			for (int i = 0; i < arrayy; i++)
			{
				for (int w = 0; w < arrayx; w++)
				{
					if (msg[w + i] == 0) cpp_jest_podjezykiem = true;

					if ((w < lines[i]) && (cpp_jest_podjezykiem == false)&&(w + check <= msg_size - 1))
					{	
						if (msg[w + check] == 0) cpp_jest_podjezykiem = true;
						array[i][w] = msg[w + check];
					}
					else
					{
						array[i][w] = ' ';
					}
				}
				check += lines[i];
			}

			//Wyswietlanie tablicy
			System.Console.WriteLine("Tablica:\n");
			for (int i = 0; i < arrayy; i++)
			{
				string tab_line = null;				
				for (int y = 0; y < arrayx; y++)
				{
                    //System.Console.WriteLine(array[i][y] + " ");
                    string newcharacter = array[i][y].ToString();
					tab_line += newcharacter;
				}
				System.Console.WriteLine(tab_line + " ");
			}

			//Wyswietlanie zaszyfrowaniej wiadomosci
			System.Console.WriteLine("\nZaszyfrowana wiadomosc:\n");
			for (int y = 0; y < arrayx; y++)
            {
				for (int i = 0; i < arrayy; i++)
                {
					if((array[i][y]!=' ') && (array[i][y] != 0))
                    {
						//System.Console.WriteLine(array[i][y]);
						System.Console.Write(array[i][y]);
					}
					answer.Add(array[i][y]);
                }
            }

			//Warunek do deszyfracji		
			System.Console.WriteLine("\n\n(1) - dalej\n(2) - koniec\n");
			string step = System.Console.ReadLine();
			cpp_jest_podjezykiem = false;
			check = 0;
			answer.Clear();

			//Wyswitlanie zaszyfrowanej wiadomosci
			if (step == "1")
			{
				System.Console.WriteLine("\nOrginalna widomosc\n\n");

				for(int y = 0; y < arrayy; y++)
                {
					for(int i = 0; i < arrayx; i++)
                    {
						if ((array[y][i] != ' ') && (array[y][i] != 0))
							//System.Console.WriteLine(array[y][i]);
							System.Console.Write(array[y][i]);
					}
                }
				System.Console.WriteLine("Koniec\n");
			}
			else
			{
				System.Console.WriteLine("Koniec\n");
			}
		}
	}
}