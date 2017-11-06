using System;
using System.Net;
using System.Text;
using System.Web.UI;

namespace Singularity.Web
{
    public class Browser
    {
        /// <summary>
        /// Launches a new window
        /// </summary>
        /// <param name="url"></param>
        /// <returns>HTML to open a new window</returns>
        static public String LaunchWindow(String url)
        {
            String output = "<script language=\"JavaScript\">";
            output += "window.open(\"" + url + "\")";
            output += "</script>";
            return output;

        }

        /// <summary>
        /// Scrapes the specified URL and returns a string containing the page html
        /// </summary>
        static public String ScrapeURL(String url)
        {
            WebClient webClient = new WebClient();
            Byte[] reqHTML = webClient.DownloadData(url);
            UTF8Encoding objUTF8 = new UTF8Encoding();
            return objUTF8.GetString(reqHTML);
        }


        //static public BrowserDeviceType DetectDeviceType(Page page)
        //{
        //    return MDetect.DetectDevice(page);
        //}

        //void MDetect_Test_OnDetectSmartPhone(object page, Singularity.Utils.MDetect.MDetectArgs args)
        //{
        //    //RunAsSmartPhone();
        //}

        //void MDetect_Test_OnDetectTierTablet(object page, Singularity.Utils.MDetect.MDetectArgs args)
        //{
        //    //RunAsTablet();
        //}

    }
}
