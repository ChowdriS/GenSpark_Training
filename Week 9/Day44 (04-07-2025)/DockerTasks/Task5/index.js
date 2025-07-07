const express = require('express');
const mongoose = require('mongoose');

const app = express();

const mongoURL = process.env.MONGO_URL;

mongoose.connect(mongoURL, {
  useNewUrlParser: true,
  useUnifiedTopology: true
})
.then(() => console.log("Connected to MongoDB"))
.catch((err) => console.error("MongoDB connection failed:", err));

app.get('/', (req, res) => {
  res.send("Hello from Node.js API with MongoDB!");
});

app.listen(3000, () => console.log("Server listening on port 3000"));
