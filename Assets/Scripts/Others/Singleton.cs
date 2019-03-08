using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // Если свойство _instance еще не настроено ...
            if (_instance == null)
            {
                // Попытаться найти объект.
                _instance = FindObjectOfType<T>();
                // Вывести собщение в случае неудачи.
                if (_instance == null)
                {
                    Debug.LogError("Can't find " + typeof(T) + "!");
                }
            }
            // Вернуть экземпляр для использования!
            return _instance;
        }
    }
}