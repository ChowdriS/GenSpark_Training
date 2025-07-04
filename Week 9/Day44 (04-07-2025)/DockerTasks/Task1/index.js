const http = require('http');

const PORT = 3000;

http.createServer((_, res) => {
  res.writeHead(200);
  res.end('Hello World!');
}).listen(PORT, () => {
  console.log(`Server running at http://localhost:${PORT}`);
});
