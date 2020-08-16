﻿/// HttpUtils.HttpMultipartParser
/// 
/// Copyright (c) 2012 Lorenzo Polidori
/// 
/// This software is distributed under the terms of the MIT License reproduced below.
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
/// and associated documentation files (the "Software"), to deal in the Software without restriction, 
/// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
/// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
/// subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all copies or substantial 
/// portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
/// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
/// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
/// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
/// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// 

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace JavaScript.Web.Utils
{
    /// <summary>
    /// Reads a multipart http data stream and returns the file name, content type and file content.
    /// Also, it returns any additional form parameters in a Dictionary.
    /// </summary>
    public class HttpMultipartParser
    {
        public HttpMultipartParser(Stream stream, string filePartName)
        {
            FilePartName = filePartName;
            Parse(stream, Encoding.UTF8);
        }

        public HttpMultipartParser(Stream stream, Encoding encoding, string filePartName)
        {
            FilePartName = filePartName;
            Parse(stream, encoding);
        }
        public HttpMultipartParser(byte[] data, string filePartName)
        {
            FilePartName = filePartName;
            Parse(data, Encoding.UTF8);
        }

        public HttpMultipartParser(byte[] data, Encoding encoding, string filePartName)
        {
            FilePartName = filePartName;
            Parse(data, encoding);
        }

        private void Parse(byte[] data, Encoding encoding)
        {
            // Copy to a string for header parsing
            string content = encoding.GetString(data);

            // The first line should contain the delimiter
            int delimiterEndIndex = content.IndexOf("\r\n");

            if (delimiterEndIndex > -1)
            {
                string delimiter = content.Substring(0, content.IndexOf("\r\n"));

                string[] sections = content.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in sections)
                {
                    if (s.Contains("Content-Disposition"))
                    {
                        // If we find "Content-Disposition", this is a valid multi-part section
                        // Now, look for the "name" parameter
                        Match nameMatch = new Regex(@"(?<=name\=\"")(.*?)(?=\"")").Match(s);
                        string name = nameMatch.Value.Trim().ToLower();

                        if (name == FilePartName)
                        {
                            // Look for Content-Type
                            Regex re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                            Match contentTypeMatch = re.Match(content);

                            // Look for filename
                            re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                            Match filenameMatch = re.Match(content);

                            // Did we find the required values?
                            if (contentTypeMatch.Success && filenameMatch.Success)
                            {
                                // Set properties
                                ContentType = contentTypeMatch.Value.Trim();
                                Filename = filenameMatch.Value.Trim();

                                // Get the start & end indexes of the file contents
                                int startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                                byte[] delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                                int endIndex = Misc.IndexOf(data, delimiterBytes, startIndex);

                                int contentLength = endIndex - startIndex;

                                // Extract the file contents from the byte array
                                byte[] fileData = new byte[contentLength];

                                Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);

                                FileContents = fileData;
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(name))
                        {
                            // Get the start & end indexes of the file contents
                            int startIndex = nameMatch.Index + nameMatch.Length + "\r\n\r\n".Length;
                            Parameters.Add(name, s.Substring(startIndex).TrimEnd(new char[] { '\r', '\n' }).Trim());
                        }
                    }
                }

                // If some data has been successfully received, set success to true
                if (FileContents != null || Parameters.Count != 0)
                    Success = true;
            }
        }

        private void Parse(Stream stream, Encoding encoding)
        {
            Success = false;

            // Read the stream into a byte array
            byte[] data = Misc.ToByteArray(stream);
            Parse(data, encoding);
        }

        public IDictionary<string, string> Parameters = new Dictionary<string, string>();

        public string FilePartName
        {
            get;
            private set;
        }

        public bool Success
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public int UserId
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            private set;
        }

        public byte[] FileContents
        {
            get;
            private set;
        }
    }
}