using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : Menu
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Pause();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isSettings)
        {
            Settings(false);
        }
    }
}
