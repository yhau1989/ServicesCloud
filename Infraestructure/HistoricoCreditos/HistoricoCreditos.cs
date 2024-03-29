﻿using Core.Interfaces;
using Core.Response;
using DbSybaseService;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Text;
using Tools;

namespace Infraestructure.HistoricoCreditos
{
    /// <summary>
    ///  Clase que maneja toda la logica del endpoint credits/sales-quotations
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay Muñoz - UNICOMER
    /// fecha creación: 28/07/2022
    /// ]]>
    public class HistoricoCreditos : IHistoricoCreditos
    {
        DbSybaseServiceOdbc dbSybaseServiceOdbc;
        private readonly IConfiguration _configuration;
        private string cConexion = "";
        List<SalesQuotation> final;



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        public HistoricoCreditos(IConfiguration configuration)
        {
            _configuration = configuration;
            //desencriptando cadena
            string ccon = _configuration["AppSettings:ConnectionStringASA17"];
            cConexion = testMD5.decrypt(ccon);
            //desencriptando cadena
        }


        /// <summary>
        /// Metodo que encapsula toda la logica
        /// </summary>
        /// <param name="id_number">ci del cliente</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        public List<SalesQuotation> Get(string id_number)
        {
            //return exampleBasicJson;
            getAsadata(id_number);
            return final;
        }



        /// <summary>
        /// Genera un json de ejemplo segun la documentacion.
        /// </summary>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<SalesQuotation> exampleBasicJson() {

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

            creditos.Add(new SalesQuotation()
            {
                salesOrderId = "115332181900JV",
                installmentAmount = Convert.ToDecimal("4523,12"),
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



        /// <summary>
        /// Se conecta a Asa y ejecuta el sp SP_SCloud_Creditos
        /// </summary>
        /// <param name="id_number">ci del cliente</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private void getAsadata(string id_number)
        {
            DataTable dtCabOrdenComp = new DataTable();
            DataTable dtDetOrdenComp = new DataTable();
            DataTable dtDetFacturas = new DataTable();
            DataTable dtDetInspecc = new DataTable();
            DataTable dtDetalleCoutas = new DataTable();

            dbSybaseServiceOdbc = new DbSybaseServiceOdbc(cConexion);
            dbSybaseServiceOdbc.Initialize();
            OdbcCommand odbcCommand = new OdbcCommand("EXEC dba.SP_SCloud_Creditos ?", dbSybaseServiceOdbc.connection);
            odbcCommand.CommandType = CommandType.StoredProcedure;
            odbcCommand.Parameters.AddWithValue("@cedula", id_number.Trim());
            var odbcReader = odbcCommand.ExecuteReader();
            dtCabOrdenComp.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                dtDetOrdenComp.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                dtDetFacturas.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                dtDetInspecc.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                dtDetalleCoutas.Load(odbcReader);
            dbSybaseServiceOdbc.Dispose();

            final = fillCreditRequestPool(dtCabOrdenComp, dtDetOrdenComp, dtDetFacturas, dtDetInspecc, dtDetalleCoutas);
        }


        /// <summary>
        /// Arma la estructura del json de respuesta a partir de los datos que retorna el sp SP_SCloud_Creditos desde ASA
        /// </summary>
        /// <param name="dtCabOrdenComp">resultset de asa #CAB_ORDENCOMP</param>
        /// <param name="dtDetOrdenComp">resultset de asa #DET_ORDENCOMP</param>
        /// <param name="dtDetFacturas">resultset de asa #DET_FACTURAS</param>
        /// <param name="dtDetInspecc">resultset de asa #DET_INSPECC</param>
        /// <param name="dtDetalleCoutas">resultset de asa #Detalle_Cuotas</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<SalesQuotation> fillCreditRequestPool(DataTable dtCabOrdenComp, DataTable dtDetOrdenComp, DataTable dtDetFacturas, DataTable dtDetInspecc, DataTable dtDetalleCoutas)
        {
            List<SalesQuotation> pool = new List<SalesQuotation>();
            SalesQuotation root = null;
            ResponseMsg dataOracle = null;

            foreach (DataRow row in dtCabOrdenComp.Rows)
            {
                dataOracle = getOracleData(row["EM_CODIGO"].ToString(), row["CL_CEDRUC"].ToString(), row["SC_IDSOL"].ToString(), dtDetOrdenComp);
                if (dataOracle.status == 0)
                {

                    DataTable creditRequestTable = (DataTable)(dataOracle.data);
                    CreditRequest _creditRequest = new CreditRequest()
                    {
                        transactionUser = creditRequestTable.Rows[0]["ANALISTA_APROBADOR"].ToString(),
                        statusId = creditRequestTable.Rows[0]["STATUSID"].ToString(),
                        creditRequestId = Convert.ToInt64(creditRequestTable.Rows[0]["EX_NUMSOL"].ToString()),
                        asignedUser = creditRequestTable.Rows[0]["ANALISTA_INICIO"].ToString(),
                        storeName = creditRequestTable.Rows[0]["EX_CODEMI"].ToString(),
                        storeId = creditRequestTable.Rows[0]["EM_NOMBRE"].ToString(),
                        status = creditRequestTable.Rows[0]["SI_DESCRI"].ToString()
                    };

                    List<Item> lineItemList = makeLineItems(row["EM_CODIGO"].ToString(), row["CL_CEDRUC"].ToString(), row["SC_IDSOL"].ToString(), dtDetOrdenComp);


                    PlanDetail _planDetail = new PlanDetail()
                    {
                        installmentNumber = Convert.ToInt32(row["pl_nplazo"].ToString()),
                        termType = row["TIPO_PLAZO"].ToString(),
                        paymentCycle = row["pl_diacuo"].ToString(),
                        planId = row["pl_codpla"].ToString(),
                        commercialPlan = row["pl_descr"].ToString(),
                    };

                    root = new SalesQuotation()
                    {
                        salesOrderId = row["sc_idsol"].ToString(),
                        installmentAmount = Convert.ToDecimal(row["sc_valcuo"].ToString()),
                        transactionDate = row["sc_fecha"].ToString(),
                        storeId = row["em_abrev"].ToString(),
                        tenderAmount = Convert.ToDecimal(row["Monto_Sol"].ToString()),
                        downPayment = Convert.ToDecimal(row["sc_cinic"].ToString()),
                        storeName = row["em_nombre"].ToString(),
                        status = row["Estado"].ToString(),
                        creditRequest = _creditRequest,
                        lineItem = lineItemList,
                        planDetail = _planDetail,
                        creditApplication = createListCreditApplicationDetail(row["EM_CODIGO"].ToString(), row["CL_CEDRUC"].ToString(), row["SC_IDSOL"].ToString(), dtDetFacturas, dtDetInspecc, dtDetalleCoutas),
                        customer = createCustomer(row, row["EM_CODIGO"].ToString(), row["CL_CEDRUC"].ToString(), row["SC_IDSOL"].ToString(), dtDetalleCoutas),
                    };

                    pool.Add(root);
                }
            }



            return pool;

        }


        /// <summary>
        /// Se conecta a Oracle para recuperar datos, encapsula la clase DbOracleContext
        /// </summary>
        /// <param name="emCodigo">codigo de emisor</param>
        /// <param name="ciCdRuc">ci del cliente</param>
        /// <param name="idSol">is de solicitud de credito</param>
        /// <param name="detOrdenComp">resultset de asa #CAB_ORDENCOMP</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private ResponseMsg getOracleData(string emCodigo, string ciCdRuc, string idSol, DataTable detOrdenComp)
        {
            // si #DET_ORDENCOMP tienen mas de una registro con EM_CODIGO, CL_CEDRUC, SC_IDSOL, con los mismos datos solo se toma el primero ya que en el json (creditRequest) esto es un objeto, no un array
            DataTable result = detOrdenComp.Select($"EM_CODIGO={emCodigo} and CL_CEDRUC={ciCdRuc} and SC_IDSOL={idSol}").CopyToDataTable();

            if (result.Rows.Count > 0)
            {
                var row = result.Rows[0];

                if (row["SL_CODSOL"].ToString().Trim().Length > 0)
                {
                    ResponseMsg data = new DbOracleContext().getCredits(row["EM_CODIGO"].ToString(), row["SL_CODSOL"].ToString(), row["CL_CEDRUC"].ToString());
                    return data;
                }

                return new ResponseMsg()
                {
                    status = 1,
                    msg = $"#DET_ORDENCOMP no tiene data para EM_CODIGO={emCodigo} and CL_CEDRUC={ciCdRuc} and SC_IDSOL={idSol} tiene SL_CODSOL es nulo"
                };
            }

            return new ResponseMsg() {
                status = 1,
                msg = $"#DET_ORDENCOMP no tiene data para EM_CODIGO={emCodigo} and CL_CEDRUC={ciCdRuc} and SC_IDSOL={idSol}"
            };

        }



        /// <summary>
        /// Retorna la data para la seccion del json "lineItems"
        /// </summary>
        /// <param name="emCodigo">codigo de emisor</param>
        /// <param name="ciCdRuc">ci del cliente</param>
        /// <param name="idSol">is de solicitud de credito</param>
        /// <param name="dtDetOrdenComp">resultset de asa #DET_ORDENCOMP</param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<Item> makeLineItems(string emCodigo, string ciCdRuc, string idSol, DataTable dtDetOrdenComp)
        {
            List<Item> items = new List<Item>();

            DataRow[] resultStock = dtDetOrdenComp.Select($"EM_CODIGO={emCodigo} and CL_CEDRUC={ciCdRuc} and SC_IDSOL={idSol} and Gex_item='STOCK'");
            DataTable dataStock = (resultStock != null && resultStock.Length > 0) ? resultStock.CopyToDataTable() : new DataTable();
            
            DataRow[] resultWarranty = dtDetOrdenComp.Select($"EM_CODIGO={emCodigo} and CL_CEDRUC={ciCdRuc} and SC_IDSOL={idSol} and Gex_item='WARRANTY'");
            DataTable dataWarranty = (resultWarranty != null && resultWarranty.Length > 0) ? resultWarranty.CopyToDataTable() : new DataTable();


            if (dataStock.Rows.Count > 0)
            {
                ItemDetail itemDetail;
                List<ItemDetail> listItemDetail = new List<ItemDetail>();
                foreach (DataRow row in dataStock.Rows)
                {
                    itemDetail = new ItemDetail()
                    {
                        upc = row["COD_ARTICULO"].ToString(),
                        brand = row["brandName"].ToString(),
                        model = row["modelName"].ToString(),
                        description = row["it_desite"].ToString(),
                        price = Convert.ToDecimal(row["precio_base"].ToString()),
                        quantity = Convert.ToInt32(row["sc_cant"].ToString()),
                        discountType = row["discountType"].ToString(),
                        discount = Convert.ToDecimal(row["Total_descts"].ToString()),
                        netPrice = Convert.ToDecimal(row["Val_facturado"].ToString()),
                        tax = Convert.ToDecimal(row["Iva_item"].ToString()),
                        relatedItem = row["Tiene_Gex"].ToString(),
                        warrantyDuration = row["Plazo_Gex"].ToString()
                    };
                    listItemDetail.Add(itemDetail);
                }

                items.Add(new Item()
                {
                    typeCode = "STOCK",
                    item = listItemDetail,
                });

            }

            if (dataWarranty.Rows.Count > 0)
            {
                ItemDetail itemDetail;
                List<ItemDetail> listItemDetail = new List<ItemDetail>();
                foreach (DataRow row in dataWarranty.Rows)
                {
                    itemDetail = new ItemDetail()
                    {
                        upc = row["COD_ARTICULO"].ToString(),
                        brand = row["brandName"].ToString(),
                        model = row["modelName"].ToString(),
                        description = row["it_desite"].ToString(),
                        price = Convert.ToDecimal(row["precio_base"].ToString()),
                        quantity = Convert.ToInt32(row["sc_cant"].ToString()),
                        discountType = row["discountType"].ToString(),
                        discount = Convert.ToDecimal(row["Total_descts"].ToString()),
                        netPrice = Convert.ToDecimal(row["Val_facturado"].ToString()),
                        tax = Convert.ToDecimal(row["Iva_item"].ToString()),
                        relatedItem = row["Tiene_Gex"].ToString(),
                        warrantyDuration = row["Plazo_Gex"].ToString()
                    };
                    listItemDetail.Add(itemDetail);
                }

                items.Add(new Item()
                {
                    typeCode = "WARRANTY",
                    item = listItemDetail,
                });

            }
            
            return items;

        }



        /// <summary>
        /// Retorna la data para la seccion del json "creditApplications"
        /// </summary>
        /// <param name="emCodigo">codigo de emisor</param>
        /// <param name="ciCdRuc">ci del cliente</param>
        /// <param name="idSol">is de solicitud de credito</param>
        /// <param name="dtDetFacturas">resultset de asa #DET_FACTURAS</param>
        /// <param name="dtDetInspecc">resultset de asa #DET_INSPECC</param>
        /// <param name="dtDetalleCoutas">resultset de asa #Detalle_Cuotas</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<CreditApplicationDetail> createListCreditApplicationDetail(string emCodigo, string ciCdRuc, string idSol, DataTable dtDetFacturas, DataTable dtDetInspecc, DataTable dtDetalleCoutas)
        {
            DataTable resultFacturas = dtDetFacturas.Select($"EM_CODIGO={emCodigo} and CL_CEDRUC={ciCdRuc} and SC_IDSOL={idSol}").CopyToDataTable();
            DataRow[] searchs = dtDetInspecc.Select($"CI_CODEMI={emCodigo} and SC_IDSOL={idSol}");
            DataTable resultInspeccion = (searchs != null && searchs.Length > 0) ? searchs.CopyToDataTable() : new DataTable();
            List<CreditApplicationDetail> creditApplications = new List<CreditApplicationDetail>();

            if (resultFacturas.Rows.Count > 0 && resultInspeccion.Rows.Count > 0)
            {
                DataRow inspeccionRow1 = resultInspeccion.Rows[0]; //solo seleccionamos 1, ojo con esto
                foreach (DataRow row in resultFacturas.Rows)
                {
                    CreditApplicationDetail creditApplicationDetail = new CreditApplicationDetail();
                    creditApplicationDetail.purchaseDate = row["ft_fecfac"].ToString();
                    creditApplicationDetail.portfolioSubSegmentDescription = row["Tipo_amort"].ToString();
                    creditApplicationDetail.arrearsInterestBalance = Convert.ToDecimal(row["DSCTO_MORA"].ToString());
                    creditApplicationDetail.installmentAmount = row["sc_valcuo"].ToString().Length > 0 ? Convert.ToDecimal(row["sc_valcuo"].ToString()) : 0;
                    creditApplicationDetail.creditRequestId = row["sc_numsol"].ToString().Length > 0 ? Convert.ToInt64(row["sc_numsol"].ToString()) : 0;
                    creditApplicationDetail.minimunPayment = row["VALOR_MAS_VENCIDO"].ToString().Length > 0 ? Convert.ToDecimal(row["VALOR_MAS_VENCIDO"].ToString()) : 0;
                    creditApplicationDetail.portfolioSubSegmentId = row["sc_amort"].ToString();
                    creditApplicationDetail.portfolioSegmentId = row["sc_codcia"].ToString();
                    creditApplicationDetail.debtNoteNumber = row["sc_numope"].ToString();
                    creditApplicationDetail.capitalBalance = row["SALDO_CAPITAL"].ToString().Length > 0 ? Convert.ToDecimal(row["SALDO_CAPITAL"].ToString()) : 0;
                    creditApplicationDetail.totalSaleAmount = row["sc_totfac"].ToString().Length > 0 ? Convert.ToDecimal(row["sc_totfac"].ToString()) : 0;
                    creditApplicationDetail.pendingInstallment = row["CUOTAS_PENDIENTES"].ToString();
                    creditApplicationDetail.pendingTotalInstallment = row["CUOTAS_TOTALES"].ToString().Length > 0 ? Convert.ToInt32(row["CUOTAS_TOTALES"].ToString()) : 0;
                    creditApplicationDetail.creditTerm = row["pl_nplazo"].ToString().Length > 0 ? Convert.ToInt32(row["pl_nplazo"].ToString()) : 0;
                    creditApplicationDetail.paymentCycle = row["pl_diacuo"].ToString();
                    creditApplicationDetail.currentInterestBalance = row["SALDO_INTERES"].ToString().Length > 0 ? Convert.ToDecimal(row["SALDO_INTERES"].ToString()) : 0;
                    creditApplicationDetail.specialInterestBalance = row["SALDO_GASTOS"].ToString().Length > 0 ? Convert.ToDecimal(row["SALDO_GASTOS"].ToString()) : 0;
                    creditApplicationDetail.portfolioSegmentDescription = row["em_nomgru"].ToString();
                    creditApplicationDetail.status = row["sc_estcre"].ToString();
                    creditApplicationDetail.expirationDate = row["sc_feculven"].ToString();
                    creditApplicationDetail.discountArrearsBalance = row["DSCTO_MORA"].ToString().Length > 0 ? Convert.ToDecimal(row["DSCTO_MORA"].ToString()) : 0;
                    creditApplicationDetail.discountSpecialInterestBalance = row["DSCTO_GASTOS"].ToString().Length > 0 ? Convert.ToDecimal(row["DSCTO_GASTOS"].ToString()) : 0;
                    creditApplicationDetail.averageArrear = row["ATRASO_PROMEDIO"].ToString().Length > 0 ? Convert.ToDecimal(row["ATRASO_PROMEDIO"].ToString()) : 0;
                    creditApplicationDetail.maxArrear = row["ATRASO_MAXIMO"].ToString().Length > 0 ? Convert.ToDecimal(row["ATRASO_MAXIMO"].ToString()) : 0;
                    creditApplicationDetail.cancelationReason = row["RAZON_CANCELACION"].ToString();
                    creditApplicationDetail.daysInArrears = row["atraso_antiguo"].ToString().Length > 0 ? Convert.ToDecimal(row["atraso_antiguo"].ToString()) : 0;
                    creditApplicationDetail.totalDue = row["TOTAL_DEUDA"].ToString().Length > 0 ? Convert.ToDecimal(row["TOTAL_DEUDA"].ToString()) : 0;
                    creditApplicationDetail.inspectionCreateDate = inspeccionRow1["ci_fecha"].ToString();
                    creditApplicationDetail.inspectionType = inspeccionRow1["TI_DESCRIPCION"].ToString();
                    creditApplicationDetail.inspectionClass = inspeccionRow1["CL_DESCRIP"].ToString();
                    creditApplicationDetail.inspectionStatus = inspeccionRow1["ESTADO_INSP"].ToString();
                    creditApplicationDetail.inspectionPerformedFlag = inspeccionRow1["ESTADO_CIERRE"].ToString();
                    creditApplicationDetail.inspectionResult = inspeccionRow1["RESULTADO"].ToString();
                    creditApplicationDetail.inspectionDate = inspeccionRow1["FECHA_MOD"].ToString();
                    creditApplicationDetail.installmentDetail = installmentDetails(emCodigo, ciCdRuc, idSol, dtDetalleCoutas);
                    creditApplicationDetail.paymentDetail = paymentDetails(emCodigo, ciCdRuc, idSol, dtDetalleCoutas);

                    creditApplications.Add(creditApplicationDetail);
                }
            }

            return creditApplications;

        }



        /// <summary>
        /// Retorna la data para la seccion del json "installmentDetail"
        /// </summary>
        /// <param name="emCodigo">codigo de emisor</param>
        /// <param name="ciCdRuc">ci del cliente</param>
        /// <param name="idSol">is de solicitud de credito</param>
        /// <param name="dtDetalleCoutas">resultset de asa #Detalle_Cuotas</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<InstallmentDetailItem> installmentDetails(string emCodigo, string ciCdRuc, string idSol, DataTable dtDetalleCoutas)
        {
            //DataRow[] searchs = dtDetalleCoutas.Select($"CT_CODEMI={emCodigo} and CT_CODCLI={ciCdRuc} and SC_IDSOL={idSol}");
            DataRow[] searchs = dtDetalleCoutas.Select($"CT_CODEMI={emCodigo} and CT_CODCLI={ciCdRuc}");
            DataTable resultCuotas = (searchs != null && searchs.Length > 0) ? searchs.CopyToDataTable() : new DataTable();
            List<InstallmentDetailItem> listInstallmentDetailItem = new List<InstallmentDetailItem>();

            if (resultCuotas.Rows.Count > 0)
            {
                InstallmentDetailItem installmentDetailItem = null;
                foreach (DataRow row in resultCuotas.Rows)
                {
                    installmentDetailItem = new InstallmentDetailItem()
                    {
                        installmentNumber = Convert.ToInt32(row["CT_CUOTA"].ToString()),
                        installmentBalance = Convert.ToDecimal(row["CT_SALDO"].ToString()),
                        installmentAmount = Convert.ToDecimal(row["IMPORTE"].ToString()),
                        interestAmount = Convert.ToDecimal(row["CT_INTERE"].ToString()),
                        creditPaymentDate = row["CT_FECVTO"].ToString(),
                        paymentDate = row["CT_FECCOB"].ToString(),
                        capitalAmount = Convert.ToDecimal(row["CT_VALOR"].ToString()),
                        status = row["CT_PAGADO"].ToString(),
                        payment = Convert.ToDecimal(row["CT_ABONO"].ToString()),
                        arrearsInterestBalance = Convert.ToDecimal(row["MORA"].ToString()),
                        specialInterestBalance = Convert.ToDecimal(row["GASTO"].ToString()),
                        discountArrearsBalance = Convert.ToDecimal(row["CT_DESCU"].ToString()),
                        discountSpecialInterestBalance = Convert.ToDecimal(row["CT_DESGAS"].ToString()),
                    };

                    listInstallmentDetailItem.Add(installmentDetailItem);
                }
            }

            return listInstallmentDetailItem;
        }


        /// <summary>
        /// Retorna la data para la seccion del json "paymentDatal"
        /// </summary>
        /// <param name="emCodigo">codigo de emisor</param>
        /// <param name="ciCdRuc">ci del cliente</param>
        /// <param name="idSol">is de solicitud de credito</param>
        /// <param name="dtDetalleCoutas">resultset de asa #Detalle_Cuotas</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<PaymentDetailItem> paymentDetails(string emCodigo, string ciCdRuc, string idSol, DataTable dtDetalleCoutas)
        {
            //DataRow[] searchs = dtDetalleCoutas.Select($"CT_CODEMI={emCodigo} and CT_CODCLI={ciCdRuc} and SC_IDSOL={idSol}");
            DataRow[] searchs = dtDetalleCoutas.Select($"CT_CODEMI={emCodigo} and CT_CODCLI={ciCdRuc}");
            DataTable resultCuotas = (searchs != null && searchs.Length > 0) ? searchs.CopyToDataTable() : new DataTable();
            List<PaymentDetailItem> listInstallmentDetailItem = new List<PaymentDetailItem>();

            if (resultCuotas.Rows.Count > 0)
            {
                PaymentDetailItem paymentDetailItem = null;
                foreach (DataRow row in resultCuotas.Rows)
                {
                    paymentDetailItem = new PaymentDetailItem()
                    {
                        description = row["recibo_manual"].ToString(),
                        storeName = row["TIENDA_EMITE_CRED"].ToString(),
                        paymentDate = row["CT_FECCOB"].ToString(),
                        paymentAmount = Convert.ToDecimal(row["VALOR_COBRADO"].ToString()),
                        receiptNumber = row["CT_NUMDOC"].ToString()
                    };

                    listInstallmentDetailItem.Add(paymentDetailItem);
                }
            }

            return listInstallmentDetailItem;
        }


        /// <summary>
        /// Retorna la data para la seccion del json "customer"
        /// </summary>
        /// <param name="dtCabOrdenComp"></param>
        /// <param name="emCodigo">codigo de emisor</param>
        /// <param name="ciCdRuc">ci del cliente</param>
        /// <param name="idSol">is de solicitud de credito</param>
        /// <param name="dtDetalleCoutas">resultset de asa #Detalle_Cuotas</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private CustomerCredito createCustomer(DataRow dtCabOrdenComp, string emCodigo, string ciCdRuc, string idSol, DataTable dtDetalleCoutas)
        {
            CustomerCredito _customer = new CustomerCredito()
            {
                firstName = dtCabOrdenComp["firstName"].ToString(),
                lastName = dtCabOrdenComp["lastName"].ToString(),
                secondLastName = dtCabOrdenComp["secondLastName"].ToString(),
                customerType = dtCabOrdenComp["customerType"].ToString(),
                marriedLastName = dtCabOrdenComp["marriedLastName"].ToString(),
                customerId = dtCabOrdenComp["customerId"].ToString(),
                businessName = dtCabOrdenComp["businessName"].ToString(),
                customerClassification = dtCabOrdenComp["TIPO_CLIENTE"].ToString(),
                thirdName = dtCabOrdenComp["thirdName"].ToString(),
                secondName = dtCabOrdenComp["secondName"].ToString(),
                idDetail = createCustomerItem(emCodigo, ciCdRuc, idSol, dtDetalleCoutas),
            };

            return _customer;
        }


        /// <summary>
        /// Retorna la data para la seccion del json "customer"
        /// </summary>
        /// <param name="emCodigo">codigo de emisor</param>
        /// <param name="ciCdRuc">ci del cliente</param>
        /// <param name="idSol">is de solicitud de credito</param>
        /// <param name="dtDetalleCoutas">resultset de asa #Detalle_Cuotas</param>
        /// <returns></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<IdDetailCreditos> createCustomerItem(string emCodigo, string ciCdRuc, string idSol, DataTable dtDetalleCoutas)
        {
            //DataRow[] searchs = dtDetalleCoutas.Select($"CT_CODEMI={emCodigo} and CT_CODCLI={ciCdRuc} and SC_IDSOL={idSol}"); no viene SC_IDSOL en la tabla
            DataRow[] searchs = dtDetalleCoutas.Select($"CT_CODEMI={emCodigo} and CT_CODCLI={ciCdRuc}");
            DataTable resultItems = (searchs != null && searchs.Length > 0) ? searchs.CopyToDataTable() : new DataTable();
            List<IdDetailCreditos> listaItems = new List<IdDetailCreditos>();

            if (resultItems.Rows.Count > 0)
            {
                IdDetailCreditos idDetailCreditos;
                foreach (DataRow row in resultItems.Rows)
                {
                    idDetailCreditos = new IdDetailCreditos()
                    {
                        idType = row["tipo_cedula"].ToString(),
                        idDocument = row["ct_codcli"].ToString(),
                    };
                    listaItems.Add(idDetailCreditos);
                }
            }

            return listaItems;
        }
    
    }
}
