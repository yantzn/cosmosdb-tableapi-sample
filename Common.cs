using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;

namespace learn_tableapi_sample
{
  public class Common
  {
    /// <summary>
    /// 接続文字列の詳細を解析し、"Settings.json" ファイルに入力されているアカウント名とアカウント キーの詳細が有効であることを検証
    /// </summary>
    /// <param name="storageConnectionString"></param>
    /// <returns></returns>
    public static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
    {
      CloudStorageAccount storageAccount;
      try
      {
        // 構成ファイルから接続文字列を取得する
        storageAccount = CloudStorageAccount.Parse(storageConnectionString);
      }
      catch (FormatException)
      {
        Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
        throw;
      }
      catch (ArgumentException)
      {
        Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
        Console.ReadLine();
        throw;
      }

      return storageAccount;
    }

    /// <summary>
    /// テーブルを作成する
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static async Task<CloudTable> CreateTableAsync(string tableName)
    {
      string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;

      // 接続文字列からストレージアカウント情報を取得する
      CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

      // テーブルクライアントを作成
      CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

      Console.WriteLine("Create a Table for the demo");

      // テーブルを参照。存在しない場合は作成する
      CloudTable table = tableClient.GetTableReference(tableName);
      if (await table.CreateIfNotExistsAsync())
      {
      Console.WriteLine("Created Table named: {0}", tableName);
      }
      else
      {
        Console.WriteLine("Table {0} already exists", tableName);
      }

      Console.WriteLine();
      return table;
    }
  }
}
