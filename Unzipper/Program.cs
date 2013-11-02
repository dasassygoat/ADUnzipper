using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unzipper
{
    class Program
    {
        
        

        static void Main(string[] args)
        {
            string dirpath = @"e:\";
            string extractDirectory = @"e:\";
            DirectoryInfo di = new DirectoryInfo(dirpath);

            foreach (FileInfo fi in di.GetFiles("*.zip"))
            {
                //add to database
                Decompress(fi, extractDirectory);
            }
            Console.WriteLine("Finished decompressing");
            Console.ReadKey();

            
        }

        public static void Decompress(FileInfo fi, string extractDirectory)
        {
            UnzipDataDataContext db = new UnzipDataDataContext();
            string compressedFile = fi.DirectoryName + fi.Name;
            string fileName = fi.Name.Replace(".zip","");
            string directory = fi.DirectoryName + fileName;


            ZipFileName zipname = new ZipFileName();
            
            var found = from name in db.ZipFileNames
                        where name.ZipFileName1 == fileName & name.Extracted == true
                        select name;
            zipname.ZipFileName1 = fileName;
            if (found.Count() == 0)
            {

                try
                {
                    Directory.CreateDirectory(directory);
                    ZipFile.ExtractToDirectory(compressedFile, directory);
                    zipname.Extracted = true;
                    File.Delete(compressedFile);

                    Console.WriteLine("Decompressed: {0}", fi.Name);

                }
                catch (Exception e)
                {
                    zipname.Extracted = false;
                    Console.WriteLine("Couldn't decompress: {0} Error: {1}", fi.Name, e.ToString());
                }
                db.ZipFileNames.InsertOnSubmit(zipname);
                db.SubmitChanges();
            }
            else {
                Console.WriteLine("nothing added because its already been found");
            }


            
        }
                    

                        
                 
               
            
        
      


    }
}
