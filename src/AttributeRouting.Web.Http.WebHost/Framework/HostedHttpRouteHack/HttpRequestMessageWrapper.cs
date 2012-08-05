﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using System.Web.Http.Hosting;

namespace AttributeRouting.Web.Http.WebHost.Framework.HostedHttpRouteHack
{
    internal class HttpRequestMessageWrapper : HttpRequestBase
    {
        private readonly string _virtualPathRoot;
        private readonly HttpRequestMessage _httpRequest;

        public HttpRequestMessageWrapper(string virtualPathRoot, HttpRequestMessage httpRequest)
        {
            if (virtualPathRoot == null)
            {
                throw new ArgumentNullException("virtualPathRoot");
            }

            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest");
            }

            _virtualPathRoot = virtualPathRoot;
            _httpRequest = httpRequest;
        }

        public override string ApplicationPath
        {
            get { return _virtualPathRoot; }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                string absolutePath = _httpRequest.RequestUri.AbsolutePath;
                if (absolutePath.StartsWith(_virtualPathRoot, StringComparison.OrdinalIgnoreCase))
                {
                    string relativePath = _virtualPathRoot.Length == 1 ? absolutePath : absolutePath.Substring(_virtualPathRoot.Length);
                    return "~" + relativePath.TrimEnd('/');
                }
                return null;
            }
        }

        public override string CurrentExecutionFilePath
        {
            get { return FilePath; }
        }

        public override string FilePath
        {
            get
            {
                string absolutePath = _httpRequest.RequestUri.AbsolutePath;
                if (absolutePath.StartsWith(_virtualPathRoot, StringComparison.OrdinalIgnoreCase))
                {
                    return absolutePath.TrimEnd('/');
                }
                return null;
            }
        }

        public override string HttpMethod
        {
            get { return _httpRequest.Method.ToString(); }
        }

        public override bool IsLocal
        {
            get
            {
                Lazy<bool> isLocal;
                if (_httpRequest.Properties.TryGetValue<Lazy<bool>>(HttpPropertyKeys.IsLocalKey, out isLocal))
                {
                    return isLocal.Value;
                }
                return false;
            }
        }

        public override string Path
        {
            get
            {
                return _httpRequest.RequestUri.AbsolutePath;
            }
        }

        public override string PathInfo
        {
            get { return String.Empty; }
        }

        public override NameValueCollection QueryString
        {
            get { return _httpRequest.RequestUri.ParseQueryString(); }
        }

        public override string RawUrl
        {
            get { return _httpRequest.RequestUri.PathAndQuery; }
        }

        public override string RequestType
        {
            get { return _httpRequest.Method.ToString(); }
        }

        public override Uri Url
        {
            get { return _httpRequest.RequestUri; }
        }
    }
}
