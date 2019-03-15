using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Infra.CrossCutting.Responses
{
    public class Response<TValue>
    {
        public Response()
        {
            Messages = new HashSet<string>();
        }

        public Response(TValue value) : this()
        {
            Value = value;
        }

        public TValue Value { get; }

        public ISet<String> Messages { get; }

        [JsonIgnore]
        public bool IsFailure => !IsSuccess;

        [JsonIgnore]
        public bool IsSuccess => Messages.Count == 0;

        public static Response<TValue> Ok()
        {
            return new Response<TValue>();
        }

        public static Response<TValue> Ok(TValue value)
        {
            return new Response<TValue>(value);
        }
        public static Response<TValue> Fail(string message)
        {
            var response = new Response<TValue>(default(TValue));
            response.Messages.Add(message);

            return response;
        }

        public static Response<TValue> Fail(IEnumerable<string> messages)
        {
            var response = new Response<TValue>(default(TValue));
            response.Messages.UnionWith(messages);

            return response;
        }
    }
}
