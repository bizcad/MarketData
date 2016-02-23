/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;

namespace MarketData.ToolBox
{
    /// <summary>
    /// Collection of compression routines
    /// </summary>
    public class Compression
    {
        /******************************************************** 
        * CLASS METHODS
        *********************************************************/
        /// <summary>
        /// Writes a zip file
        /// </summary>
        /// <param name="zipPath"></param>
        /// <param name="filenamesAndData"></param>
        /// <returns></returns>
        public static bool Zip(string zipPath, System.Collections.Generic.Dictionary<string, string> filenamesAndData)
        {
            var success = true;
            var buffer = new byte[4096];
            try
            {
                using (var stream = new ZipOutputStream(System.IO.File.Create(zipPath)))
                {
                    foreach (var filename in filenamesAndData.Keys)
                    {
                        var file = filenamesAndData[filename].GetBytes();
                        var entry = stream.PutNextEntry(filename);
                        using (var ms = new System.IO.MemoryStream(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = ms.Read(buffer, 0, buffer.Length);
                                stream.Write(buffer, 0, sourceBytes);
                            }
                            while (sourceBytes > 0);
                        }
                    }
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (System.Exception err)
            {
                System.Console.WriteLine("Compression.ZipData(): " + err.Message);
                success = false;
            }
            return success;
        }
        /// <summary>
        /// Writes a zip file
        /// </summary>
        /// <param name="zipPath">string - the path to the zip file to be written.</param>
        /// <param name="internalFilename">string - the name of the file within the zip file.  It can be a path</param>
        /// <param name="data">string - the data to be written into the zip file.</param>
        /// <returns>nothing</returns>
        public static async Task ZipAsync(string zipPath, string internalFilename, string data)
        {
            byte[] buf = Encoding.ASCII.GetBytes(data);
            using (var fs = File.Create(zipPath, buf.Length))
            {
                using (var s = new ZipOutputStream(fs))
                {
                    s.PutNextEntry(internalFilename);
                    await s.WriteAsync(buf, 0, buf.Length);
                }
            }
        }
        /// <summary>
        /// Writes a zip file
        /// </summary>
        /// <param name="zipPath">string - the path to the zip file to be written.</param>
        /// <param name="internalFilename">string - the name of the file within the zip file.  It can be a path</param>
        /// <param name="data">string - the data to be written into the zip file.</param>
        public static void Zip(string zipPath, string internalFilename, string data)
        {
            byte[] buf = Encoding.ASCII.GetBytes(data);

            using (var fs = File.Create(zipPath, buf.Length))
            {
                using (var s = new ZipOutputStream(fs))
                {
                    s.PutNextEntry(internalFilename);
                    s.Write(buf, 0, buf.Length);
                }
            }
        }

        public static string Unzip(string zipPath, string internalFilename)
        {
            string contents;
            using (var ms = new MemoryStream())
            {
                using (ZipFile zip = ZipFile.Read(zipPath))
                {
                    ZipEntry entry = zip[internalFilename];
                    entry.Extract(ms);  // extract uncompressed content into a memorystream 
                    byte[] buf = ms.GetBuffer();
                    contents = Encoding.Default.GetString(buf);
                    contents = contents.Substring(0, contents.LastIndexOf("\r", System.StringComparison.Ordinal));
                }
            }
            return contents;
        }

        public static void RenameInternal(FileInfo info)
        {
            string contents = string.Empty;
            string internalFilename = string.Empty;
            string newInternalFilename = info.Name.Replace(".zip", ".csv");
            
            using (ZipFile zip = ZipFile.Read(info.FullName))
            {
                foreach (ZipEntry entry in zip.Entries.Where(entry => entry.FileName.Contains("Minute")))
                {
                    internalFilename = string.Empty;
                    if (entry.FileName != info.Name.Replace(".zip", ".csv"))
                    {
                        internalFilename = entry.FileName;
                    }
                }
            }

            try
            {
                if (internalFilename.Length <= 0) return;
                string data = Unzip(info.FullName, internalFilename);
                File.Delete(info.FullName);
                //Thread.Sleep(250);
                Zip(info.FullName, newInternalFilename, data);
                Console.WriteLine(info.FullName);
                //Thread.Sleep(150);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }            

        }

        public static void AddTextToZip(string zipPath, string internalFilename, string data)
        {
            string contents;
            using (var ms = new MemoryStream())
            {
                using (ZipFile zip = ZipFile.Read(zipPath))
                {
                    ZipEntry entry = zip[internalFilename];
                    entry.Extract(ms);  // extract uncompressed content into a memorystream 
                    byte[] buf = ms.GetBuffer();
                    contents = Encoding.Default.GetString(buf);


                    
                    contents += data;
                }
            }
            Zip(zipPath, internalFilename, contents);
        }
    }
}
