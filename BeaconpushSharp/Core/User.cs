﻿using System;
using System.Net;
using BeaconpushSharp.Core;
using BeaconpushSharp.ResponseData;

namespace BeaconpushSharp.Core
{
    public class User : EntityBase, IUser
    {
        private readonly string _username;

        public User(string username, IRequestFactory requestFactory, IJsonSerializer jsonSerializer, IRestClient restClient)
            : base(requestFactory, jsonSerializer, restClient)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }
            _username = username;
        }

        public string Username
        {
            get { return _username; }
        }

        public bool IsOnline()
        {
            var request = RequestFactory.CreateIsUserOnlineRequest(Username);
            var response = RestClient.Execute(request);
            ThrowOnUnexpectedStatusCode(response, HttpStatusCode.OK, HttpStatusCode.NotFound);
            return response.Status == HttpStatusCode.OK;
        }

        public void ForceSignOut()
        {
            var request = RequestFactory.CreateForceUserSignOutRequest(Username);
            var response = RestClient.Execute(request);
            ThrowOnUnexpectedStatusCode(response, HttpStatusCode.NoContent, HttpStatusCode.OK);
        }

        public void Send(object message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            var data = JsonSerializer.Serialize(message);
            var request = RequestFactory.CreateSendMessageToUserRequest(Username, data);
            var response = RestClient.Execute(request);
            ThrowOnUnexpectedStatusCode(response);
        }
    }
}