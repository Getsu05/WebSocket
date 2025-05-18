using System.Text;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.UI;

public class WebSocketClient : MonoBehaviour
{
    WebSocket websocket;
    public Text estadoTexto;
    public InputField mensajeInput;
    public Text chatTexto;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () => {
            Debug.Log("Conectado!");
            estadoTexto.text = " Conectado";
        };

        websocket.OnError += (e) => {
            Debug.Log("Error: " + e);
            estadoTexto.text = " Error: " + e;
        };

        websocket.OnClose += (WebSocketCloseCode closeCode) => {
            Debug.Log("Desconectado con código: " + closeCode);
            estadoTexto.text = " Desconectado (" + closeCode + ")";
        };


        websocket.OnMessage += (bytes) => {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Mensaje recibido: " + message);
            chatTexto.text += message + "\n";
        };

        await websocket.Connect();
    }

    void Update()
    {
        if (websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
    }


    public async void EnviarMensaje()
    {
        if (websocket.State == WebSocketState.Open)
        {
            string texto = mensajeInput.text;
            await websocket.SendText(texto);
            mensajeInput.text = "";
        }
    }

    public async void Reconectar()
    {
        await websocket.Connect();
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
