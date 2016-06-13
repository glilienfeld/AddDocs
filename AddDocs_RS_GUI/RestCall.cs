﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.IO;

namespace AddDocs_RS_GUI
{
    public class RestCall
    {
        RestClient client;
        RestRequest request;
        IRestResponse response;

        public RestCall(string hash, string user, string pw, string uri, Method httpMethod, string contentType)
        {
            client = new RestClient(uri);
            request = new RestRequest(httpMethod);
            request.AddHeader("content-type", contentType);
            request.AddHeader("x-integrationserver-password", pw);
            request.AddHeader("x-integrationserver-username", user);
            request.AddHeader("x-integrationserver-session-hash", hash);
        }

        public RestCall (string user, string pw, string uri, Method httpMethod, string contentType)
        {
            client = new RestClient(uri);
            request = new RestRequest(httpMethod);
            request.AddHeader("content-type", contentType);
            request.AddHeader("x-integrationserver-password", pw);
            request.AddHeader("x-integrationserver-username", user);
            //request.AddHeader("x-integrationserver-session-hash", hash);
        }

        public string GetConnection()
        {
            response = client.Execute(request);
            return response.Headers[0].Value.ToString();
        }

        public string PostDoc(INOW_Doc doc)
        {
            request.AddParameter("application/xml", doc.CreatePostDocXML(), ParameterType.RequestBody);
            response = client.Execute(request);
            string location = response.Headers[3].Value.ToString();
            string[] delims = { "document/" };
            string[] parts = location.Split(delims, StringSplitOptions.None);
            string docid = parts[1];
            return docid;
        }

        public void PostDocPage(string docid, string file)
        {
            request.AddHeader("x-integrationserver-resource-name", "dwa.tif");
            byte[] fileBytes = File.ReadAllBytes(file);
            request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}