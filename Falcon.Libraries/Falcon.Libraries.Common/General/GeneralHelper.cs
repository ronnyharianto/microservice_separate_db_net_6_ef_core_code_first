using Falcon.Libraries.Common.Object;
using System.Text.RegularExpressions;

namespace Falcon.Libraries.Common.General
{
    public class GeneralHelper : IGeneralHelper
    {
        private static ConfigObject? config = null;
        public GeneralHelper()
        {
        }
        #region Get Config
        public static ConfigObject? Config
        {
            get
            {
                if (config != null)
                    return config;
                else
                    return GetConfig();
            }
            set
            {
                config = value;
            }
        }
        private static ConfigObject? GetConfig()
        {
            ConfigObject? result;
            if (config != null)
                result = config;
            else
            {
                string path = AppContext.BaseDirectory;

                string raw_data = "";
                string absolute_path = System.IO.Path.Combine(path, "appsettings.json");
                using (StreamReader reader = new StreamReader(absolute_path))
                {
                    raw_data = reader.ReadToEnd();
                }
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigObject>(raw_data);
            }
            return result;
        }
        public ConfigObject? GetDataConfig()
        {
            return GetConfig();
        }
        #endregion

        public bool OnlyNumberCharacter(string value)
        {
            return Regex.IsMatch(value, "^(\\d|\\w)+$", RegexOptions.IgnoreCase);
        }
        public bool OnlyNumber(string value)
        {
            return Regex.IsMatch(value, "^[0-9]", RegexOptions.IgnoreCase);
        }

        public string GenerateID()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        public string RemoveSpecialCharacters(string input)
        {
            Regex r = new Regex("(?:[^0-9]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, string.Empty);
        }

        public string NumberToRupiah(decimal angka)
        {
            string result = string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "Rp.{0:N}", angka);
            result = result.Remove(result.Length - 3);
            return result;
        }
        public string NumberToRupiahWithoutPrefix(decimal angka)
        {
            string result = string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "{0:N}", angka);
            result = result.Remove(result.Length - 3);
            return result;
        }
        public string RemoveSpecialChar(string input)
        {
            Regex r = new Regex("(?:[^0-9a-zA-Z]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, string.Empty);
        }
    }
}
