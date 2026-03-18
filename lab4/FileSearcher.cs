using System;
using System.Collections.Generic;

namespace Search {
  public class FileSearcher 
  {
    private string headerMessage;
    private string noFilesMessage;
    private string foundFilePrefix;
    private string filePathPrefix;
    private string creationDatePrefix;

    public FileSearcher()
    {
      headerMessage = "\n=== Search Results ====";
      noFilesMessage = "No Files Found.";
      foundFilePrefix = "Found File: ";
      filePathPrefix = " Path: ";
      creationDatePrefix = " Creation Date: ";
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

      Console.WriteLine(headerMessage);

      if (foundFiles.Count == 0)
      {
        Console.WriteLine(noFilesMessage);
      }
      else
      {
        for (resultIndex = 0; resultIndex < foundFiles.Count; ++resultIndex)
        {
          foundFile = foundFiles[resultIndex];
          Console.WriteLine(foundFilePrefix + foundFile.GetFileName());
          Console.WriteLine(filePathPrefix + foundFile.GetFilePath());
          Console.WriteLine(creationDatePrefix + foundFile.GetCreationDate());
        }
      }
    }
  }
}