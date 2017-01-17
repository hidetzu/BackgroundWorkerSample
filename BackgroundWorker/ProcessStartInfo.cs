using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BackgroundWorkerSample
{
    public class ProcessStartInfo
    {
        public string Exe { get; }
        public string Parameters { get; }

        public ProcessStartInfo(string imgPath)
        {
            var exe = ConfigurationManager.AppSettings["rundll32"];
            Exe = exe;
            var photoViewer = ConfigurationManager.AppSettings["photoViewerdll"];
            //var arguments = "\"C:\\Program Files\\Windows Photo Viewer\\PhotoViewer.dll\", ImageView_Fullscreen C:\\workspace\\70c07400.jpg";
            var arguments = "\"" + photoViewer + "\"";
            arguments += ", ImageView_Fullscreen ";
            arguments += System.IO.Path.GetFullPath(imgPath);

            Parameters = arguments;
        }
    }
}