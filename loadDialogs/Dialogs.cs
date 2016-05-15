using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;


namespace loadDialogs
{
	class CustomException : Exception
	{
		public CustomException(string message)
		{

		}
	}

	[System.AttributeUsage(System.AttributeTargets.Class |
		System.AttributeTargets.Struct,
		AllowMultiple = true)]// multiuse attribute


	public class Character : System.Attribute
	{
		//-------------------------------Private--------------------------------
		private static List<Character> all = new List<Character>();
		private string id;
		private string name;

		private static void loadCharacters()
		{
			XElement root;
			if (all.Count () == 0) {
				Console.WriteLine ("Real Load from XML File");
				XDocument xchar = XDocument.Load ("characters.xml");
				root = xchar.Root;
			
				IEnumerable<XElement> chars =
					(from el in root.Elements ("char")
					//where (string)el.Attribute("id") == id.ToString()
					select el);

				List<Character> l = new List<Character> ();
				foreach (XElement c in chars) {
					Character ch = new Character ((string)c.Attribute ("id"), (c.FirstNode as XText).Value);
					l.Add (ch);
				}
				all = l;
			}
		}

		//-------------------------------Public Methods--------------------------------
		public static List<Character>get_all(){
			loadCharacters();
			return all;
		}

		private Character (string i, string n){
			id = i;
			name = n;
		}
		public static Character find(int i)
		{
			//Input Control to be done 
			loadCharacters ();
			foreach (Character c in all){
				if (c.get_id () == i.ToString())
						return c;
			}
			throw new CustomException("Character Not Found");
		}

		public static Character find(string i)
		{
			//Input Control to be done 
			loadCharacters ();
			foreach (Character c in all){
				if (c.get_id () == i)
					return c;
			}
			throw new CustomException("Character Not Found");
		}

		public string get_name(){
			return name;
		}
		public string get_id(){
			return id.ToString();
		}

	}
		
	public class Dialog : System.Attribute
	{
		private string id;

		private List<Part> parts;

		public Dialog(string i, List<Part> p){
			id = i;
			parts = p;
		}
		public string ToString(){
			string render = "";
			foreach (Part p in parts) {

				render = render + p.ToString();
				render = render + Environment.NewLine;
			}
			return render;
		}

		public string get_id(){
			return id;
		}
	}

	public class Part : System.Attribute
	{
		private Character chr;
		private string text;

		public Part(Character c, string t){
			chr = c;
			text = t;
		}

		public string get_chr(){
			return chr.get_name();
		}

		public string get_text(){
			return text;
		}

		public override string ToString(){
			string render = get_chr () + ":" + get_text ();
			return render;
		}


	}

	public class Dialogs : System.Attribute
	{
		private static List<Dialog> all = new List<Dialog>();
			/*public IEnumerable<Part> content()
		{
			return xdialog.Parts();
		}
		/*public XElement find_parts (int id){
			return;
		}*/
		
		private Dialogs (){
		}

		private static void loadDialogs()
		{
			XElement root;
			if (all.Count () == 0) {
					Console.WriteLine ("Real Load from XML File");
					XDocument srcTree = XDocument.Load ("dialogs.xml");
					root = srcTree.Root;
		
				IEnumerable<XElement> dialogs =
					(from el in root.Elements ("d")
						//where (string)el.Attribute("id") == id.ToString()
						select el);

				List<Dialog> l = new List<Dialog> ();

				foreach (XElement d in dialogs) {
					//Console.WriteLine ("---------------------------Getting Dialogs' parts-----------------------");
					//---------------------------Parsing Dialogs' parts-----------------------
					IEnumerable<XElement> parts =
						(from el in d.Elements ("part")
							//where (string)el.Attribute("id") == id.ToString()
							select el);

					List<Part> l_parts = new List<Part> ();
					foreach (XElement p in parts) {
						//Console.WriteLine ("---------------------------Parsing part-----------------------");
						Part part = new Part (Character.find((string)p.Attribute ("char")), (p.FirstNode as XText).Value);
						l_parts.Add (part);
					}
					//Console.WriteLine ("---------------------------Parsing Dialog------------------------------");
					//---------------------------Parsing Dialogs------------------------------
					Dialog diag = new Dialog ((string)d.Attribute ("id"), l_parts);
					l.Add (diag);
				}
				all = l;
			}
		}
		/*public static List<Dialog> get_all(){
			loadDialogs ();
			return all;
		}*/
			
		public static string ToString(){
			loadDialogs	();
			int i = 1;
			string render = "----------------------Dialogs-----------------------------\n";
			string end = "----------------------Dialogs: End-----------------------------\n";
			foreach (Dialog d in all) {
				string sd = "----------------------Dialog " + i.ToString() + ": Start-----------------------------\n";
				render = render + sd;
				render = render + d.ToString(); 
				//foreach (XElement e in el.Elements())
				string ed = "----------------------Dialog " + i.ToString() + ": End-----------------------------\n";
				render = render + ed;
				i = i + 1;
			}
			render = render + end;
			return render;
		}

		public Dialog find(int id){
			loadDialogs ();
			foreach (Dialog d in all)
				if (d.get_id () == id.ToString ())
					return d;
			throw new CustomException ("Dialog Not Found");
		}
		public Dialog find(string id){
			loadDialogs ();
			foreach (Dialog d in all)
				if (d.get_id () == id)
					return d;
			throw new CustomException ("Dialog Not Found");
		}
	}



}

