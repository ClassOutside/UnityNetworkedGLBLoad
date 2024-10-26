using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using GLTFast;
using GLTFast.Logging;
using GLTFast.Loading;

public class SpawnManager : MonoBehaviour
{

    public string baseURL = "https://:3001/";
    public GameObject objectSpawn;
    private string getModelMapping = "models/glb";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnGLB()
    {
        StartCoroutine(SpawnGLBAsync(baseURL));
    }

    private IEnumerator SpawnGLBAsync(string urlWithParams)
    {
        // Load GLTF asynchronously in the background
        Task<GltfImport> gltfTask = LoadGLB(urlWithParams);

        // Wait until the GLTF is fully loaded
        yield return new WaitUntil(() => gltfTask.IsCompleted);

        if (gltfTask.IsFaulted || gltfTask.Result == null)
        {
            Debug.LogError("Failed to load GLTF model.");
            yield break;
        }

        var gltf = gltfTask.Result;

        // Call instantiateGLB asynchronously
        var instantiateTask = instantiateGLB(gltf);
    }

    public async Task<GltfImport> LoadGLB(string urlWithParams) {

        // Create and configure import settings
        var settings = new ImportSettings
        {
            GenerateMipMaps = true,
            AnisotropicFilterLevel = 3,
            NodeNameMethod = NameImportMethod.OriginalUnique
        };
        
        var gltf = new GltfImport(downloadProvider: new CustomDownloadProvider(), logger: new ConsoleLogger()); // For Local area network testing with Self Signed Certificates
        // var gltf = new GltfImport(downloadProvider: new DefaultDownloadProvider(), logger: new ConsoleLogger()); // 

        var success = await gltf.Load(new System.Uri(urlWithParams), settings);

        if (success)
        {
            return gltf;
        }
        else
        {
            Debug.LogError($"Loading glTF from URL failed! URL: {urlWithParams}");
            return null;  
        }
    }

    public async Task<GameObject> instantiateGLB(GltfImport gltf){
        
            // Create a new GameObject to hold the glTF scene
            var parentGameObject = new GameObject("glb_Model");
            parentGameObject.transform.SetParent(objectSpawn.transform, false); 
            await gltf.InstantiateMainSceneAsync(parentGameObject.transform);
            
            Debug.Log("glTF successfully loaded and instantiated!");

            return parentGameObject;
        
    }

}
