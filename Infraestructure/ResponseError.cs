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


