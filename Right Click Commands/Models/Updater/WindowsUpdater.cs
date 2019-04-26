using Newtonsoft.Json;
using RestSharp;
using Right_Click_Commands.Views.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Updater
{
    public class WindowsUpdater : IUpdater
    {
        //  Constants
        //  =========

        private const string repoURL = "https://api.tobysmith.uk/github?repo=Right-Click-Commands";
        private const string vPrefix = "v";
        private const string msiExtension = ".msi";

        //  Methods
        //  =======

        public async Task<Asset> CheckForUpdateAsync()
        {
            RestClient client = new RestClient(repoURL);
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = await Task.Run(() =>
            {
                return client.Execute(request);
            });

            if (response == null)
            {
                return null;
            }

            switch (response.ResponseStatus)
            {
                case ResponseStatus.TimedOut:
                case ResponseStatus.Error:
                case ResponseStatus.Aborted:
                default:
                    return null;

                case ResponseStatus.Completed when response.StatusCode == HttpStatusCode.OK:
                    GithubRelease release = JsonConvert.DeserializeObject<GithubRelease>(response.Content);

                    if (release == null)
                        return null;

                    if (release.IsDraft)
                        return null;

                    Version.TryParse(release.Tag.Split('-')[0].Replace(vPrefix, string.Empty), out Version version);

                    if (Assembly.GetExecutingAssembly().GetName().Version.CompareTo(version) >= 0)
                        return null;

                    Asset asset = release.Assets.FirstOrDefault(r => r.URL.EndsWith(msiExtension));

                    if (asset == null)
                        return null;

                    return asset;
            }
        }

        public void UpdateTo(Asset asset)
        {
            UpdateWindow updateWindow = new UpdateWindow(asset);
            updateWindow.ShowDialog();
        }
    }
}
