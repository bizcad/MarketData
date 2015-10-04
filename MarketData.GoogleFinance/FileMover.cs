using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.GoogleFinance
{
    public static class FileMover
    {
        public static void MoveFiles(DirectoryInfo sourceRoot)
        {
            DirectoryInfo destRoot = new DirectoryInfo(@"H:\GoogleFinanceData\equity\usa\minute\");
            DirectoryInfo root;

            var subdirs1 = sourceRoot.GetDirectories();

            foreach (DirectoryInfo info1 in subdirs1)
            {

                //System.Threading.Thread.Sleep(100);
                var subdirs2 = info1.GetDirectories();
                foreach (DirectoryInfo info3 in subdirs2)
                {

                    try
                    {
                        if (!Directory.Exists(destRoot.FullName + info3.Name))
                        {
                            try
                            {
                                destRoot.CreateSubdirectory(info3.Name);
                            }
                            catch (Exception e)
                            {
                                string comment = "";
                            }
                        }
                        
                        Debug.WriteLine(info3.Name);

                        var files = info3.GetFiles();
                        foreach (var file3 in files)
                        {
                            try
                            {
                                string oldname = file3.FullName;
                                string newname = destRoot.FullName + info3.Name + @"\" + file3.Name;
                                File.Move(oldname, newname);

                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + file3.FullName);
                            }
                        }
                        System.Threading.Thread.Sleep(250);
                        //                Directory.Delete(info3.FullName, true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + info3.FullName);
                    }
                }
            }
        }

    }
}
