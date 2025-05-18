const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });

wss.on('connection', (ws) => {
  console.log("Nuevo cliente conectado");

  ws.on('message', (data) => {
    // Enviar a todos los clientes conectados
    wss.clients.forEach(client => {
      if (client.readyState === WebSocket.OPEN) {
        client.send(data);
      }
    });
  });

  ws.on('close', () => {
    console.log("Cliente desconectado");
  });
});

console.log("Servidor WebSocket en ws://localhost:8080");
