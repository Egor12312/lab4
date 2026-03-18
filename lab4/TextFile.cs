using System;
using System.IO;

namespace FileModels {
  public class TextFile {
    private string _filePath;
    private string _fileName;
    private string _fileContent;
    private DateTime _creationDate;

    public TextFile()
    {
      this._filePath = string.Empty;
      this._fileName = string.Empty;
      this._fileContent = string.Empty;
      this._creationDate = DateTime.Now;
    }

    public TextFile(string filePath)
    {
      string fullPath;

      fullPath = filePath;

      this._filePath = fullPath;
      this._fileName = Path.GetFileName(fullPath);
      this._creationDate = File.GetCreationTime(fullPath);
      this._fileContent = File.ReadAllText(fullPath);
    }

    public string GetFilePath()
    {
      return this._filePath;
    }

    public string GetFileName()
    {
      return this._fileName;
    }

    public string GetFileContent()
    {
      return this._fileContent;
    }

    public void SetFileContent(string content)
    {
      this._fileContent = content;
    }

    public DateTime GetCreationDate()
    {
      return this._creationDate;
    }
  }
}