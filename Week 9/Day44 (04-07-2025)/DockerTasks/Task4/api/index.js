const express = require('express');
const cors = require('cors');

const PORT = 5000;
const app = express();

app.use(cors());

app.get('/', (req, res) => {
  res.json({ message: '"Hello from Backend!"' });
});

app.use((req, res) => {
  res.status(404).json({ error: 'Not found' });
});

app.listen(PORT, () => {
  console.log(`Server running at http://localhost:${PORT}`);
});
