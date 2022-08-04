namespace Core.Response
{
    public class TransactionDocument
    {
        public string transactionDocumentType { get; set; }
        public string transactionDocumentNumber { get; set; }
        public string transactionDocumentDate { get; set; }
    }

    public class PosIdentity
    {
        public string warrantyDuration { get; set; }
    }

    public class StockItem
    {
        public string serializedItemModelNumber { get; set; }
    }

    public class RelatedItem
    {
        public string upc { get; set; }
    }

    public class ItemProd
    {
        public string itemId { get; set; }
        public string brandName { get; set; }
        public PosIdentity posIdentity { get; set; }
        public string upc { get; set; }
        public string NumberCAE { get; set; }
        public string ComboID { get; set; }
        public StockItem stockItem { get; set; }
        public string id { get; set; }
        public string itemDescription { get; set; }
        public string modelName { get; set; }

    }

    public class ItemGex
    {
        public string itemId { get; set; }
        public string brandName { get; set; }
        public string upc { get; set; }
        public string id { get; set; }
        public string itemDescription { get; set; }
        public RelatedItem relatedItem { get; set; }
    }

    public class SaleReturnProd
    {
        public string regularUnitPrice { get; set; }
        public double netPrice { get; set; }
        public ItemProd item { get; set; }
        public int quantity { get; set; }
        public int extendedDiscount { get; set; }
        public string typeCode { get; set; }
    }

    public class SaleReturnProdPadre
    {
        public SaleReturnProd saleReturn { get; set; }
        public string lineItemType { get; set; }

    }

    public class SaleReturnGexPadre
    {
        public SaleReturnGex saleReturn { get; set; }
        public string lineItemType { get; set; }

    }

    public class SaleReturnGex
    {

        public object warrantyDuration { get; set; }
        public object wntyMonthsDuration { get; set; }
        public ItemGex item { get; set; }
        public object warrantyContractId { get; set; }
        public string warrantyDocumentNumber { get; set; }
        public string warrantyPurchaseDate { get; set; }
        public int quantity { get; set; }
        public string extendedDiscount { get; set; }
        public string regularUnitPrice { get; set; }
        public string netPrice { get; set; }
        public string typeCode { get; set; }
    }


    public class LocationAttributes
    {
        public string chainName { get; set; }
        public string chainId { get; set; }
    }

    public class Location
    {
        public string locationName { get; set; }
        public string locationId { get; set; }
        public string locationType { get; set; }
        public LocationAttributes locationAttributes { get; set; }
    }

    public class Customer
    {
        public string customerIdDocumentNumber { get; set; }
        public int quantityInvoice { get; set; }
        public int quantityCreditInvoice { get; set; }
        public int quantityDueInvoice { get; set; }
    }

    public class RetailTransactionResponse
    {
        public string transactionUser { get; set; }
        public string transactionDateTime { get; set; }
        public string workStationId { get; set; }
        public string countryISOCode { get; set; }
        public string trainingFlag { get; set; }
        public string transactionId { get; set; }
        public string transactionType { get; set; }
        public string salesCondition { get; set; }
        public List<TransactionDocument> transactionDocuments { get; set; }
        public string keyedOfflineFlag { get; set; }
        public string documentNumber { get; set; }
        public List<Object> lineItem { get; set; }
        public string fiscalFlag { get; set; }
        public string transactionDocumentNumber { get; set; }
        public Location location { get; set; }
        public string currencyISOCode { get; set; }
        public object channelId { get; set; }
        public Customer customer { get; set; }
    }

    public class RootRetailTransaction
    {
        public List<RetailTransactionResponse> retailTransaction { get; set; }
    }
}
