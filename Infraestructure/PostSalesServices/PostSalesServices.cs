using Core.Interfaces;
using Core.Response;
using DbSybaseService;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Text;
using Tools;

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

            //desencriptando cadena
            string cConexion = _configuration["AppSettings:ConnectionStringASA9"];
            byte[] toBytes = Encoding.UTF8.GetBytes(cConexion);
            byte[] ketToBytes = Encoding.UTF8.GetBytes("xxxTokenxxx");
            cConexion = testMD5.decrypt(cConexion);
            //desencriptando cadena

            dbSybaseServiceOdbc = new DbSybaseServiceOdbc(cConexion);
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
                var customerBase = new CustomerPostSerice()
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
                    idDetail = new List<IdDetail>() { new IdDetail { idType = drCab["cl_tipdoc"].ToString(), idDescription = drCab["des_tipodoc"].ToString(), idNumber = drCab["idNumber"].ToString() } },
                };

                var addressDetailBase = new AddressDetail()
                {
                    stateRegionId = drCab["stateRegionId"].ToString(),
                    stateRegion = drCab["stateRegion"].ToString(),
                    cityId = drCab["cityId"].ToString(),
                    city = drCab["city"].ToString(),
                    neighborhoodId = drCab["neighborhoodId"].ToString(),
                    neighborhood = drCab["neighborhood"].ToString(),
                    addressLine1 = drCab["direccion_destinatario"].ToString(),
                };

                var phoneDetailBase = new PhoneDetail()
                {
                    phoneNumberType = drCab["phoneNumberType"].ToString(),
                    phoneNumberId = drCab["telefono_destinatario"].ToString(),
                };

                var productDetailBase = new ProductDetail()
                {
                    description = drCab["IT_DESITE"].ToString(),
                    chain = drCab["ca_nombre"].ToString(),
                    store = drCab["Emisor_Factura"].ToString(),
                    serviceOrderSource = drCab["Emisor_Factura"].ToString(),
                    item = drCab["COD_ARTICULO"].ToString(),
                    brand = drCab["marca"].ToString(),
                    model = drCab["model"].ToString(),
                    serial = drCab["SERIE"].ToString(),
                    accesories = drCab["accesories"].ToString(),
                    status = drCab["Estado_Orden"].ToString(),
                    repairType = drCab["Tipo_Orden"].ToString(),
                    completionType = drCab["Ult_estado_cierre"].ToString(),
                    repairWorkshop = drCab["Nombre_taller"].ToString(),
                    repairReason = drCab["Ultimo_motivo_seg"].ToString(),
                    completionDate = drCab["Ultimo_motivo_seg"].ToString(),
                    serviceDuration = drCab["fecha_real"].ToString(),
                    damage = drCab["des_danio"].ToString(),
                    borrowStatus = drCab["borrowStatus"].ToString(),
                    borrowComment = drCab["borrowComment"].ToString(),
                };

                var serviceCostBase = new ServiceCost()
                {
                    spareParts = Convert.ToInt32(drCab["spareParts"].ToString()),
                    miscellaneous = Convert.ToDecimal(drCab["miscellaneous"].ToString()),
                    transportation = Convert.ToDecimal(drCab["transportation"].ToString()),
                    labor = Convert.ToDecimal(drCab["labor"].ToString()),
                    subTotal = Convert.ToDecimal(drCab["subTotal"].ToString()),
                    discount = Convert.ToDecimal(drCab["discount"].ToString()),
                    netTotal = Convert.ToDecimal(drCab["netTotal"].ToString()),
                    tax = Convert.ToDecimal(drCab["tax"].ToString()),
                    total = (drCab["total"] != null && drCab["total"].ToString().Length > 0) ? Convert.ToDecimal(drCab["total"].ToString()) : 0,
                    downPayment = Convert.ToDecimal(drCab["downPayment"].ToString()),
                    pendingPayment = Convert.ToDecimal(drCab["pendingPayment"].ToString()),
                    invoiceNumber = drCab["FACTURA_FULL_PROV"].ToString(),
                    downPaymentNumber = Convert.ToDecimal(drCab["downPaymentNumber"].ToString()),
                    CustomerInvoiceNumber = drCab["Factura_Full_cli"].ToString(),
                    VendorTaxId = drCab["IdProv"].ToString(),
                    VendorName = drCab["NomProv"].ToString(),
                    VendorInvoiceData = drCab["FchProv"].ToString(),
                };

                postSalesServicesResponse = new PostSalesServicesResponse()
                {
                    serviceOrderId = Convert.ToInt32(drCab["num_os"]),
                    countryISOCode = drCab["isoCode"].ToString(),
                    customer = customerBase,
                    addressDetail = addressDetailBase,
                    phoneDetail = phoneDetailBase,
                    productDetail = productDetailBase,                    
                    changeRegistry = GetChangeRegistry(DtCambio, Convert.ToInt32(drCab["num_os"]), Convert.ToInt32(drCab["cod_emisor_fac"])),
                    eventRegistry =  GetEventRegistry(DtDetalleSeguimiento, Convert.ToInt32(drCab["num_os"]), Convert.ToInt32(drCab["cod_emisor_fac"])),                   
                    serviceCost = serviceCostBase
                };           
                ListPostSalesServicesResponse.Add(postSalesServicesResponse);
            }
            return ListPostSalesServicesResponse;
        }

        private List<ChangeRegistry> GetChangeRegistry(DataTable DtCambio, int os, int emisor)
        {                        
            DataRow[] dr = DtCambio.Select($"num_os={os} and cod_emisor_fac={emisor}");
            List<ChangeRegistry> changeRegistryList = new List<ChangeRegistry>();

            if (dr.Count() > 0)
            {
                foreach (DataRow drItem in dr) {
                    changeRegistryList.Add(new ChangeRegistry() {
                        reason = drItem["motivo_cam"].ToString(),
                        number = drItem["numero_cam"].ToString(),
                        authorizationDate = drItem["fecha_cam"].ToString(),
                        authorizationUser = drItem["Usuario_cam"].ToString()
                    });
                }
             }
            
            return (changeRegistryList);
        }
        private List<EventRegistry> GetEventRegistry(DataTable DtDetalleSeguimiento, int os, int emisor)
        {
            DataRow[] dr = DtDetalleSeguimiento.Select($"num_os={os} and cod_emisor_fac={emisor}");
            List<EventRegistry> eventRegistryList = new List<EventRegistry>();

            if (dr.Count() > 0)
            {
                foreach (DataRow drItem in dr) {
                    eventRegistryList.Add(new EventRegistry() {
                        number = drItem["numero_seg"].ToString(),
                        description = drItem["observa_seg"].ToString(),
                        technicianName = drItem["Usuario_Seg"].ToString(),
                        creationDate = drItem["fecha_seg"].ToString(),
                        reason = drItem["motivo_seg"].ToString()
                    });
                }
            }
            return (eventRegistryList);
        }

    }
}
