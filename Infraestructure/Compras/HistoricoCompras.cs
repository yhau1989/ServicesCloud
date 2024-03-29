﻿using Core.Interfaces;
using Core.Response;
using DbSybaseService;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Text;
using Tools;


namespace Infraestructure.Compras
{
    /// <summary>
    /// Clase que retorna toda la logica para el enpoint sterops/retail-transaction
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay Muñoz - UNICOMER
    /// fecha creación: 28/07/2022
    /// ]]>
    public class HistoricoCompras : IHistoricoCompras
    {
        DbSybaseServiceOdbc dbSybaseServiceOdbc;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        public HistoricoCompras(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        /// <summary>
        /// Metodo que encapsula toda la logica
        /// </summary>
        /// <param name="id_number">ci del cliente</param>
        /// <returns>List<RetailTransactionResponse></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        public List<RetailTransactionResponse> Get(string id_number)
        {
            DataTable DtTotal = new DataTable();
            DataTable DtCabecera = new DataTable();
            DataTable DtDetalle = new DataTable();

            //desencriptando cadena
            string cConexion = _configuration["AppSettings:ConnectionStringASA17"];
            cConexion = testMD5.decrypt(cConexion);
            //desencriptando cadena

            dbSybaseServiceOdbc = new DbSybaseServiceOdbc(cConexion);
            dbSybaseServiceOdbc.Initialize();
            OdbcCommand odbcCommand = new OdbcCommand("EXEC dba.SP_SCloud_Facturas ?", dbSybaseServiceOdbc.connection);
            odbcCommand.CommandType = CommandType.StoredProcedure;
            odbcCommand.Parameters.AddWithValue("@cedula", id_number.Trim());
            var odbcReader = odbcCommand.ExecuteReader();
            DtTotal.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                DtCabecera.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                DtDetalle.Load(odbcReader);
            dbSybaseServiceOdbc.Dispose();
            List<RetailTransactionResponse> ListHitoricoComprasCliente;
            ListHitoricoComprasCliente = FillHistorioCompras(DtTotal, DtCabecera, DtDetalle);
            return (ListHitoricoComprasCliente);
        }



        /// <summary>
        /// Arma la estructura del json de respuesta a partir de los datos que retorna el sp SP_SCloud_Facturas desde ASA
        /// </summary>
        /// <param name="DtTotal">tabla 1 del sp SP_SCloud_Facturas</param>
        /// <param name="DtCabecera">tabla 2 del sp SP_SCloud_Facturas</param>
        /// <param name="DtDetalle">tabla 3 del sp SP_SCloud_Facturas</param>
        /// <returns>List<RetailTransactionResponse></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<RetailTransactionResponse> FillHistorioCompras(DataTable DtTotal, DataTable DtCabecera, DataTable DtDetalle)
        {
            DataTable dtTransactionDocument = new DataTable();
            List<RetailTransactionResponse> ListretailTransaction = new List<RetailTransactionResponse>();
            foreach (DataRow drCab in DtCabecera.Rows)
            {
                RetailTransactionResponse retailTransaction;
                retailTransaction = new RetailTransactionResponse()
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
                    lineItem = GetLineItem(DtDetalle, Convert.ToInt32(drCab["EM_CODIGO"].ToString()), Convert.ToInt32(drCab["FT_IDFACT"].ToString()), Convert.ToInt32(drCab["TF_TIPFAC"].ToString())),
                    fiscalFlag = "0",
                    transactionDocumentNumber = drCab["Factura_full"].ToString(),
                    location = new Location()
                    {
                        locationName = drCab["EM_NOMBRE"].ToString(),
                        locationId = drCab["em_abrev"].ToString(),
                        locationType = drCab["Tipo_Tienda"].ToString(),
                        locationAttributes = new LocationAttributes()
                        {
                            chainName = drCab["ca_nombre"].ToString(),
                            chainId = drCab["chainId"].ToString(),
                        }
                    },
                    currencyISOCode = drCab["currencyISOCode"].ToString(),
                    channelId = drCab["channelId"].ToString(),
                    customer = new Customer()
                    {
                        customerIdDocumentNumber = drCab["cl_cedruc"].ToString(),
                        quantityInvoice = Convert.ToInt32(DtTotal.Rows[0]["CANT_TOTFAC"]),
                        quantityCreditInvoice = Convert.ToInt32(DtTotal.Rows[0]["CANT_ACREDITO"]),
                        quantityDueInvoice = Convert.ToInt32(DtTotal.Rows[0]["CANT_VENCIDAS"]),
                    }
                };
                ListretailTransaction.Add(retailTransaction);
            }
            return (ListretailTransaction);
        }



        /// <summary>
        /// Arma la estructura del json de respuesta a partir de los datos que retorna el sp SP_SCloud_Facturas desde ASA
        /// </summary>
        /// <param name="Tipo_Doc">tipo de documento de identidad</param>
        /// <param name="Factura_full"></param>
        /// <param name="Fecha_Doc"></param>
        /// <returns>List<TransactionDocument></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
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




        /// <summary>
        ///  arma la seccion del json LineItems
        /// </summary>
        /// <param name="DtDetalle"></param>
        /// <param name="emisor"></param>
        /// <param name="factura"></param>
        /// <param name="ptovta"></param>
        /// <returns>List<Object></returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private List<Object> GetLineItem(DataTable DtDetalle, int emisor, int factura, int ptovta)
        {
            // selecciono la factura con que voy a trabajar 
            DataTable dt;
            bool existeGex;
            dt =   DtDetalle.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta}").CopyToDataTable();

            // obtnego las interacciones que debo de realizar por item ( distinct por emisor, factura, pto venta e item
            DataTable dtDisitnct;
            string[] sColumnasDistinct = { "EM_CODIGO", "FT_IDFACT", "TF_TIPFAC", "COD_ARTICULO" , "lineItemType" };
            dtDisitnct = dt.DefaultView.ToTable(true, sColumnasDistinct);

            List<Object> listlineItem = new List<Object>();
            SaleReturnProdPadre lineItemProd;
            SaleReturnGexPadre lineItemGex;

            
            foreach (DataRow dr in dtDisitnct.Rows)
            {
                lineItemProd = new SaleReturnProdPadre()
                {
                    saleReturn = GetSaleReturnProd(dt, emisor, factura, ptovta, dr["COD_ARTICULO"].ToString()),
                    lineItemType ="",    
                };

                existeGex = false;
                lineItemGex = new SaleReturnGexPadre()
                {
                   saleReturn = GetSaleReturnGex(dt, emisor, factura, ptovta, dr["COD_ARTICULO"].ToString(), ref existeGex),
                   lineItemType = "",
               };
               
                listlineItem.Add(lineItemProd);
                if (existeGex)
                {
                    listlineItem.Add(lineItemGex);
                }
               
            }                
            return (listlineItem);
        }



        /// <summary>
        /// Arma la seccion del json SaleReturn
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="emisor"></param>
        /// <param name="factura"></param>
        /// <param name="ptovta"></param>
        /// <param name="idItem"></param>
        /// <returns>SaleReturnProd</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private SaleReturnProd GetSaleReturnProd(DataTable dt, int emisor, int factura, int ptovta, string idItem)
        {                        
            DataRow[] drProcuto = null;
            drProcuto = dt.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta} and COD_ARTICULO='{idItem}' and tipo_producto='STOCK'");

            DataRow[] drGex = null;
            drGex = dt.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta} and COD_ARTICULO='{idItem}' and tipo_producto='WARRANTY'");

            SaleReturnProd saleReturn = new SaleReturnProd();

            if (drProcuto.Count() > 0)
            {               
                saleReturn.regularUnitPrice = drProcuto[0]["precio_base"].ToString();
                saleReturn.netPrice = Convert.ToDouble(drProcuto[0] ["val_facturado"].ToString());
                saleReturn.item = new ItemProd()
                {
                    itemId = idItem,
                    brandName = drProcuto[0]["brandName"].ToString(),
                    posIdentity = new PosIdentity()
                    {
                        warrantyDuration = (drGex.Count() > 0 ? drGex[0]["warrantyDuration"].ToString() : "")
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
            return (saleReturn);
        }




        /// <summary>
        /// Arma la seccion del json SaleReturnGex
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="emisor"></param>
        /// <param name="factura"></param>
        /// <param name="ptovta"></param>
        /// <param name="idItem"></param>
        /// <param name="existeGex"></param>
        /// <returns>SaleReturnGex</returns>
        /// <![CDATA[ 
        /// Autor: Samuel Pilay Muñoz - UNICOMER
        /// fecha creación: 28/07/2022
        /// ]]>
        private SaleReturnGex GetSaleReturnGex(DataTable dt, int emisor, int factura, int ptovta, string idItem, ref bool existeGex)
        {

           // DataRow[] drProcuto = null;
           // drProcuto = dt.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta} and COD_ARTICULO='{idItem}' and tipo_producto='Producto'");

            DataRow[] drGex = null;
            drGex = dt.Select($"EM_CODIGO={emisor} and FT_IDFACT={factura} and TF_TIPFAC={ptovta} and COD_ARTICULO='{idItem}' and tipo_producto='WARRANTY'");

            SaleReturnGex saleReturn = new SaleReturnGex();

            existeGex = false;

            if (drGex.Count() > 0)
            {
                saleReturn.warrantyDuration = drGex[0]["warrantyDuration"].ToString();
                saleReturn.wntyMonthsDuration = drGex[0]["Plazo_Gex"].ToString();
                saleReturn.warrantyContractId = drGex[0]["warrantyContractId"].ToString(); 
                saleReturn.warrantyDocumentNumber = drGex[0]["warrantyDocumentNumber"].ToString();
                saleReturn.warrantyPurchaseDate = drGex[0]["warrantyPurchaseDate"].ToString();
                saleReturn.quantity = Convert.ToInt32(drGex[0]["ft_cant"]);
                saleReturn.extendedDiscount = drGex[0]["extendedDiscount"].ToString();
                saleReturn.regularUnitPrice = drGex[0]["precio_base"].ToString();
                saleReturn.netPrice =  drGex[0]["val_facturado"].ToString();
                saleReturn.typeCode = drGex[0]["tipo_garantia"].ToString(); 

                saleReturn.item = new ItemGex()
                {
                    itemId = idItem,
                    brandName = drGex[0]["brandName"].ToString(),
                    upc = drGex[0]["upc"].ToString(),
                    id = drGex[0]["id"].ToString(),
                    itemDescription = drGex[0]["it_desite"].ToString(),
                    relatedItem = new RelatedItem()
                    {
                        upc = drGex[0]["upc"].ToString()
                    }
                };
                existeGex = true;
            }
            return (saleReturn);
        }       
    }
}
