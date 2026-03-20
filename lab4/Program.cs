using System;
using System.Collections.Generic;
using FileModels;
using Serialization;
using Search;
using Memento;
using Indexing;

class Program {
  private static int _menuEditFile;
  private static int _menuUndoChanges;
  private static int _menuSaveXml;
  private static int _menuSaveBinary;
  private static int _menuLoadXml;
  private static int _menuLoadBinary;
  private static int _menuSearch;
  private static int _menuIndex;
  private static int _menuExit;

  private static string _binaryDumpPath;
  private static string _xmlDumpPath;
  private static char _keywordsSeparator;
  private static int _maxFilesToDisplay;

  static Program()
  {
    _menuEditFile = 1;
    _menuUndoChanges = 2;
    _menuSaveXml = 3;
    _menuSaveBinary = 4;
    _menuLoadXml = 5;
    _menuLoadBinary = 6;
    _menuSearch = 7;
    _menuIndex = 8;
    _menuExit = 0;
    _binaryDumpPath = "file_backup.bin";
    _xmlDumpPath = "file_backup.xml";
    _keywordsSeparator = ',';
    _maxFilesToDisplay = 3;
  }

  static void Main(string[] args)
  {
    TextFile currentFile;
    SerializationHandler serialization;
    FileSearcher searcher;
    TextEditor textEditor;
    FileIndexer indexer;
    History history;

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
    string inputLine;
    bool parseSuccess;
    string editContent;
    string pathInput;
    bool isRunning;
    int userChoice;

    string searchInput;
    string[] searchArray;
    List<string> searchList;
    string currentWord;
    int index;

    currentFile = new TextFile();
    serialization = new SerializationHandler();
    searcher = new FileSearcher();
    indexer = new FileIndexer();
    textEditor = null;
    history = new History();
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

      inputLine = Console.ReadLine();
      parseSuccess = int.TryParse(inputLine, out userChoice);

      if (!parseSuccess)
      {
        userChoice = -1;
      }

      if (userChoice == _menuEditFile)
      {
        Console.Write("Enter New File Content: ");
        editContent = Console.ReadLine();

        if (textEditor == null)
        {
          textEditor = new TextEditor(currentFile);
        }

        IMemento stateBeforeEdit;
        stateBeforeEdit = new TextFileMemento(currentFile.GetFileContent());
        history.Push(stateBeforeEdit);
        Console.WriteLine("State Saved Before Edit.");

        textEditor.EditContent(editContent);
        textEditor.ShowContent();

        currentFile = textEditor.GetCurrentFile();
      }
      else if (userChoice == _menuUndoChanges)
      {
        if (textEditor != null)
        {
          IMemento previousState;

          if (history.GetCount() > 1)
          {
            previousState = history.Pop();
            currentFile.SetFileContent(previousState.GetContent());
            textEditor = new TextEditor(currentFile);
            Console.WriteLine("Undo Completed Using History.");
            textEditor.ShowContent();
          }
          else
          {
            Console.WriteLine("Cannot Undo: History Is Empty.");
          }
        }
        else
        {
          Console.WriteLine("First Edit Or Load A File (Option 1, 5 Or 6).");
        }
      }
      else if (userChoice == _menuSaveXml)
      {
        Console.WriteLine("Performing XML Serialization To File: " + _xmlDumpPath);
        serialization.XmlSerialize(currentFile, _xmlDumpPath);
      }
      else if (userChoice == _menuSaveBinary)
      {
        Console.WriteLine("Performing Binary Serialization To File: " + _binaryDumpPath);
        serialization.BinarySerialize(currentFile, _binaryDumpPath);
      }
      else if (userChoice == _menuLoadXml)
      {
        Console.WriteLine("Loading From XML File: " + _xmlDumpPath);
        currentFile = serialization.XmlDeserialize(_xmlDumpPath);
        textEditor = new TextEditor(currentFile);

        history = new History();
        IMemento loadedState;
        loadedState = new TextFileMemento(currentFile.GetFileContent());
        history.Push(loadedState);

        Console.WriteLine("Loaded File: " + currentFile.GetFileName());
        Console.WriteLine("Content: " + currentFile.GetFileContent());
      }
      else if (userChoice == _menuLoadBinary)
      {
        Console.WriteLine("Loading From Binary File: " + _binaryDumpPath);
        currentFile = serialization.BinaryDeserialize(_binaryDumpPath);
        textEditor = new TextEditor(currentFile);

        history = new History();
        IMemento loadedState;
        loadedState = new TextFileMemento(currentFile.GetFileContent());
        history.Push(loadedState);

        Console.WriteLine("Loaded File: " + currentFile.GetFileName());
        Console.WriteLine("Content: " + currentFile.GetFileContent());
      }
      else if (userChoice == _menuSearch)
      {
        Console.Write("Enter Keywords (Comma Separated): ");
        searchInput = Console.ReadLine();

        searchArray = searchInput.Split(_keywordsSeparator);
        searchList = new List<string>();

        for (index = 0; index < searchArray.Length; ++index)
        {
          currentWord = searchArray[index].Trim();

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
      else if (userChoice == _menuIndex)
      {
        Console.Write("Enter Directory Path: ");
        pathInput = Console.ReadLine();

        indexer.IndexFilesInDirectory(pathInput);
        indexer.ShowIndexedFiles(_maxFilesToDisplay);
      }
      else if (userChoice == _menuExit)
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