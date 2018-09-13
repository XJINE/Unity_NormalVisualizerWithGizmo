using UnityEngine;

[ExecuteInEditMode]
public class NormalVisualizerWithGizmo : MonoBehaviour
{
    #region Enum

    public enum Type
    {
        Vertex,
        Surface
    }

    #endregion Enum

    #region Field

    public Type drawType = NormalVisualizerWithGizmo.Type.Surface;

    [Range(0f, 1)]
    public float normalLength = 0.1f;

    public Color normalColor = Color.white;

    public bool  normalColorFromDirection = true;

    protected Mesh mesh;

    protected new Transform transform;

    #endregion Field

    #region Method

    protected virtual void OnEnable()
    {
        if (this.mesh == null)
        {
            MeshFilter meshFilter = base.gameObject.GetComponent<MeshFilter>();

            if (meshFilter == null)
            {
                this.mesh = base.gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            }
            else
            {
                this.mesh = base.gameObject.GetComponent<MeshFilter>().sharedMesh;
            }
        }

        if (this.transform == null)
        {
            this.transform = base.transform;
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Color     previousColor  = Gizmos.color;
        Matrix4x4 previousMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(this.transform.position,
                                      this.transform.rotation,
                                      this.transform.localScale);

        switch (this.drawType)
        {
            case Type.Surface:
                {
                    DrawSurfaceNormalGizmos();
                    break;
                }
            case Type.Vertex:
                {
                    DrawVertexNormalGizmos();
                    break;
                }
        }

        Gizmos.color  = previousColor;
        Gizmos.matrix = previousMatrix;
    }

    protected virtual void DrawVertexNormalGizmos()
    {
        Vector3 [] vertices = this.mesh.vertices;
        Vector3 [] normals  = this.mesh.normals;
        Vector3 normal;

        Gizmos.color = this.normalColor;

        for (int i = 0; i< this.mesh.vertexCount; i++)
        {
            normal = Vector3.Normalize(normals[i]);

            if (this.normalColorFromDirection)
            {
                Gizmos.color = new Color(normal.x, normal.y, normal.z);
            }

            Gizmos.DrawRay(vertices[i], normal * this.normalLength);
        }
    }

    protected virtual void DrawSurfaceNormalGizmos()
    {
        Vector3[] vertices  = this.mesh.vertices;
        Vector3[] normals   = this.mesh.normals;
            int[] triangles = this.mesh.triangles;

        Vector3 normal;
        Vector3 position;

        int triangleIndex0;
        int triangleIndex1;
        int triangleIndex2;

        Gizmos.color = this.normalColor;

        for (int i = 0; i <= triangles.Length - 3; i += 3)
        {
            triangleIndex0 = triangles[i];
            triangleIndex1 = triangles[i + 1];
            triangleIndex2 = triangles[i + 2];

            position = (vertices[triangleIndex0]
                      + vertices[triangleIndex1]
                      + vertices[triangleIndex2]) / 3;

            normal = (normals[triangleIndex0]
                    + normals[triangleIndex1]
                    + normals[triangleIndex2]) / 3;

            normal = Vector3.Normalize(normal);

            if (this.normalColorFromDirection)
            {
                Gizmos.color = new Color(normal.x, normal.y, normal.z);
            }

            Gizmos.DrawRay(position, normal * this.normalLength);
        }
    }

    #endregion Method
}