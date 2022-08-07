using System;
namespace Api.Commons
{
    public class Response
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public Response(int code , string message)
        {

            Code = code;
            Message = message;
        }

    }
}

