using FloorOrderApp.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FloorOrderApp.Data.Interfaces;
using FloorOrderApp.Models;

namespace FloorOrderApp.BLL
{
    public static class RepoFactory
    { 
        private static IOrderRepo _currentRepo;

        public static IOrderRepo GetRepo()
        {
            string logType = ConfigurationManager.AppSettings["repotype"];
            //ConfigurationSaveMode blah = ConfigurationSaveMode.Modified;
            //blah
            if (logType == "m")
            {
                _currentRepo = new MockOrdersRepo();
            }
            else
            {
                _currentRepo = new ProdOrdersRepo();
            }

            return _currentRepo;
        }
    }
}
