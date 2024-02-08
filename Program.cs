using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NearestNeighbor
{
    class Program
    {
        static void Main(string[] args)
        {
            //The Nearest Neighbor method is the simplest interpolation method where the
            //interpolated point is assigned the value of the nearest point with a known value.

            //## tp 45.76861111 16.26805556

            //** t1 44.64 17.12 416.03
            //** t2 44.61 17.21 557.11
            //** t3 44.67 17.2 454.8
            //** t4 44.58 17.23 856.64
            //** t5 44.65 17.2 649.32
            //** t6 44.57 17.13 485.6
            //** t7 44.62 17.15 289.22
            //** t8 44.51 17.25 1176.72
            //** t9 44.55 17.19 540.63
            //** t10 44.64 17.17 598.3

            double fiTrazena = 0.0;
            double laTrazena = 0.0;
            double poluprecnikZemlje = 6371000.0;

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(@"..\ulaz.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                List<double> fi = new List<double>();
                List<double> la = new List<double>();
                List<double> h = new List<double>();
                List<double> fm = new List<double>();
                List<double> dF = new List<double>();
                List<double> dL = new List<double>();
                List<double> distance = new List<double>();

                while ((line = streamReader.ReadLine()) != null)
                {
                    // Split input string into lines
                    string[] lines = line.Split(' ', '\t', '|', '#', '$', '%', '&', '(', ')', '=', ';', '_', '\n', ',');

                    // Process line
                    if (line.Contains("##"))
                    {
                        fiTrazena = Convert.ToDouble(lines[4]) * Math.PI / 180.0;
                        laTrazena = Convert.ToDouble(lines[5]) * Math.PI / 180.0;
                    }
                    if (line.Contains("**"))
                    {
                        fi.Add(Convert.ToDouble(lines[2]) * Math.PI / 180.0);
                        la.Add(Convert.ToDouble(lines[3]) * Math.PI / 180.0);
                        h.Add(Convert.ToDouble(lines[4]));
                    }
                }

                for (int i = 0; i < fi.Count; i++)
                {
                    fm.Add((fi[i] + fiTrazena) * 0.5);
                    dF.Add(fi[i] - fiTrazena);
                    dL.Add(la[i] - laTrazena);
                }
                //VERIFY THE VALUES OF THE OBTAINED LENGTHS — It is not necessary in such cases when searching for the shortest length.
                for (int i = 0; i < fi.Count; i++)
                {
                    double df2 = dF[i] * dF[i];
                    double codFmDl = dL[i] * Math.Cos(fm[i]);
                    double codFmDl_2 = codFmDl * codFmDl;

                    distance.Add(poluprecnikZemlje * Math.Sqrt(df2 + codFmDl_2));

                    Console.WriteLine(distance[i]);
                }

                double minDist = distance.Min();
                //Find the index of the smallest length - what is that length in the list of distances?
                int indeksNajmanjeDuzine = distance.IndexOf(minDist);

                Console.WriteLine("*********** Najmanje Rastojanje *************");
                Console.WriteLine("------------ >     : " + minDist.ToString());
                Console.WriteLine("------------ >     : " + (indeksNajmanjeDuzine + 1).ToString());

                //The height of the Nearest Neighbor search method is the value of the point t10 that is closest to the desired point.
                Console.WriteLine("*********** Visina Trazene Tacke *************");
                Console.WriteLine("------------ >     : " + h[indeksNajmanjeDuzine].ToString());


            }
        }
    }
}
