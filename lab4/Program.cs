using System;
using System.Collections.Generic;
using FileModels;
using Serialization;
using Search;
using Memento;
using Indexing;

class Program {
  private static int menuEditFile;
  private static int menuUndoChanges;
  private static int menuSaveXml;
  private static int menuSaveBinary;
  private static int menuLoadXml;
  private static int menuLoadBinary;
  private static int menuSearch;
  private static int menuIndex;
  private static int menuExit;

  private static string binaryDumpPath;
  private static string xmlDumpPath;
  private static char keywordsSeparator;
  private static int maxFilesToDisplay;

  static Program()
  {
    menuEditFile = 1;
    menuUndoChanges = 2;
    menuSaveXml = 3;
    menuSaveBinary = 4;
    menuLoadXml = 5;
    menuLoadBinary = 6;
    menuSearch = 7;
    menuIndex = 8;
    menuExit = 0;

    binaryDumpPath = "file_backup.bin";
    xmlDumpPath = "file_backup.xml";
    keywordsSeparator = ',';
    maxFilesToDisplay = 3;
  }

  static void Main(string[] args)
  {
    TextFile currentFile;
    SerializationHandler serialization;
    FileSearcher searcher;
    TextEditor textEditor;
    FileIndexer indexer;
    bool isRunning;
    int userChoice;

    string newContent;
    string keyWordsInput;
    string[] keyWordsArray;
    List<string> keyWordsList;
    string trimmedWord;
    List<TextFile> indexedFiles;
    List<TextFile> searchResults;
    string directoryPath;

    int wordIndex;
    int fileIndex;

    currentFile = new TextFile();
    serialization = new SerializationHandler();
    searcher = new FileSearcher();
    indexer = new FileIndexer();
    textEditor = null;
    isRunning = true;

    newContent = string.Empty;
    keyWordsInput = string.Empty;
    keyWordsArray = null;
    keyWordsList = null;
    trimmedWord = string.Empty;
    indexedFiles = null;
    searchResults = null;
    directoryPath = string.Empty;

    wordIndex = 0;
    fileIndex = 0;
    userChoice = 0;

    Console.WriteLine("=== Console Application: Working With Text Files ===");
    Console.WriteLine();

    while (isRunning)
    {
      Console.WriteLine();
      Console.WriteLine("Menu:");
      Console.WriteLine("1. Edit File Content");
      Console.WriteLine("2. Undo Changes - Memento Pattern");
      Console.WriteLine("3. Save As XML - Serialization");
      Console.WriteLine("4. Save As Binary - Serialization");
      Console.WriteLine("5. Load From XML - Deserialization");
      Console.WriteLine("6. Load From Binary - Deserialization");
      Console.WriteLine("7. Search Files By Keyword");
      Console.WriteLine("8. Index Files In Directory");
      Console.WriteLine("0. Exit");
      Console.Write("Your Choice: ");

      string inputLine;
      inputLine = Console.ReadLine();

      bool parseSuccess;
      parseSuccess = int.TryParse(inputLine, out userChoice);

      if (!parseSuccess)
      {
        -- userChoice;
      }

      if (userChoice == menuEditFile)
      {
        string editContent;

        Console.Write("Enter New File Content: ");
        editContent = Console.ReadLine();

        if (textEditor == null)
        {
          textEditor = new TextEditor(currentFile);
        }

        textEditor.EditContent(editContent);
        textEditor.ShowContent();

        currentFile = textEditor.GetCurrentFile();
      }
      else if (userChoice == menuUndoChanges)
      {
        if (textEditor != null)
        {
          textEditor.Undo();
          textEditor.ShowContent();
        }
        else
        {
          Console.WriteLine("First Edit Or Load A File (Option 1, 5 Or 6).");
        }
      }
      else if (userChoice == menuSaveXml)
      {
        Console.WriteLine("Performing XML Serialization To File: " + xmlDumpPath);
        serialization.XmlSerialize(currentFile, xmlDumpPath);
      }
      else if (userChoice == menuSaveBinary)
      {
        Console.WriteLine("Performing Binary Serialization To File: " + binaryDumpPath);
        serialization.BinarySerialize(currentFile, binaryDumpPath);
      }
      else if (userChoice == menuLoadXml)
      {
        Console.WriteLine("Loading From XML File: " + xmlDumpPath);
        currentFile = serialization.XmlDeserialize(xmlDumpPath);
        textEditor = new TextEditor(currentFile);
        Console.WriteLine("Loaded File: " + currentFile.GetFileName());
        Console.WriteLine("Content: " + currentFile.GetFileContent());
      }
      else if (userChoice == menuLoadBinary)
      {
        Console.WriteLine("Loading From Binary File: " + binaryDumpPath);
        currentFile = serialization.BinaryDeserialize(binaryDumpPath);
        textEditor = new TextEditor(currentFile);
        Console.WriteLine("Loaded File: " + currentFile.GetFileName());
        Console.WriteLine("Content: " + currentFile.GetFileContent());
      }
      else if (userChoice == menuSearch)
      {
        string searchInput;
        string[] searchArray;
        List<string> searchList;
        string currentWord;
        int idx;

        Console.Write("Enter Keywords (Comma Separated): ");
        searchInput = Console.ReadLine();

        searchArray = searchInput.Split(keywordsSeparator);
        searchList = new List<string>();

        for (idx = 0; idx < searchArray.Length; ++idx)
        {
          currentWord = searchArray[idx].Trim();

          if (!string.IsNullOrEmpty(currentWord))
          {
            searchList.Add(currentWord);
          }
        }

        indexedFiles = indexer.GetIndexedFiles();

        if (indexedFiles.Count == 0)
        {
          Console.WriteLine("No Indexed Files. Please Index Directory First (Option 8).");
        }
        else
        {
          searchResults = searcher.SearchByKeywords(indexedFiles, searchList);
          searcher.PrintSearchResults(searchResults);
        }
      }
      else if (userChoice == menuIndex)
      {
        string pathInput;

        Console.Write("Enter Directory Path: ");
        pathInput = Console.ReadLine();

        indexer.IndexFilesInDirectory(pathInput);
        indexer.ShowIndexedFiles(maxFilesToDisplay);
      }
      else if (userChoice == menuExit)
      {
        isRunning = false;
      }
      else
      {
        Console.WriteLine("Invalid Input. Please Try Again.");
      }
    }

    Console.WriteLine();
    Console.WriteLine("Program Finished. Press Enter To Exit.");
    Console.ReadLine();
  }
}