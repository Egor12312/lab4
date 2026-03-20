using System;
using System.Collections.Generic;

namespace Memento {
  public interface IMemento {
    string GetContent();
  }

  public interface IHistory {
    void Push(IMemento memento);
    IMemento Pop();
    IMemento Peek();
    int GetCount();
  }

  public interface ITextEditor {
    void EditContent(string newContent);
    void Undo();
    void ShowContent();
    FileModels.TextFile GetCurrentFile();
  }

  public interface IOriginator {
    object GetMemento();
    void SetMemento(object memento);
  }

  public class TextFileMemento : IMemento {
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

  public class History : IHistory {
    private Stack<IMemento> _history;

    public History()
    {
      this._history = new Stack<IMemento>();
    }

    public void Push(IMemento memento)
    {
      this._history.Push(memento);
    }

    public IMemento Pop()
    {
      return this._history.Pop();
    }

    public IMemento Peek()
    {
      return this._history.Peek();
    }

    public int GetCount()
    {
      return this._history.Count;
    }
  }

  public class TextEditor : ITextEditor, IOriginator {
    private FileModels.TextFile _currentFile;
    private IHistory _history;

    public TextEditor(FileModels.TextFile file)
    {
      IMemento initialMemento;

      this._currentFile = file;
      this._history = new History();

      initialMemento = new TextFileMemento(this._currentFile.GetFileContent());
      this._history.Push(initialMemento);
    }

    public void EditContent(string newContent)
    {
      IMemento newMemento;

      this._currentFile.SetFileContent(newContent);

      newMemento = new TextFileMemento(newContent);
      this._history.Push(newMemento);

      Console.WriteLine("State Saved.");
    }

    public void Undo()
    {
      int minimumHistoryCount;

      minimumHistoryCount = 1;

      IMemento previousState;

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

    public object GetMemento()
    {
      string currentContent;

      currentContent = this._currentFile.GetFileContent();

      return new TextFileMemento(currentContent);
    }

    public void SetMemento(object memento)
    {
      TextFileMemento textFileMemento;

      if (memento is TextFileMemento)
      {
        textFileMemento = (TextFileMemento)memento;
        this._currentFile.SetFileContent(textFileMemento.GetContent());
        Console.WriteLine("Memento Restored.");
      }
      else
      {
        Console.WriteLine("Invalid Memento Type.");
      }
    }
  }
}