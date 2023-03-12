using Notepad.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Notepad.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Explorer> _explorerCollection;
        private int _currentIndex;
        private bool _visibilityNotePad;
        private bool _visibilityExplorer;
        private bool _visibilitySaveButton;
        private bool _visibilityOpenButton;
        private string _outTextBox;
        private string _outTextFolder;
        private string _saveButtonText;
        public int CurrentIndexProperties { get => _currentIndex; set 
            {
                this.RaiseAndSetIfChanged(ref _currentIndex, value);
                if (_visibilityOpenButton && _visibilitySaveButton == false)
                {
                    if (_explorerCollection[_currentIndex] is Files) OutTextFolderProperties = _explorerCollection[_currentIndex].Header;
                    else OutTextFolderProperties = "";
                    SaveButtonTextProperties = "открыть";
                }
                else if (_visibilityOpenButton == false && _visibilitySaveButton)
                {
                    if (_explorerCollection[_currentIndex] is Files)
                    {
                        SaveButtonTextProperties = "сохранить";
                        OutTextFolderProperties = _explorerCollection[_currentIndex].Header;
                    }
                    else
                    {
                        SaveButtonTextProperties = "открыть";
                        OutTextFolderProperties = "";
                    }
                }
            } }
        public MainWindowViewModel()
        {
            _visibilityNotePad = true;
            _visibilityExplorer = false;
            _visibilitySaveButton = false;
            _visibilityOpenButton = false;
            _outTextBox = string.Empty;
            _outTextFolder = string.Empty;
            _saveButtonText = "открыть";
            _explorerCollection = new ObservableCollection<Explorer>();
            FillCollection(_path);
        }
        public void ReturnBack()
        {
            _outTextFolder = string.Empty;
            VisibilityNotePadProperties = true;
            VisibilityExplorerProperties = false;
            _visibilitySaveButton = false;
            _visibilityOpenButton = false;
        }
        public void OpenExplorer()
        {
            _outTextFolder = string.Empty;
            VisibilityNotePadProperties = false;
            VisibilityExplorerProperties = true;
            _visibilitySaveButton = false;
            _visibilityOpenButton = true;
        }
        public void SaveExplorer()
        {
            _outTextFolder = "";
            VisibilityNotePadProperties = false;
            VisibilityExplorerProperties = true;
            _visibilitySaveButton = true;
            _visibilityOpenButton = false;
            _currentIndex = 0;
        }
        public void ClickButton()
        {
            if (VisibilityOpenButtonProperties) openButton_openRegime();
            else if (VisibilitySaveButtonProperties) openButton_saveRegime();
        }
        public void openButton_openRegime()
        {
            if (_explorerCollection[CurrentIndexProperties] is Directories)
            {
                if (_explorerCollection[CurrentIndexProperties].Header == "..")
                {
                    var tempPathFirst = Directory.GetParent(_path);
                    if (tempPathFirst != null)
                    {
                        FillCollection(tempPathFirst.FullName);
                        _path = tempPathFirst.FullName;
                    }
                    else if (tempPathFirst == null) FillCollection("");
                }
                else
                {
                    var tempPathSecond = _explorerCollection[_currentIndex].SourceName;
                    FillCollection(_explorerCollection[CurrentIndexProperties].SourceName);
                    _path = tempPathSecond;
                }
            }
            else
            {
                LoadFile(_explorerCollection[_currentIndex].SourceName);
                ReturnBack();
            }
        }
        public void openButton_saveRegime()
        {
            if (_explorerCollection[CurrentIndexProperties] is Directories && OutTextFolderProperties == "")
            {
                SaveButtonTextProperties = "открыть";
                if (_explorerCollection[CurrentIndexProperties].Header == "..")
                {
                    var tempPathFirst = Directory.GetParent(_path);
                    if (tempPathFirst != null) FillCollection(tempPathFirst.FullName);
                    else if (tempPathFirst == null) FillCollection("");
                    _path = tempPathFirst!.FullName;
                }
                else
                {
                    var tempPathSecond = _explorerCollection[_currentIndex].SourceName;
                    FillCollection(_explorerCollection[CurrentIndexProperties].SourceName);
                    _path = tempPathSecond;
                }
            }
            else if (_explorerCollection[CurrentIndexProperties] is Files || OutTextFolderProperties != "")
            {
                if (OutTextFolderProperties == _explorerCollection[_currentIndex].Header) SaveFile(_explorerCollection[CurrentIndexProperties].SourceName, 0);
                else
                {
                    var tempPathThird = _path;
                    tempPathThird += "\\" + OutTextFolderProperties;
                    SaveFile(tempPathThird, 1);
                }
                ReturnBack();
            }
        }
        public void LoadFile(string tempPath)
        {
            string newText = String.Empty;
            StreamReader read = new StreamReader(_explorerCollection[_currentIndex].SourceName);
            while (read.EndOfStream != true)
            {
                newText += read.ReadLine() + "\n";
            }
            OutTextBoxProperties = newText;
        }
        public async void SaveFile(string tempPath, int flag)
        {
            if (flag == 0)
            {
                using (StreamWriter write = new StreamWriter(tempPath))
                {
                    write.Write(OutTextBoxProperties);
                }
            }
            else
            {
                using (FileStream fileStream = new FileStream(tempPath, FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.Default.GetBytes(OutTextBoxProperties);
                    await fileStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }
        public void FillCollection(string varPath)
        {
            _explorerCollection.Clear();
            if (varPath != "")
            {
                var directoryInformation = new DirectoryInfo(varPath);
                _explorerCollection.Add(new Directories(".."));
                foreach (var directory in directoryInformation.GetDirectories())
                {
                    _explorerCollection.Add(new Directories(directory));
                }
                foreach (var fileinfo in directoryInformation.GetFiles())
                {
                    _explorerCollection.Add(new Files(fileinfo));
                }
            }
            else if (varPath == "")
            {
                foreach (var disk in Directory.GetLogicalDrives())
                {
                    _explorerCollection.Add(new Directories(disk));
                }
            }
            CurrentIndexProperties = 0;
        }
        public void DoubleTap()
        {
            if (VisibilityOpenButtonProperties) openButton_openRegime();
            else if (VisibilitySaveButtonProperties) openButton_saveRegime();
        }
        public bool VisibilityNotePadProperties
        {
            get => _visibilityNotePad; 
            set => this.RaiseAndSetIfChanged(ref _visibilityNotePad, value);
        }
        public bool VisibilityExplorerProperties
        {
            get => _visibilityExplorer; 
            set => this.RaiseAndSetIfChanged(ref _visibilityExplorer, value);
        }
        public bool VisibilitySaveButtonProperties
        {
            get => _visibilitySaveButton; 
            set => this.RaiseAndSetIfChanged(ref _visibilitySaveButton, value);
        }
        public bool VisibilityOpenButtonProperties
        {
            get => _visibilityOpenButton; 
            set => this.RaiseAndSetIfChanged(ref _visibilityOpenButton, value);
        }
        public string OutTextBoxProperties
        {
            get => _outTextBox; 
            set => this.RaiseAndSetIfChanged(ref _outTextBox, value);
        }
        public string OutTextFolderProperties 
        {
            get => _outTextFolder; 
            set { this.RaiseAndSetIfChanged(ref _outTextFolder, value); 
                if (_outTextFolder != "") SaveButtonTextProperties = "cохранить"; } 
        }
        public string SaveButtonTextProperties
        {
            get => _saveButtonText; 
            set => this.RaiseAndSetIfChanged(ref _saveButtonText, value);
        }
        public ObservableCollection<Explorer> ExplorerCollectionProperties
        {
            get => _explorerCollection; 
            set => this.RaiseAndSetIfChanged(ref _explorerCollection, value);
        }
        private string _path = Directory.GetCurrentDirectory();
    }
}


