using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class TestAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityFactory.factory.LoadDragonBonesData("Player/MC_ANIMASI_fix_ske");
        UnityFactory.factory.LoadTextureAtlasData("Player/MC_ANIMASI_fix_tex");

        var armatureComponent = UnityFactory.factory.BuildArmatureComponent("MC");

        armatureComponent.animation.Play("Walk");

        // Change armatureposition.
        armatureComponent.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
