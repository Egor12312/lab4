using System;
using System.Collections.Generic;
using System.IO;

namespace Indexing {
  public class FileIndexer {
    private List<FileModels.TextFile> _indexedFiles;

    public FileIndexer()
    {
      this._indexedFiles = new List<FileModels.TextFile>();
    }

    public void IndexFilesInDirectory(string directoryPath)
    {
      string textFilePattern;
      string[] textFiles;
      string currentFilePath;
      FileModels.TextFile currentFile;
      int fileIndex;

      textFilePattern = "*.txt";

      this._indexedFiles.Clear();

      if (!Directory.Exists(directoryPath))
      {
        Console.WriteLine("error: directory does not exist.");
        return;
      }

      textFiles = Directory.GetFiles(directoryPath, textFilePattern);

      for (fileIndex = 0; fileIndex < textFiles.Length; ++fileIndex)
      {
        currentFilePath = textFiles[fileIndex];
        currentFile = new FileModels.TextFile(currentFilePath);
        this._indexedFiles.Add(currentFile);
      }

      Console.WriteLine("indexing completed. files found: " + this._indexedFiles.Count);
    }

    public List<FileModels.TextFile> GetIndexedFiles()
    {
      return this._indexedFiles;
    }

    public void ShowIndexedFiles(int maxFilesToShow)
    {
      int displayCount;
      int fileIndex;

      FileModels.TextFile file;

      if (this._indexedFiles.Count == 0)
      {
        Console.WriteLine("index is empty.");
        return;
      }

      Console.WriteLine("\nindexed files:");

      displayCount = maxFilesToShow;

      if (this._indexedFiles.Count < displayCount)
      {
        displayCount = this._indexedFiles.Count;
      }

      for (fileIndex = 0; fileIndex < displayCount; ++fileIndex)
      {
        file = this._indexedFiles[fileIndex];
        Console.WriteLine("  " + (fileIndex + 1) + ". " + file.GetFileName());
      }

      if (this._indexedFiles.Count > maxFilesToShow)
      {
        Console.WriteLine("  ... and " + (this._indexedFiles.Count - maxFilesToShow) + " more files");
      }
    }
  }
}