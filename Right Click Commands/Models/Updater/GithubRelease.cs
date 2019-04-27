using Newtonsoft.Json;

namespace Right_Click_Commands.Models.Updater
{
    public class GithubRelease
    {
        private Asset[] assets;

        //  JSON Properties
        //  ===============

        [JsonProperty("html_url")]
        public string URL { get; set; }

        [JsonProperty("tag_name")]
        public string Tag { get; set; }

        [JsonProperty("draft")]
        public bool IsDraft { get; set; }

        [JsonProperty("assets")]
        public Asset[] Assets
        {
            get => assets;
            set
            {
                assets = value;

                foreach (Asset asset in Assets)
                {
                    asset.UpdateURL = URL;
                }
            }
        }
    }
}
