using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scenes.Login
{
    public class LoginBehaviourScript : MonoBehaviour
    {
        private static string LoginFileStorage => Application.persistentDataPath + "/login-settings.json";
        
        public InputField uriText;
        public InputField privateKeyText;
        public InputField addressText;
        
        // Start is called before the first frame update
        void Start()
        {
            var (uri, key, address) = ("", "", "");
            if (File.Exists(LoginFileStorage))
            {
                try
                {
                    var content = File.ReadAllText(LoginFileStorage);
                    (uri, key, address) = JsonConvert.DeserializeObject<(string, string, string)>(content);
                }
                catch(Exception)
                {
                    File.Delete(LoginFileStorage);
                }
            }
            uriText.text = uri;
            privateKeyText.text = key;
            addressText.text = address;
        }

        public void Login()
        {
            var serialized = JsonConvert.SerializeObject((uriText.text, privateKeyText.text, addressText.text));
            File.WriteAllText(LoginFileStorage, serialized);
            GameSettings.Uri = uriText.text;
            GameSettings.PrivateKey = privateKeyText.text;
            GameSettings.Address = addressText.text;

            SceneManager.LoadScene("Scenes/CollecionList/CollectionListScene", LoadSceneMode.Single);
        }
    }
}
