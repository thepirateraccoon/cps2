namespace CPSAssignment2.Benchmark.Models.MongoDb.SaleDeNorm
{
    public class Item
    {
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
    }
}