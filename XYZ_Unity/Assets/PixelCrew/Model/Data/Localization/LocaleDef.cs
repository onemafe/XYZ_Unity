using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
public class LocaleDef : ScriptableObject
{
    // ru https://docs.google.com/spreadsheets/d/e/2PACX-1vQu4IHDiUf4u6FzGgfh7qoQ_vQZZmFLPPSHGtE36NV2SBy8KcWX7WszVslbLTCaMeFWxll2dfcDrJ7x/pub?gid=1163146841&single=true&output=tsv
    // en https://docs.google.com/spreadsheets/d/e/2PACX-1vQu4IHDiUf4u6FzGgfh7qoQ_vQZZmFLPPSHGtE36NV2SBy8KcWX7WszVslbLTCaMeFWxll2dfcDrJ7x/pub?gid=0&single=true&output=tsv

    [SerializeField] private string _url;
    [SerializeField] private List<LocaleItem> _localeItems;

    private UnityWebRequest _request;

    [ContextMenu("Update locale")]
    public void LoadLocale()
    {
        if (_request != null) return;

        _request = UnityWebRequest.Get(_url);
        _request.SendWebRequest().completed += OnDataLoaded;
    }

    private void OnDataLoaded(AsyncOperation operation)
    {
        if(operation.isDone)
        {
            var rows = _request.downloadHandler.text.Split('\n');
            foreach (var row in rows)
            {
                AddLocaleItem(row);
            }

        }
    }

    private void AddLocaleItem(string row)
    {
        try
        {
            var parts = row.Split('\t');
            _localeItems.Add(new LocaleItem { Key = parts[0], Value = parts[1] });
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't parse row {row}. \n {e}");
        }
    }

    [Serializable]
    private class LocaleItem
    {
        public string Key;
        public string Value;
    }
}
