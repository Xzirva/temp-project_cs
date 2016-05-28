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
		private string profile;
		private string name;

		private static void loadCharacters()
		{
			XElement root;
			if (all.Count () == 0) {
				Console.WriteLine ("Real Load from XML File");
				XDocument xchar = XDocument.Load ("story.xml");
				root = xchar.Root;
			
				IEnumerable<XElement> char_tag =
					(from el in root.Elements ("characters")
					//where (string)el.Attribute("id") == id.ToString()
					select el);

				List<Character> l = new List<Character> ();
				XElement c = char_tag.First();

					IEnumerable<XElement> chars =
						(from el in c.Elements ("char")
							//where (string)el.Attribute("id") == id.ToString()
							select el);

					foreach(XElement ch in chars){

						Character cha = new Character ((string)ch.Attribute ("id"), (string)ch.Attribute ("face"),(ch.FirstNode as XText).Value);
						l.Add (cha);
						//Console.WriteLine (ch);
					
				}
				all = l;
			}
		}

		//-------------------------------Public Methods--------------------------------
		public static List<Character>get_all(){
			loadCharacters();
			return all;
		}

		private Character (string i, string picture,string n){
			id = i;
			profile = picture; 
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
		public static int count(){
			loadCharacters ();
			return all.Count ();
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
		public string get_profile(){
			return profile;
		}

	}

	public class Scene : System.Attribute{

		private static List<Scene> all;
		private string id;
		private string map;
		private List<Content> contents;
		private List<Choice> choices;

		private static void loadScenes(){
			XElement root;
			if (all == null || all.Count () == 0) {
				Console.WriteLine ("Real Load from XML File");
				XDocument xchar = XDocument.Load ("story.xml");
				root = xchar.Root;

				IEnumerable<XElement> _tag =
					(from el in root.Elements ("scenes")
						//where (string)el.Attribute("id") == id.ToString()
						select el);

				List<Scene> l = new List<Scene> ();
				XElement c = _tag.First();

				IEnumerable<XElement> scenes =
					(from el in c.Elements ("scene")
						//where (string)el.Attribute("id") == id.ToString()
						select el);

				foreach(XElement s in scenes){
					List<Content> lc = new List<Content> ();
					IEnumerable<XElement> content_tag =
						(from el in s.Elements ("content")
							//where (string)el.Attribute("id") == id.ToString()
							select el);
					XElement ct = content_tag.First();
					 
					IEnumerable<XElement> cins =
						(from el in ct.Elements ("cinematic")
							//where (string)el.Attribute("id") == id.ToString()
							select el);
					foreach (XElement cin in cins) {
						Content cont = Content.buildCinematic((string)cin.Attribute ("id"));
						lc.Add (cont);
						//Console.WriteLine (lc.Count ());
					}

					IEnumerable<XElement>  dialogs =
						(from el in ct.Elements ("dialog")
							//where (string)el.Attribute("id") == id.ToString()
							select el);
					foreach (XElement d in dialogs) {
						IEnumerable<XElement> parts =
							(from el in d.Elements ("part")
							//where (string)el.Attribute("id") == id.ToString()
							select el);

						List<Part> l_parts = new List<Part> ();
						foreach (XElement p in parts) {
							//Console.WriteLine ("---------------------------Parsing part-----------------------");
							Part part = new Part (Character.find ((string)p.Attribute ("char")), (p.FirstNode as XText).Value);
							l_parts.Add (part);
						}
						//Console.WriteLine ("---------------------------Parsing Dialog------------------------------");
						//---------------------------Parsing Dialogs------------------------------
						Content diag = Content.buildDialog ((string)d.Attribute ("id"), l_parts);
						lc.Add (diag);
						//Console.WriteLine (lc.Count ());
					}






					Scene sc = new Scene((string)s.Attribute ("id"), (string)s.Attribute ("map"),lc);
					IEnumerable<XElement> choices =
						(from el in s.Elements ("choices")
							//where (string)el.Attribute("id") == id.ToString()
							select el);
					foreach (XElement cho in choices) {
						IEnumerable<XElement> choices_s =
							(from el in cho.Elements ("c")
								//where (string)el.Attribute("id") == id.ToString()
								select el);

						List<Choice> l_choices = new List<Choice> ();
						foreach (XElement p in choices_s) {
							//Console.WriteLine ("---------------------------Parsing part-----------------------");
							Choice choi = new Choice ((string)p.Attribute ("goto"), (p.FirstNode as XText).Value);
							l_choices.Add (choi);
						}
						//Console.WriteLine ("---------------------------Parsing Dialog------------------------------");
						//---------------------------Parsing Dialogs------------------------------
						sc.addChoices(l_choices);
						//Console.WriteLine (lc.Count ());
					}
					l.Add (sc);
					//Console.WriteLine (ch);

				}
				all = l;
			}
		}

		public List<Choice> get_choices(){
			return choices;
		}

		public static List<Scene> get_all(){
			return all;
		}

		private Scene(string i, string m, List<Content> c){
			id = i;
			map = m;
			contents = c;
		}

		public void addChoices(List<Choice> ch){
			this.choices = ch;
		}

		public static int count(){
			loadScenes ();
			return all.Count ();
		}

		public List<Content> get_contents(){
			return contents;
		}

		public static Scene find(int i)
		{
			//Input Control to be done 
			loadScenes ();
			foreach (Scene s in all){
				if (s.get_id() == i.ToString())
					return s;
			}
			throw new CustomException("Scene Not Found");
		}

		public static Scene find(string i)
		{
			//Input Control to be done 
			loadScenes ();
			foreach (Scene s in all){
				if (s.get_id() == i)
					return s;
			}
			throw new CustomException("Scene Not Found");
		}
		public string get_id(){
			return id;
		}

		public string ToStringChoices(){
			string render = "";
			foreach (Choice c in choices) {

				render = render + c.ToString();
				render = render + Environment.NewLine;
			}
			return render;
		}

		public string get_map(){
			return map;
		}
			
	}

	public enum content_type {DIALOG, CINEMATIC, CHOICE};

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

	public class Choice : System.Attribute
	{
		private string next_scene;
		private string text;


		public Choice(string ns, string t){
			next_scene = ns;
			text = t;
		}

		public string get_next_scene(){
			return next_scene;
		}

		public string get_text(){
			return text;
		}

		public override string ToString(){

			return get_text ();
		}


	}
	public class Content : System.Attribute
	{
		private string id;
		public static content_type type;

		private List<Part> parts;

		public static Content buildDialog(string i, List<Part> p){
			Content d = new Content (i, content_type.DIALOG);
			d.parts = p;
			return d;
		}
		public static Content buildCinematic(string i){
			Content c = new Content (i, content_type.CINEMATIC);
			return c;
		}

		public Content (string i, content_type t){
			id = i;
			type = t;
		}
		public Content (content_type t){
			type = t;
		}

		public string ToStringDialog(){
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
		public content_type get_type(){
			return type;
		}
	}
////
//	public class Dialogs : System.Attribute
//	{
//		private static List<Dialog> all = new List<Dialog>();
//			/*public IEnumerable<Part> content()
//		{
//			return xdialog.Parts();
//		}
//		/*public XElement find_parts (int id){
//			return;
//		}*/
//		
//		private Dialogs (){
//		}
//
//		private static void loadDialogs()
//		{
//			XElement root;
//			if (all.Count () == 0) {
//					Console.WriteLine ("Real Load from XML File");
//					XDocument srcTree = XDocument.Load ("dialogs.xml");
//					root = srcTree.Root;
//		
//				IEnumerable<XElement> dialogs =
//					(from el in root.Elements ("d")
//						//where (string)el.Attribute("id") == id.ToString()
//						select el);
//
//				List<Dialog> l = new List<Dialog> ();
//
//				foreach (XElement d in dialogs) {
//					//Console.WriteLine ("---------------------------Getting Dialogs' parts-----------------------");
//					//---------------------------Parsing Dialogs' parts-----------------------
//					IEnumerable<XElement> parts =
//						(from el in d.Elements ("part")
//							//where (string)el.Attribute("id") == id.ToString()
//							select el);
//
//					List<Part> l_parts = new List<Part> ();
//					foreach (XElement p in parts) {
//						//Console.WriteLine ("---------------------------Parsing part-----------------------");
//						Part part = new Part (Character.find((string)p.Attribute ("char")), (p.FirstNode as XText).Value);
//						l_parts.Add (part);
//					}
//					//Console.WriteLine ("---------------------------Parsing Dialog------------------------------");
//					//---------------------------Parsing Dialogs------------------------------
//					Dialog diag = new Dialog ((string)d.Attribute ("id"), l_parts);
//					l.Add (diag);
//				}
//				all = l;
//			}
//		}
//		/*public static List<Dialog> get_all(){
//			loadDialogs ();
//			return all;
//		}*/
//			
//		public static string ToString(){
//			loadDialogs	();
//			int i = 1;
//			string render = "----------------------Dialogs-----------------------------\n";
//			string end = "----------------------Dialogs: End-----------------------------\n";
//			foreach (Dialog d in all) {
//				string sd = "----------------------Dialog " + i.ToString() + ": Start-----------------------------\n";
//				render = render + sd;
//				render = render + d.ToString(); 
//				//foreach (XElement e in el.Elements())
//				string ed = "----------------------Dialog " + i.ToString() + ": End-----------------------------\n";
//				render = render + ed;
//				i = i + 1;
//			}
//			render = render + end;
//			return render;
//		}
//
//		public Dialog find(int id){
//			loadDialogs ();
//			foreach (Dialog d in all)
//				if (d.get_id () == id.ToString ())
//					return d;
//			throw new CustomException ("Dialog Not Found");
//		}
//		public Dialog find(string id){
//			loadDialogs ();
//			foreach (Dialog d in all)
//				if (d.get_id () == id)
//					return d;
//			throw new CustomException ("Dialog Not Found");
//		}
//	}
//
//
//
}

