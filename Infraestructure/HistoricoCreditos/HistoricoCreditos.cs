using Core.Interfaces;
using Core.Response;
using Microsoft.Extensions.Configuration;

namespace Infraestructure.HistoricoCreditos
{
    public class HistoricoCreditos : IHistoricoCreditos
    {
        private readonly IConfiguration _configuration;

        public HistoricoCreditos(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<SalesQuotation> Get(string id_number)
        {
            List<SalesQuotation> creditos = new List<SalesQuotation>();



            InstallmentDetailItem installmentDetailItem = new InstallmentDetailItem() 
            {
                installmentNumber = 1,
                installmentBalance = 0,
                installmentAmount = 47,
                interestAmount = 33,
                creditPaymentDate = "04/02/2019",
                paymentDate = "07/02/2019",
                capitalAmount = 13,
                status = "P",
                payment = 47,
                arrearsInterestBalance = 0,
                specialInterestBalance = 0,
                discountArrearsBalance = 0,
                discountSpecialInterestBalance = 0,
            };

            List<InstallmentDetailItem> listInstallmentDetailItem = new List<InstallmentDetailItem>();
            listInstallmentDetailItem.Add(installmentDetailItem);
            listInstallmentDetailItem.Add(installmentDetailItem);



            PaymentDetailItem paymentDetailItem = new PaymentDetailItem()
            {
                description = "PAGOS DE CLIENTES - CARTERA VIGENTE",
                storeName = "CCH CHALATENANGO",
                paymentDate = "07/02/2019",
                paymentAmount = 478,
                receiptNumber = "53300900075936"
            };

            List<PaymentDetailItem> listPaymentDetailItem = new List<PaymentDetailItem>();
            listPaymentDetailItem.Add(paymentDetailItem);
            listPaymentDetailItem.Add(paymentDetailItem);

            CreditApplicationDetail creditApplicationDetail = new CreditApplicationDetail()
            {
                purchaseDate = "04/01/2019",
                portfolioSubSegmentDescription = "COVID 19.",
                arrearsInterestBalance = 0,
                installmentAmount = 475,
                creditRequestId = 123,
                minimunPayment = 0,
                portfolioSubSegmentId = "00002",
                portfolioSegmentId = "00015",
                debtNoteNumber = "53300009639",
                capitalBalance = 0,
                totalSaleAmount = 640,
                pendingInstallment = "dsd",
                pendingTotalInstallment = 0,
                creditTerm = 24,
                paymentCycle = "MENSUAL",
                currentInterestBalance = 0,
                specialInterestBalance = 65,
                portfolioSegmentDescription = "CUENTAS CASH",
                status = "R",
                expirationDate = "04/01/2021",
                discountArrearsBalance = 0,
                discountSpecialInterestBalance = 0,
                averageArrear = 2,
                maxArrear = 10,
                cancelationReason = "Cancelacion total",
                daysInArrears = 0,
                totalDue = 0,
                inspectionCreateDate = "04/01/2021",
                inspectionType = "Inspeccion",
                inspectionClass = "",
                inspectionStatus = "",
                inspectionPerformedFlag = "",
                inspectionResult = "",
                inspectionDate = "",
                installmentDetail = listInstallmentDetailItem,
                paymentDetail = listPaymentDetailItem,
            };

            List<CreditApplicationDetail> listCreditApplicationDetail = new List<CreditApplicationDetail>();
            listCreditApplicationDetail.Add(creditApplicationDetail);
            listCreditApplicationDetail.Add(creditApplicationDetail);



            IdDetailCreditos idDetailCreditos = new IdDetailCreditos()
            {
                idType = "1",
                idDocument = "02168984-3",
            };

            CustomerCredito _customer = new CustomerCredito()
            {
                firstName = "Mario",
                lastName = "RAMIREZ",
                secondLastName = "",
                customerType = "N",
                marriedLastName = "",
                customerId = "627738",
                businessName = " ",
                customerClassification = "F",
                thirdName = "",
                secondName = "DE JESUS",
                idDetail = new List<IdDetailCreditos> { idDetailCreditos, idDetailCreditos },
            };

            PlanDetail _planDetail = new PlanDetail()
            {
                installmentNumber = 24,
                termType = "C",
                paymentCycle = "MENSUAL",
                planId = "0178",
                commercialPlan = "FACILITO CASH 1B",
            };

            CreditRequest _creditRequest = new CreditRequest()
            {
                transactionUser = "EOLIVA",
                statusId = "G",
                creditRequestId = 12737171,
                asignedUser = " ",
                storeName = "CCH CHALATENANGO",
                storeId = "533",
                status = "GENERADA"
            };

            ItemDetail itemDetail = new ItemDetail()
            {
                upc = "902342442",
                brand = "Sony",
                model = "mx-12",
                description = "Tv 42",
                price = 93129,
                quantity = 1,
                discountType = "normal",
                discount = 20,
                netPrice = 29,
                tax = 0,
                relatedItem = "",
                warrantyDuration = "12"
            };

            List<ItemDetail> listItemDetail = new List<ItemDetail>();
            listItemDetail.Add(itemDetail);

            Item item1 = new Item()
            {
                typeCode = "STOCK",
                item = listItemDetail,
            };

            Item item2 = new Item()
            {
                typeCode = "WRTY",
                item = listItemDetail,
            };

            List<Item> lineItemList = new List<Item>();
            lineItemList.Add(item1);
            lineItemList.Add(item2);

            creditos.Add(new SalesQuotation() {
                salesOrderId = "115332181900JV",
                installmentAmount = 4735,
                transactionDate = "04/01/2019",
                storeId = "533",
                tenderAmount = 64068,
                downPayment = 0,
                storeName = "CCH CHALATENANGO",
                status = "R",
                creditRequest = _creditRequest,
                lineItem = lineItemList,
                planDetail = _planDetail,
                creditApplication = listCreditApplicationDetail,
                customer = _customer,
            });
            creditos.Add(new SalesQuotation());

            return creditos;
        }
    }
}
