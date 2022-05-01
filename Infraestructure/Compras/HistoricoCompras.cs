using Core.Interfaces;
using Core.Response;
using DbSybaseService;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;

namespace Infraestructure.Compras
{
    public class HistoricoCompras : IHistoricoCompras
    {

        DbSybaseServiceOdbc dbSybaseServiceOdbc;
        private readonly IConfiguration _configuration;

        public HistoricoCompras(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<RetailTransaction> Get()
        {

            DataTable DtTotal = new DataTable();
            DataTable DtCabecera = new DataTable();
            DataTable DtDetalle = new DataTable();

            dbSybaseServiceOdbc = new DbSybaseServiceOdbc(_configuration["AppSettings:ConnectionStringASA17"]);
            dbSybaseServiceOdbc.Initialize();
            OdbcCommand odbcCommand = new OdbcCommand("EXEC dba.SP_SCloud_Facturas ?", dbSybaseServiceOdbc.connection);
            odbcCommand.CommandType = CommandType.StoredProcedure;
            odbcCommand.Parameters.AddWithValue("@cedula", "0901657080");
            var odbcReader = odbcCommand.ExecuteReader();

            DtTotal.Load(odbcReader);

            if (odbcReader.IsClosed == false)
                DtCabecera.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                DtDetalle.Load(odbcReader);

            dbSybaseServiceOdbc.Dispose();

            List<RetailTransaction> ListHitoricoComprasCliente;
            ListHitoricoComprasCliente = FillHistorioCompras(DtTotal, DtCabecera, DtDetalle);
            return (ListHitoricoComprasCliente);
        }

        private List<RetailTransaction> FillHistorioCompras(DataTable DtTotal, DataTable DtCabecera, DataTable DtDetalle)
        {

            DataTable dtTransactionDocument = new DataTable();

            List<RetailTransaction> ListretailTransaction = new List<RetailTransaction>();
            foreach (DataRow drCab in DtCabecera.Rows)
            {
                RetailTransaction retailTransaction;

                retailTransaction = new RetailTransaction()
                {
                    transactionUser = drCab["ve_nombre"].ToString(),
                    transactionDateTime = drCab["ft_fecfac"].ToString(),
                    workStationId = drCab["workStationId"].ToString(),
                    countryISOCode = drCab["countryISOCode"].ToString(),
                    trainingFlag = drCab["trainingFlag"].ToString(),
                    transactionId = drCab["transactionId"].ToString(),
                    transactionType = drCab["transactionType"].ToString(),
                    salesCondition = drCab["Forma_pago"].ToString(),
                    transactionDocuments = GetTransactionDocument(drCab["Tipo_Doc"].ToString(), drCab["Factura_full"].ToString(), drCab["Fecha_Doc"].ToString()),
                    keyedOfflineFlag = drCab["keyedOfflineFlag"].ToString(),
                    documentNumber = drCab["cl_cedruc"].ToString(),
                    lineItem = GetLineItem(DtDetalle, Convert.ToInt32(drCab["EM_CODIGO"].ToString()), Convert.ToInt32(drCab["FT_IDFACT"].ToString()), Convert.ToInt32(drCab["TF_TIPFAC"].ToString()))


                };
                ListretailTransaction.Add(retailTransaction);
            }

            return (ListretailTransaction);
        }

        private List<TransactionDocument> GetTransactionDocument(string Tipo_Doc, string Factura_full, string Fecha_Doc)
        {
            List<TransactionDocument> transactionDocuments = new List<TransactionDocument>();
            TransactionDocument transactionDocument = new TransactionDocument();
            transactionDocument.transactionDocumentType = Tipo_Doc;
            transactionDocument.transactionDocumentNumber = Factura_full;
            transactionDocument.transactionDocumentDate = Fecha_Doc;
            transactionDocuments.Add(transactionDocument);
            return transactionDocuments;
        }

        private List<LineItem> GetLineItem(DataTable DtDetalle, int emisor, int factura, int ptovta)
        {


            // selecciono la factura con que voy a trabajar 
            DataTable dt;
            dt =   DtDetalle.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta}").CopyToDataTable();

            // obtnego las interacciones que debo de realizar por item ( distinct por emisor, factura, pto venta e item
            DataTable dtDisitnct;
            string[] sColumnasDistinct = { "EM_CODIGO", "FT_IDFACT", "TF_TIPFAC", "COD_ARTICULO" , "lineItemType" };
            dtDisitnct = dt.DefaultView.ToTable(true, sColumnasDistinct);

            

            List<LineItem> listlineItem = new List<LineItem>();
            LineItem lineItem;

            foreach (DataRow dr in dtDisitnct.Rows)
            {
                lineItem = new LineItem()
                {
                    saleReturn = GetSaleReturn(dt, emisor, factura, ptovta, dr["COD_ARTICULO"].ToString()),
                    lineItemType = dr["lineItemType"].ToString()
                };

                listlineItem.Add(lineItem);
            }

            return (listlineItem);
        }

        private SaleReturn GetSaleReturn(DataTable dt, int emisor, int factura, int ptovta, string idItem )
        {
                        
            DataRow[] drProcuto = null;
            drProcuto = dt.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta} and COD_ARTICULO='{idItem}' and tipo_producto='Producto'");

            DataRow[] drGex = null;
            drGex = dt.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta} and COD_ARTICULO='{idItem}' and tipo_producto='Garantia'");

            SaleReturn saleReturn = new SaleReturn();

            if (drProcuto.Count() > 0)
            {
                saleReturn.regularUnitPrice = drProcuto[0]["precio_base"].ToString();
                saleReturn.netPrice = Convert.ToDouble(drProcuto[0] ["val_facturado"].ToString());
                saleReturn.item = new Item()
                {
                    itemId = idItem,
                    brandName = drProcuto[0]["brandName"].ToString(),
                    posIdentity = new PosIdentity()
                    {
                        warrantyDuration = (drGex.Count() > 0 ? drGex[0]["PlazoDesc_GEX"].ToString() : "")
                    },
                    upc = drProcuto[0]["upc"].ToString(),
                    NumberCAE = drProcuto[0]["fa_numcae"].ToString(),
                    ComboID = drProcuto[0]["FT_IDCOMB"].ToString(),
                    stockItem = new StockItem ()
                    {
                        serializedItemModelNumber = drProcuto[0]["serie"].ToString(),
                    },
                    id = drProcuto[0]["id"].ToString(),
                    itemDescription = drProcuto[0]["it_desite"].ToString(),
                    modelName = drProcuto[0]["modelName"].ToString(),
                };
                saleReturn.quantity = Convert.ToInt32(drProcuto[0]["ft_cant"]);
                saleReturn.extendedDiscount = Convert.ToInt32(drProcuto[0]["extendedDiscount"].ToString());
                saleReturn.typeCode = drProcuto[0]["tipo_producto"].ToString();                
            }


           /* if (drGex.Count() > 0)
            {
                saleReturn.warrantyDuration = drGex[0]["precio_base"].ToString();
                saleReturn.warrantyMonthsDuration = Convert.ToDouble(drProcuto[0]["val_facturado"].ToString());
            }
           */


            return (saleReturn);
        }
    }
}

//tallerTipo.IdTipo = Convert.ToInt32(rowDeta["IdTipo"].ToString());
//tallerTipo.Descripcion = rowDeta["Descripciontipo"].ToString();


//saleReturn.regularUnitPrice = (string)Convert.ToDecimal((drProcuto["precio_base"].ToString));

/*
public class LineItem
{
    public SaleReturn saleReturn { get; set; }
    public string lineItemType { get; set; }
}



public class SaleReturn
{
    public int? regularUnitPrice { get; set; }
    public int netPrice { get; set; }
    public Item item { get; set; }
    public int? quantity { get; set; }
    public int? extendedDiscount { get; set; }
    public string typeCode { get; set; }
    public object warrantyDuration { get; set; }
    public object warrantyMonthsDuration { get; set; }
    public object warrantyContractId { get; set; }
    public string warrantyDocumentNumber { get; set; }
    public string warrantyPurchaseDate { get; set; }
}



public class RetailTransaction2
{
   

    
    public List<LineItem> lineItem { get; set; }
    public int fiscalFlag { get; set; }
    public int transactionDocumentNumber { get; set; }
    public Location location { get; set; }
    public string currencyISOCode { get; set; }
    public object channelId { get; set; }
    public Customer customer { get; set; }
*/