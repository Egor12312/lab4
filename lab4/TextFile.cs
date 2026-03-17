using System;
using System.IO;

namespace FileModels {
  public class TextFile {
    private string filePath;
    private string fileName;
    private string fileContent;
    private DateTime creationDate;

    public TextFile()
    {
      this.filePath = string.Empty;
      this.fileName = string.Empty;
      this.fileContent = string.Empty;
      this.creationDate = DateTime.Now;
    }

    public TextFile(string filePath)
    {
      string fullPath;

      fullPath = filePath;

      this.filePath = fullPath;
      this.fileName = Path.GetFileName(fullPath);
      this.creationDate = File.GetCreationTime(fullPath);
      this.fileContent = File.ReadAllText(fullPath);
    }

    public string GetFilePath()
    {
      return this.filePath;
    }

    public string GetFileName()
    {
      return this.fileName;
    }

    public string GetFileContent()
    {
      return this.fileContent;
    }

    public void SetFileContent(string content)
    {
      this.fileContent = content;
    }

    public DateTime GetCreationDate()
    {
      return this.creationDate;
    }
  }
}