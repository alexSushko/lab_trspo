using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
namespace lab_p.Controllers
{
    public class respondM {
        public int[,] matrix;
        public string filename;
        public int countsOfRowA;
        public int countsOfRowB;
        public int countsOfItemsA;
        public int countsOfItemsB;
    }
    public class ValuesController : ApiController
    {
      
        [HttpGet]
        public List<respondM> Get(string name)
        {
         
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/");
            //string[] filenames = Directory.GetFiles(sPath);
            var result = new List<respondM>();
            string filename = sPath + name+".txt";
            
                
                using(StreamReader sr = new StreamReader(filename))
                {
                    int[,] a;
                    int[,] b;
                    readMatrixFromFile(sr, out a, out b);
                    int[,] r = multiplyMatrix(a, b);
                    result.Add(new respondM
                    {
                        matrix = r,
                        countsOfItemsA = a.GetLength(1),
                        countsOfItemsB = b.GetLength(1),
                        countsOfRowA = a.GetLength(0),
                        countsOfRowB = b.GetLength(0),
                        filename = name+".txt"
                    });
                
            }
            return result;
        }

        //public async Task<int[,]> Post()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        return null;
        //    }
        //    var provider = new MultipartMemoryStreamProvider();
        //    путь к папке на сервере
        //    string root = System.Web.HttpContext.Current.Server.MapPath("~/Files/");
        //    await Request.Content.ReadAsMultipartAsync(provider);

        //    foreach (var file in provider.Contents)
        //    {
        //        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
        //        string fileArray = await file.ReadAsStringAsync();
        //        int[,] a;
        //        int[,] b;
        //        using (System.IO.FileStream fs = new System.IO.FileStream(root + filename, System.IO.FileMode.Create))
        //        {
        //            await
        //            await multiplyMatrix(a, b);
        //            await fs.WriteAsync(fileArray, 0, fileArray.Length);
        //            fileArray.Read()
        //        }
        //    }
        //    return new int[1, 1];
        //}
        [HttpPost()]
        public List<respondM> Post()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
           

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            List<respondM> result = new List<respondM>();
            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                Stream hpf = hfc[iCnt].InputStream;
                
                if (hpf.Length > 0)
                {
                    using (StreamReader sr = new StreamReader(hpf))
                    {
                        int[,] a;
                        int[,] b;
                        readMatrixFromFile(sr,out a,out b);
                        int[,] r = multiplyMatrix(a, b);
                        result.Add(new respondM
                        {
                            matrix = r,
                            countsOfItemsA = a.GetLength(1),
                            countsOfItemsB = b.GetLength(1),
                            countsOfRowA = a.GetLength(0),
                            countsOfRowB = b.GetLength(0),
                            filename = hfc[iCnt].FileName
                        });
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                    
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                
                return result;
            }
            else
            {
                return null;
            }
        }

        private void readMatrixFromFile(StreamReader sr, out int[,] a, out int[,] b)
        {

            List<List<int>> resList = new List<List<int>>();
            string line;
            int i=0;
            while ((line = sr.ReadLine()) != "B:")
            {
                resList.Add(new List<int>());
                foreach(string s in line.Split(' '))
                {
                    resList[i].Add(int.Parse(s));
                }
                i++;
            }
            a = new int[resList.Count, resList[1].Count];
            for (i = 0; i < resList.Count; i++)
            {
                for (int j = 0; j < resList[0].Count; j++) {

                    a[i, j] = resList[i][j];
                        }
            }
            resList=new List<List<int>>();
            i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                resList.Add(new List<int>());
                foreach (string s in line.Split(' '))
                {
                    resList[i].Add(int.Parse(s));
                }
                i++;
            }
            b = new int[resList.Count, resList[1].Count];
            for (i = 0; i < resList.Count; i++)
            {
                for (int j = 0; j < resList[0].Count; j++)
                {

                    b[i, j] = resList[i][j];
                }
            }


        }
        private int[,] multiplyMatrix(int[,] a, int[,] b)
        {
            try
            {
                
                if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");
            }
            catch (Exception e)
            {
                return null;
            }
            int[,] r = new int[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        r[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return r;

        }
    }
}
