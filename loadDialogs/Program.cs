using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace loadDialogs
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			//Dialogs d = new Dialogs();
			//d.ToString ();
			//XElement dialog = d.find_dialog (6);
			//Console.WriteLine (dialog);
			//List<Character> l = Character.get_all ();
			//foreach(Character c in l)
				Console.WriteLine (Dialogs.ToString());
		}
	}
}
