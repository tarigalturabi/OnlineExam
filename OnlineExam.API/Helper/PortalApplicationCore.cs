
using KFU.Common;
using System.Data;

namespace Portal.Ui.Arabic
{
    public class PortalApplicationCore : IPortalApplicationCore
    {
        public string GetPageView(string folder, string file)
        {
            //return $"~/Views/Pages/{folder}/{file}";
            return $"~/Views/{folder}/{file}";
        }

		public int IsActive()
		{
            return 0 ;
		}
		public int InternalService()
		{
            return 0;
		}
	}
}
