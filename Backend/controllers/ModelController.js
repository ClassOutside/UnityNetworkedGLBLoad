// controllers/myController.js
const ModelService = require("../services/ModelService");
const express = require("express");

let fileName = "bell_mushroom.glb";

class ModelController {
  static getData(req, res) {
    const data = ModelService.getData();
    res.json(data);
  }

  static async getGLB(req, res) {
    try {
      const filePath = ModelService.getGLBFilePath(fileName);
      ModelService.streamFile(filePath, res);
    } catch (error) {
      return res.status(404).json({ message: error.message });
    }
  }

  static getRoutes() {
    const router = express.Router();

    // Define routes
    router.get("/glb", this.getGLB);

    return router;
  }
}

module.exports = ModelController;
