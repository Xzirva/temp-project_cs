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
			Console.WriteLine (Scene.find(Scene.find(1).get_choices().First().get_next_scene()));
			Console.WriteLine (Character.find(2).get_profile());
		}
	}
}