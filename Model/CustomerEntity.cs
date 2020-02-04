using Microsoft.Azure.Cosmos.Table;

namespace learn_tableapi_sample.Model
{
    /// <summary>
    /// 顧客の名を行キーとして、姓をパーティション キーとしてそれぞれ使用するエンティティ クラスを定義
    /// テーブルに格納するエンティティは、TableEntity クラスから派生した型など、サポートされている型である必要がある
    /// </summary>
    public class CustomerEntity : TableEntity
    {
        // エンティティ型で、パラメーターのないコンストラクターを必ず公開する必要がある
        public CustomerEntity()
        {
        }
        // テーブルに格納するエンティティのプロパティは、
        // その型のパブリック プロパティであること、また、値の取得と設定の両方に対応していることが必要
        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
