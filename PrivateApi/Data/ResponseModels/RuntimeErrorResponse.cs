using System;
using MongoDB.Bson.Serialization.Attributes;
using PrivateApi.MongoDB;

namespace PrivateApi.Data.ResponseModels
{
	public class RuntimeErrorResponse
	{
        [BsonId]
        public string Id { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorLocation { get; set; }

        public DateTime Date { get; set; }

        public RuntimeErrorResponse(string ErrorMessage, string ErrorLocation)
		{
            Id = Guid.NewGuid().ToString();
            Date = DateTime.Now.ToLocalTime();
            this.ErrorMessage = ErrorMessage;
            this.ErrorLocation = ErrorLocation;
        }

        public override string ToString()
        {
            return @$"
                {Id}-{Date}\n
                Error: {ErrorMessage}.\n
                At {ErrorLocation} 
            ";
        }
    }
}

