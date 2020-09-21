using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class BendingManager : MonoBehaviour
{

    private const string BENDING_FEATURE = "ENABLE_BENDING";
    private const string UNLIT_BENDING_FEATURE = "ENABLE_BENDING_UNLIT";

    //#region Monobehaviour

    private void Awake() {
        if (Application.isPlaying) {
            Shader.EnableKeyword(BENDING_FEATURE);
            Shader.EnableKeyword(UNLIT_BENDING_FEATURE);
        }
        else {
            Shader.DisableKeyword(BENDING_FEATURE);
            Shader.DisableKeyword(UNLIT_BENDING_FEATURE);
        }
    }

    //private void OnEnable() {
    //    RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    //    RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    //}

    //private void OnDisable() {
    //    RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    //    RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    //}

    //#endregion

    //#region Methods

    //private static void OnBeginCameraRendering(ScriptableRenderContext ctx, Camera cam) {
    //    cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) * cam.worldToCameraMatrix;
    //}

    //private static void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam) {
    //    cam.ResetCullingMatrix();
    //}

    //#endregion

}