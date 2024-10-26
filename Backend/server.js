const express = require("express");
const https = require("https");
const fs = require("fs");
const cors = require("cors");
const ModelController = require("./controllers/ModelController");

const app = express();

// Load SSL certificate and key
const options = {
  key: fs.readFileSync("./keys/key.pem"),
  cert: fs.readFileSync("./keys/cert.pem"),
};

// Enable CORS for all routes
app.use(cors());

// Use the routes defined in ModelController
app.use("/models", ModelController.getRoutes());

// Create HTTPS server
const PORT = 3001;
https.createServer(options, app).listen(PORT, () => {
  console.log(`HTTPS Server running on https://localhost:${PORT}`);
});
