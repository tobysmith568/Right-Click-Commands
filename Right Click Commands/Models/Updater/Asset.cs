using Newtonsoft.Json;

namespace Right_Click_Commands.Models.Updater
{
    public class Asset
    {
        //  Variables
        //  =========

        private static readonly string[] dataSizes = { "B", "KB", "MB", "GB", "TB" };
        private int bytes;

        //  JSON Properties
        //  ===============

        [JsonProperty("browser_download_url")]
        public string URL { get; set; }

        [JsonProperty("size")]
        public int Bytes
        {
            get => bytes;
            set
            {
                bytes = value;

                int len = value;
                int order = 0;
                while (len >= 1024 && order < dataSizes.Length - 1)
                {
                    order++;
                    len = len / 1024;
                }
                ReadableSize = string.Format("{0:0.##} {1}", len, dataSizes[order]);
            }
        }

        //  Properties
        //  ==========

        public string ReadableSize { get; private set; }
    }
}
