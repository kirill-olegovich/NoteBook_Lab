using System.IO;

namespace Notepad.Models;
public abstract class Explorer
{
    public Explorer(string name)
    {
        Header = name;
    }

    public string Header { get; set; }
    public string Image { get; set; } = null!;
    public string SourceName { get; set; } = null!;
}

public class Files : Explorer
{
    public Files(string name) : base(name)
    {
        SourceName = name;
        Image = "Assets/img/file.png";
    }

    public Files(FileInfo fileName) : base(fileName.Name)
    {
        SourceName = fileName.FullName;
        Image = "Assets/img/file.png";
    }
}

public class Directories : Explorer
{
    public Directories(string name) : base(name)
    {
        SourceName = name;
        Image = "Assets/img/backfolder.png";
    }

    public Directories(DirectoryInfo directoryName) : base(directoryName.Name)
    {
        SourceName = directoryName.FullName;
        Image = "Assets/img/folder.png";
    }
}