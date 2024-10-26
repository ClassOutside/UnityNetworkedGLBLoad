// services/myService.js
const path = require("path");
const fs = require("fs");

let baseDirectory = path.resolve(__dirname, "../public/Models/");

class ModelService {
  static getData() {
    return { message: "Hello from My Service!" };
  }

  static getGLBFilePath(fileName) {
    return path.join(baseDirectory, fileName);
  }

  static streamFile(filePath, res) {
    // Set headers for file download
    res.set({
      "Content-Type": "model/gltf-binary",
      "Content-Disposition": 'attachment; filename=""',
    });

    // Stream the file
    fs.createReadStream(filePath).pipe(res);
  }
}

module.exports = ModelService;
