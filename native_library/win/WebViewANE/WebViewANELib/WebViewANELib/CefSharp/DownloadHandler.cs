﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using CefSharp;

namespace WebViewANELib.CefSharp {
    public class DownloadHandler : IDownloadHandler {
        private readonly string _saveToDirectory;
        public event EventHandler<DownloadItem> OnBeforeDownloadFired;
        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        public DownloadHandler(string saveToDirectory = null) {
            _saveToDirectory = saveToDirectory;
        }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IBeforeDownloadCallback callback)
        {
            var handler = OnBeforeDownloadFired;
            handler?.Invoke(this, downloadItem);

            if (callback.IsDisposed) return;
            using (callback) {
                var downloadPath = string.IsNullOrEmpty(_saveToDirectory)
                    ? downloadItem.SuggestedFileName
                    : _saveToDirectory + "\\" + downloadItem.SuggestedFileName;
                var showDialog = string.IsNullOrEmpty(_saveToDirectory);
                callback.Continue(downloadPath, showDialog);
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem,
            IDownloadItemCallback callback) {
            var handler = OnDownloadUpdatedFired;
            handler?.Invoke(this, downloadItem);
        }
    }
}