using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class Raymarch : SceneViewFilter
{
    [SerializeField] private Shader _shader;

    public Material _raymarchMaterial
    {
        get
        {
            if (!_raymarchMat && _shader)
            {
                _raymarchMat = new Material(_shader);
                _raymarchMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return _raymarchMat;
        }
    }
    private Material _raymarchMat;

    public Camera _camera
    {
        get
        {
            if (!_cam)
            {
                _cam = GetComponent<Camera>();
            }
            return _cam;
        }
    }
    private Camera _cam;

	public Transform _directionLight;

	public float _maxDistance;
    public Color _mainColor;
	public Vector4 _sphere1, _box1;
    public Vector3 _modInterval;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!_raymarchMaterial)
        {
            Graphics.Blit(source, destination);
            return;
        }

		_raymarchMaterial.SetVector("_LightDir", _directionLight ? _directionLight.forward : Vector3.down);
        _raymarchMaterial.SetMatrix("_CamFrustum", CamFrustum(_camera));
        _raymarchMaterial.SetMatrix("_CamToWorld", _camera.cameraToWorldMatrix);
		_raymarchMaterial.SetFloat("_maxDistance", _maxDistance);
		_raymarchMaterial.SetVector("_sphere1", _sphere1);
        _raymarchMaterial.SetVector("_box1", _box1);
        _raymarchMaterial.SetVector("_modInterval", _modInterval);
        _raymarchMaterial.SetColor("_mainColor", _mainColor);

        RenderTexture.active = destination;
		_raymarchMaterial.SetTexture("_MainTex", source);
        GL.PushMatrix();
        GL.LoadOrtho();
        _raymarchMaterial.SetPass(0);
        GL.Begin(GL.QUADS);

        //BL
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f);

        //BR
        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f);

        //TR
        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f);

        //TL
        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
    }

    private Matrix4x4 CamFrustum(Camera cam)
    {
        Matrix4x4 frustum = Matrix4x4.identity;
        float fov = Mathf.Tan((cam.fieldOfView * 0.5f) * Mathf.Deg2Rad);

        Vector3 goUp = Vector3.up * fov;
        Vector3 goRight = Vector3.right * fov * cam.aspect;

        Vector3 TL = (-Vector3.forward - goRight + goUp);
        Vector3 TR = (-Vector3.forward + goRight + goUp);
        Vector3 BR = (-Vector3.forward + goRight - goUp);
        Vector3 BL = (-Vector3.forward - goRight - goUp);

        frustum.SetRow(0, TL);
        frustum.SetRow(1, TR);
        frustum.SetRow(2, BR);
        frustum.SetRow(3, BL);

        return frustum;
    }
}
