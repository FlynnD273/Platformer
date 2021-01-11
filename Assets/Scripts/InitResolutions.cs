using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InitResolutions : MonoBehaviour
{

    private Dropdown d;

    // Start is called before the first frame update
    void Start()
    {
        d = GetComponent<Dropdown>();
        d.ClearOptions();

        var options = Screen.resolutions.Select(o => new Dropdown.OptionData($"{o.width} x {o.height}")).ToList();
        options.Reverse();
        d.AddOptions(options);
    }
}
