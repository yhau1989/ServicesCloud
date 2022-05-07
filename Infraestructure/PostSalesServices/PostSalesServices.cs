using Core.Interfaces;
using Core.Response;
using DbSybaseService;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;

namespace Infraestructure.PostSalesServices
{
    public class PostSalesServices : IPostSalesServices
    {

        DbSybaseServiceOdbc dbSybaseServiceOdbc;
        private readonly IConfiguration _configuration;

        public PostSalesServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<PostSalesServicesResponse> Get(string id_number)
        {

            DataTable DtCabecera = new DataTable();
            DataTable DtCambio = new DataTable();
            DataTable DtDetalleSeguimiento = new DataTable();

            dbSybaseServiceOdbc = new DbSybaseServiceOdbc(_configuration["AppSettings:ConnectionStringASA9"]);
            dbSybaseServiceOdbc.Initialize();
            OdbcCommand odbcCommand = new OdbcCommand("EXEC dba.SP_SCloud_Servifacil ?", dbSybaseServiceOdbc.connection);
            odbcCommand.CommandType = CommandType.StoredProcedure;
            odbcCommand.Parameters.AddWithValue("@cedula", id_number.Trim());
            var odbcReader = odbcCommand.ExecuteReader();
            DtCabecera.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                DtCambio.Load(odbcReader);
            if (odbcReader.IsClosed == false)
                DtDetalleSeguimiento.Load(odbcReader);
            dbSybaseServiceOdbc.Dispose();

            List<PostSalesServicesResponse> ListPostSalesServices;
            ListPostSalesServices = FillPostSalesServices(DtCabecera, DtCambio, DtDetalleSeguimiento);
            return (ListPostSalesServices);
        }

        private List<PostSalesServicesResponse> FillPostSalesServices(DataTable DtCabecera, DataTable DtCambio, DataTable DtDetalleSeguimiento)
        {
            List<PostSalesServicesResponse> ListPostSalesServicesResponse = new List<PostSalesServicesResponse>();
            PostSalesServicesResponse postSalesServicesResponse;

            foreach (DataRow drCab in DtCabecera.Rows)
            {
                postSalesServicesResponse = new PostSalesServicesResponse()
                {
                    serviceOrderId = Convert.ToInt32(drCab["num_os"]),
                    countryISOCode = drCab["isoCode"].ToString(),
                    status = drCab["Estado_Orden"].ToString(),
                    repairType = drCab["Tipo_Orden"].ToString(),
                    completionType = drCab["Ult_estado_cierre"].ToString(),
                    repairWorkshop = drCab["Nombre_taller"].ToString(),
                    repairReson = drCab["Ultimo_motivo_seg"].ToString(),
                    completionDate = drCab["Ultimo_motivo_seg"].ToString(),
                    serviceDuration = drCab["fecha_real"].ToString(),
                    damage = drCab["des_danio"].ToString(),
                    borrowStatus = drCab["borrowStatus"].ToString(),
                    borrowComment = drCab["borrowComment"].ToString(),
                    customer = new CustomerPostSerice()
                    {
                        customerId = drCab["customerId"].ToString(),
                        fullName = drCab["Full_Name"].ToString(),
                        firstName = drCab["firstName"].ToString(),
                        secondName = drCab["secondName"].ToString(),
                        thirdName = drCab["thirdName"].ToString(),
                        lastName = drCab["lastName"].ToString(),
                        secondLastName = drCab["secondLastName"].ToString(),
                        marriedLastName = drCab["marriedLastName"].ToString(),
                        vip = drCab["vip"].ToString(),
                        idDetail = new List<IdDetail>() { new IdDetail { idType = drCab["cl_tipdoc"].ToString(), idDescription = drCab["des_tipodoc"].ToString(), idNumber = drCab["idNumber"].ToString()  } },
                     },
                    addressDetail = new AddressDetail()
                    {
                        stateRegionId = drCab["stateRegionId"].ToString(),
                        stateRegion = drCab["stateRegion"].ToString(),                        
                        cityId = drCab["cityId"].ToString(),
                        city = drCab["city"].ToString(),
                        neighborhoodId = drCab["neighborhoodId"].ToString(),
                        neighborhood = drCab["neighborhood"].ToString(),
                        addressLine1 = drCab["direccion_destinatario"].ToString(),
                    },
                    phoneDetail = new PhoneDetail()
                    {
                        phoneNumberType = drCab["phoneNumberType"].ToString(),                        
                        phoneNumberId = drCab["telefono_destinatario"].ToString(),
                    },
                    productDetail = new ProductDetail()
                    {
                        description  = drCab["IT_DESITE"].ToString(),
                        chain = drCab["ca_nombre"].ToString(),
                        store = drCab["Emisor_Factura"].ToString(),
                        serviceOrderSource = drCab["Emisor_Factura"].ToString(),
                        item = drCab["Emisor_Factura"].ToString(),
                        brand = drCab["COD_ARTICULO"].ToString(),
                        model = drCab["model"].ToString(),
                        serial = drCab["SERIE"].ToString(),
                        accesories = drCab["accesories"].ToString(),
                    },                    
                    changeRegistry = GetChangeRegistry(DtCambio, Convert.ToInt32(drCab["num_os"]), Convert.ToInt32(drCab["cod_emisor_fac"])),
                    eventRegistry =  GetEventRegistry(DtDetalleSeguimiento, Convert.ToInt32(drCab["num_os"]), Convert.ToInt32(drCab["cod_emisor_fac"])),                   
                    serviceCost = new ServiceCost()
                     {
                        spareParts = Convert.ToInt32(drCab["spareParts"].ToString()),
                        miscellaneous = Convert.ToDecimal(drCab["miscellaneous"].ToString()),
                        transportation = Convert.ToDecimal(drCab["transportation"].ToString()),
                        labor = Convert.ToDecimal(drCab["labor"].ToString()),
                        subTotal = Convert.ToDecimal(drCab["subTotal"].ToString()),
                        discount = Convert.ToDecimal(drCab["discount"].ToString()),
                        netTotal = Convert.ToDecimal(drCab["netTotal"].ToString()),
                        tax = Convert.ToDecimal(drCab["tax"].ToString()),
                      //  total = (drCab["total"] == null || drCab["total"] =="")  ? 0 : Convert.ToDecimal(drCab["total"].ToString()), 
                        downPayment = Convert.ToDecimal(drCab["downPayment"].ToString()),
                        pendingPayment = Convert.ToDecimal(drCab["pendingPayment"].ToString()),
                      //  invoiceNumber = Convert.ToDecimal(drCab["FACTURA_FULL_PROV"].ToString()), NULO
                        downPaymentNumber = Convert.ToDecimal(drCab["downPaymentNumber"].ToString()),
                        CustomerInvoiceNumber = drCab["Factura_Full_cli"].ToString(),
                        VendorTaxId = Convert.ToDecimal(drCab["IdProv"].ToString()),
                        VendorName = drCab["NomProv"].ToString(),
                        VendorInvoiceData = drCab["FchProv"].ToString(),
                    }
                };
                ListPostSalesServicesResponse.Add(postSalesServicesResponse);
            }
            return ListPostSalesServicesResponse;
        }

        private ChangeRegistry GetChangeRegistry(DataTable DtCambio, int os, int emisor)
        {                        
           
            DataRow[] dr = null;
            dr = DtCambio.Select($"num_os={os} and cod_emisor_fac={emisor}");
            ChangeRegistry changeRegistry = new ChangeRegistry();

            if (dr.Count() > 0)
            {
                changeRegistry.reason = dr[0]["motivo_cam"].ToString();
                changeRegistry.number = dr[0]["numero_cam"].ToString();
                changeRegistry.authorizationDate = dr[0]["fecha_cam"].ToString();
                changeRegistry.authorizationUser = dr[0]["Usuario_cam"].ToString();
               
             } else
            {
                changeRegistry = null;
            };            
            return (changeRegistry);
        }
        private EventRegistry GetEventRegistry(DataTable DtDetalleSeguimiento, int os, int emisor)
        {
            DataRow[] dr = null;
            dr = DtDetalleSeguimiento.Select($"num_os={os} and cod_emisor_fac={emisor}");
            EventRegistry eventRegistry = new EventRegistry();

            if (dr.Count() > 0)
            {
                eventRegistry.number = dr[0]["numero_seg"].ToString();
                eventRegistry.description = dr[0]["observa_seg"].ToString();
                eventRegistry.technicianName = dr[0]["Usuario_Seg"].ToString();
                eventRegistry.creationDate = dr[0]["fecha_seg"].ToString();
                eventRegistry.reason = dr[0]["motivo_seg"].ToString();
            }
            else
            {
                eventRegistry = null;
            };
            return (eventRegistry);
        }

    }
}
