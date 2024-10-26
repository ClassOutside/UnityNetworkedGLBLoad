using GLTFast.Loading;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CustomDownloadProvider : IDownloadProvider
{
    public async Task<IDownload> Request(Uri uri)
    {
        using var request = UnityWebRequest.Get(uri);
        request.certificateHandler = new BypassCertificate();

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            throw new Exception(request.error);
        }

        return new ByteDownload(request.downloadHandler.data);
    }

    public async Task<ITextureDownload> RequestTexture(Uri uri, bool linear)
    {
        using var request = UnityWebRequestTexture.GetTexture(uri);
        request.certificateHandler = new BypassCertificate();

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            throw new Exception(request.error);
        }

        return new TextureDownload(DownloadHandlerTexture.GetContent(request));
    }
}

// Implementing IDownload
public class ByteDownload : IDownload
{
    public byte[] Data { get; }
    public bool Success => true; // Assuming success, adjust as needed
    public string Error => null; // Return error message if needed
    public string Text => null; // Return text content if needed
    public bool? IsBinary => true; // Adjusted to bool? to match interface

    public ByteDownload(byte[] data)
    {
        Data = data;
    }

    public void Dispose()
    {
        // Clean up if necessary
    }
}

// Implementing ITextureDownload
public class TextureDownload : ITextureDownload
{
    public Texture2D Texture { get; }
    public byte[] Data => Texture?.EncodeToPNG(); // If applicable, return texture data
    public bool Success => Texture != null; // Check if texture loaded successfully
    public string Error => null; // Return error message if needed
    public string Text => null; // Not applicable for textures
    public bool? IsBinary => true; // Adjusted to bool? to match interface

    public TextureDownload(Texture2D texture)
    {
        Texture = texture;
    }

    public void Dispose()
    {
        // Clean up if necessary
    }
}
