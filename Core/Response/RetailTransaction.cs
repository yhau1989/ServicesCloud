﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class Item
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
        public RelatedItem relatedItem { get; set; }
    }

    public class SaleReturn
    {
        public string regularUnitPrice { get; set; }
        public double netPrice { get; set; }
        public Item item { get; set; }
        public int quantity { get; set; }
        public int extendedDiscount { get; set; }
        public string typeCode { get; set; }
        public object warrantyDuration { get; set; }
        public object warrantyMonthsDuration { get; set; }
        public object warrantyContractId { get; set; }
        public string warrantyDocumentNumber { get; set; }
        public string warrantyPurchaseDate { get; set; }
    }

    public class LineItem
    {
        public SaleReturn saleReturn { get; set; }
        public string lineItemType { get; set; }
    }

    public class LocationAttributes
    {
        public string chainName { get; set; }
        public string chainId { get; set; }
    }

    public class Location
    {
        public string locationName { get; set; }
        public int locationId { get; set; }
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

    public class RetailTransaction
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
        public List<LineItem> lineItem { get; set; }
        public int fiscalFlag { get; set; }
        public int transactionDocumentNumber { get; set; }
        public Location location { get; set; }
        public string currencyISOCode { get; set; }
        public object channelId { get; set; }
        public Customer customer { get; set; }
    }    
}
