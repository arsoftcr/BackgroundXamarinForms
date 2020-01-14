using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundXamarinForms
{
    public class BackgroundSharedService
    {

        private static BackgroundSharedService _instance;

        static BackgroundSharedService()
        {
        }

        private BackgroundSharedService()
        {
        }

       
        public static BackgroundSharedService Instance { get; } =
            _instance ?? (_instance = new BackgroundSharedService());


      
        public void Start()
        {
            BackgroundAggregatorService.Instance.Start();
        }
    }
}
