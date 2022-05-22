using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AddressDetail
    {
        public string stateRegionId { get; set; }
        public string stateRegion { get; set; }
        public string cityId { get; set; }
        public string city { get; set; }
        public string neighborhoodId { get; set; }
        public string neighborhood { get; set; }
        public string addressLine1 { get; set; }
    }

    public class ChangeRegistry
    {
        public string reason { get; set; }
        public string number { get; set; }
        public string authorizationDate { get; set; }
        public string authorizationUser { get; set; }
    }

    public class CustomerPostSerice
    {
        public string customerId { get; set; }
        public string fullName { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string thirdName { get; set; }
        public string lastName { get; set; }
        public string secondLastName { get; set; }
        public string marriedLastName { get; set; }
        public string vip { get; set; }
        public List<IdDetail> idDetail { get; set; }
    }

    public class EventRegistry
    {
        public string number { get; set; }
        public string description { get; set; }
        public string technicianName { get; set; }
        public string creationDate { get; set; }
        public string reason { get; set; }
    }

    public class IdDetail
    {
        public string idType { get; set; }
        public string idDescription { get; set; }
        public string idNumber { get; set; }
    }

    public class PhoneDetail
    {
        public string phoneNumberType { get; set; }
        public string phoneNumberId { get; set; }
    }

    public class ProductDetail
    {
        public string description { get; set; }
        public string chain { get; set; }
        public string store { get; set; }
        public string serviceOrderSource { get; set; }
        public string item { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string serial { get; set; }
        public string accesories { get; set; }
    }

  

    public class ServiceCost
    {
        public int spareParts { get; set; }
        public decimal miscellaneous { get; set; }
        public decimal transportation { get; set; }
        public decimal labor { get; set; }
        public decimal subTotal { get; set; }
        public decimal discount { get; set; }
        public decimal netTotal { get; set; }
        public decimal tax { get; set; }
        public decimal total { get; set; }
        public decimal downPayment { get; set; }
        public decimal pendingPayment { get; set; }
        public decimal invoiceNumber { get; set; }
        public decimal downPaymentNumber { get; set; }
        public string CustomerInvoiceNumber { get; set; }
        public decimal VendorTaxId { get; set; }
        public string VendorName { get; set; }
        public string VendorInvoiceData { get; set; }
    }

    public class PostSalesServicesResponse
    {
        public int serviceOrderId { get; set; }
        public string countryISOCode { get; set; }
        public string status { get; set; }
        public string repairType { get; set; }
        public string completionType { get; set; }
        public string repairWorkshop { get; set; }
        public string repairReson { get; set; }
        public string creationDate { get; set; }
        public string completionDate { get; set; }
        public string serviceDuration { get; set; }
        public string damage { get; set; }
        public string borrowStatus { get; set; }
        public string borrowComment { get; set; }
        public CustomerPostSerice customer { get; set; }
        public AddressDetail addressDetail { get; set; }
        public PhoneDetail phoneDetail { get; set; }
        public ProductDetail productDetail { get; set; }
        public List<ChangeRegistry> changeRegistry { get; set; }
        public List<EventRegistry> eventRegistry { get; set; }
        public ServiceCost serviceCost { get; set; }
    }

    public class RootPostSalesServices
    {
        public List<PostSalesServicesResponse> serviceOrder { get; set; }
    }

}
