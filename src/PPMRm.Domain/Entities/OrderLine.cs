namespace PPMRm.Entities
{
    public class OrderLine
    {
        protected OrderLine() { }
        internal OrderLine(int orderLineNumber, string productId, string itemId, string uOM, decimal orderedQuantity)
        {
            OrderLineNumber = orderLineNumber;
            ProductId = productId;
            ItemId = itemId;
            UOM = uOM;
            OrderedQuantity = orderedQuantity;
        }

        public int OrderLineNumber { get; private set; }
        public string ProductId { get; private set; }
        public string ItemId { get; private set; }
        public string UOM { get; private set; }
        public decimal OrderedQuantity { get; private set; }
    }
}
