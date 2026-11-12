using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PZ6.Models;

namespace PZ6
{
    internal class Helper
    {


        static public ateilerEntities GetContext()
        {
            return new ateilerEntities();

        }
    }
}
