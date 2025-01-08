const express = require("express");
const mongoose = require("mongoose");
const bodyParser = require("body-parser");

// Connect to MongoDB
mongoose.connect("mongodb://localhost:27017/leaderboard", {
    useNewUrlParser: true,
    useUnifiedTopology: true,
});

// Define the Player Score schema
const playerScoreSchema = new mongoose.Schema({
    name: { type: String, required: true, unique: true },
    score: { type: Number, required: true },
});

// Create the Player Score model
const Leaderboard = mongoose.model("Leaderboard", playerScoreSchema);

// Initialize Express app
const app = express();
app.use(bodyParser.json());

// Endpoint to post a new score
app.post("/api/leaderboard", async (req, res) => {
    const { name, score } = req.body;

    if (!name || !score) {
        return res.status(400).json({ msg: "Please enter all fields" });
    }

    try {
        // Check if the player already exists
        let playerScore = await Leaderboard.findOne({ name });

        if (playerScore) {
            // If player exists, update the score
            playerScore.score = score; // Update the score
            const updatedScore = await playerScore.save(); // Save the updated score
            return res.json(updatedScore); // Return the updated score
        } else {
            // If player does not exist, create a new score entry
            const newScore = new Leaderboard({ name, score });
            const scoreSaved = await newScore.save();
            return res.json(scoreSaved);
        }
    } catch (error) {
        console.log(error);
        res.status(500).json({ msg: "Server Error" });
    }
});

// Endpoint to get the leaderboard
app.get("/api/leaderboard", async (req, res) => {
    try {
        const scores = await Leaderboard.find().sort({ score: -1 }); // Sort by score descending
        res.json(scores);
    } catch (error) {
        console.log(error);
        res.status(500).json({ msg: "Server Error" });
    }
});

// Start the server
const PORT = process.env.PORT || 3009;
app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});
