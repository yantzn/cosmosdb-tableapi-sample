using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using learn_tableapi_sample.Model;

namespace learn_tableapi_sample
{
  class BasicSamples
  {
    /// <summary>
    /// "demo" で始まるテーブルを作成し、生成された GUID をそのテーブル名に追加する
    /// </summary>
    /// <returns></returns>
    public async Task RunSamples()
    {
      Console.WriteLine("Azure Cosmos DB Table - Basic Samples\n");
      Console.WriteLine();

      string tableName = "demo" + Guid.NewGuid().ToString().Substring(0, 5);

      // テーブル参照して、存在しなければ作成を行う。
      CloudTable table = await Common.CreateTableAsync(tableName);

      try
      {
        // CRUD操作を行う
        await BasicDataOperationsAsync(table);
      }
      finally
      {
        // テーブルを削除する
        await table.DeleteIfExistsAsync();
      }
    }

    /// <summary>
    /// CRUD操作を行う
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static async Task BasicDataOperationsAsync(CloudTable table)
    {
      // 顧客エンティティを作成
      CustomerEntity customer = new CustomerEntity("Harp", "Walter")
      {
        Email = "Walter@contoso.com",
        PhoneNumber = "425-555-0101"
      };

      // 名と姓が "Harp Walter" である顧客エンティティを追加
      Console.WriteLine("Insert an Entity.");
      customer = await SamplesUtils.InsertOrMergeEntityAsync(table, customer);

      // ユーザの電話番号を更新
      Console.WriteLine("Update an existing Entity using the InsertOrMerge Upsert Operation.");
      customer.PhoneNumber = "425-555-0105";
      await SamplesUtils.InsertOrMergeEntityAsync(table, customer);
      Console.WriteLine();

      // エンティティを取得
      Console.WriteLine("Reading the updated Entity.");
      customer = await SamplesUtils.RetrieveEntityUsingPointQueryAsync(table, "Harp", "Walter");
      Console.WriteLine();

      // エンティティの削除
      Console.WriteLine("Delete the entity. ");
      await SamplesUtils.DeleteEntityAsync(table, customer);
      Console.WriteLine();
    }
  }
}
