using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Clses para formar el json response de los metodos de historicos de creditos
/// </summary>
namespace Core.Response
{
    public class RootHistoricoCreditoResponse
    {
        public List<SalesQuotation> salesQuotation { get; set; }
    }

    public class SalesQuotation
    {
        public string salesOrderId {get; set;}
        public decimal installmentAmount {get; set;}
        public string transactionDate {get; set;}
        public string storeId {get; set;}
        public decimal tenderAmount {get; set;}
        public decimal downPayment {get; set;}
        public string storeName {get; set;}
        public string status { get; set; }
        public CreditRequest creditRequest { get; set; }
        public List<Item> lineItem { get; set; }
        public PlanDetail planDetail { get; set; }
        public List<CreditApplicationDetail> creditApplication { get; set; }
        public CustomerCredito customer { get; set; }
    }

    public class CreditRequest
    {
        public string transactionUser {get; set;}
        public string statusId {get; set;}
        public Int64 creditRequestId {get; set;}
        public string asignedUser {get; set;}
        public string storeName {get; set;}
        public string storeId {get; set;}
        public string status { get; set; }
    }

    public class Item
    {
        public string typeCode { get; set; }
        public List<ItemDetail> item { get; set; }
    }

    public class ItemDetail
    {
        public string upc {get; set; }
        public string brand {get; set; }
        public string model {get; set; }
        public string description {get; set; }
        public decimal price {get; set; }
        public Int32 quantity {get; set; }
        public string discountType {get; set; }
        public decimal discount {get; set; }
        public decimal netPrice {get; set; }
        public decimal tax {get; set; }
        public string relatedItem {get; set; }
        public string warrantyDuration { get; set; }
    }

    public class PlanDetail
    {
        public Int32 installmentNumber {get; set;}
        public string termType {get; set;}
        public string paymentCycle {get; set;}
        public string planId {get; set;}
        public string commercialPlan { get; set; }
    }

    public class CreditApplicationDetail
    {
        public string purchaseDate {get; set;}
        public string portfolioSubSegmentDescription {get; set;}
        public decimal arrearsInterestBalance {get; set;}
        public decimal installmentAmount {get; set;}
        public Int64 creditRequestId {get; set;}
        public decimal minimunPayment {get; set;}
        public string portfolioSubSegmentId {get; set;}
        public string portfolioSegmentId {get; set;}
        public string debtNoteNumber {get; set;}
        public decimal capitalBalance {get; set;}
        public decimal totalSaleAmount {get; set;}
        public string pendingInstallment {get; set;}
        public Int32 pendingTotalInstallment {get; set;}
        public Int32 creditTerm {get; set;}
        public string paymentCycle {get; set;}
        public decimal currentInterestBalance {get; set;}
        public decimal specialInterestBalance {get; set;}
        public string portfolioSegmentDescription {get; set;}
        public string status {get; set;}
        public string expirationDate {get; set;}
        public decimal discountArrearsBalance {get; set;}
        public decimal discountSpecialInterestBalance {get; set;}
        public decimal averageArrear {get; set;}
        public decimal maxArrear {get; set;}
        public string cancelationReason {get; set;}
        public decimal daysInArrears {get; set;}
        public decimal totalDue {get; set;}
        public string inspectionCreateDate {get; set;}
        public string inspectionType {get; set;}
        public string inspectionClass {get; set;}
        public string inspectionStatus {get; set;}
        public string inspectionPerformedFlag {get; set;}
        public string inspectionResult {get; set;}
        public string inspectionDate { get; set; }
        public List<InstallmentDetailItem> installmentDetail { get; set; }
        public List<PaymentDetailItem> paymentDetail { get; set; }
    }

    public class InstallmentDetailItem
    {
        public Int32 installmentNumber { get; set; }
        public decimal installmentBalance { get; set; }
        public decimal installmentAmount { get; set; }
        public decimal interestAmount { get; set; }
        public string creditPaymentDate { get; set; }
        public string paymentDate { get; set; }
        public decimal capitalAmount { get; set; }
        public string status { get; set; }
        public decimal payment { get; set; }
        public decimal arrearsInterestBalance { get; set; }
        public decimal specialInterestBalance { get; set; }
        public decimal discountArrearsBalance { get; set; }
        public decimal discountSpecialInterestBalance { get; set; }
    }

    public class PaymentDetailItem
    {
        public string description { get; set; }
        public string storeName { get; set; }
        public string paymentDate { get; set; }
        public decimal paymentAmount { get; set; }
        public string receiptNumber { get; set; }

    }

    public class CustomerCredito
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string secondLastName { get; set; }
        public string customerType { get; set; }
        public string marriedLastName { get; set; }
        public string customerId { get; set; }
        public string businessName { get; set; }
        public string customerClassification { get; set; }
        public string thirdName { get; set; }
        public string secondName { get; set; }
        public List<IdDetailCreditos> idDetail { get; set; }
    }

    public class IdDetailCreditos
    {
        public string idType { get; set; }
        public string idDocument { get; set; }
    }




}
