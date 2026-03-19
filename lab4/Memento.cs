using System;
using System.Collections.Generic;

namespace Memento {
  public class TextFileMemento {
    private string _content;

    public TextFileMemento(string content)
    {
      this._content = content;
    }

    public string GetContent()
    {
      return this._content;
    }
  }

  public class History {
    private Stack<TextFileMemento> _history;

    public History()
    {
      this._history = new Stack<TextFileMemento>();
    }

    public void Push(TextFileMemento memento)
    {
      this._history.Push(memento);
    }

    public TextFileMemento Pop()
    {
      return this._history.Pop();
    }

    public TextFileMemento Peek()
    {
      return this._history.Peek();
    }

    public int GetCount()
    {
      return this._history.Count;
    }
  }

  public class TextEditor {
    private FileModels.TextFile _currentFile;
    private History _history;

    public TextEditor(FileModels.TextFile file)
    {
      TextFileMemento initialMemento;

      this._currentFile = file;
      this._history = new History();

      initialMemento = new TextFileMemento(this._currentFile.GetFileContent());
      this._history.Push(initialMemento);
    }

    public void EditContent(string newContent)
    {
      TextFileMemento newMemento;

      this._currentFile.SetFileContent(newContent);

      newMemento = new TextFileMemento(newContent);
      this._history.Push(newMemento);

      Console.WriteLine("State Saved.");
    }

    public void Undo()
    {
      int minimumHistoryCount;

      minimumHistoryCount = 1;

      TextFileMemento previousState;

      if (this._history.GetCount() > minimumHistoryCount)
      {
        this._history.Pop();
        previousState = this._history.Peek();
        this._currentFile.SetFileContent(previousState.GetContent());
        Console.WriteLine("Undo Completed.");
      }
      else
      {
        Console.WriteLine("Cannot Undo: History Is Empty.");
      }
    }

    public void ShowContent()
    {
      string contentHeader;

      contentHeader = "\n--- Current Content ---";

      Console.WriteLine(contentHeader);
      Console.WriteLine(this._currentFile.GetFileContent());
    }

    public FileModels.TextFile GetCurrentFile()
    {
      return this._currentFile;
    }
  }
}