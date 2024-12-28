using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using UnityEngine;


public class AddressablesBuild : MonoBehaviour
{
    [MenuItem("Build/Build Addressables For Android")]
    public static void BuildAddressablesForAndroid()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        AddressableAssetSettings.BuildPlayerContent();
    }

    [MenuItem("Build/Build Addressables For iOS")]
    public static void BuildAddressablesForiOS()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        AddressableAssetSettings.BuildPlayerContent();
    }

    [MenuItem("Build/Build Addressables For Windows")]
    public static void BuildAddressablesForWindows()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        AddressableAssetSettings.BuildPlayerContent();
    }
}
