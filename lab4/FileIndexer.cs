using System;
using System.Collections.Generic;
using System.IO;

namespace Indexing {
  public class FileIndexer {
    private List<FileModels.TextFile> indexedFiles;

    public FileIndexer()
    {
      this.indexedFiles = new List<FileModels.TextFile>();
    }

    public void IndexFilesInDirectory(string directoryPath)
    {
      const string textFilePattern = "*.txt";

      string[] textFiles;
      string currentFilePath;
      FileModels.TextFile currentFile;
      int fileIndex;

      this.indexedFiles.Clear();

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
        this.indexedFiles.Add(currentFile);
      }

      Console.WriteLine("indexing completed. files found: " + this.indexedFiles.Count);
    }

    public List<FileModels.TextFile> GetIndexedFiles()
    {
      return this.indexedFiles;
    }

    public void ShowIndexedFiles(int maxFilesToShow)
    {
      int displayCount;
      int fileIndex;
      FileModels.TextFile file;

      if (this.indexedFiles.Count == 0)
      {
        Console.WriteLine("index is empty.");
        return;
      }

      Console.WriteLine("\nindexed files:");

      displayCount = maxFilesToShow;

      if (this.indexedFiles.Count < displayCount)
      {
        displayCount = this.indexedFiles.Count;
      }

      for (fileIndex = 0; fileIndex < displayCount; ++fileIndex)
      {
        file = this.indexedFiles[fileIndex];
        Console.WriteLine("  " + (fileIndex + 1) + ". " + file.GetFileName());
      }

      if (this.indexedFiles.Count > maxFilesToShow)
      {
        Console.WriteLine("  ... and " + (this.indexedFiles.Count - maxFilesToShow) + " more files");
      }
    }
  }
}