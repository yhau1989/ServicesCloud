

/// <summary>
/// Clases base para los json de response
/// </summary>
/// <![CDATA[ 
/// Autor: Samuel Pilay Muñoz
/// fecha creación: 28/07/2022
/// ]]>
namespace Infraestructure
{

    public class ResponseError
    {
        public string code { get; set; }
        public string id { get; set; }
        public string detail { get; set; }
        public string title { get; set; }
    }

    public class GeneralResponse
    {
        public object generalResponse { get; set; }
    }
}


