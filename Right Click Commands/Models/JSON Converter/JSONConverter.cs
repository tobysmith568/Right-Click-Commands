using Newtonsoft.Json;

namespace Right_Click_Commands.Models.JSON_Converter
{
    public class JSONConverter : IJSONConverter
    {
        //  Variables
        //  =========

        public T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string ToJson<T>(T model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
