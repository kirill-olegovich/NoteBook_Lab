using System.IO;

namespace FileNotepad.Models
{
	public class Dir : FilesAndDir
	{
		public Dir(string name) : base(name)
		{
			Path = name;
			Image = "Assets/back_folder.png";
		}

		public Dir(DirectoryInfo dirname) : base(dirname.Name)
		{
			Path = dirname.FullName;
			Image = "Assets/dir.png";
		}
	}
}
