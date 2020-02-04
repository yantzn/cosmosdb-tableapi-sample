using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using learn_tableapi_sample.Model;

namespace learn_tableapi_sample
{
  public class SamplesUtils
  {
    /// <summary>
    ///  エンティティの挿入とマージには、TableOperation クラス内の InsertOrMerge メソッドが使用される
    ///  テーブルに追加は、CloudTable.ExecuteAsync メソッドを呼び出すことによって実行されます
    /// </summary>
    /// <param name="table"></param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static async Task<CustomerEntity> InsertOrMergeEntityAsync(CloudTable table, CustomerEntity entity)
    {
      if (entity == null)
      {
        throw new ArgumentNullException("entity");
      }
      try
      {
        // InsertOrReplace tableを操作する
        TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

        // 追加する
        TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
        CustomerEntity insertedCustomer = result.Result as CustomerEntity;

        // 要求ユニット使用量を取得
        if (result.RequestCharge.HasValue)
        {
          Console.WriteLine("Request Charge of InsertOrMerge Operation: " + result.RequestCharge);
        }

        return insertedCustomer;
      }
      catch (StorageException e)
      {
        Console.WriteLine(e.Message);
        Console.ReadLine();
        throw;
      }
    }

    /// <summary>
    ///  TableOperation クラスの Retrieve メソッドを使用してパーティションからエンティティを取得
    ///  顧客エンティティのパーティション キー、行キー、メール、電話番号を取得する
    /// </summary>
    /// <param name="table"></param>
    /// <param name="partitionKey"></param>
    /// <param name="rowKey"></param>
    /// <returns></returns>
    public static async Task<CustomerEntity> RetrieveEntityUsingPointQueryAsync(CloudTable table, string partitionKey, string rowKey)
    {
      try
      {
        // エンティティを取得
        TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(partitionKey, rowKey);
        TableResult result = await table.ExecuteAsync(retrieveOperation);
        CustomerEntity customer = result.Result as CustomerEntity;
        if (customer != null)
        {
          Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", customer.PartitionKey, customer.RowKey, customer.Email, customer.PhoneNumber);
        }

        // 要求ユニット使用量を取得
        if (result.RequestCharge.HasValue)
        {
          Console.WriteLine("Request Charge of Retrieve Operation: " + result.RequestCharge);
        }

        return customer;
      }
      catch (StorageException e)
      {
        Console.WriteLine(e.Message);
        Console.ReadLine();
        throw;
      }
    }

    /// <summary>
    /// ユーザー エンティティを取得して削除
    /// </summary>
    /// <param name="table"></param>
    /// <param name="deleteEntity"></param>
    /// <returns></returns>
    public static async Task DeleteEntityAsync(CloudTable table, CustomerEntity deleteEntity)
    {
      try
      {
        if (deleteEntity == null)
        {
          throw new ArgumentNullException("deleteEntity");
        }

        // エンティティを削除する
        TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
        TableResult result = await table.ExecuteAsync(deleteOperation);

        // 要求ユニット使用量を取得
        if (result.RequestCharge.HasValue)
        {
          Console.WriteLine("Request Charge of Delete Operation: " + result.RequestCharge);
        }
      }
      catch (StorageException e)
      {
        Console.WriteLine(e.Message);
        Console.ReadLine();
        throw;
      }
    }
  }
}
