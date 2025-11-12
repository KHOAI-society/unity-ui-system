using System;
using System.Collections.Generic;
using UnityEngine;

namespace Khoai
{
    public partial class KTheme
    {
        private static KTheme _instance;
        public static KTheme Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var resources = Resources.LoadAll<KTheme>("");
                if (resources.Length == 0)
                {
                    throw new Exception("There are no SriptableObject KTheme in Resources folder");
                }
                else if (resources.Length > 1)
                {
                    string message = "There more than 1 SriptableObject KTheme in Resources folder\n";
                    foreach (var res in resources)
                    {
                        message += $"{res.name} \n";
                    }
                    throw new Exception(message);
                }

                _instance = resources[0];

                return _instance;
            }
        }
    }
}
