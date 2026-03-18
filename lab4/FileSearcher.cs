using System;
using System.Collections.Generic;

namespace Search {
  public class FileSearcher {
    private string _headerMessage;
    private string _noFilesMessage;
    private string _foundFilePrefix;
    private string _filePathPrefix;
    private string _creationDatePrefix;

    public FileSearcher()
    {
      _headerMessage = "\n=== Search Results ====";
      _noFilesMessage = "No Files Found.";
      _foundFilePrefix = "Found File: ";
      _filePathPrefix = " Path: ";
      _creationDatePrefix = " Creation Date: ";
    }

    public List<FileModels.TextFile> SearchByKeywords(List<FileModels.TextFile> filesToSearch, List<string> keyWords)
    {
      List<FileModels.TextFile> foundFiles;
      int fileIndex;
      int wordIndex;
      FileModels.TextFile currentFile;
      string fileContentLower;
      string currentKeyWord;
      bool containsKeyWord;

      if (filesToSearch.Count == 0)
      {
        Console.WriteLine("No Files To Search.");
        return new List<FileModels.TextFile>();
      }

      foundFiles = new List<FileModels.TextFile>();

      for (fileIndex = 0; fileIndex < filesToSearch.Count; ++fileIndex)
      {
        currentFile = filesToSearch[fileIndex];
        fileContentLower = currentFile.GetFileContent().ToLower();

        containsKeyWord = false;

        for (wordIndex = 0; wordIndex < keyWords.Count; ++wordIndex)
        {
          currentKeyWord = keyWords[wordIndex].ToLower();

          if (fileContentLower.Contains(currentKeyWord))
          {
            containsKeyWord = true;
            break;
          }
        }

        if (containsKeyWord)
        {
          foundFiles.Add(currentFile);
        }
      }

      return foundFiles;
    }

    public void PrintSearchResults(List<FileModels.TextFile> foundFiles)
    {
      int resultIndex;
      FileModels.TextFile foundFile;

      Console.WriteLine(_headerMessage);

      if (foundFiles.Count == 0)
      {
        Console.WriteLine(_noFilesMessage);
      }
      else
      {
        for (resultIndex = 0; resultIndex < foundFiles.Count; ++resultIndex)
        {
          foundFile = foundFiles[resultIndex];
          Console.WriteLine(_foundFilePrefix + foundFile.GetFileName());
          Console.WriteLine(_filePathPrefix + foundFile.GetFilePath());
          Console.WriteLine(_creationDatePrefix + foundFile.GetCreationDate());
        }
      }
    }
  }
}