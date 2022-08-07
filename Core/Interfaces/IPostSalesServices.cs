using Core.Response;

namespace Core.Interfaces
{
    /// <summary>
    /// interfaz para los metodos de historico de servicios
    /// </summary>
    /// <![CDATA[ 
    /// Autor: Samuel Pilay Muñoz - UNICOMER
    /// fecha creación: 28/07/2022
    /// ]]>
    public interface IPostSalesServices
    {
        List<PostSalesServicesResponse> Get(string id_number);
    }
}
