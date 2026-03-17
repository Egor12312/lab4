using System;
using System.IO;
using System.Xml.Serialization;

namespace Serialization {
  public class SerializationHandler {
    public void BinarySerialize(FileModels.TextFile textFile, string binaryFilePath)
    {
      StreamWriter writer;
      string filePath;
      string fileName;
      string fileContent;
      DateTime creationDate;

      filePath = textFile.GetFilePath();
      fileName = textFile.GetFileName();
      fileContent = textFile.GetFileContent();
      creationDate = textFile.GetCreationDate();

      writer = new StreamWriter(binaryFilePath);

      using (writer)
      {
        writer.WriteLine("=== Binary Serialization ===");
        writer.WriteLine("FilePath:" + filePath);
        writer.WriteLine("FileName:" + fileName);
        writer.WriteLine("CreationDate:" + creationDate.ToString());
        writer.WriteLine("=== Content ===");
        writer.WriteLine(fileContent);
      }

      Console.WriteLine("Binary Serialization Completed To File: " + binaryFilePath);
    }

    public FileModels.TextFile BinaryDeserialize(string binaryFilePath)
    {
      StreamReader reader;
      string line;
      FileModels.TextFile result;
      string filePath;
      string fileName;
      string fileContent;
      DateTime creationDate;

      filePath = string.Empty;
      fileName = string.Empty;
      fileContent = string.Empty;
      creationDate = DateTime.Now;

      reader = new StreamReader(binaryFilePath);

      using (reader)
      {
        line = reader.ReadLine();

        while (line != null)
        {
          if (line.StartsWith("FilePath:"))
          {
            filePath = line.Substring("FilePath:".Length);
          }
          else if (line.StartsWith("FileName:"))
          {
            fileName = line.Substring("FileName:".Length);
          }
          else if (line.StartsWith("CreationDate:"))
          {
            DateTime.TryParse(line.Substring("CreationDate:".Length), out creationDate);
          }
          else if (line == "=== Content ===")
          {
            fileContent = reader.ReadToEnd();
            break;
          }

          line = reader.ReadLine();
        }
      }

      result = new FileModels.TextFile();
      result.SetFileContent(fileContent);

      Console.WriteLine("Binary Deserialization Completed From File: " + binaryFilePath);

      return result;
    }

    public void XmlSerialize(FileModels.TextFile textFile, string xmlFilePath)
    {
      SerializableTextFile serializableObject;
      XmlSerializer serializer;
      StreamWriter writer;
      string filePath;
      string fileName;
      string fileContent;
      DateTime creationDate;

      filePath = textFile.GetFilePath();
      fileName = textFile.GetFileName();
      fileContent = textFile.GetFileContent();
      creationDate = textFile.GetCreationDate();

      serializableObject = new SerializableTextFile();
      serializableObject.SetFilePath(filePath);
      serializableObject.SetFileName(fileName);
      serializableObject.SetFileContent(fileContent);
      serializableObject.SetCreationDate(creationDate);

      serializer = new XmlSerializer(typeof(SerializableTextFile));
      writer = new StreamWriter(xmlFilePath);

      using (writer)
      {
        serializer.Serialize(writer, serializableObject);
      }

      Console.WriteLine("Xml Serialization Completed To File: " + xmlFilePath);
    }

    public FileModels.TextFile XmlDeserialize(string xmlFilePath)
    {
      XmlSerializer serializer;
      StreamReader reader;
      SerializableTextFile loadedObject;
      FileModels.TextFile result;
      string filePath;
      string fileName;
      string fileContent;
      DateTime creationDate;

      serializer = new XmlSerializer(typeof(SerializableTextFile));
      reader = new StreamReader(xmlFilePath);

      using (reader)
      {
        loadedObject = (SerializableTextFile)serializer.Deserialize(reader);
        filePath = loadedObject.GetFilePath();
        fileName = loadedObject.GetFileName();
        fileContent = loadedObject.GetFileContent();
        creationDate = loadedObject.GetCreationDate();
      }

      result = new FileModels.TextFile();
      result.SetFileContent(fileContent);

      Console.WriteLine("Xml Deserialization Completed From File: " + xmlFilePath);

      return result;
    }
  }

  [Serializable]
  public class SerializableTextFile {
    private string filePath;
    private string fileName;
    private string fileContent;
    private DateTime creationDate;

    public SerializableTextFile()
    {
      this.filePath = string.Empty;
      this.fileName = string.Empty;
      this.fileContent = string.Empty;
      this.creationDate = DateTime.Now;
    }

    public string GetFilePath()
    {
      return this.filePath;
    }

    public void SetFilePath(string path)
    {
      this.filePath = path;
    }

    public string GetFileName()
    {
      return this.fileName;
    }

    public void SetFileName(string name)
    {
      this.fileName = name;
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

    public void SetCreationDate(DateTime date)
    {
      this.creationDate = date;
    }
  }
}