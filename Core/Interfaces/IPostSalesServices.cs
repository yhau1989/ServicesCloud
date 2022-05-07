using Core.Response;

namespace Core.Interfaces
{
    public  interface IPostSalesServices
    {
        List<PostSalesServicesResponse> Get(string id_number);
    }
}
