﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var algo = new MeanReversionPaperOnly();
            algo.Run();
            Console.Read();
        }
    }
}
