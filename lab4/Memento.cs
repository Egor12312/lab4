using System;
using System.Collections.Generic;

namespace Memento {
  public class TextFileMemento {
    private string content;

    public TextFileMemento(string content)
    {
      this.content = content;
    }

    public string GetContent()
    {
      return this.content;
    }
  }

  public class History {
    private Stack<TextFileMemento> history;

    public History()
    {
      this.history = new Stack<TextFileMemento>();
    }

    public void Push(TextFileMemento memento)
    {
      this.history.Push(memento);
    }

    public TextFileMemento Pop()
    {
      return this.history.Pop();
    }

    public TextFileMemento Peek()
    {
      return this.history.Peek();
    }

    public int GetCount()
    {
      return this.history.Count;
    }
  }

  public class TextEditor {
    private FileModels.TextFile currentFile;
    private History history;

    public TextEditor(FileModels.TextFile file)
    {
      TextFileMemento initialMemento;

      this.currentFile = file;
      this.history = new History();

      initialMemento = new TextFileMemento(this.currentFile.GetFileContent());
      this.history.Push(initialMemento);
    }

    public void EditContent(string newContent)
    {
      TextFileMemento newMemento;

      this.currentFile.SetFileContent(newContent);

      newMemento = new TextFileMemento(newContent);
      this.history.Push(newMemento);

      Console.WriteLine("State Saved.");
    }

    public void Undo()
    {
      const int minimumHistoryCount = 1;

      TextFileMemento previousState;

      if (this.history.GetCount() > minimumHistoryCount)
      {
        this.history.Pop();
        previousState = this.history.Peek();
        this.currentFile.SetFileContent(previousState.GetContent());
        Console.WriteLine("Undo Completed.");
      }
      else
      {
        Console.WriteLine("Cannot Undo: History Is Empty.");
      }
    }

    public void ShowContent()
    {
      const string contentHeader = "\n--- Current Content ---";

      Console.WriteLine(contentHeader);
      Console.WriteLine(this.currentFile.GetFileContent());
    }

    public FileModels.TextFile GetCurrentFile()
    {
      return this.currentFile;
    }
  }
}