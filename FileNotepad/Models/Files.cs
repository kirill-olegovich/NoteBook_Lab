using System.IO;

namespace FileNotepad.Models
{
	public class Files : FilesAndDir
	{
		public Files(string name) : base(name)
		{
			Path = name;
			Image = "Assets/file.png";
		}

		public Files(FileInfo filename) : base(filename.Name)
		{
			Path = filename.FullName;
			Image = "Assets/file.png";
		}
	}
}
